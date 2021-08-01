using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherForecastController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var client = _httpClientFactory.CreateClient("Weather");
            string result = await client.GetStringAsync("data/sk/101200101.html");
            return result;
        }
    }
}
