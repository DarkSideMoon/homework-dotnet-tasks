using System;
using System.Threading;
using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Cache;
using Metrics.DotNet.Samples.Services.Client;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Counter;

namespace Metrics.DotNet.Samples.Host.Controllers
{
    /// <summary>
    /// Combine controller with postgresql, redis, mongodb and elasticsearch
    /// </summary>
    [ApiController]
    [Route("combine")]
    public class CombineController : ControllerBase
    {
        private readonly IPostgresBookRepository _postgresRepository;
        private readonly IElasticSearchBookClient _elasticSearchClient;
        private readonly IMongoDbBookRepository _mongoDbRepository;
        private readonly IStorage<Book> _cacheStorage;
        private readonly IMetrics _metrics;

        public CombineController(
            IPostgresBookRepository postgresRepository,
            IElasticSearchBookClient elasticSearchClient,
            IMongoDbBookRepository mongoDbRepository,
            IStorage<Book> cacheStorage,
            IMetrics metrics)
        {
            _postgresRepository = postgresRepository;
            _elasticSearchClient = elasticSearchClient;
            _mongoDbRepository = mongoDbRepository;
            _cacheStorage = cacheStorage;
            _metrics = metrics;
        }

        /// <summary>
        /// Get random book from difference sources - postgresql, redis, mongodb and elasticsearch
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCombineRandomBook()
        {
            _metrics.Measure.Counter.Increment(new CounterOptions
            {
                Context = "HomeworkDotNetTasks",
                MeasurementUnit = Unit.Results,
                Name = "count.combine.requests"
            });

            Book randomBook;
            using (_metrics.Measure.Timer.Time(MetricsHelper.PostgresGetTimer))
            {
                randomBook = await _postgresRepository.GetRandomBook();
            }

            using (_metrics.Measure.Timer.Time(MetricsHelper.MongodbGetTimer))
            {
                var mongoDbAllBooks = await _mongoDbRepository.GetAllBooks();
            }

            using (_metrics.Measure.Timer.Time(MetricsHelper.RedisGetTimer))
            {
                var cacheBook = await _cacheStorage.GetItem(new Random().Next(1, 10000).ToString());
            }

            using (_metrics.Measure.Timer.Time(MetricsHelper.ElasticGetTimer))
            {
                var searchBook = await _elasticSearchClient.SearchMatchAll();
            }

            return Ok(randomBook);
        }

        /// <summary>
        /// Get random book from difference sources - postgresql, redis, mongodb and elasticsearch
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(string text)
        {
            Thread.Sleep(100);
            var random = new Random().Next(0, 3);

            if (random == 1)
                throw new Exception(text);

            throw new ArgumentException(text);
        }
    }
}
