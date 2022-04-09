using ChartDirector;
using Public.ChartConfig;
using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.Chart
{
    public class ChartFactory : IChartFactory
    {
        // XY
        private IXY XY;
        public IXY GetXY(List<ScanFile> scanFileList)
        {
            BuildXY(XY, scanFileList);
            return XY;
        }
        private void BuildXY(IXY xy, List<ScanFile> scanFileList)
        {
        }
        // Chart
        private XYChart BaseChart = new XYChart(1116, 566, 0xccccff);
        public BaseChart GetChart(ChartProp chartProp)
        {
            BuildChart(BaseChart, chartProp);
            return BaseChart;
        }
        private void BuildChart(XYChart baseChart, ChartProp chartProp)
        {
        }
    }
}
