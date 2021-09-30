using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.WebAPI.Entities;

namespace TestProject.WebAPI.Repositories
{
    public interface IUserRepository
    {
        // Create
        Task<ObjectId> Create(MongoUser user);

        // Read
        Task<MongoUser> Get(ObjectId objectId);
        Task<IEnumerable<MongoUser>> Get();

        // Update
        Task<bool> Update(ObjectId objectId, MongoUser user);

        // Delete
        Task<bool> Delete(ObjectId objectId);
    }
}
