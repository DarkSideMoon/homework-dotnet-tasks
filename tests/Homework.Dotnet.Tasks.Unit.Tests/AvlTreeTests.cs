using Homework.Dotnet.Tasks.Services.Algorithms;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Homework.Dotnet.Tasks.Unit.Tests
{
    public class AvlTreeTestCases
    {
        public static int[] AvlTreeCases =
        {
            1,
            10,
            100,
            1000,
            10000,
            100000
        };
    }

    [TestFixture]
    public class AvlTreeTests
    {
        private Stopwatch _stopwatch;

        [SetUp]
        public void Setup()
        {
            _stopwatch = new Stopwatch();
        }

        [Test]
        [TestCaseSource(typeof(AvlTreeTestCases), nameof(AvlTreeTestCases.AvlTreeCases))]
        public void TestAVLTree_TimeElementsCreation_ShouldSuccess(int count)
        {
            var avlTree = new AvlTree<int>();
            _stopwatch.Start();
            for (var i = 0; i < count; i++)
            {
                avlTree.Add(i);
            }

            _stopwatch.Stop();

            Console.WriteLine($"Count elements: {count}. Time elapsed: {_stopwatch.Elapsed}");
        }
    }
}
