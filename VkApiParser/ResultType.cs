using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkApiParser
{
    public sealed class ResultType
    {
        public bool IsList { get; set; }
        public int MaxElements { get; set; }
        public string Type { get; set; }

        public ResultType(string type, bool isList = false, int maxElements = -1)
        {
            Type = type;
            IsList = IsList;
            MaxElements = maxElements;
        }
    }
}
