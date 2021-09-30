using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TestProject.WebAPI.Entities;
using TestProject.WebAPI.Helpers;
using TestProject.WebAPI.Models;

namespace TestProject.WebAPI.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Credentials> GetAll();
        Credentials GetById(string userName);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Credentials> _users = new List<Credentials>();
        Credentials _config;
        public UserService(IConfiguration configuration)
        {
            _config = new TestProject.WebAPI.Helpers.Credentials();
            configuration.GetSection(nameof(Credentials)).Bind(_config);
            _users.Add(new Credentials() { Password = _config.Password, Secret = _config.Secret, UserName = _config.UserName });
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.UserName == model.Name );

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        // helper methods
        public IEnumerable<Credentials> GetAll()
        {
            return _users;
        }

        public Credentials GetById(string userName)
        {
            return _users.FirstOrDefault(x => x.UserName == userName);
        }

        private string generateJwtToken(Credentials user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Username", user.UserName.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    
}