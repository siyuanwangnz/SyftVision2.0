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
    public class MixChartFactoryS : ChartFactoryS
    {
        public MixChartFactoryS(XYFactoryS xyFactoryS) : base(xyFactoryS)
        {
        }

        public override BaseChart SetChart(Setting setting, XYItemS xyItemS)
        {
            XYChart c = new XYChart(1000, 500);

            if (xyItemS == null) return null;

            // Set chart position, size and style
            c.setPlotArea(60, 30, c.getWidth() - 90, c.getHeight() - 70, 0xf8f8f8, 0xffffff);

            // Add a title
            c.addTitle(Chart.TopCenter, setting.Name, "Arial Bold", 16);

            // Set x y axis stems and the label font
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // Set x axis
            c.xAxis().setLabels(xyItemS.LabelList.ToArray());

            // Add line layers
            foreach (var yLayer in xyItemS.YLayerList)
            {
                LineLayer lineLayer = c.addLineLayer2();
                lineLayer.setLineWidth(2);
                lineLayer.setFastLineMode();
                lineLayer.addDataSet(yLayer.YList.ToArray(), yLayer.XYLegend.Color, yLayer.XYLegend.Content).setDataSymbol(Chart.CircleShape, 9, yLayer.XYLegend.Color, yLayer.XYLegend.Color);
                lineLayer.setHTMLImageMap("", "", "title='{value} & {xLabel} ({dataSetName})'");
            }

            // Add a multi-bar layer
            BarLayer layer = c.addBarLayer2(Chart.Side);
            // Set 50% overlap between bars
            layer.setOverlapRatio(0.5);
            // Set border color
            layer.setBorderColor(Chart.Transparent);
            // Add data
            foreach (var yLayer in xyItemS.YLayerList)
            {
                layer.addDataSet(yLayer.YList.ToArray(), unchecked((int)(yLayer.XYLegend.Color + 0x80000000)), yLayer.XYLegend.Content);
            }
            layer.setHTMLImageMap("", "", "title='{value} & {xLabel} ({dataSetName})'");

            // Upper limit mark
            BoxWhiskerLayer markLayer1 = c.addBoxWhiskerLayer(null, null, null, null, xyItemS.UpperList.ToArray(), -1, 0x8080ff);
            markLayer1.setLineWidth(3);
            markLayer1.setDataGap(0.1);
            // Tool tip for the mark layer
            markLayer1.setHTMLImageMap("", "", "title='Upper Limit: {med} at {xLabel}'");

            // Under limit mark
            BoxWhiskerLayer markLayer2 = c.addBoxWhiskerLayer(null, null, null, null, xyItemS.UnderList.ToArray(), -1, 0xff8080);
            markLayer2.setLineWidth(3);
            markLayer2.setDataGap(0.1);
            // Tool tip for the mark layer
            markLayer2.setHTMLImageMap("", "", "title='Under Limit: {med} at {xLabel}'");

            return c;
        }
    }
}