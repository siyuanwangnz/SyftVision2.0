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
    public class TextChartFactoryS : ChartFactoryS
    {
        public TextChartFactoryS(XYFactoryS xyFactoryS) : base(xyFactoryS)
        {
        }
        public override BaseChart SetChart(Setting setting, XYItemS xyItemS)
        {
            XYChart c = new XYChart(1000, 500);

            if (xyItemS == null) return null;

            // Set chart position, size and style
            c.setPlotArea(150, 30, c.getWidth() - 200, c.getHeight() - 70, Chart.Transparent, Chart.Transparent, Chart.Transparent, Chart.Transparent, Chart.Transparent);

            // Add a title
            c.addTitle(Chart.TopCenter, setting.Name, "Arial Bold", 16);

            // Set x y axis stems and the label font
            c.xAxis().setColors(Chart.Transparent, Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent, Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);

            // Swap the axis so that the bars are drawn horizontally
            c.swapXY(true);
            // Reverse x axis
            c.xAxis().setReverse(true);
            // Set y axis limit
            c.yAxis().setLinearScale(0, 10, 5);

            // Set x axis
            c.xAxis().setLabels(xyItemS.LabelList.ToArray());

            // Set bar layer
            BarLayer layer = c.addBarLayer3(xyItemS.YLayerList.Select(a => a.YList.First()).ToArray(), xyItemS.YLayerList.Select(a => a.XYLegend.Color).ToArray());
            // Set the bar gap to 10%
            layer.setBarGap(0.1);
            // Set border color
            layer.setBorderColor(Chart.Transparent);
            // Set bar label
            int i = 0;
            foreach (var label in xyItemS.YLayerList.Select(a => a.XYLegend.Content))
            {
                layer.addCustomDataLabel(i, i, label, "Arial Bold", 12, 0xffffff);
                i++;
            }

            return c;
        }
    }
}
