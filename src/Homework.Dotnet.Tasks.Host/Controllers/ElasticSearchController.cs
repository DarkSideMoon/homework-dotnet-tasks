using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Services.Client;
using Microsoft.AspNetCore.Mvc;

namespace Homework.Dotnet.Tasks.Host.Controllers
{
    /// <summary>
    /// Controller to work with ElasticSearch
    /// </summary>
    [ApiController]
    [Route("search")]
    public class ElasticSearchController : ControllerBase
    {
        private readonly IElasticSearchBookClient _elasticSearchClient;

        public ElasticSearchController(IElasticSearchBookClient elasticSearchClient)
        {
            _elasticSearchClient = elasticSearchClient;
        }

        /// <summary>
        /// Search books by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpPost("{searchString}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var result = await _elasticSearchClient.Search(searchString);
            return Ok(result);
        }
    }
}
