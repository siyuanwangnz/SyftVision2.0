﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchConfig.Models
{
    public class InstruBatch
    {
        public InstruBatch(string file)
        {
            File = file;
        }
        public string File { get; }
    }
}
