using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var maxValueEver = 0;
            var registerList = new Dictionary<string, int>();
            var input = File.ReadAllLines(@".\Input\Day8Input.txt");

            foreach(var line in input)
            {
                var instruction = line.Split(' ');
                var modValue = int.Parse(instruction[2]);

                // Add to the register list
                if (!registerList.ContainsKey(instruction[0]))
                {
                    UpdateRegisterList(registerList, instruction[0]);
                }

                // Get the value to inc/dec
                if (instruction[1] == "dec") { modValue *=  -1; }

                // Get the condition and evaluate it
                var registerToCheck = instruction[4];
                var compOperator = instruction[5];
                var compValue = int.Parse(instruction[6]);
                var existingRegValue = GetRegisterValue(registerList, registerToCheck);

                var isItTrue = false;
                switch (compOperator)
                {
                    case ">":                        
                        isItTrue = existingRegValue > compValue;
                        break;
                    case "<":
                        isItTrue = existingRegValue < compValue;
                        break;
                    case ">=":
                        isItTrue = existingRegValue >= compValue;
                        break;
                    case "<=":
                        isItTrue = existingRegValue <= compValue;
                        break;
                    case "!=":
                        isItTrue = existingRegValue != compValue;
                        break;
                    default:
                        isItTrue = existingRegValue == compValue;
                        break;
                }

                if (isItTrue)
                {
                    var initRegValue = GetRegisterValue(registerList, instruction[0]);
                    UpdateRegisterList(registerList, instruction[0], initRegValue + modValue);

                    if (maxValueEver < initRegValue + modValue)
                    {
                        maxValueEver = initRegValue + modValue;
                    }
                }

                // Uncomment to debug values of registry list
                //string regListString = "";
                //foreach(var kvp in registerList)
                //{
                //    regListString += $"{kvp.Key} = {kvp.Value}, ";
                //}
                //Console.WriteLine($"Registry list is now {regListString}");
            }

            //Part 1 - Get the largest value of all the registers
            Console.WriteLine($"The largest value is {registerList.Values.Max()}");

            //Part 2
            Console.WriteLine($"The largest value ever is {maxValueEver}");

            Console.ReadLine();
        }

        private static void UpdateRegisterList(Dictionary<string, int> existingReg, string regId, int? regVal = null)
        {
            if (!existingReg.ContainsKey(regId))
            {
                existingReg.Add(regId, regVal ?? 0);
            }

            if (existingReg.ContainsKey(regId) && regVal.HasValue)
            {
                //Console.WriteLine($"Updating {regId} to {regVal}");
                existingReg[regId] = regVal.Value;
            }
        }

        private static int GetRegisterValue(Dictionary<string, int> existingReg, string regId)
        {
            if (!existingReg.ContainsKey(regId))
            {
                UpdateRegisterList(existingReg, regId);
            }
            else
            {
                return existingReg[regId];
            }            

            return 0;
        }
    }
}