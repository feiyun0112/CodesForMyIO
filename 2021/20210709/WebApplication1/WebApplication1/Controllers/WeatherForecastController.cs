using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            //温度大于20， Summary必须以C开头
            var filter = @"p => p.TemperatureC > 20 && p.Summary.StartsWith(""C"")";

            //由于使用了String.StartsWith，所以要引用String所在的Assembly
            var options = ScriptOptions.Default.AddReferences(typeof(WeatherForecast).Assembly,
                typeof(String).Assembly);

            Func<WeatherForecast, bool> expression = await CSharpScript.EvaluateAsync<Func<WeatherForecast, bool>>(filter, options);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).AsQueryable().Where(expression)
            .ToArray();
        }
    }
}
