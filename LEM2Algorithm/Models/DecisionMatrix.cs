using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEM2Algorithm.Models
{
    internal class DecisionMatrix
    {
        public readonly int dataRowsNumber;
        public readonly int attributesNumber;

        public string[] attributes;
        public string[,] values;

        public DecisionMatrix(string[] attributes, string[,] values)
        {
            this.attributes = attributes;
            this.values = values;

            if (values.GetLength(1) != attributes.Length)
                throw new ArgumentException("attributes should have as many elements as number of columns in values");

            dataRowsNumber = values.GetLength(0);
            attributesNumber = values.GetLength(1);
        }

        public string GetValue(int row, int attributeIndex)
        {
            return values[row, attributeIndex];
        }
    }
}
