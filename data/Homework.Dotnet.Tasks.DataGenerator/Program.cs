using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Services.Cache;
using Homework.Dotnet.Tasks.Services.Client;
using Homework.Dotnet.Tasks.Services.Repository;
using Homework.Dotnet.Tasks.Services.Settings;
using Microsoft.Extensions.Options;
using Nest;

namespace Homework.Dotnet.Tasks.DataGenerator
{
    class Program
    {
        static void Main(string[] _)
        {
            // GenerateMySqlData();

            // GeneratePostgresSqlData();

            // GenerateElasticSearchData();

            // GenerateMongodbData();

            GenerateRedisData();
        }

        private static void GenerateRedisData()
        {
            Console.WriteLine("Generate random data for redis!");

            var stopwatch = new Stopwatch();
            var stopwatchAll = new Stopwatch();

            var redisSettings = Options.Create(new RedisSettings
            {
                ConnectionString = "localhost:6379,serviceName=mymaster,password=bitnami"
            });
            var redisConnectionFactory = new RedisConnectionFactory(redisSettings);
            var redisStorage = new RedisStorage<Contracts.Book>(redisConnectionFactory);

            stopwatchAll.Start();
            for (var i = 0; i < 100; i++)
            {
                stopwatch.Start();
                var redisData = FakeData.Book.Generate(1000);
                redisStorage.SetPipelineWithFireAndForget(redisData).GetAwaiter().GetResult();


                Console.WriteLine("Insert 1000 rows, iteration: " + i + ", elapsed in seconds: " + stopwatch.Elapsed.Seconds);
                stopwatch.Reset();
            }
            stopwatchAll.Stop();

            Console.WriteLine("All time elapsed in minutes: " + stopwatchAll.Elapsed.Minutes);
            Console.ReadLine();
        }

        private static void GenerateMySqlData()
        {
            Console.WriteLine("Generate random data for mysql!");

            var postgresSettings = Options.Create(new MySqlSettings
            {
                ConnectionString = "server=localhost;port=6446;user=homeworkuser;password=homeworkuser;database=homeworkdb"
            });

            var stopwatch = new Stopwatch();
            var stopwatchAll = new Stopwatch();
            stopwatchAll.Start();
            var mySqlDb = new MySqlBookRepository(postgresSettings);
            for (var i = 0; i < 10000; i++)
            {
                stopwatch.Start();
                var mySqlData = FakeData.Book.Generate(1000);
                mySqlDb.SetBooks(mySqlData).GetAwaiter().GetResult();
                stopwatch.Stop();

                Console.WriteLine("Insert 1000 rows, iteration: " + i + ", elapsed in seconds: " + stopwatch.Elapsed.Seconds);
                stopwatch.Reset();
            }
            stopwatchAll.Stop();

            Console.WriteLine("All time elapsed in minutes: " + stopwatchAll.Elapsed.Minutes);
            Console.ReadLine();
        }

        private static void GeneratePostgresSqlData()
        {
            Console.WriteLine("Generate random data for postgres!");

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
        }

        private static void GenerateElasticSearchData()
        {
            Console.WriteLine("Generate random data for elastic search!");

            var settings = Options.Create(new ElasticSearchSetting
            {
                IndexName = "book",
                Uri = "http://localhost:9200"
            });
            var elasticClient = new ElasticSearchBookClient(settings);

            var stopwatch = new Stopwatch();
            var stopwatchAll = new Stopwatch();
            stopwatchAll.Start();
            for (var i = 0; i < 100; i++)
            {
                stopwatch.Start();

                var elasticData = FakeData.BookDocument.Generate(1000);

                foreach (var data in elasticData)
                {
                    data.Suggest = new CompletionField
                    {
                        Input = new[] { data.Title, data.AuthorEmail, data.AuthorLastName }
                    };
                }

                elasticClient.BulkUpdate(elasticData);
                stopwatch.Stop();

                Console.WriteLine("Insert 1000 rows, iteration: " + i + ", elapsed in seconds: " + stopwatch.Elapsed.Seconds);
            }
            stopwatchAll.Stop();

            Console.WriteLine("All time elapsed in minutes: " + stopwatchAll.Elapsed.Minutes);
            Console.ReadLine();
        }

        private static void GenerateMongodbData()
        {
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
