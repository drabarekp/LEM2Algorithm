using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEM2Algorithm.Models
{
    public struct AttributeValuePair
    {
        public int attribute;
        public string value;

        public AttributeValuePair(int attribute, string value)
        {
            this.attribute = attribute;
            this.value = value;
        }

        public override string ToString()
        {
            return $"({attribute}, {value})";
        }
    }
}
