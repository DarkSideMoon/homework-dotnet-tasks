using System;
using System.Linq;

namespace Homework.Dotnet.Tasks.Services.Algorithms
{
    /// <summary>
    /// https://www.geeksforgeeks.org/counting-sort/
    /// https://exceptionnotfound.net/counting-sort-csharp-the-sorting-algorithm-family-reunion/
    /// </summary>
    public static class CountingSort
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
        public static void Sort(int[] array)
        {
            int max = array.Max();
            int min = array.Min();
            int range = max - min + 1;
            
            int[] count = new int[range];
            int[] output = new int[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                count[array[i] - min]++;
            }

            for (int i = 1; i < count.Length; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = array.Length - 1; i >= 0; i--)
            {
                output[count[array[i] - min] - 1] = array[i];
                count[array[i] - min]--;
            }

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = output[i];
            }
        }

        public static int[] RandomArrayGenerator(int length)
        {
            int[] array = new int[length];

            var randNum = new Random();
            for (int i = 0; i < array.Length; i++)
                array[i] = randNum.Next(0, 1000);

            return array;
        }

        public static void ConsoleWriteLineArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
                Console.Write($"{array[i]} ");

            Console.WriteLine();
        }
    }
}
