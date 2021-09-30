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

namespace TestProject.WebAPI.Features.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<GetUserQueryResponse>>
    {
        private readonly TestProjectDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly ILogger<GetUserQueryHandler> _logger;

        public GetUserQueryHandler(TestProjectDbContext dbContext, IMediator mediator, ILogger<GetUserQueryHandler> logger)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<GetUserQueryResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<GetUserQueryResponse>();

            var item = await _dbContext.Users.Where(x => x.Email == request.Email).FirstAsync();
            if (item != null)
            {
                var user = new GetUserQueryResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    MonthlySalary = item.MonthlySalary,
                    MonthlyExpenses = item.MonthlyExpenses
                };

                result.Value = user;
            }
            else
            {
                result.AddError("record not found");
            }
            return result;
        }
    }
}
