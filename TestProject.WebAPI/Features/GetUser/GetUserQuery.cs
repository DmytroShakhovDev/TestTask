using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.WebAPI.Controllers;

namespace TestProject.WebAPI.Features.GetUser
{
    public class GetUserQuery: IRequest<Result<GetUserQueryResponse>>
    {
        public string Email { get; set; }
    }
}
