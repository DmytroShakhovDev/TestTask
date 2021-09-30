using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TestProject.WebAPI.Models
{
    public class UserModel
    {
        [BsonId]
        [DataMember]
        public MongoDB.Bson.ObjectId _id { get; set; }
        [DataMember]
        public ObjectId UserId { get => _id; set => _id = value; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }
    }
}
