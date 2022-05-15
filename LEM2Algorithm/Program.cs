using System;
using LEM2Algorithm.Models;

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
            string[] decisions = new string[] { "yes", "yes", "no", "yes", "yes", "no", "no" };

            var matrix = new DecisionMatrix(attributes, values, decisions);

            var reduct = new ReductFinder(matrix);
            var reducedMatrix = reduct.Johnson();

            var dm = new DecisionMaker(reducedMatrix);
            var result = dm.LEM2(new System.Collections.Generic.HashSet<int>(new int[] {0, 1, 3, 4}));
            Console.WriteLine(result);
        }
    }
}

