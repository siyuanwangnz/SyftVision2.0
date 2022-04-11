using ChartDirector;
using Public.ChartBuilder.XY;
using Public.ChartConfig;
using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder
{
    public abstract class ChartFactory
    {
        public ChartFactory(XYFactory xyFactory)
        {
            XYFactory = xyFactory;
        }
        public XYFactory XYFactory { get; }
        public List<XYItem> XYItemList { get; private set; }
        public ChartProp ChartProp { get; private set; }
        public List<XYLegend> GetXYLegendList(ChartProp chartProp, List<ScanFile> scanFileList)
        {
            ChartProp = chartProp;
            XYItemList = XYFactory.GetXYItemList(ChartProp, scanFileList);
            return XYItemList.Select(a => a.XYLegend).ToList();
        }
        public BaseChart GetChart()
        {
            return SetChart(ChartProp, XYItemList);
        }
        public abstract BaseChart SetChart(ChartProp chartProp, List<XYItem> xyItemList);

    }
}
