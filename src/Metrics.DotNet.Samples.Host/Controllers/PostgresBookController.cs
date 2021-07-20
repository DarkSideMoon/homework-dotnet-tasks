using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
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
        /// Get random book from PostgreSQL
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GeRandomBook()
        {
            var result = await _postgresRepository.GetRandomBook();
            return Ok(result);
        }

        /// <summary>
        /// Get books from PostgreSQL by count
        /// </summary>
        /// <returns></returns>
        [HttpGet("{count}")]
        public async Task<IActionResult> GetDbBooks(int count)
        {
            var result = await _postgresRepository.GetBooks(count);
            return Ok(result);
        }

        /// <summary>
        /// Get book from PostgreSQL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var result = await _postgresRepository.GetBook(id);
            return Ok(result);
        }

        /// <summary>
        /// Get all books from PostgreSQL
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDbAllBooks()
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
        public async Task<IActionResult> SetBook([FromBody] Book book)
        {
            await _postgresRepository.SetBook(book);
            return Ok();
        }
    }
}
