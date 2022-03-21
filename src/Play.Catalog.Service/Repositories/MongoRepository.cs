using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T: IEntity
    {
      //  public const string CollectionName = "Items";
        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;
        public MongoRepository(IMongoDatabase mongoDatabase, string CollectionName)
        {
            //var mongoDbClient = new MongoClient("mongodb://localhost:27017");
            ////create a docker container for mongo db //=====>  docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo 
            //var database = mongoDbClient.GetDatabase("Catalog");
            dbCollection = mongoDatabase.GetCollection<T>(CollectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<T> filter = filterBuilder.Eq(x => x.Id, entity.Id);

            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(x => x.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}
