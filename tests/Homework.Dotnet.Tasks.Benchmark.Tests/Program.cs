using BenchmarkDotNet.Running;
using Homework.Dotnet.Tasks.Services.Algorithms;

namespace Homework.Dotnet.Tasks.Benchmark.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            // Avl Tree 
            BenchmarkRunner.Run<AvlTreeCreator>();

            // CountingSort
            // BenchmarkRunner.Run<CountingSortCreator>();
        }
    }
}
