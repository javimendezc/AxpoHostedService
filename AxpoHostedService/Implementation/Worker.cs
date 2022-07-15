using Axpo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AxpoHostedService
{
    public class Worker : IWorker
    {
        private readonly ILogger _logger;
        private readonly IReportGen _reportGen;
        private readonly IPowerService _powerService;

        public Worker(ILogger<Worker> logger, IReportGen reportGen, IPowerService powerService)
        {
            _logger = logger;
            _reportGen = reportGen;
            _powerService = powerService;
        }

        public async Task DoWork()
        {
            bool exceptionHappened = false;
            IEnumerable<PowerTrade> powerTrades = null;
            DateTime moment = DateTime.Now;

            _logger.LogInformation($"The Elapsed event was raised at {moment}");

            do
            {
                try
                {
                    _logger.LogInformation($"Trying to get trades at {moment}");

                    powerTrades = await _powerService.GetTradesAsync(moment);
                    if (powerTrades.Any())
                    {
                        _logger.LogInformation($"Generating report for {powerTrades.ToList().Count} PowerTrades at {moment}");
                        _logger.LogDebug(JsonSerializer.Serialize(powerTrades));
                        _reportGen.SaveReport(powerTrades, moment);
                    }
                    exceptionHappened = false;
                }
                catch (Axpo.PowerServiceException ex)
                {
                    _logger.LogError($"Error getting trades: {ex.Message}");
                    exceptionHappened = true;
                }
            }
            while (exceptionHappened);
        }
    }
}
