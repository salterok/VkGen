using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkApiParser
{
    public sealed class ApiVersion
    {
        public Version Version { get; set; }
        /// <summary>
        /// Api changes description
        /// </summary>
        /// <remarks>Should be break into separate fields to provide ability to build older api version.</remarks>
        public string Desciption { get; set; }
    }
}
