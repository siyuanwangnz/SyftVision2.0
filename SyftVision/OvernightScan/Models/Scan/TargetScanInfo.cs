using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class TargetScanInfo : ScanInfo
    {
        public TargetScanInfo(string filename, string foldername, List<MarkInfo> markList, string batch) : base(filename, foldername)
        {
            MarkList = markList;
            Batch = batch;
        }
        public string Batch { get; private set; }
        public List<MarkInfo> MarkList { get; private set; }
        public List<string> MarkCodeList { get => MarkList.Select(a => a.MarkCode).ToList(); }
    }
}
