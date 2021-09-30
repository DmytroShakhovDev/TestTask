using System.ComponentModel.DataAnnotations;

namespace TestProject.WebAPI.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}