using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public class XYItem
    {
        public XYItem(XYLegend xyLegend, List<Layer> multiLayer)
        {
            XYLegend = xyLegend;
            MultiLayer = multiLayer;
        }
        public XYItem(XYLegend xyLegend, Layer singleLayer)
        {
            XYLegend = xyLegend;
            SingleLayer = singleLayer;
        }
        public XYLegend XYLegend { get; }
        public List<Layer> MultiLayer { get; }
        public Layer SingleLayer { get; }
        public class Layer
        {
            public Layer(List<double> xList, List<double> yList)
            {
                XList = xList;
                YList = yList;
            }
            public Layer(List<string> labelList, List<double> yList)
            {
                LabelList = labelList;
                YList = yList;
            }
            public List<double> XList { get; }
            public List<double> YList { get; }
            public List<string> LabelList { get; }
        }
    }
}
