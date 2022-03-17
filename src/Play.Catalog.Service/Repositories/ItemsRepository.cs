using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Repositories
{
    public class ItemsRepository
    {
        public const string CollectionName = "Items";
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        public ItemsRepository()
        {
            var mongoDbClient = new MongoClient("mongodb://localhost:27017");
            //create a docker container for mongo db //=====>  docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo 
            var database = mongoDbClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<Item>(CollectionName);
        }

        public async  Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(e => e.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<Item> filter = filterBuilder.Eq(x => x.Id, entity.Id);

            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
             FilterDefinition<Item> filter = filterBuilder.Eq(x => x.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}
