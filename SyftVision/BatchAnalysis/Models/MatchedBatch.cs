using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.Models
{
    public class MatchedBatch
    {
        public MatchedBatch(List<ScanFile> scanList)
        {
            ScanList = scanList;
        }
        public string Name { get => ScanList.First().Date_Time; }
        public List<ScanFile> ScanList { get; }
        public bool IsChecked { get; set; }

    }
}
