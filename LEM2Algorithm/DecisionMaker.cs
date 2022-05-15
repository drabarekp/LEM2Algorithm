using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LEM2Algorithm.Models;
using LEM2Algorithm.Utils;

namespace LEM2Algorithm
{
    internal class DecisionMaker
    {

        private InformationMatrix pairs;

        public DecisionMaker(InformationMatrix pairs)
        {
            this.pairs = pairs;
        }

        public HashSet<HashSet<AttributeValuePair>> LEM2(HashSet<int> attributesB)
        {
            HashSet<int> attributesG = attributesB.ToHashSet();
            //Dictionary<int, string> tau = new Dictionary<int, string>(); //tau is a set of sets
            var tau = new HashSet<HashSet<AttributeValuePair>>(new HashSetComparer<AttributeValuePair>());

            while (attributesG.Count > 0)
            {
                HashSet<AttributeValuePair> T = new HashSet<AttributeValuePair>();
                HashSet<AttributeValuePair> TG = GenerateTG(attributesG);

                while(T.Count == 0 || !BlockOfSet(T).IsSubsetOf(attributesB))
                    InnerWhile(ref TG, ref T, ref attributesG);

                foreach(var t in T)
                {
                    var subtractedT = Subtract(T, t);
                    if (BlockOfSet(subtractedT).IsSubsetOf(attributesB))
                    {
                        T = subtractedT;
                    }
                }
                tau.Add(T);
                var sumOfBlocksOfSets = SumOfBlocksOfSetsInAFamily(tau);
                attributesG = Subtract(attributesB, sumOfBlocksOfSets);
            }

            foreach(var T in tau)
            {
                var tauWithoutT = Subtract(tau, T);
                var sumOfBlocksInTauWithoutT = SumOfBlocksOfSetsInAFamily(tauWithoutT);
                if(HashSetEqual(sumOfBlocksInTauWithoutT, attributesB))
                {
                    tau = tauWithoutT;
                }
            }

            return tau;
        }

        private HashSet<AttributeValuePair> GenerateTG(HashSet<int> G)
        {
            var result = new HashSet<AttributeValuePair>();

            for(int row = 0; row < pairs.dataRowsNumber; row++)
            {
                for(int attributeIndex = 0; attributeIndex < pairs.attributesNumber; attributeIndex++)
                {
                    string value = pairs.GetValue(row, attributeIndex);
                    var tBrackets = Block(attributeIndex, value);
                    tBrackets.IntersectWith(G); // intersect in place
                    if(tBrackets.Count > 0)
                    {
                        result.Add(new AttributeValuePair(attributeIndex, value));
                    }
                }
            }

            return result;
        }

        private HashSet<int> Block(int attribute, string value)
        {
            var block = new HashSet<int>();

            for(int row = 0; row<pairs.dataRowsNumber; row++)
            {
                if(value == pairs.GetValue(row, attribute))
                {
                    block.Add(row);
                }
            }
            return block;
        }

        private HashSet<int> Block(AttributeValuePair pair)
        {
            return Block(pair.attribute, pair.value);
        }

        private void InnerWhile(ref HashSet<AttributeValuePair> TG, ref HashSet<AttributeValuePair> T, ref HashSet<int> G)
        {
            AttributeValuePair[] TGArray = TG.ToArray();
            int[] cardinalityBlocktIntersectG = new int[TG.Count];
            int[] cardinalityBlockt = new int [TG.Count];

            for(int i=0; i<TG.Count; i++)
            {
                var Blockt = Block(TGArray[i]);
                cardinalityBlockt[i] = Blockt.Count;

                //now Blockt is Blockt instersected G
                Blockt.IntersectWith(G);
                cardinalityBlocktIntersectG[i] = Blockt.Count;
            }

            int maxTG = int.MinValue;
            int minT = int.MaxValue;
            int indexToChoose = -1;

            for(int i=0; i<TG.Count; i++)
            {
                if (cardinalityBlocktIntersectG[i] == maxTG && cardinalityBlockt[i] < minT)
                {
                    minT = cardinalityBlockt[i];
                    indexToChoose = i;
                    continue;
                }
                if(cardinalityBlocktIntersectG[i] > maxTG)
                {
                    maxTG = cardinalityBlocktIntersectG[i];
                    minT = cardinalityBlockt[i];
                    indexToChoose = i;
                    continue;
                }
            }

            var t = TGArray[indexToChoose];

            //mutating
            T.Add(t);
            G.IntersectWith(Block(t)); // THIS IS CHANGE FROM THE SOURCE
            //G = Subtract(G, Block(t));
            TG = GenerateTG(G);
            TG = Subtract(TG, T);
        }

        private HashSet<int> BlockOfSet(HashSet<AttributeValuePair> set)
        {
            if (set.Count == 0)
                return new HashSet<int>(Enumerable.Range(0, pairs.dataRowsNumber));

            HashSet<int> result = new HashSet<int>();
            bool resultInitialised = false;

            foreach (var t in set)
            {
                if (!resultInitialised)
                {
                    result = Block(t);
                    resultInitialised = true;
                }
                else
                {
                    result.IntersectWith(Block(t));
                }
            }

            return result;
        }

        private HashSet<T> Subtract<T>(HashSet<T> h1, HashSet<T> h2)
        {
            var result = h1.ToHashSet();
            result.RemoveWhere(p => h2.Contains(p));
            return result;
        }

        private HashSet<T> Subtract<T>(HashSet<T> h1, T t)
        {
            var result = h1.ToHashSet();
            result.Remove(t);
            return result;
        }
        private HashSet<int> SumOfBlocksOfSetsInAFamily(HashSet<HashSet<AttributeValuePair>> family)
        {
            var result = new HashSet<int>();

            foreach (var set in family)
            {
                result.UnionWith(BlockOfSet(set));
            }

            return result;
        }

        private bool HashSetEqual<T>(HashSet<T> x, HashSet<T> y)
        {
            if (x.Count != y.Count)
                return false;

            foreach (var xEl in x)
            {
                if (!y.Contains(xEl)) return false;
            }

            return true;
        }
    }
}
