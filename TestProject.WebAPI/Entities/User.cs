using System;
using System.Text.Json.Serialization;

namespace TestProject.WebAPI.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
}