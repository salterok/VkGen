﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkApiParser
{
    public sealed class MethodGroupList : List<MethodGroup>
    {
        public Version ApiVersion { get; set; }
    }
}
