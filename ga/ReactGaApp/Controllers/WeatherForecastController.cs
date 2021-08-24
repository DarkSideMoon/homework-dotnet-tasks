using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReactGaApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IGaHttpClient _gaHttpClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IGaHttpClient gaHttpClient)
        {
            _logger = logger;
            _gaHttpClient = gaHttpClient;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "v" , "1" },
                    { "tid" , "" },
                    { "cid" , "47942" },
                    { "t" , "event" },
                    { "ec" , "Get All data" },
                    { "ea" , "Read All" }, 
                    { "el" , "simpleapp" }, 
                    { "ev" , "500" },  
                    { "hl" , "en" }
                });

            var data = await _gaHttpClient.CollectDataAsync(content);
            return Ok(data);
        }

        [HttpPost]
        public ActionResult Post([FromQuery] int id, [FromBody] string data)
        { 
            _gaHttpClient.InsertDataAsync(id, data);
            return Ok();
        }
    }
}
