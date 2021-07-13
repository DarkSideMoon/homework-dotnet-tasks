using Metrics.DotNet.Samples.Host.Models;
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

        public BookController(IElasticSearchBookClient elasticSearchClient)
        {
            _elasticSearchClient = elasticSearchClient;
        }

        /// <summary>
        /// Get book from MongoDb
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            return Ok();
        }

        /// <summary>
        /// Get all books from MongoDb
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok();
        }

        /// <summary>
        /// Set book in MongoDb
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SetBook(BookRequest book)
        {
            return Ok();
        }

        /// <summary>
        /// Search books by params
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("search/{id}")]
        public async Task<IActionResult> SearchBook(Guid id)
        {
            return Ok();
        }
    }
}
