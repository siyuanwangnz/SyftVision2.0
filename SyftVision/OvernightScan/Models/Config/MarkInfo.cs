using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class MarkInfo
    {
        public MarkInfo(string main, string sub)
        {
            Main = main;
            Sub = sub;
        }
        public string Main { get; private set; }
        public string Sub { get; private set; }
        public string MarkCode { get => $"{Main}{Sub}".ToLower().Replace(" ", ""); }
    }
}
