using System;

namespace Metrics.DotNet.Samples.DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generate random data!");

            var elasticData = FakeData.BookDocument.Generate(100000);

            Console.ReadLine();
        }
    }
}
