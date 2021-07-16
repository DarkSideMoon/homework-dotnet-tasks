using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IElasticSearchBookClient _elasticSearchClient;
        private readonly IMongoDbBookClient _mongoDbClient;

        public BookController(IElasticSearchBookClient elasticSearchClient, IMongoDbBookClient mongoDbClient)
        {
            _elasticSearchClient = elasticSearchClient;
            _mongoDbClient = mongoDbClient;
        }

        /// <summary>
        /// Get book from MongoDb
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var result = await _mongoDbClient.GetBook(id);
            return Ok(result);
        }

        /// <summary>
        /// Get all books from MongoDb
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBooks()
        {
            var result = await _mongoDbClient.GetAllBooks();
            return Ok(result);
        }

        /// <summary>
        /// Set book in MongoDb
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SetBook(Book book)
        {
            await _mongoDbClient.SetBook(book);
            return Ok();
        }

        /// <summary>
        /// Search books by params
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost("search/{title}")]
        public async Task<IActionResult> Search(string title)
        {
            var result = await _elasticSearchClient.Search(title);
            return Ok(result);
        }
    }
}
