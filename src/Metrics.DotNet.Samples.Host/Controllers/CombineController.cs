using System;
using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Cache;
using Metrics.DotNet.Samples.Services.Client;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        public CombineController(
            IPostgresBookRepository postgresRepository, 
            IElasticSearchBookClient elasticSearchClient, 
            IMongoDbBookRepository mongoDbRepository)
        {
            _postgresRepository = postgresRepository;
            _elasticSearchClient = elasticSearchClient;
            _mongoDbRepository = mongoDbRepository;
        }

        /// <summary>
        /// Get random book from difference sources - postgresql, redis, mongodb and elasticsearch
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCombineRandomBook()
        {
            var randomBook = await _postgresRepository.GetRandomBook();

            var mongoDbAllBooks = await _mongoDbRepository.GetAllBooks();

            var cacheBook = await _cacheStorage.GetOrSetItem(new Random().Next(1, 10000).ToString(),
                async () => await _postgresRepository.GetRandomBook());

            var searchBook = await _elasticSearchClient.Search(randomBook.Title);

            return Ok(randomBook);
        }
    }
}
