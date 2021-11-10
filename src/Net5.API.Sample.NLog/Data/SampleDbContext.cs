using Microsoft.EntityFrameworkCore;
using Net5.API.Sample.Model;

namespace Net5.API.Sample.Data
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }

        public DbSet<LogEntry> LogEntries { get; set; }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}