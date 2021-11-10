using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Net5.API.Sample.Data;
using Net5.API.Sample.Model;

namespace Net5.API.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogEntryController : ControllerBase
    {
        private readonly SampleDbContext dbContext;
        private readonly ILogger<LogEntryController> logger;

        public LogEntryController(ILogger<LogEntryController> logger, SampleDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<LogEntry>> Get()
        {
            try
            {
                return await dbContext.LogEntries.OrderByDescending(x => x.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred in '{nameof(LogEntryController)}/{nameof(Get)}([GET])'");
            }

            return new LogEntry[] { };
        }
    }
}
