using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.Chart.XY
{
    public class XYItem
    {
        public XYItem(XYLegend xyLegend, List<Layer> layerList)
        {
            XYLegend = xyLegend;
            LayerList = layerList;
        }
        public XYLegend XYLegend { get; }
        public List<Layer> LayerList { get; }
        public class Layer
        {
            public Layer(List<double> xList, List<double> yList, List<string> labelList)
            {
                XList = xList;
                YList = yList;
                LabelList = labelList;
            }
            public List<double> XList { get; }
            public List<double> YList { get; }
            public List<string> LabelList { get; }
        }
    }
}
