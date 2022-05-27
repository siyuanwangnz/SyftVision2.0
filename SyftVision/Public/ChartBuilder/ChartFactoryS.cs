using ChartDirector;
using Public.ChartBuilder.XY;
using Public.SettingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder
{
    public abstract class ChartFactoryS
    {
        public ChartFactoryS(XYFactoryS xyFactoryS)
        {
            XYFactoryS = xyFactoryS;
        }
        public XYFactoryS XYFactoryS { get; }
        public XYItemS XYItemS { get; private set; }
        public Setting Setting { get; private set; }
        public List<XYLegend> GetXYLegendList(Setting setting)
        {
            Setting = setting;
            XYItemS = XYFactoryS.GetXYItemS(Setting);
            if (XYItemS == null) return new List<XYLegend>();
            return XYItemS.YLayerList.Select(a => a.XYLegend).ToList();
        }
        public BaseChart GetChart()
        {
            return SetChart(Setting, XYItemS);
        }
        public abstract BaseChart SetChart(Setting setting, XYItemS xyItemS);
    }
}
