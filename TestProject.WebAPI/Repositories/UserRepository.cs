using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.WebAPI.Entities;

namespace TestProject.WebAPI.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly IMongoCollection<MongoUser> _users;

        public UserRepository(IMongoClient client)
        {
            var database = client.GetDatabase("UserDb");
            var collection = database.GetCollection<MongoUser>(nameof(MongoUser));

            _users = collection;
        }
        public async Task<ObjectId> Create(MongoUser user)
        {
            await _users.InsertOneAsync(user);

            return user.UserId;
        }
        public Task<MongoUser> Get(ObjectId objectId)
        {
            var filter = Builders<MongoUser>.Filter.Eq(u => u._id, objectId);
            var user = _users.Find(filter).FirstOrDefaultAsync();

            return user;
        }

        public async Task<IEnumerable<MongoUser>> Get()
        {
            var users = await _users.Find(_ => true).ToListAsync();

            return users;
        }

        public async Task<bool> Update(ObjectId objectId, MongoUser user)
        {
            var filter = Builders<MongoUser>.Filter.Eq(u => u._id, objectId);
            var update = Builders<MongoUser>.Update
                .Set(u => u.Name, user.Name)
                .Set(u => u.CreateDate, user.CreateDate);
                
            var result = await _users.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }
        public async Task<bool> Delete(ObjectId objectId)
        {
            var filter = Builders<MongoUser>.Filter.Eq(u => u._id, objectId);
            var result = await _users.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }
    }
}
