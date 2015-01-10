using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkApiParser
{
    public sealed class Method
    {
        public string Url { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRequireAccessToken { get; set; }
        
        public List<string> AccessRights { get; set; }

        public List<Parameter> Parameters { get; set; }

        public ResultType ResultType { get; set; }

        public Method() 
        {
            AccessRights = new List<string>();
            Parameters = new List<Parameter>();
        }
    }
}
