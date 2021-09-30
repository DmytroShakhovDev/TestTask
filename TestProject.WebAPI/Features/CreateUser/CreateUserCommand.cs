using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.WebAPI.Controllers;

namespace TestProject.WebAPI.Features.CreateUser
{
    public class CreateUserCommand : IRequest<Result<CreateUserCommandResponse>>
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public decimal MonthlySalary { get; set; }
        public decimal MonthlyExpenses { get; set; }
    }
}
