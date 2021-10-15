using BenchmarkDotNet.Attributes;
// using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace Homework.Dotnet.Tasks.Services.Algorithms
{
    /// <summary>
    /// https://benchmarkdotnet.org/articles/configs/diagnosers.html
    /// </summary>
    [MemoryDiagnoser]
    // [NativeMemoryProfiler]
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    public class AvlTreeCreator
    {
        /// <summary>
        /// Results:
        /// |             Method |   Items |        Mean |     Error |    StdDev |      Gen 0 |      Gen 1 |     Gen 2 | Allocated |
        /// |------------------- |-------- |------------:|----------:|----------:|-----------:|-----------:|----------:|----------:|
        /// | CreateInt32AvlTree |  100000 |    23.25 ms |  0.292 ms |  0.259 ms |  1125.0000 |   468.7500 |  156.2500 |      6 MB |
        /// | CreateInt32AvlTree |  500000 |   134.75 ms |  1.806 ms |  1.601 ms |  5500.0000 |  2250.0000 |  500.0000 |     31 MB |
        /// | CreateInt32AvlTree | 1000000 |   241.05 ms |  1.027 ms |  0.961 ms | 10000.0000 |  3000.0000 |         - |     61 MB |
        /// | CreateInt32AvlTree | 2000000 |   513.85 ms |  1.945 ms |  1.819 ms | 20000.0000 |  7000.0000 |         - |    122 MB |
        /// | CreateInt32AvlTree | 3000000 |   959.42 ms |  6.987 ms |  6.535 ms | 31000.0000 | 11000.0000 | 1000.0000 |    183 MB |
        /// | CreateInt32AvlTree | 5000000 | 1,709.17 ms | 19.262 ms | 16.085 ms | 52000.0000 | 18000.0000 | 1000.0000 |    305 MB |
        /// </summary>
        [Params(100000, 500000, 1000000, 2000000, 3000000, 5000000)]
        public int Items { get; set; }

        [Benchmark]
        public AvlTree<int> CreateInt32AvlTree()
        {
            var avlTree = new AvlTree<int>();
            for (var i = 0; i < Items; i++)
            {
                avlTree.Add(i);
            }

            return avlTree;
        }
    }
}
