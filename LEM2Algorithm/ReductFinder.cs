using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LEM2Algorithm.Models;

namespace LEM2Algorithm
{
    public class ReductFinder
    {
        private DiscernabilityMatrixv2 M;
        internal ReductFinder(DecisionMatrix decisionMatrix)
        {
            M = new DiscernabilityMatrixv2(decisionMatrix);
        }

        public DecisionMatrix Johnson()
        {
            List<string> R = new List<string>();
            while(true)
            {
                string attr = M.FindAttributeWithMaxOccurrences();
                if (attr == null)
                    break;

                M.DeleteAllCellsContainingAttribute(attr);
                R.Add(attr);
            }

            string[,] values = new string[M.dataRowsNumber, R.Count];
            int i = 0;
            foreach(var attr in R)
            {
                int index = Array.IndexOf(M.attributes, attr);
                if(index>=0)
                {
                    for (int j = 0; j < M.dataRowsNumber; j++)
                        values[j, i] = M.values[j, index];

                    i++;
                }
            }

            return new DecisionMatrix(R.ToArray(), values, M.decisions);
        }
    }
}
