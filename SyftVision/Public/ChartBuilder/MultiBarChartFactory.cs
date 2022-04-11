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
    public class MultiBarChartFactory : ChartFactory
    {
        public MultiBarChartFactory(XYFactory xyFactory) : base(xyFactory)
        {
        }

        public override BaseChart SetChart(ChartProp chartProp, List<XYItem> xyItemList)
        {
            XYChart c = new XYChart(1116, 520, 0xccccff);

            c.setPlotArea(80, 50, c.getWidth() - 110, c.getHeight() - 110, 0xf8f8f8, 0xffffff);

            // Enable clipping mode to clip the part of the data that is outside the plot area.
            c.setClipping();

            // Add a title to the chart using 15pt Arial Bold font
            c.addTitle(Chart.TopCenter, chartProp.Code, "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);

            // Set the x and y axis stems to transparent and the label font to 10pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // Add axis title using 10pt Arial Bold font
            c.yAxis().setTitle("value", "Arial Bold", 10);
            c.xAxis().setTitle("component", "Arial Bold", 10);
            // Add layer
            // Add a multi-bar layer with multi data sets
            BarLayer layer = c.addBarLayer2(Chart.Side);
            //layer.setBorderColor(Chart.Transparent);
            // Use soft lighting effect with light direction from the left
            //layer.setBorderColor(Chart.Transparent, Chart.softLighting(Chart.Left));
            // Display labela on top of bars using 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 10);
            layer.setAggregateLabelFormat("{value}");
            // Set 50% overlap between bars
            layer.setOverlapRatio(0);


            foreach (var xyItem in xyItemList)
            {
                c.xAxis().setLabels(xyItem.SingleLayer.LabelList.ToArray());
                layer.addDataSet(xyItem.SingleLayer.YList.ToArray(), xyItem.XYLegend.Color, xyItem.XYLegend.Content);
            }

            layer.setHTMLImageMap("", "", "title='{value} at {xLabel} (Scan: {dataSetName})'");

            //Add backgroung layer
            BarLayer blayer1 = c.addBarLayer(chartProp.ComponentList.Select(a => a.Limit).ToArray(), unchecked((int)0x80ff8080));
            blayer1.setBorderColor(Chart.SameAsMainColor);
            blayer1.setBarGap(0.01);
            blayer1.setHTMLImageMap("", "", "title='Lower Limit: {value} at {xLabel} - Value should be above this level!'");

            return c;
        }
    }
}
