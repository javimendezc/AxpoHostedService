using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AxpoHostedService
{
    internal class DummyWorker : IWorker
    {
        private readonly ILogger _logger;

        public DummyWorker(ILogger<DummyWorker> logger) { 
            _logger = logger; 
        }

        public async Task DoWork()
        {
            await Task.Run(() => _logger.LogInformation($"Dummy worker running at {DateTime.Now}"));
        }
    }
}
