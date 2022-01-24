using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    public class ScanStatus
    {
        public ScanStatus(string name, string status, string result)
        {
            Name = name;
            Status = status;
            Result = result;
        }
        public string Name { get; private set; }
        public string Status { get; private set; }
        public string Result { get; private set; }
    }
}
