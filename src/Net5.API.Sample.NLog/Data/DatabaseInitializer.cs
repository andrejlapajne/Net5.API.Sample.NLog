using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Net5.API.Sample.Model;

namespace Net5.API.Sample.Data
{
    public static class DatabaseInitializer
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public static void Initialize(SampleDbContext context, ILogger logger)
        {
            context.Database.EnsureCreated();

            if (context.WeatherForecasts.Any())
            {
                logger.LogInformation("Database is already initialized.");
                return;
            }

            var random = new Random();
            var count = random.Next(10, 20);
            logger.LogInformation($"{count} {nameof(WeatherForecast)} records will be generated and stored in the database.");
            context.WeatherForecasts.AddRange(Enumerable.Range(1, count).Select(x => CreateNewWeatherForecast(random)));
            context.SaveChanges();
        }

        private static WeatherForecast CreateNewWeatherForecast(Random random)
        {
            return new WeatherForecast
            {
                Date = DateTime.Now.AddDays(random.Next(-100, 0)),
                TemperatureC = random.Next(-20, 55),
                Summary = Summaries[random.Next(Summaries.Length)]
            };
        }
    }
}