using System;
using Homework.Dotnet.Tasks.Services.Algorithms;
using NUnit.Framework;
using System.Diagnostics;

namespace Homework.Dotnet.Tasks.Unit.Tests
{
    public class CountingSortTestCases
    {
        public static int[] CountingSortCases =
        {
            1000
        };
    }

    [TestFixture]
    public class CountingSortTests
    {
        private Stopwatch _stopwatch;

        [SetUp]
        public void Setup()
        {
            _stopwatch = new Stopwatch();
        }

        [Test]
        [TestCaseSource(typeof(CountingSortTestCases), nameof(CountingSortTestCases.CountingSortCases))]
        public void TestCountingSort(int count)
        {
            int[] array = CountingSort.RandomArrayGenerator(count);

            // Write line initial array
            CountingSort.ConsoleWriteLineArray(array);

            _stopwatch.Start();
            // Sort
            CountingSort.Sort(array);

            _stopwatch.Stop();

            // Write line sorting array
            CountingSort.ConsoleWriteLineArray(array);
            Console.WriteLine($"Count elements: {count}. Time elapsed: {_stopwatch.Elapsed}");
        }
    }
}
