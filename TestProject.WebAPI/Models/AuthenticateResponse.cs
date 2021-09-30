using System;
using TestProject.WebAPI.Entities;
using TestProject.WebAPI.Helpers;

namespace TestProject.WebAPI.Models
{
    public class AuthenticateResponse
    {
        public string Name { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(Credentials user, string token)
        {
            Name = user.UserName;
            Token = token;
        }
    }
}