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
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        // restrictions
        public uint MaxElements { get; set; }
        public uint MinLength { get; set; }
        public uint MaxLength { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }

        public ResultType()
        {

        }
    }
}
