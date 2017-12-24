using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day10Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentPosition = 0;
            var skip = 0;
            var data = new int[256];

            for(var x = 0; x < 256; x++)
            {
                data[x] = x;
            }

            // Get the reverse lengths
            var reverseLengths = ConvertStringToAscii("157,222,1,2,177,254,0,228,159,140,249,187,255,51,76,30");
            reverseLengths.AddRange(new List<int>() { 17, 31, 73, 47, 23 });

            // Second, instead of merely running one round like you did above, run a total of 64 rounds, using the same length sequence in each round.
            // The current position and skip size should be preserved between rounds.
            // For example, if the previous example was your first round, you would start your second round with the same length sequence(3, 4, 1, 5, 17, 31, 73, 47, 23, now assuming they came from ASCII codes and include the suffix), but start with the previous round's current position (4) and skip size (4).
            for (var round = 0; round < 64; round++)
            {
                data = KnotHash(data, reverseLengths.ToArray(), ref currentPosition, ref skip);
            }

            // Your next task is to reduce these to a list of only 16 numbers called the dense hash. 
            // To do this, use numeric bitwise XOR to combine each consecutive block of 16 numbers in the sparse hash (there are 16 such blocks in a list of 256 numbers).
            var block = 0;
            var denseHash = new int[16];
            for (var ii = 0; ii < 16; ii++)
            {
                denseHash[block++] = data[0] ^ data[1] ^ data[2] ^ data[3] ^ data[4] ^ data[5] ^
                                     data[6] ^ data[7] ^ data[8] ^ data[9] ^ data[10] ^ data[11] ^
                                     data[12] ^ data[13] ^ data[14] ^ data[15];

                data = data.Skip(16).ToArray();
            }

            var answer = "";
            foreach(var hash in denseHash)
            {
                answer += hash.ToString("X2");
            }

            Console.WriteLine(answer.ToLower());

            // Old Part 1
            //Console.WriteLine($"{data[0]} * {data[1]} = {(data[0] * data[1])}");
            Console.ReadLine();
        }

        static List<int> ConvertStringToAscii(string input)
        {
            var byteArray = Encoding.ASCII.GetBytes(input);
            return byteArray.Select(i => (int)i).ToList();
        }

        static int[] KnotHash(int[] data, int[] reverseLengths, ref int cursorIndex, ref int skip)
        {
            foreach (var length in reverseLengths)
            {
                // Reverse subsection of the list
                var jj = 0;
                while (jj < length / 2)
                {
                    // get the start and end positions to swap
                    while (cursorIndex >= data.Length) { cursorIndex -= data.Length; }
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

            return data;
        }
    }
}
