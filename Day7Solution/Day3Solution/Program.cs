using System;
using System.Collections.Generic;
using System.Drawing;

namespace Day3Solution
{
    public enum DirectionEnum
    {
        right,
        up,
        left,
        down
    }

    class Program
    {
        static void Main(string[] args)
        {
            int sum = 1;
            int puzzleInput = 277678;

            var currentStepCounter = 0;
            var maxStepChange = 1;
            var direction = DirectionEnum.right;

            Point currentLocation = new Point(0, 0);
            var systemMatrix = new Dictionary<Point, int>();

            while (sum < puzzleInput)
            {
                // Find adjacent cells and get the sum to store
                if (currentLocation.X != 0 || currentLocation.Y != 0)
                {
                    sum = GetSumOfSurrounding(systemMatrix, currentLocation);
                }

                // Keep track of the sum
                Console.WriteLine($"({currentLocation.X},{currentLocation.Y}) = {sum}");
                systemMatrix.Add(currentLocation, sum);

                // Move the location
                switch (direction)
                {
                    case DirectionEnum.up:
                        currentLocation = new Point(currentLocation.X, currentLocation.Y+1);
                        break;
                    case DirectionEnum.left:
                        currentLocation = new Point(currentLocation.X - 1, currentLocation.Y);
                        break;
                    case DirectionEnum.down:
                        currentLocation = new Point(currentLocation.X, currentLocation.Y - 1);
                        break;
                    default:
                        currentLocation = new Point(currentLocation.X + 1, currentLocation.Y);
                        break;
                }

                // Check to see if we need to change direction
                currentStepCounter++;
                if (currentStepCounter == maxStepChange)
                {
                    currentStepCounter = 0;
                    switch (direction)
                    {
                        case DirectionEnum.up:
                            direction = DirectionEnum.left;
                            maxStepChange++;
                            break;
                        case DirectionEnum.left:
                            direction = DirectionEnum.down;
                            break;
                        case DirectionEnum.down:
                            maxStepChange++;
                            direction = DirectionEnum.right;
                            break;
                        default:
                            direction = DirectionEnum.up;
                            break;
                    }
                }

            }

            Console.ReadLine();
        }

        static int GetSumOfSurrounding(Dictionary<Point, int> systemMatrix, Point currentLocation)
        {
            var sum = 0;

            // top
            if (systemMatrix.ContainsKey(new Point(currentLocation.X, currentLocation.Y + 1)))
            {
                //Console.Write(systemMatrix[new Point(currentLocation.X, currentLocation.Y + 1)] + "+");
                sum += systemMatrix[new Point(currentLocation.X, currentLocation.Y + 1)];
            }

            // top right
            if (systemMatrix.ContainsKey(new Point(currentLocation.X + 1, currentLocation.Y + 1)))
            {
                //Console.Write(systemMatrix[new Point(currentLocation.X + 1, currentLocation.Y + 1)] + "+");
                sum += systemMatrix[new Point(currentLocation.X + 1, currentLocation.Y + 1)];
            }

            // right
            if (systemMatrix.ContainsKey(new Point(currentLocation.X + 1, currentLocation.Y)))
            {
                //Console.Write(systemMatrix[new Point(currentLocation.X + 1, currentLocation.Y)] + "+");
                sum += systemMatrix[new Point(currentLocation.X + 1, currentLocation.Y)];
            }

            // bot right
            if (systemMatrix.ContainsKey(new Point(currentLocation.X + 1, currentLocation.Y - 1)))
            {
                //Console.Write(systemMatrix[new Point(currentLocation.X + 1, currentLocation.Y - 1)] + "+");
                sum += systemMatrix[new Point(currentLocation.X + 1, currentLocation.Y - 1)];
            }

            // bot
            if (systemMatrix.ContainsKey(new Point(currentLocation.X, currentLocation.Y - 1)))
            {
                //Console.Write(systemMatrix[new Point(currentLocation.X, currentLocation.Y - 1)] + "+");
                sum += systemMatrix[new Point(currentLocation.X, currentLocation.Y - 1)];
            }

            // bot left
            if (systemMatrix.ContainsKey(new Point(currentLocation.X - 1, currentLocation.Y - 1)))
            {
                //Console.Write(systemMatrix[new Point(currentLocation.X - 1, currentLocation.Y - 1)] + "+");
                sum += systemMatrix[new Point(currentLocation.X - 1, currentLocation.Y - 1)];
            }

            // left
            if (systemMatrix.ContainsKey(new Point(currentLocation.X - 1, currentLocation.Y)))
            {
                //Console.Write(systemMatrix[new Point(currentLocation.X - 1, currentLocation.Y)] + "+");
                sum += systemMatrix[new Point(currentLocation.X - 1, currentLocation.Y)];
            }

            // top left
            if (systemMatrix.ContainsKey(new Point(currentLocation.X - 1, currentLocation.Y + 1)))
            {
                //Console.Write(systemMatrix[new Point(currentLocation.X - 1, currentLocation.Y + 1)] + "+");
                sum += systemMatrix[new Point(currentLocation.X - 1, currentLocation.Y + 1)];
            }
            //Console.WriteLine();

            return sum;
        }
    }
}
