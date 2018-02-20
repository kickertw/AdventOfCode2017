using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;

namespace Day22Solution
{
    class Program
    {
        static int matrixSize = 3;
        static int infectionCount = 0;
        static Direction facing = Direction.Up;
        static Point currentPosition = new Point(12,12);
        static List<StringBuilder> matrix = new List<StringBuilder>();

        static void Main(string[] args)
        {
            const int totalMoves = 10000000;

            string[] lines = File.ReadAllLines(".\\input.txt");

            foreach (var line in lines)
            {
                matrix.Add(new StringBuilder(line));
            }

            for (var ii = 0; ii < totalMoves; ii++)
            {
                //Console.Clear();
                DoWork(false);

                // Debug only
                //foreach (var row in matrix)
                //{
                //    Console.WriteLine(row.ToString());
                //}
                //Console.WriteLine(ii + " " + output);
                //Console.ReadLine();
            }

            // Part 1/2
            Console.WriteLine(infectionCount);
            Console.ReadLine();
        }

        static void ExpandMatrix()
        {
            var matrixWidth = matrix[0].Length;

            foreach (var t in matrix)
            {
                t.Insert(0, ".");
                t.Append(".");
            }

            matrix.Insert(0, new StringBuilder(".".PadLeft(matrixWidth + 2, '.')));
            matrix.Add(new StringBuilder(".".PadLeft(matrixWidth + 2, '.')));

            matrixSize += 2;
            currentPosition.X++;
            currentPosition.Y++;
        }

        static void DoWork(bool isPartOne = true)
        {
            //var debug = "";
            var currentRow = matrix[currentPosition.X];

            if (isPartOne)
            {
                // 1. Turn (infected = right, clean = left)
                facing = Turn(currentRow[currentPosition.Y] == '#' ? Direction.Right : Direction.Left);

                // 2. infect/clean the node
                if (currentRow[currentPosition.Y] == '#')
                {
                    currentRow[currentPosition.Y] = '.';
                }
                else
                {
                    infectionCount++;
                    currentRow[currentPosition.Y] = '#';
                }

                // 3. Move
                Move();
            }
            else
            {
                //debug += $"Current Node [{currentRow[currentPosition.Y]}] - ";

                // 1. Turn (clean = left/infected = right/flagged = reverse)
                if (currentRow[currentPosition.Y] == '#')
                {
                    facing = Turn(Direction.Right);
                }
                else if (currentRow[currentPosition.Y] == '.')
                {
                    facing = Turn(Direction.Left);
                }
                else if (currentRow[currentPosition.Y] == 'F')
                {
                    facing = Turn(Direction.Reverse);
                }

                //debug += $" Facing [{facing.ToString()}] - ";

                // 2. infect/clean the node
                if (currentRow[currentPosition.Y] == '.')
                {
                    currentRow[currentPosition.Y] = 'W';
                }
                else if (currentRow[currentPosition.Y] == 'W')
                {
                    infectionCount++;
                    currentRow[currentPosition.Y] = '#';
                }
                else if (currentRow[currentPosition.Y] == '#')
                {
                    currentRow[currentPosition.Y] = 'F';
                }
                else
                {
                    currentRow[currentPosition.Y] = '.';
                }

                //debug += $"Node changed to [{currentRow[currentPosition.Y]}] - ";

                // 3. Move
                Move();

                //debug += $"Moved to Row [{currentPosition.X}] + Col [{currentPosition.Y}]";
            }

            //return debug;
        }

        static Direction Turn(Direction nextTurn)
        {
            if (nextTurn == Direction.Reverse)
            {
                switch (facing)
                {
                    case Direction.Left:
                        return Direction.Right;
                    case Direction.Right:
                        return Direction.Left;
                    case Direction.Up:
                        return Direction.Down;
                    case Direction.Down:
                        return Direction.Up;
                }
            }

            switch (facing)
            {
                case Direction.Up:
                    return nextTurn == Direction.Right ? Direction.Right : Direction.Left;
                case Direction.Right:
                    return nextTurn == Direction.Right ? Direction.Down : Direction.Up;
                case Direction.Down:
                    return nextTurn == Direction.Right ? Direction.Left : Direction.Right;
                case Direction.Left:
                    return nextTurn == Direction.Right ? Direction.Up : Direction.Down;
            }

            throw new Exception("wtf...");
        }

        static void Move()
        {
            switch (facing)
            {
                case Direction.Up:
                    currentPosition.X--;
                    break;
                case Direction.Right:
                    currentPosition.Y++;
                    break;
                case Direction.Down:
                    currentPosition.X++;
                    break;
                case Direction.Left:
                    currentPosition.Y--;
                    break;
            }

            if (currentPosition.X < 0 || currentPosition.X >= matrixSize || currentPosition.Y < 0 ||
                currentPosition.Y >= matrixSize)
            {
                ExpandMatrix();
            }
        }
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        Reverse
    }
}
