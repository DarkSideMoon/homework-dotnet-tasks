using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Metrics.DotNet.Samples.Services.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Services.Repository
{
    public class MongoDbBookRepository : IMongoDbBookRepository
    {
        private readonly IOptions<MongoSettings> _setting;
        private readonly IMongoDatabase _mongoDb;

        public MongoDbBookRepository(IOptions<MongoSettings> setting)
        {
            _setting = setting;

            var dbClient = new MongoClient(setting.Value.ConnectionString);
            _mongoDb = dbClient.GetDatabase(setting.Value.DatabaseName);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            var collection = _mongoDb.GetCollection<Book>(_setting.Value.CollectionName);
            return await collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Book> GetBook(Guid id)
        {
            var collection = _mongoDb.GetCollection<Book>(_setting.Value.CollectionName);
            return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task SetBook(Book book)
        {
            try
            {
                var collection = _mongoDb.GetCollection<Book>(_setting.Value.CollectionName);
                await collection.InsertOneAsync(book);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetBooks(List<Book> books)
        {
            try
            {
                var collection = _mongoDb.GetCollection<Book>(_setting.Value.CollectionName);
                await collection.InsertManyAsync(books);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
