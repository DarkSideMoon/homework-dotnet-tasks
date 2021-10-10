using System;
using System.Collections.Generic;
using System.Linq;

namespace Homework.Dotnet.Tasks.Services.Algorithms
{
    /// <summary>
    /// https://www.geeksforgeeks.org/counting-sort/
    /// </summary>
    public static class CountingSort
    {
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
