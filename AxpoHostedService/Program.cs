using Axpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AxpoHostedService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                    configBuilder.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IPowerService, PowerService>();
                    services.AddTransient<IWorker, Worker>();
                    services.AddTransient<IReportGen>((serviceProvider) =>
                    {
                        var pathCsv = serviceProvider.GetRequiredService<IConfiguration>().GetSection("path_csv").Value;
                        ILogger<ReportGen> logger = serviceProvider.GetRequiredService<ILogger<ReportGen>>();
                        return new ReportGen(logger, pathCsv);
                    });
                    services.AddTransient<IHostedService>((serviceProvider) =>
                    {
                        var interval = Int32.Parse(serviceProvider.GetRequiredService<IConfiguration>().GetSection("interval").Value);
                        var worker = serviceProvider.GetRequiredService<IWorker>();
                        return new TimedHostedService(worker, interval);
                    });
                })
                .UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
                                .ReadFrom.Configuration(hostingContext.Configuration)
                                .WriteTo.Console(LogEventLevel.Information));

            await hostBuilder.RunConsoleAsync();
        }
    }
}
