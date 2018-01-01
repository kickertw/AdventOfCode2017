using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18Solution
{
    public class Register
    {
        public string Name { get; set; }
        public long Value { get; set; }
        public long? LastValuePlayed { get; set; }

        public void Set(long newValue)
        {
            Value = newValue;
        }

        public void Add(long addVal)
        {
            Value += addVal;
        }

        public void Mult(long val)
        {
            Value *= val;
        }

        public void Mod(long val)
        {
            Value %= val;
        }

        public void Snd()
        {
            LastValuePlayed = Value;
        }

        public long? Rcv()
        {
            if (Value != 0)
            {
                return LastValuePlayed;
            }

            return null;
        }

        public long Jgz(long currentIndex, long offset)
        {
            if (Value > 0)
            {
                return currentIndex + offset;
            }

            return currentIndex;
        }
    }



    public struct Instruction
    {
        public string Type;
        public string RegName;
        public string RegVal;
    }

    class Program
    {
        static void Main(string[] args)
        {
            long location = 0;
            var justJumped = false;
            long? lastVal = null;
            var registry = new List<Register>();
            string[] lines = File.ReadAllLines(".\\input.txt");
            
            while (location >=0 && location < lines.Length)
            {
                var instruction = ParseInstruction(lines[location]);
                var regSetting = registry.FirstOrDefault(i => i.Name == instruction.RegName);

                if (regSetting == null)
                {
                    regSetting = new Register() { Name = instruction.RegName, Value = 0, LastValuePlayed = null };
                    registry.Add(regSetting);
                }

                switch (instruction.Type)
                {
                    case "snd":
                        regSetting.Snd();
                        Console.WriteLine($"({location}) - Playing Sound [{regSetting.LastValuePlayed}]");
                        break;
                    case "set":
                        regSetting.Set(GetRegValue(registry, instruction.RegVal));
                        Console.WriteLine($"({location}) - Setting Value of {regSetting.Name} to [{regSetting.Value}]");
                        break;
                    case "add":
                        regSetting.Add(GetRegValue(registry, instruction.RegVal));
                        Console.WriteLine($"({location}) - Adding {instruction.RegVal} - {regSetting.Name} is now [{regSetting.Value}]");
                        break;
                    case "mul":
                        regSetting.Mult(GetRegValue(registry, instruction.RegVal));
                        Console.WriteLine($"({location}) - Multiplying by {instruction.RegVal} - {regSetting.Name} is now [{regSetting.Value}]");
                        break;
                    case "mod":
                        regSetting.Mod(GetRegValue(registry, instruction.RegVal));
                        Console.WriteLine($"({location}) - Moding by {instruction.RegVal} - {regSetting.Name} is now [{regSetting.Value}]");
                        break;
                    case "rcv":                        
                        lastVal = regSetting.Rcv();
                        Console.WriteLine($"({location}) - Recovering last sound of {regSetting.Name} [{lastVal}]");

                        if (lastVal != null) { location = -100; }
                        break;
                    default:
                        //jump
                        var oldLocation = location;
                        location = regSetting.Jgz(location, GetRegValue(registry, instruction.RegVal));

                        if (oldLocation == location)
                        {
                            Console.WriteLine($"({location}) - No jumping because val = 0.  Going to the next instruction.");
                        }
                        else
                        {
                            Console.WriteLine($"({location}) - Jumping to {location}");
                            justJumped = true;
                        }
                        break;
                }

                if (!justJumped) location++;
                justJumped = false;
            }

            Console.WriteLine($"Last sound played = {lastVal}");
            Console.ReadLine();
        }
        
        static Instruction ParseInstruction(string input)
        {
            var inputArr = input.Split(' ');
            var retVal = new Instruction
            {
                Type = inputArr[0],
                RegName = inputArr[1]
            };

            if (inputArr.Length == 3)
            {
                retVal.RegVal = inputArr[2];
            }

            return retVal;
        }

        static long GetRegValue(List<Register> regList, string value)
        {
            int numberVal = 0;

            if (int.TryParse(value, out numberVal))
            {
                return numberVal;
            }

            var reg = regList.FirstOrDefault(i => i.Name == value);
            return reg?.Value ?? 0;
        }
    }
}
