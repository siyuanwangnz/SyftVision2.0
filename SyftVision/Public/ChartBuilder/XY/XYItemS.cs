using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public class XYItemS
    {
        public XYItemS(List<string> labelList, List<YLayer> yLayerList, List<double> upperList, List<double> underList)
        {
            LabelList = labelList;
            YLayerList = yLayerList;
            UpperList = upperList;
            UnderList = underList;
        }
        public List<YLayer> YLayerList { get; }
        public List<double> UpperList { get; }
        public List<double> UnderList { get; }
        public List<string> LabelList { get; }
        public class YLayer
        {
            public YLayer(XYLegend xyLegend, List<double> yList)
            {
                YList = yList;
                XYLegend = xyLegend;
            }
            public List<double> YList { get; }
            public XYLegend XYLegend { get; }
        }
    }
}