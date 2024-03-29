﻿using ChartDirector;
using Public.ChartBuilder.XY;
using Public.ChartConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder
{
    public class BarChartFactory : ChartFactory
    {
        public BarChartFactory(XYFactory xyFactory) : base(xyFactory)
        {
        }

        public override BaseChart SetChart(ChartProp chartProp, List<XYItem> xyItemList)
        {
            XYChart c = new XYChart(1116, 520);

            // Set chart position, size and style
            c.setPlotArea(60, 30, c.getWidth() - 90, c.getHeight() - 70, 0xf8f8f8, 0xffffff);

            // Add a title
            c.addTitle(Chart.TopCenter, chartProp.Code, "Arial Bold", 16);

            // Set x y axis stems and the label font
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // Add a multi-bar layer
            BarLayer layer = c.addBarLayer2(Chart.Side);
            // Set 0% overlap between bars
            layer.setOverlapRatio(0);
            // Add data
            foreach (var xyItem in xyItemList)
            {
                c.xAxis().setLabels(xyItem.SingleLayer.LabelList.ToArray());
                layer.addDataSet(xyItem.SingleLayer.YList.ToArray(), xyItem.XYLegend.Color, xyItem.XYLegend.Content);
            }
            layer.setHTMLImageMap("", "", "title='{value} & {xLabel} ({dataSetName})'");

            //Add backgroung layer
            BarLayer blayer1 = c.addBarLayer(chartProp.ComponentList.Select(a => a.Limit).ToArray(), chartProp.ExpectedRangeColor);
            blayer1.setBorderColor(Chart.SameAsMainColor);
            blayer1.setBarGap(0.01);
            blayer1.setHTMLImageMap("", "", "title='Limit: {value} & {xLabel}'");

            return c;
        }
    }
}
