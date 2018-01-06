using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20Solution
{
    public class Particle
    {
        public int Id { get; set; }
        public Point Location { get; set; }
        public Point Velocity { get; set; }
        public Point Acceleration { get; set; }
        public bool IsDuplicate { get; set; }

        public override bool Equals(object obj)
        {
            var b = obj as Particle;
            return this.Location.Equals(b.Location);
        }

        public override int GetHashCode()
        {
            var hash = this.Location.GetHashCode();
            return hash;
        }

        public void MoveLocation()
        {
            Location.X += Velocity.X;
            Location.Y += Velocity.Y;
            Location.Z += Velocity.Z;
        }

        public void ChangeVelocity()
        {
            Velocity.X += Acceleration.X;
            Velocity.Y += Acceleration.Y;
            Velocity.Z += Acceleration.Z;
        }

        public long ManhattenDistance()
        {
            return Math.Abs(Location.X) + Math.Abs(Location.Y) + Math.Abs(Location.Z);
        }

        public Particle(int id)
        {
            Id = id;
        }
    }

    public class Point
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }

        public Point(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object obj)
        {
            var b = obj as Point;
            return X == b.X && Y == b.Y && Z == b.Z;
        }

        public override int GetHashCode()
        {
            return $"{X}{Y}{Z}".GetHashCode();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(".\\input.txt");
            List<Particle> particles = new List<Particle>();

            var id = 0;
            // Gather Particle information
            foreach(var line in lines)
            {
                particles.Add(ParseInput(line.Trim(), id++));
            }

            var winningId = 0;
            // Determine which will be the closest to the origin
            for (var ii = 0; ii < 1000; ii++)
            {
                var minDist = long.MaxValue;
                RemoveDuplicates(ref particles);

                foreach (var particle in particles)
                {
                    var dist = particle.ManhattenDistance();
                    
                    if (particle.ManhattenDistance() < minDist)
                    {
                        winningId = particle.Id;
                        minDist = particle.ManhattenDistance();
                    }

                    particle.ChangeVelocity();
                    particle.MoveLocation();
                }

                particles.RemoveAll(i => i.IsDuplicate);
                Console.WriteLine($"[{ii}] - ID={winningId} is the closest to the origin");
            }

            Console.WriteLine($"ID={winningId} is the closest to the origin");
            Console.WriteLine($"There are {particles.Count()} particles remaining");
            Console.ReadLine();
        }

        static void RemoveDuplicates(ref List<Particle> particles)
        {
            for (var ii = 0; ii < particles.Count; ii++){
                var cpl = particles[ii].Location;
                var dupes = particles.FindAll(i => i.Location.Equals(cpl));

                if (dupes.Count > 1)
                {
                    Console.WriteLine($"Found and removing {dupes.Count} duplicates");
                    particles.RemoveAll(i => i.Location.Equals(cpl));
                    ii = 0;
                }
            }
        }

        // Creates a particle from a string input
        // e.g. -  p=<1199,-2918,1457>, v=<-13,115,-8>, a=<-7,8,-10>
        static Particle ParseInput(string input, int Id)
        {
            var retVal = new Particle(Id);
            var particleProps = input.Split(", ");

            foreach(var prop in particleProps)
            {
                var subInput = prop.Substring(3);
                subInput = subInput.Remove(subInput.Length - 1);
                var points = subInput.Split(',');

                if (prop.StartsWith("p"))
                {                    
                    retVal.Location = new Point(long.Parse(points[0]), long.Parse(points[1]), long.Parse(points[2]));
                }
                else if (prop.StartsWith("v"))
                {
                    retVal.Velocity = new Point(long.Parse(points[0]), long.Parse(points[1]), long.Parse(points[2]));
                }
                else
                {
                    retVal.Acceleration = new Point(long.Parse(points[0]), long.Parse(points[1]), long.Parse(points[2]));
                }
            }

            return retVal;
        }
    }
}
