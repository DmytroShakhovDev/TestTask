using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.WebAPI.Data
{
    [Table("Accounts")]
    public class Account
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Credit { get; set; }
    }
}
