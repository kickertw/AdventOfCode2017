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

        public virtual void Snd()
        {
            LastValuePlayed = Value;
        }

        public virtual long? Rcv()
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

    public class Program
    {
        public int Id { get; set; }
        public long Location { get; set; }
        public bool IsWaiting { get; set; }
        public Queue<long> InQueue { get; set; }
        public List<Register> Registry { get; set; }
        public int QueueCounter { get; set; }
        public bool JustJumped { get; set; }

        public Program(int id)
        {
            Id = id;
            Location = 0;
            IsWaiting = false;
            Registry = new List<Register>();
            InQueue = new Queue<long>();
            QueueCounter = 0;
            JustJumped = false;
        }

        public void SendToQueue(long val)
        {
            InQueue.Enqueue(val);
            QueueCounter++;
        }

        /// <summary>
        /// Gets value from the queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="registry"></param>
        /// <param name="regName"></param>
        /// <returns>True if queue has a value, false if queue is empty</returns>
        public bool GetFromQueue(string regName)
        {
            if (InQueue.Any())
            {
                var register = Registry.FirstOrDefault(i => i.Name == regName);

                if (register != null)
                {
                    register.Value = InQueue.Dequeue();
                    Console.WriteLine($"Register {register.Name} now has value {register.Value}");
                    return true;
                }
            }

            return false;
        }
    }
    
    public class LoneStarRunner
    {
        static void Main(string[] args)
        {
            long? lastVal = null;
            string[] lines = File.ReadAllLines(".\\input.txt");

            //Part1Solution(ref lastVal, lines);
            //Console.WriteLine($"Last sound played = {lastVal}");

            Part2Solution(lines);

            Console.ReadLine();
        }

        private static void Part1Solution(ref long? lastVal, string[] lines)
        {
            long location = 0;
            var justJumped = false;
            var registry = new List<Register>();

            while (location >= 0 && location < lines.Length)
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
        }

        private static void SwapRunningProgram(ref int index1, ref int index2)
        {
            var tmp = index1;
            index1 = index2;
            index2 = tmp;
        }

        private static void Part2Solution(string[] lines)
        {
            long tempVal = -1;
            var activeProgramIdx = 0;
            var inactiveProgramIdx = 1;
            var programs = new Program[] { new Program(0), new Program(1) };
            programs[1].IsWaiting = true;

            // Initializing Registers
            foreach(var line in lines)
            {
                var inst = ParseInstruction(line, true);
                if (!long.TryParse(inst.RegName, out tempVal))
                {
                    programs[0].Registry.Add(new Register() { Name = inst.RegName, Value = 0, LastValuePlayed = null });
                    programs[1].Registry.Add(new Register() { Name = inst.RegName, Value = 1, LastValuePlayed = null });
                }                
            }

            while ((!programs[0].IsWaiting || programs[0].InQueue.Any()) ||
                   (!programs[1].IsWaiting || programs[1].InQueue.Any()))
            {
                var runningProgram = programs[activeProgramIdx];
                var waitingProgram = programs[inactiveProgramIdx];

                if (runningProgram.IsWaiting && !runningProgram.InQueue.Any() &&
                    waitingProgram.IsWaiting && !waitingProgram.InQueue.Any())
                {
                    Console.WriteLine($"Program 1 sent a value to Program 0 [{programs[0].QueueCounter}] times.");
                    return;
                }

                Console.WriteLine($"Active Program [{activeProgramIdx}]");
                runningProgram.IsWaiting = false;

                while (runningProgram.Location >= 0 && runningProgram.Location < lines.Length && !runningProgram.IsWaiting)
                {
                    Console.Write($"    {runningProgram.Location} {lines[runningProgram.Location]} - ");
                    var instruction = ParseInstruction(lines[runningProgram.Location], true);
                    Register regSetting = null;

                    if (!string.IsNullOrEmpty(instruction.RegName))
                    {
                        regSetting = runningProgram.Registry.FirstOrDefault(i => i.Name == instruction.RegName);
                    }

                    switch (instruction.Type)
                    {
                        case "set":
                            regSetting.Set(GetRegValue(runningProgram.Registry, instruction.RegVal));
                            Console.WriteLine($"({runningProgram.Location}) - Setting Value of {regSetting.Name} to [{regSetting.Value}]");
                            break;
                        case "add":
                            regSetting.Add(GetRegValue(runningProgram.Registry, instruction.RegVal));
                            Console.WriteLine($"({runningProgram.Location}) - Adding {instruction.RegVal} - {regSetting.Name} is now [{regSetting.Value}]");
                            break;
                        case "mul":
                            regSetting.Mult(GetRegValue(runningProgram.Registry, instruction.RegVal));
                            Console.WriteLine($"({runningProgram.Location}) - Multiplying by {instruction.RegVal} - {regSetting.Name} is now [{regSetting.Value}]");
                            break;
                        case "mod":
                            regSetting.Mod(GetRegValue(runningProgram.Registry, instruction.RegVal));
                            Console.WriteLine($"({runningProgram.Location}) - Moding by {instruction.RegVal} - {regSetting.Name} is now [{regSetting.Value}]");
                            break;
                        case "snd":
                            waitingProgram.SendToQueue(GetRegValue(runningProgram.Registry, instruction.RegVal));
                            Console.WriteLine($"Sent value to other queue for Program-{inactiveProgramIdx}");
                            break;
                        case "rcv":
                            if (!runningProgram.GetFromQueue(instruction.RegName))
                            {
                                runningProgram.IsWaiting = true;
                                SwapRunningProgram(ref activeProgramIdx, ref inactiveProgramIdx);
                                Console.WriteLine(" Switching to Program " + activeProgramIdx);
                            }
                            break;
                        default:
                            //jump
                            var oldLocation = runningProgram.Location;
                            if (regSetting != null)
                            {
                                runningProgram.Location = regSetting.Jgz(runningProgram.Location, GetRegValue(runningProgram.Registry, instruction.RegVal));
                            }
                            else
                            {
                                runningProgram.Location += GetRegValue(runningProgram.Registry, instruction.RegVal);
                            }

                            if (oldLocation == runningProgram.Location)
                            {
                                Console.WriteLine($"({runningProgram.Location}) - No jumping because val <= 0.  Going to the next instruction.");
                            }
                            else
                            {
                                Console.WriteLine($"({oldLocation}) - Jumping to {runningProgram.Location}");
                                runningProgram.JustJumped = true;
                            }
                            break;
                    }

                    if (!runningProgram.IsWaiting)
                    {
                        if (!runningProgram.JustJumped) runningProgram.Location++;
                        runningProgram.JustJumped = false;
                    }
                }
            }

            Console.WriteLine($"Program 1 sent a value to Program 0 [{programs[0].QueueCounter}] times.");
        }

        static Instruction ParseInstruction(string input, bool isPart2 = false)
        {
            var inputArr = input.Split(' ');

            var retVal = new Instruction
            {
                Type = inputArr[0]
                
            };

            if (isPart2 && (retVal.Type == "snd" || retVal.Type == "rcv"))
            {
                if (retVal.Type == "rcv")
                {
                    retVal.RegName = inputArr[1];
                }
                else
                {
                    retVal.RegVal = inputArr[1];
                }                
            }
            else
            {
                retVal.RegName = inputArr[1];

                if (inputArr.Length == 3)
                {
                    retVal.RegVal = inputArr[2];
                }
            }

            return retVal;
        }

        static long GetRegValue(List<Register> regList, string value)
        {
            long numberVal = 0;

            if (long.TryParse(value, out numberVal))
            {
                return numberVal;
            }

            var reg = regList.FirstOrDefault(i => i.Name == value);
            return reg?.Value ?? 0;
        }
    }
}
