using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.WebAPI.Data
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public decimal MonthlySalary { get; set; }
        public decimal MonthlyExpenses { get; set; }
    }
}
