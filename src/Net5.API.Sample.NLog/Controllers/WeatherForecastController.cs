using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Net5.API.Sample.Data;
using Net5.API.Sample.Model;

namespace Net5.API.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> logger;
        private readonly SampleDbContext dbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, SampleDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            try
            {
                return await dbContext.WeatherForecasts.OrderByDescending(x => x.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred in '{nameof(WeatherForecastController)}/{nameof(Get)}([GET])'");
            }

            return new WeatherForecast[] { };
        }
    }
}
