using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.WebAPI.Features.GetUser
{
    public class GetUserQueryResponse
    {
        public Guid Id;
        public string Email { get; set; }
        public string Name { get; set; }
        public decimal MonthlySalary { get; set; }
        public decimal MonthlyExpenses { get; set; }
    }
}
