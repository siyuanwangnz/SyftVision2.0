using ChartDirector;
using Public.Chart;
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
            ChartProp = chartProp;
            ScanFileList = scanFileList;
            XY = ChartProp.ChartType.ChartFactory.GetXY(ScanFileList);
            Chart = ChartProp.ChartType.ChartFactory.GetChart(ChartProp);
        }
        public SyftChart(BaseChart chart)
        {
            Chart = chart;
        }
        public ChartProp ChartProp { get; }
        public List<ScanFile> ScanFileList { get; }
        public IXY XY { get; }
        public BaseChart Chart { get; }
    }
}
