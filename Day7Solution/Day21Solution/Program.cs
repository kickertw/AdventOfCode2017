using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day21Solution
{
    class Program
    {
        static Dictionary<string, string> instructions = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            var startingSquare = ".#./..#/###";      
            string[] lines = File.ReadAllLines(".\\input.txt");

            // Get instructions and generate matching permutations (should be at most an additional 7)
            foreach (var line in lines)
            {
                var kvp = line.Split(" => ");
                instructions.Add(kvp[0], kvp[1]);

                // Rotate 3 times and add to the instructions
                var alteredInput = kvp[0];
                for(var ii = 1; ii < 4; ii++)
                {
                    alteredInput = Rotate(alteredInput);
                    AddInstruction(ref instructions, alteredInput, kvp[1]);
                }

                // Flip and rotate 3 times and add to the instructions
                alteredInput = Flip(kvp[0]);
                AddInstruction(ref instructions, alteredInput, kvp[1]);
                for (var ii = 1; ii < 4; ii++)
                {
                    alteredInput = Rotate(alteredInput);
                    AddInstruction(ref instructions, alteredInput, kvp[1]);
                }
            }

            var nextSquare = startingSquare;
            for (int ii = 0; ii < 5; ii++)
            {
                nextSquare = GrowSquare(nextSquare);

                Console.WriteLine("Showing square of size " + nextSquare.Split('/')[0].Length);
                foreach (var row in nextSquare.Split('/'))
                {
                    Console.WriteLine(row);
                }
                Console.WriteLine();
            }

            Console.WriteLine($"There are {nextSquare.Count(i => i == '#')} pixels turned on");
            Console.WriteLine("Press enter to end program...");
            Console.ReadLine();
        }

        static string GrowSquare(string startingSquare)
        {
            var rows = startingSquare.Split('/');
            if (rows.Length <= 3)
            {
                return instructions[startingSquare];
            }
            else
            {
                // Breaking up and reassembling a square 2 rows at a time
                if (rows.Length % 2 == 0)
                {
                    var newSquareIndex = 0;
                    var totalSquares = Convert.ToInt32(Math.Pow(rows.Length / 2, 2));
                    var newSquare = new string[totalSquares];

                    // Break up the larger square into smaller ones.  Then grow each smaller square
                    for (var ii = 0; ii < rows.Length; ii += 2)
                    {
                        var firstRowSplit = Split(rows[ii], 2);
                        var secondRowSplit = Split(rows[ii + 1], 2);

                        for (var jj = 0; jj < rows.Length / 2; jj++)
                        {
                            newSquare[newSquareIndex++] = GrowSquare(firstRowSplit[jj] + "/" + secondRowSplit[jj]);
                        }
                    }

                    // Reassemble each smaller square into one mega square
                    // in single string notation
                    var lastBlock = 0;
                    StringBuilder newSquareStringFinal = new StringBuilder();
                    StringBuilder newSquareString1 = new StringBuilder();
                    StringBuilder newSquareString2 = new StringBuilder();
                    StringBuilder newSquareString3 = new StringBuilder();

                    for (var ii = 0; ii < rows.Length / 2; ii++)
                    {
                        newSquareString1.Clear();
                        newSquareString2.Clear();
                        newSquareString3.Clear();
                        for (var jj = 0; jj < rows.Length / 2; jj++)
                        {
                            var splitNS = newSquare[lastBlock + jj].Split('/');
                            newSquareString1.Append(splitNS[0]);
                            newSquareString2.Append(splitNS[1]);
                            newSquareString3.Append(splitNS[2]);
                        }
                        lastBlock += rows.Length / 2;

                        newSquareStringFinal.Append(newSquareString1).Append("/").Append(newSquareString2).Append("/").Append(newSquareString2);
                        if (ii < rows.Length / 2 - 1) { newSquareStringFinal.Append("/"); }
                    }

                    return newSquareStringFinal.ToString();
                }
                else // if (rows.Length % 3 == 0)
                {
                    var newSquareIndex = 0;
                    var totalSquares = Convert.ToInt32(Math.Pow(rows.Length / 3, 2));
                    var newSquare = new string[totalSquares];

                    // Break up the larger square into smaller ones.  Then grow each smaller square
                    for (var ii = 0; ii < rows.Length / 3; ii++)
                    {
                        var firstRowSplit = Split(rows[ii], 3);
                        var secondRowSplit = Split(rows[ii + 1], 3);
                        var thirdRowSplit = Split(rows[ii + 2], 3);

                        for (var jj = 0; jj < rows.Length / 3; jj++)
                        {
                            newSquare[newSquareIndex++] = GrowSquare(firstRowSplit[jj] + "/" + secondRowSplit[jj] + "/" + thirdRowSplit[jj]);
                        }
                    }

                    // Reassemble each smaller square into one mega square
                    // in single string notation
                    var lastBlock = 0;
                    StringBuilder newSquareStringFinal = new StringBuilder();
                    StringBuilder newSquareString1 = new StringBuilder();
                    StringBuilder newSquareString2 = new StringBuilder();
                    StringBuilder newSquareString3 = new StringBuilder();
                    StringBuilder newSquareString4 = new StringBuilder();

                    for (var ii = 0; ii < rows.Length / 3; ii++)
                    {
                        newSquareString1.Clear();
                        newSquareString2.Clear();
                        newSquareString3.Clear();
                        newSquareString4.Clear();
                        for (var jj = 0; jj < rows.Length / 3; jj++)
                        {
                            var splitNS = newSquare[lastBlock + jj].Split('/');
                            newSquareString1.Append(splitNS[0]);
                            newSquareString2.Append(splitNS[1]);
                            newSquareString3.Append(splitNS[2]);
                            newSquareString4.Append(splitNS[2]);
                        }
                        lastBlock += rows.Length / 3;

                        newSquareStringFinal.Append(newSquareString1).Append("/").Append(newSquareString2).Append("/").Append(newSquareString3).Append("/").Append(newSquareString4);
                        if (ii < rows.Length / 3 - 1) { newSquareStringFinal.Append("/"); }
                    }

                    return newSquareStringFinal.ToString();
                }
            }
        }

        static string[] Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize))
                .ToArray();
        }

        static void AddInstruction(ref Dictionary<string,string> instructions, string key, string val)
        {
            if (!instructions.ContainsKey(key))
            {
                instructions.Add(key, val);
            }
        }

        static string Rotate(string input)
        {
            var retVal = "";
            var charRows = new char[3, 3];
            retVal = input;

            var rows = retVal.Split('/');
            var arraySize = rows[0].Length;

            // Create the Array
            for (var jj = 0; jj < arraySize; jj++)
            {
                var tempArr = rows[jj].ToCharArray();
                for (var kk = 0; kk < arraySize; kk++)
                {
                    charRows[jj, kk] = tempArr[kk];
                }                
            }

            // Add Key
            if (arraySize == 2)
            {
                retVal = charRows[1, 0].ToString() + charRows[0, 0].ToString() + "/" + charRows[1, 1].ToString() + charRows[0, 1].ToString();
            }
            else
            {
                retVal = charRows[2, 0].ToString() + charRows[1, 0].ToString() + charRows[0, 0].ToString() + "/" + 
                         charRows[2, 1].ToString() + charRows[1, 1].ToString() + charRows[0, 1].ToString() + "/" +
                         charRows[2, 2].ToString() + charRows[1, 2].ToString() + charRows[0, 2].ToString();
            }

            // Rotate
            charRows = RotateClockwise(charRows, arraySize);

            return retVal;
        }

        static string Flip(string input)
        {
            var rows = input.Split('/');
            
            if (rows[0].Length == 2)
            {
                return rows[1] + "/" + rows[0];
            }

            return rows[2] + "/" + rows[1] + "/" + rows[0];
        }
        
        static char[,] RotateClockwise(char[,] old, int arraySize)
        {
            var retVal = new char[3, 3];

            // Add Key
            if (arraySize == 2)
            {
                retVal[0, 0] = old[1, 0];
                retVal[0, 1] = old[0, 0];
                retVal[1, 0] = old[1, 1];
                retVal[1, 1] = old[0, 1];
            }
            else
            {
                retVal[0, 0] = old[2, 0];
                retVal[0, 1] = old[1, 0];
                retVal[0, 2] = old[0, 0];
                retVal[1, 0] = old[2, 1];
                retVal[1, 1] = old[1, 1];
                retVal[1, 2] = old[0, 1];
                retVal[2, 0] = old[2, 2];
                retVal[2, 1] = old[1, 2];
                retVal[2, 2] = old[0, 2];
            }

            return retVal;
        }
    }
}
