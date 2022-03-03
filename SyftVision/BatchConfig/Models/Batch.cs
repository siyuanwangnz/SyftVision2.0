using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchConfig.Models
{
    public class Batch
    {
        public Batch(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}
