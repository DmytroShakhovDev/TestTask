using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.WebAPI.Controllers;
using TestProject.WebAPI.Features.GetUser;

namespace TestProject.WebAPI.Features.GetUsersList
{
    public class GetUsersListQuery : IRequest<Result<List<GetUserQueryResponse>>>
    {
    }
}
