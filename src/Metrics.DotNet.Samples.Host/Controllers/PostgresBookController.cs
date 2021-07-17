using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Host.Controllers
{
    /// <summary>
    /// Controller to work with PostgreSql
    /// </summary>
    [ApiController]
    [Route("postgres")]
    public class PostgresBookController : ControllerBase
    {
        private readonly IPostgresBookRepository _postgresRepository;

        public PostgresBookController(IPostgresBookRepository postgresRepository)
        {
            _postgresRepository = postgresRepository;
        }

        /// <summary>
        /// Get book from PostgreSQL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostgresBook(Guid id)
        {
            var result = await _postgresRepository.GetBook(id);
            return Ok(result);
        }

        /// <summary>
        /// Get all books from PostgreSQL
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMongoDbAllBooks()
        {
            var result = await _postgresRepository.GetAllBooks();
            return Ok(result);
        }

        /// <summary>
        /// Set book in PostgreSQL
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SetPostgresBook([FromBody] Book book)
        {
            await _postgresRepository.SetBook(book);
            return Ok();
        }
    }
}
