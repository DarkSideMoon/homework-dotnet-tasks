using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Host.Controllers
{
    /// <summary>
    /// Controller to work with MongoDb
    /// </summary>
    [ApiController]
    [Route("mongo")]
    public class MongoBookController : ControllerBase
    {
        private readonly IMongoDbBookRepository _mongoDbRepository;

        public MongoBookController(IMongoDbBookRepository mongoDbRepository)
        {
            _mongoDbRepository = mongoDbRepository;
        }

        /// <summary>
        /// Get book from MongoDb
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMongoDbBook(Guid id)
        {
            var result = await _mongoDbRepository.GetBook(id);
            return Ok(result);
        }

        /// <summary>
        /// Get all books from MongoDb
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMongoDbAllBooks()
        {
            var result = await _mongoDbRepository.GetAllBooks();
            return Ok(result);
        }

        /// <summary>
        /// Set book in MongoDb
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SetMongoDbBook([FromBody] Book book)
        {
            await _mongoDbRepository.SetBook(book);
            return Ok();
        }
    }
}
