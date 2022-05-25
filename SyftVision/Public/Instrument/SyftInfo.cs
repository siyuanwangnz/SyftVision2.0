using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.Instrument
{
    public class SyftInfo
    {
        public SyftInfo(string category, string item, string content)
        {
            Category = category;
            Item = item;
            Content = content;
        }
        public string Category { get; }
        public string Item { get; }
        public string Content { get; }
    }
}
