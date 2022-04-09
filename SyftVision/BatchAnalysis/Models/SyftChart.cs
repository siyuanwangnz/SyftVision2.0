using ChartDirector;
using Public.ChartConfig;
using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.Models
{
    public class SyftChart
    {
        public SyftChart(ChartProp chartProp, List<ScanFile> scanFileList)
        {

        }
        public SyftChart(BaseChart chart)
        {
            Chart = chart;
        }
        public BaseChart Chart { get; }
    }
}
