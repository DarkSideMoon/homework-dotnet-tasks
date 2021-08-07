using System.Collections.Generic;
using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Contracts;
using Homework.Dotnet.Tasks.Services.Repository.Interfaces;
using Homework.Dotnet.Tasks.Services.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Homework.Dotnet.Tasks.Services.Repository
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

        public async Task<Book> GetBook(int id)
        {
            var collection = _mongoDb.GetCollection<Book>(_setting.Value.CollectionName);
            return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task SetBook(Book book)
        {
            var collection = _mongoDb.GetCollection<Book>(_setting.Value.CollectionName);
            await collection.InsertOneAsync(book);
        }

        public async Task SetBooks(List<Book> books)
        {
            var collection = _mongoDb.GetCollection<Book>(_setting.Value.CollectionName);
            await collection.InsertManyAsync(books);
        }
    }
}
