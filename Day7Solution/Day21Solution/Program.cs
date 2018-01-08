using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day21Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var startingSquare = ".#./..#/###";
            Dictionary<string, string> instructions = new Dictionary<string, string>();            
            string[] lines = File.ReadAllLines(".\\input.txt");

            // Get instructions and generate matching permutations (should be at most an additional 7)
            foreach (var line in lines)
            {
                var kvp = line.Split(" => ");
                instructions.Add(kvp[0], kvp[1]);

                // Rotate 3 times and add to the instructions
                var alteredInput = kvp[0];
                for(var ii = 1; ii < 3; ii++)
                {
                    alteredInput = Rotate(alteredInput);
                    AddInstruction(ref instructions, alteredInput, kvp[1]);
                }

                // Flip and rotate 3 times and add to the instructions
                alteredInput = Flip(kvp[0]);
                instructions.Add(alteredInput, kvp[1]);
                for (var ii = 1; ii < 3; ii++)
                {
                    alteredInput = Rotate(alteredInput);
                    AddInstruction(ref instructions, alteredInput, kvp[1]);
                }
            }

            var newSquare = startingSquare;
            for (var ii = 0; ii < 1; ii++)
            {
                var newSquareRows = newSquare.Split('/');
                var squareSize = newSquareRows.Length;

                if (squareSize <= 3)
                {
                    newSquare = instructions[startingSquare];
                }
                else 
                {
                    var totalSquares = 0;

                    if (squareSize % 3 == 0)
                    {

                    }
                    else
                    {
                        totalSquares = Convert.ToInt32(Math.Pow(squareSize / 2, 2));
                        var splitSquares = new string[totalSquares];                // holds the mini-squares

                        // Find the pattern and repeat (below is example of size 4 square)
                        var currentRow = 0;
                        splitSquares[0] = newSquareRows[currentRow].Substring(0, 2) + "/" + newSquareRows[currentRow + 1].Substring(0, 2);
                        splitSquares[1] = newSquareRows[currentRow].Substring(2, 2) + "/" + newSquareRows[currentRow + 1].Substring(2, 2);
                        currentRow += 2;
                        splitSquares[2] = newSquareRows[currentRow].Substring(0, 2) + "/" + newSquareRows[currentRow + 1].Substring(0, 2);
                        splitSquares[3] = newSquareRows[currentRow].Substring(2, 2) + "/" + newSquareRows[currentRow + 1].Substring(2, 2);

                    }
                }
            }

            Console.WriteLine("There are " + newSquare.Count(i => i == '#') + " squares that are turned on");
            Console.WriteLine("Press enter to end program...");
            Console.ReadLine();
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
                retVal = charRows[1, 0] + charRows[0, 0] + "/" + charRows[1, 1] + charRows[0, 1];
            }
            else
            {
                retVal = charRows[2, 0] + charRows[1, 0] + charRows[0, 0] + "/" + 
                         charRows[2, 1] + charRows[1, 1] + charRows[0, 1] + "/" +
                         charRows[2, 2] + charRows[1, 2] + charRows[0, 2];
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
