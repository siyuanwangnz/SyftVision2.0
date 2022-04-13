using ChartDirector;
using Public.ChartBuilder.XY;
using Public.ChartConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder
{
    public class OverlapLineChartFactory : ChartFactory
    {
        public OverlapLineChartFactory(XYFactory xyFactory) : base(xyFactory)
        {
        }

        public override BaseChart SetChart(ChartProp chartProp, List<XYItem> xyItemList)
        {
            XYChart c = new XYChart(1116, 520);

            // Set chart position, size and style
            c.setPlotArea(80, 30, c.getWidth() - 110, c.getHeight() - 70, -1, -1, -1, 0x40dddddd, 0x40dddddd);

            // Add a title
            c.addTitle(Chart.TopCenter, chartProp.Code, "Arial Bold", 16);

            // Set the x and y axis stems and the label font
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // Add axis number format
            c.setNumberFormat(',');

            // Add layers
            foreach (var xyItem in xyItemList)
            {
                foreach (var xylayer in xyItem.MultiLayer)
                {
                    LineLayer layer = c.addLineLayer2();
                    layer.setLineWidth(1);
                    layer.setFastLineMode();

                    layer.setXData(xylayer.XList.ToArray());
                    layer.addDataSet(xylayer.YList.ToArray(), xyItem.XYLegend.Color, $"({xyItem.XYLegend.Content}) {xylayer.Content}");
                    layer.setHTMLImageMap("", "", "title='{value} & {x} {dataSetName}'");
                }
            }

            return c;
        }
    }
}
