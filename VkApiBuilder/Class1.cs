using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkApiParser;

namespace VkApiBuilder
{
    public class Class1
    {
        public static void Main()
        {
            var parser = new Parser();
            parser.NotifyState += text => Console.WriteLine(text);
            parser.Parse().Wait();
        }
    }
}
