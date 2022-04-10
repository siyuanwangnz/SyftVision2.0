using ChartDirector;
using Public.Chart;
using Public.Chart.XY;
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
            XYLegendList = ChartProp.ChartType.ChartFactory.GetXYLegendList(scanFileList);
            Chart = ChartProp.ChartType.ChartFactory.GetChart(ChartProp);
        }
        public SyftChart(BaseChart chart)
        {
            Chart = chart;
        }
        public ChartProp ChartProp { get; }
        public List<ScanFile> ScanFileList { get; }
        public List<XYLegend> XYLegendList { get; }
        public BaseChart Chart { get; }
    }
}
