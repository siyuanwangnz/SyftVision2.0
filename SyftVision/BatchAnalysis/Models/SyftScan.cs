using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.Models
{
    public class SyftScan
    {
        public SyftScan(string scan, string status, string result)
        {
            Scan = scan;
            Status = status;
            Result = result;
        }
        public string Scan { get; }
        public string Status { get; }
        public string Result { get; }
    }
}
