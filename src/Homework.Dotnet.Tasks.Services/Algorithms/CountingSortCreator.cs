using BenchmarkDotNet.Attributes;

namespace Homework.Dotnet.Tasks.Services.Algorithms
{
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    public class CountingSortCreator
    {
        /// <summary>
        /// Results:
        ///            Method |    Count |      Mean |     Error |    StdDev |
        /// ----------------- |--------- |----------:|----------:|----------:|
        ///  TestCountingSort | 100000   | 2.427 ms  | 0.0050 ms | 0.0042 ms |
        ///  TestCountingSort | 500000   | 11.732 ms | 0.0448 ms | 0.0419 ms |
        ///  TestCountingSort | 1000000  | 22.255 ms | 0.3152 ms | 0.2632 ms |
        ///  TestCountingSort | 2000000  | 47.865 ms | 0.3177 ms | 0.2972 ms |
        ///  TestCountingSort | 3000000  | 72.233 ms | 1.0432 ms | 0.9247 ms |
        ///  TestCountingSort | 5000000  | 119.651 ms| 0.5681 ms | 0.5036 ms |
        ///  TestCountingSort | 10000000 | 243.0 ms  | 2.66 ms   | 2.22 ms   | -> Interesting result for 10M
        /// </summary>
        [Params(100000, 500000, 1000000, 2000000, 3000000, 5000000, 10000000)]
        public static int Count { get; set; }

        [Benchmark]
        public void TestCountingSort()
        {
            int[] array = CountingSort.RandomArrayGenerator(Count);
            CountingSort.Sort(array);
        }
    }
}
