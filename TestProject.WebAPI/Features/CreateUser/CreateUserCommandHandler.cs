using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.WebAPI.Data;
using MediatR;
using TestProject.WebAPI.Controllers;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace TestProject.WebAPI.Features.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
    {
        private readonly TestProjectDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(TestProjectDbContext dbContext, IMediator mediator, ILogger<CreateUserCommandHandler> logger)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<CreateUserCommandResponse>();
            if ((request.Name == null) || (request.Name == "") || (request.Name.Length == 0))
            {
                result.AddError(Resource.name_cant_be_empty);
                return result;
            }
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = request.Email,
                    MonthlySalary= request.MonthlySalary,
                    MonthlyExpenses=request.MonthlyExpenses

                };
                result.Value = new CreateUserCommandResponse()
                {
                    Id = user.Id
                };
                _dbContext.Users.Add(user);

                _dbContext.SaveChanges();

                await transaction.CommitAsync();
                _logger.LogInformation("CreateUserCommand Id:", user.Id);
            }

            return result;
        }
    }

}