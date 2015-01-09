using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkApiParser
{
    public sealed class MethodGroup : List<Method>
    {
        public string Title { get; set; }
    }
}
