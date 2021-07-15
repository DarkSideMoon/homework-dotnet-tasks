using Metrics.DotNet.Samples.Services;
using Metrics.DotNet.Samples.Services.Settings;
using Microsoft.Extensions.Options;
using System;

namespace Metrics.DotNet.Samples.DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generate random data!");

            var settings = Options.Create(new ElasticSearchSetting
            {
                IndexName = "book",
                Uri = "http://localhost:9200"
            });
            var elasticClient = new ElasticSearchBookClient(settings);
            for (var i = 0; i < 10; i++)
            {
                var elasticData = FakeData.BookDocument.Generate(1000);
                elasticClient.BulkUpdate(elasticData);
            }


            Console.ReadLine();
        }
    }
}
