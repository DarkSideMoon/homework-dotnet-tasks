using Metrics.DotNet.Samples.Services.Client;
using Metrics.DotNet.Samples.Services.Repository;
using Metrics.DotNet.Samples.Services.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.DataGenerator
{
    class Program
    {
        static void Main(string[] _)
        {
            Console.WriteLine("Generate random data for postgres!");

            /*
            var postgresSettings = Options.Create(new PostgresSettings
            {
                ConnectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=Password123"
            });
            var postgresDb = new PostgresBookRepository(postgresSettings);
            for (var i = 0; i < 100; i++)
            {
                var postgresData = FakeData.Book.Generate(1000);
                postgresDb.SetBooks(postgresData).GetAwaiter().GetResult();

                Console.WriteLine("Insert 1000 rows, iteration: " + i);
            }

            Console.ReadLine();
            */
            Console.WriteLine("Generate random data for elastic search!");
            
            var settings = Options.Create(new ElasticSearchSetting
            {
                IndexName = "book",
                Uri = "http://localhost:9200"
            });
            var elasticClient = new ElasticSearchBookClient(settings);
            for (var i = 0; i < 10; i++)
            {
                var elasticData = FakeData.BookDocument.Generate(1000);
                elasticClient.BulkUpdate(elasticData);
            }

            Console.ReadLine();
            Console.WriteLine("Generate random data for mongodb!");

            var mongoSettings = Options.Create(new MongoSettings
            {
                DatabaseName = "books",
                CollectionName = "book",
                ConnectionString = "mongodb://127.0.0.1:27017"
            });
            var mongoClient = new MongoDbBookRepository(mongoSettings);

            for (var i = 0; i < 10; i++)
            {
                var mongoData = FakeData.Book.Generate(1000);
                Task.Run(async () => await mongoClient.SetBooks(mongoData));
            }

            Console.WriteLine("Random data was generated successfully!");
            Console.ReadLine();
        }
    }
}
