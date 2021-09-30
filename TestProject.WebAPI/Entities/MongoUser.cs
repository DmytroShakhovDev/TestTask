using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.WebAPI.Entities
{
    public class MongoUser
    {
        public ObjectId _id { get; set; }
        public ObjectId UserId { get => _id; set => _id = value; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
