using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

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
            .UseStartup<Startup>()
            .UseSerilog()
            .Build();
    }
}
