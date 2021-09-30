using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestProject.WebAPI;
using TestProject.WebAPI.Controllers;
using TestProject.WebAPI.Data;
using TestProject.WebAPI.Features.CreateUser;
using TestProject.WebAPI.Features.GetUser;
using Xunit;

namespace TestProject.Tests
{
    public class UserTests : BaseTest, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly List<User> _users = new List<User>();

        public UserTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {

        }

        [Fact]
        public async Task AddUserTest()
        {
            var user = new CreateUserCommand()
            {
                Name = "Test User 123" + Guid.NewGuid(),
                Email = "email"+Guid.NewGuid()+"@example.com",
                MonthlyExpenses=100,
                MonthlySalary=120
                
            };

            var response = await _client.PostAsync($"api/Users/CreateUser", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await response.Content.ReadAsStringAsync();

            var resultObject = JsonConvert.DeserializeObject<Result<CreateUserCommandResponse>>(result);
            Guid Id = resultObject.Value.Id;

            ReloadDbEntites();
            var usersCount =
               _dbContext.Users.Where(x => x.Email == user.Email).Count();

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(1, usersCount);
        }
        [Fact]
        public async Task GetUserTest()
        {
            var user = new CreateUserCommand()
            {
                Name = "Test User 123" + Guid.NewGuid(),
                Email = "email" + Guid.NewGuid() + "@example.com",
                MonthlyExpenses = 100,
                MonthlySalary = 120

            };

            var response = await _client.PostAsync($"api/Users/CreateUser", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await response.Content.ReadAsStringAsync();

            var resultObject = JsonConvert.DeserializeObject<Result<CreateUserCommandResponse>>(result);
            Guid Id = resultObject.Value.Id;

            ReloadDbEntites();
            var usersCount =
               _dbContext.Users.Where(x => x.Email == user.Email).Count();

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(1, usersCount);

            var responseGet = await _client.GetAsync($"api/Users/GetUser?Email={user.Email}");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resultGet = await responseGet.Content.ReadAsStringAsync();
            var resultObjectGet = JsonConvert.DeserializeObject<Result<GetUserQueryResponse>>(result);
            Assert.Equal(user.Email,resultObjectGet.Value.Email);
        }

    }
}
