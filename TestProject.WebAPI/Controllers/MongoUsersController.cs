using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.WebAPI.Entities;
using TestProject.WebAPI.Repositories;
using TestProject.WebAPI.Services;

namespace TestProject.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MongoUsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private RabbitMQClient _rabbitMQClient;
        public MongoUsersController(IUserRepository userRepository, RabbitMQClient rabbitMQClient)
        {
            _userRepository = userRepository;
            _rabbitMQClient = rabbitMQClient;
        }
       
        [HttpPost]
        [Authorize]
        [Route("/api/MongoUsers/CreateUser")]
        public async Task<IActionResult> Create(MongoUser user)
        {
            var id = await _userRepository.Create(user);
            _rabbitMQClient.PushMessage("userque", "Create user");
            return new JsonResult(id.ToString());
        }

        [HttpGet("{id}")]
        [Authorize]
        [Route("/api/MongoUsers/GetUser")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userRepository.Get(ObjectId.Parse(id));
            _rabbitMQClient.PushMessage("userque", $"Get single user by id:{id}");
            return new JsonResult(user);
        }

        [HttpGet]
        [Authorize]
        [Route("/api/MongoUsers/GetAll")]
        public async Task<IActionResult> Get()
        {
            var users = await _userRepository.Get();
            _rabbitMQClient.PushMessage("userque", "Get All users");
            return new JsonResult(users);
        }

        [HttpPut("{id}")]
        [Authorize]
        [Route("/api/MongoUsers/UpdateUser")]
        public async Task<IActionResult> Update(string id, MongoUser user)
        {
            var result = await _userRepository.Update(ObjectId.Parse(id), user);
            _rabbitMQClient.PushMessage("userque", $"Update single user by id:{id}");
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [Route("/api/MongoUsers/DeleteUser")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userRepository.Delete(ObjectId.Parse(id));
            _rabbitMQClient.PushMessage("userque", $"Delete single user by id:{id}");
            return new JsonResult(result);
        }
       
    }
}
