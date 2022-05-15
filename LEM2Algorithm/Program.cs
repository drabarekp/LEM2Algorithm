using System;
using LEM2Algorithm.Models;
using System.Collections;
using System.Collections.Generic;

namespace LEM2Algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {


            string[] attributes = new string[] { "Temperatue", "Weakness", "Headache", "Nausea" };
            string[,] values = new string[,]
            {
                { "very_high", "yes", "yes", "no"},
                { "high", "yes", "no", "yes"},
                { "normal", "no", "no", "no"},
                { "normal", "yes", "yes", "yes"},
                {"high", "no", "yes", "no" },
                { "high", "no", "no", "no"},
                {"normal", "no", "yes", "no" },
            };

            var matrix = new DecisionMatrix(attributes, values);
            var dm = new DecisionMaker(matrix);
            var result = dm.LEM2(new HashSet<int>(new int[] {0, 1, 3, 4}));
            PrintHashSet(result);
        }

        static void PrintHashSet(IEnumerable hashobj)
        {
            Console.Write("{");

            foreach (var el in hashobj)
            {
                var enumel = el as IEnumerable;
                if (enumel is not null)
                    PrintHashSet(enumel);
                else
                    PrintHashSet(el);
            }

            Console.Write("}"); 
        }
        static void PrintHashSet(object obj)
        {
            Console.Write(obj);
        }
    }
}

