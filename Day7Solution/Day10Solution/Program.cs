using System;
using System.Collections.Generic;

namespace Day10Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var cursorIndex = 0;
            var skip = 0;
            var data = new int[256];

            for(var x = 0; x < 256; x++)
            {
                data[x] = x;
            }

            var reverseLengths = new int[] { 157, 222, 1, 2, 177, 254, 0, 228, 159, 140, 249, 187, 255, 51, 76, 30 };

            foreach (var length in reverseLengths)
            {
                // Reverse subsection of the list
                var jj = 0;
                while (jj < length / 2 )
                {
                    // get the start and end positions to swap
                    var startIndex = cursorIndex + jj;
                    var endIndex = cursorIndex + length - 1 - jj;
                    if (startIndex >= data.Length) startIndex -= data.Length;
                    if (endIndex >= data.Length) endIndex -= data.Length;

                    // store the beginning value
                    var tempHolder = data[startIndex];

                    // swap
                    data[startIndex] = data[endIndex];
                    data[endIndex] = tempHolder;
                    jj++;
                }

                // move the cursor and increase skip size by one
                cursorIndex += length + skip++;
                if (cursorIndex >= data.Length) cursorIndex -= data.Length;
            }

            //Console.WriteLine(string.Join(",", data));
            Console.WriteLine($"{data[0]} * {data[1]} = {(data[0] * data[1])}");
            Console.ReadLine();
        }
    }
}
