using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEM2Algorithm.Models
{
    public class DecisionMatrix : InformationMatrix
    {
        public string[] decisions;
        public DecisionMatrix(string[] attributes, string[,] values, string []decisions) : base(attributes, values)
        {
            if(values.GetLength(0)!=decisions.Length)
                throw new ArgumentException("decisions should have as many elements as number of rows in values");

            this.decisions = decisions;
        }
    }
}
