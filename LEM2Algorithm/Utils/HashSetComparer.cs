using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEM2Algorithm.Utils
{
    public class HashSetComparer<T> : IEqualityComparer<HashSet<T>>
    {
        public bool Equals(HashSet<T> x, HashSet<T> y)
        {
            if(x.Count != y.Count)
                return false;

            foreach(var xEl in x)
            {
                if (!y.Contains(xEl)) return false;
            }

            return true;
        }

        public int GetHashCode([DisallowNull] HashSet<T> hashSet)
        {
            int result = 0;
            unchecked
            {
                foreach (var el in hashSet)
                {
                    result += el.GetHashCode();
                }
            }
            return result;
        }
    }
}
