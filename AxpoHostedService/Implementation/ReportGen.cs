using Axpo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxpoHostedService
{
    public class ReportGen : IReportGen
    {
        private const int NUMBER_PERIODS = 24;
        private int[] arrayHours = new int[] { 23, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22};
        private string templateLine = "{0}:00;{1}\n"; 


        private readonly ILogger<ReportGen> _logger;
        private string _pathCsv = Directory.GetCurrentDirectory();

        public ReportGen(ILogger<ReportGen> logger, string pathCsv) { 
            _logger = logger;
            _pathCsv = pathCsv;

            if (!Directory.Exists(pathCsv)) {
                Directory.CreateDirectory(pathCsv);
            }
        }

        public void SaveReport(IEnumerable<PowerTrade> powerTrades, DateTime moment)
        {
            _logger.LogInformation($"SaveReport at {moment}");

            string fileContent = "Local Time;Volume\n";

            DateTime dateTime = powerTrades.Select(x => x.Date).FirstOrDefault();
            List<PowerPeriod[]> powerPeriods = powerTrades.Select(x => x.Periods).ToList();
            for (int i = 0; i < NUMBER_PERIODS; i++)
            {
                var sum = powerPeriods.Select(x => x[i].Volume).Sum();
                string line = string.Format(templateLine, arrayHours[i].ToString("00"), sum);
                fileContent += line;
            }

            File.AppendAllText($"{_pathCsv}\\PowerPosition_{moment.Year}{moment.Month.ToString("00")}{moment.Day.ToString("00")}_{moment.Hour.ToString("00")}{moment.Minute.ToString("00")}{moment.Second.ToString("00")}.csv", fileContent);
        }
    }
}
