using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using TestProject.WebAPI.Data;
using TestProject.WebAPI.Features.CreateUser;
using TestProject.WebAPI.Features.GetUser;
using TestProject.WebAPI.Features.GetUsersList;
using TestProject.WebAPI.Models;
using TestProject.WebAPI.Services;

namespace TestProject.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        private readonly RabbitMQClient _rabbitMQClient;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IMediator mediator, RabbitMQClient rabbitMQClient, IUserService userService)
        {
            _logger = logger;
            _mediator = mediator;
            _rabbitMQClient= rabbitMQClient;
            _userService = userService;
        }
        [HttpPost("authenticate")]
        [Route("/api/Users/Authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        [HttpPost]
        [Route("/api/Users/CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand query)
        {
            var result = await _mediator.Send(query);
            if (result.HasErrors) return BadRequest(result);
            return Created("CreateUser", result);
        }

        [HttpGet]
        [Route("/api/Users/GetUser")]
        public async Task<IActionResult> GetUser([FromQuery] GetUserQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.HasErrors)
            {
                if (result.Errors.First() == "record not found")
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Created("GetUser", result);
        }

        [HttpGet]
        [Route("/api/Users/GetUsersList")]
        public async Task<IActionResult> GetUsersList([FromQuery] GetUsersListQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.HasErrors) return BadRequest(result);
            Log.Logger.Write(Serilog.Events.LogEventLevel.Verbose, "Call /api/Users/GetUsersList");
            return Created("GetUsersList", result);
        }
    }
}
