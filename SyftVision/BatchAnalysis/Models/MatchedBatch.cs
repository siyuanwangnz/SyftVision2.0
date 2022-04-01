using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.Models
{
    public class MatchedBatch
    {
        public MatchedBatch(string testName, bool isChecked)
        {
            TestName = testName;
            IsChecked = isChecked;
        }
        public string TestName { get; set; }
        public List<Scan> ScansList { get; set; }
        public bool IsChecked { get; set; }

    }
}
