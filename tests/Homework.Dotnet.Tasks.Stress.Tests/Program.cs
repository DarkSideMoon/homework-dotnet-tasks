using System;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using NBomber.Plugins.Network.Ping;

namespace Homework.Dotnet.Tasks.Stress.Tests
{
    class Program
    {
        static void Main(string[] _)
        {
            Console.WriteLine("Start stress testing!");

            var step = Step.Create("Get home book step",
                clientFactory: HttpClientFactory.Create(),
                execute: context =>
                {
                    var request = Http.CreateRequest("GET", "http://localhost:5000/Home/Book");
                    return Http.Send(request, context);
                });


            var scenario = ScenarioBuilder
                .CreateScenario("Get home book", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    Simulation.InjectPerSec(rate: 10000, during: TimeSpan.FromSeconds(30))
                );

            // creates ping plugin that brings additional reporting data
            var pingPluginConfig = PingPluginConfig.CreateDefault(new[] { "http://localhost:5000" });
            var pingPlugin = new PingPlugin(pingPluginConfig);

            NBomberRunner
                .RegisterScenarios(scenario)
                .WithWorkerPlugins(pingPlugin)
                .Run();
        }
    }
}
