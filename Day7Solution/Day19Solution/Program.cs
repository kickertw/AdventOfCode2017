using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day19Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(".\\input.txt");
            var maze = new string[1000, 1000];
            var currentPoint = new Point();
            var maxWidth = lines[0].Length;
            var maxHeight = lines.Length;

            // Map the "maze"
            for(var ii=0; ii < lines.Length; ii++)
            {
                var input = lines[ii].ToCharArray();
                for(var jj=0; jj < input.Length; jj++)
                {
                    if (ii == 0 && input[jj] == '|') { currentPoint.X = jj; currentPoint.Y = 0; }
                    maze[jj,ii] = input[jj].ToString();
                }
            }

            // Traverse the "maze"
            int numberOfSteps = TraverseMaze(currentPoint, maze, maxWidth, maxHeight);
            Console.WriteLine($"Number Of Steps = {numberOfSteps}");
            Console.ReadLine();
        }

        static int TraverseMaze(Point currentPoint, string[,] maze, int maxWidth, int maxHeight)
        {
            var numberOfSteps = 0;
            var direction = "S";
            var lettersHit = "";

            while (currentPoint.X >= 0 && currentPoint.Y >= 0 && currentPoint.X < maxWidth && currentPoint.Y < maxHeight)
            {
                var pathInput = maze[currentPoint.X, currentPoint.Y];
                //Console.WriteLine(currentPoint.X + "," + currentPoint.Y + " - " + pathInput);

                if (pathInput.Trim().Length == 0)
                {
                    break;
                }
                else if (pathInput == "+")
                {
                    // Find the next direction to move to
                    if (direction == "S" || direction == "N")
                    {
                        // look left or right
                        if (currentPoint.X > 0 && maze[currentPoint.X - 1, currentPoint.Y].Trim().Length > 0) { direction = "W"; currentPoint.X--; }
                        else { direction = "E"; currentPoint.X++; }
                    }
                    else
                    {
                        // look N/S
                        if (currentPoint.Y < maxHeight - 1 && maze[currentPoint.X, currentPoint.Y + 1].Trim().Length > 0) { direction = "S"; currentPoint.Y++; }
                        else { direction = "N"; currentPoint.Y--; }
                    }
                }                
                else if (pathInput != "|" && pathInput != "-")
                {
                    lettersHit += pathInput;
                    MoveThroughMaze(ref currentPoint, direction);
                }
                else
                {
                    MoveThroughMaze(ref currentPoint, direction);
                }

                numberOfSteps++;
            }

            Console.WriteLine(lettersHit);
            return numberOfSteps;
        }

        static void MoveThroughMaze(ref Point currentPoint, string direction)
        {
            if (direction == "N") { currentPoint.Y--; }
            else if (direction == "S") { currentPoint.Y++; }
            else if (direction == "E") { currentPoint.X++; }
            else { currentPoint.X--; }
        }
    }
}
