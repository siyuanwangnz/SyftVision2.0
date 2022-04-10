using ChartDirector;
using Public.Chart.XY;
using Public.ChartConfig;
using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.Chart
{
    public abstract class ChartFactory
    {
        public ChartFactory(XYFactory xyFactory)
        {
            XYFactory = xyFactory;
        }
        public XYFactory XYFactory { get; }
        public List<XYItem> XYItemList { get; private set; }
        public List<XYLegend> GetXYLegendList(List<ScanFile> scanFileList)
        {
            XYItemList = XYFactory.GetXYItemList(scanFileList);
            return XYItemList.Select(a => a.XYLegend).ToList();
        }
        public BaseChart GetChart(ChartProp chartProp)
        {
            return SetChart(chartProp, XYItemList);
        }
        public abstract BaseChart SetChart(ChartProp chartProp, List<XYItem> xyItemList);

    }
}
