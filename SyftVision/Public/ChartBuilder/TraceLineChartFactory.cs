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
    public class TraceLineChartFactory : ChartFactory
    {
        public TraceLineChartFactory(XYFactory xyFactory) : base(xyFactory)
        {
        }

        public override BaseChart SetChart(ChartProp chartProp, List<XYItem> xyItemList)
        {
            XYChart c = new XYChart(1116, 520);

            // Set chart position, size and style
            c.setPlotArea(80, 30, c.getWidth() - 110, c.getHeight() - 100, -1, -1, -1, 0x40dddddd, 0x40dddddd);

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
                c.xAxis().setLabels(xyItem.SingleLayer.LabelList.ToArray());
                c.xAxis().setLabelStyle("Arial", 8, Chart.TextColor, 30);

                LineLayer layer = c.addLineLayer2();
                layer.setLineWidth(1);
                layer.setFastLineMode();
                layer.addDataSet(xyItem.SingleLayer.YList.ToArray(), xyItem.XYLegend.Color, xyItem.XYLegend.Content);
                layer.setHTMLImageMap("", "", "title='{value} & {xLabel} ({dataSetName})'");
            }
            // Set step of labels
            int step = (int)Math.Round((double)xyItemList.Max(a => a.SingleLayer.LabelList.Count) / 40);
            c.xAxis().setLabelStep(step);

            return c;
        }
    }
}
