using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Axpo;
using Serilog.Events;

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
                    configBuilder.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                })
                //.ConfigureLogging((hostContext, configLogging) =>
                //{
                //    configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                //    configLogging.AddConsole();
                //    configLogging.AddFile("log.log");
                //})
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IPowerService, PowerService>();
                    services.AddTransient<IWorker, DummyWorker>();
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
                                .MinimumLevel.Debug()
                                .WriteTo.File("log.log")
                                .WriteTo.Console(LogEventLevel.Information));




            await hostBuilder.RunConsoleAsync();
        }
    }
}
