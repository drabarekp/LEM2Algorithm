using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEM2Algorithm.Models
{
    public class DiscernabilityMatrixv2
    {
        private List<string>[,] matrix;
        private List<int> decision1Objects = new List<int>();
        private List<int> decision2Objects = new List<int>();

        public readonly int dataRowsNumber;
        public readonly int attributesNumber;
        public string[] attributes;
        public readonly string[,] values;
        public readonly string[] decisions;
        public DiscernabilityMatrixv2(DecisionMatrix dm)
        {
            dataRowsNumber = dm.dataRowsNumber;
            attributesNumber = dm.attributesNumber;
            attributes = dm.attributes;
            values = dm.values;
            decisions = dm.decisions;

            DetermineObjectsByDecision(dm);

            matrix = new List<string>[decision1Objects.Count, decision2Objects.Count];

            for (int i = 0; i < decision1Objects.Count; i++)
                for (int j = 0; j < decision2Objects.Count; j++)
                    matrix[i, j] = new List<string>();

            for (int i = 0; i < decision1Objects.Count; i++)
                for (int j = 0; j < decision2Objects.Count; j++)
                    for (int k = 0; k < dm.attributesNumber; k++)
                        if (dm.values[decision1Objects[i], k].CompareTo(dm.values[decision2Objects[j], k]) != 0)
                            matrix[i, j].Add(dm.attributes[k]);



                    for (int i = 0; i < decision1Objects.Count; i++)
                for (int j = 0; j < decision2Objects.Count; j++)
                    if (matrix[i, j].Count == 0)
                        matrix[i, j] = null;


        }
        private void DetermineObjectsByDecision(DecisionMatrix dm)
        {
            string decision1 = dm.decisions[0];

            for(int i=0;i<dm.decisions.Length;i++)
            {
                if (dm.decisions[i] != decision1)
                    decision1Objects.Add(i);
                else
                    decision2Objects.Add(i);
            }
        }
        public string FindAttributeWithMaxOccurrences()
        {
            Dictionary<string, int> occurrences = new Dictionary<string, int>();
            foreach (var a in attributes)
                occurrences.Add(a, 0);

            for (int i = 0; i < decision1Objects.Count; i++)
                for (int j = 0; j < decision2Objects.Count; j++)
                    if (matrix[i, j] != null)
                        foreach (var a in matrix[i, j])
                            occurrences[a]++;

            int maxValue = -1;
            string maxKey = "";
            foreach (var pair in occurrences)
                if (pair.Value > maxValue)
                {
                    maxValue = pair.Value;
                    maxKey = pair.Key;
                }

            if (maxValue <= 0)
                return null;

            return maxKey;
        }

        public void DeleteAllCellsContainingAttribute(string attribute)
        {
            for (int i = 0; i < decision1Objects.Count; i++)
                for (int j = 0; j < decision2Objects.Count; j++)
                    if (matrix[i, j] != null && matrix[i, j].Contains(attribute))
                        matrix[i, j] = null;
        }
    }
}
