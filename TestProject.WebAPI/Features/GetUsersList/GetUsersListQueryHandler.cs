using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestProject.WebAPI.Controllers;
using TestProject.WebAPI.Data;
using TestProject.WebAPI.Features.GetUser;

namespace TestProject.WebAPI.Features.GetUsersList
{
    public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, Result<List<GetUserQueryResponse>>>
    {
        private readonly TestProjectDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly ILogger<GetUsersListQueryHandler> _logger;

        public GetUsersListQueryHandler(TestProjectDbContext dbContext, IMediator mediator, ILogger<GetUsersListQueryHandler> logger)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<List<GetUserQueryResponse>>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<GetUserQueryResponse>>();

            foreach (var item in await _dbContext.Users.ToListAsync())
            {
                var user = new GetUserQueryResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    MonthlySalary = item.MonthlySalary,
                    MonthlyExpenses = item.MonthlyExpenses
                };

                result.Value.Add(user);
            }

            return result;
        }
    }
}
