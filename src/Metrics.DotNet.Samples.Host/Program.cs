using System;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Health;

namespace Metrics.DotNet.Samples.Host
{
    public class Program
    {
        private static string _environmentName;

        public static void Main(string[] args)
        {
            var webHost = CreateWebHost(args);

            var configiration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{_environmentName}.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configiration)
                .Enrich.FromLogContext()
                .CreateLogger();

            webHost.Run();
        }

        public static IWebHost CreateWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .ConfigureLogging((hostingContext, config) =>
             {
                 config.ClearProviders();
                 _environmentName = hostingContext.HostingEnvironment.EnvironmentName;
             })
             .ConfigureMetricsWithDefaults(x =>
             {
                 x.Report.ToInfluxDb(options =>
                 {
                     options.InfluxDb.BaseUri = new Uri("http://localhost:8086");
                     options.InfluxDb.Database = "telegraf";
                     options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                 });

             })
             //.ConfigureHealthWithDefaults(
             //    (context, builder) =>
             //    {
             //        builder.OutputHealth.AsPlainText()
             //            .HealthChecks.AddCheck("check 1", () => new ValueTask<App.Metrics.Health.HealthCheckResult>(App.Metrics.Health.HealthCheckResult.Healthy()))
             //            .Report.ToMetrics();
             //    })
             .UseHealth()
            .UseStartup<Startup>()
            .UseSerilog()
            .UseMetrics()
            .UseMetricsEndpoints()
            .UseMetricsWebTracking()
            .Build();
    }
}
