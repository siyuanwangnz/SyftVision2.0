using ChartDirector;
using MathNet.Numerics.Statistics;
using Public.Global;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class ReactionTimeLineChartInfo : ChartInfo
    {
        public ReactionTimeLineChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, bool lastBatchSelectionEnable = true)
            : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
        }

        private XYChart SingleBatchXYChartGenerator(List<Scan> scans, string batchName, int firstScanID, int lastScanID, int scanCount)
        {
            if (this.Batches == null) return null;

            XYChart c = new XYChart(1114, 650, 0xccccff);

            c.setPlotArea(80, 50, c.getWidth() - 110, c.getHeight() - 180, 0xffffff, -1, Chart.Transparent, 0x40dddddd, 0x40dddddd);

            // Enable clipping mode to clip the part of the data that is outside the plot area.
            c.setClipping();

            // Add a title to the chart using 15pt Arial Bold font
            c.addTitle(Chart.TopCenter, $"{this.ChartConfig.ChartName} - {this.ChartConfig.SubChartName} Reaction Time", "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);

            // Add a text box
            TextBox b = c.addText(557, 30, $"Batch: {batchName}, ID: {firstScanID} - {lastScanID} ({scanCount} Scans)", "Arial Bold", 10, 0x4A235A);
            b.setAlignment(Chart.TopCenter);
            b.setBackground(Chart.Transparent);
            // Add a legend box at (450, 40) (top right corner of the chart) with vertical layout
            // and 8 pts Arial Bold font. Set the background color to semi-transparent grey.
            //LegendBox b1 = c.addLegend(60, 40, false, "Arial Bold", 8);
            ////b.setAlignment(Chart.Left);
            //b1.setBackground(0x40dddddd);
            //b1.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 10pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);
            //Add zone
            double meanY = Statistics.Mean(scans.Select(a => a.GetReactionTime()).ToList());
            c.yAxis().addZone(meanY * 0.98, meanY * 1.02, unchecked((int)0x8080ff80));

            // Add axis title using 10pt Arial Bold font
            c.yAxis().setTitle("ms", "Arial Bold", 10);
            c.xAxis().setTitle("scan time", "Arial Bold", 10);
            c.xAxis().setMargin(20, 20);
            // Set the y axis label format to nn%
            c.yAxis().setLabelFormat("{value|4}");
            //
            // Add layer
            // 
            c.xAxis().setLabels(scans.Select(a => $"{a.GetFileInfo().ScanDate}\r\n(ID - {a.GetFileInfo().ScanID})").ToArray());
            c.xAxis().setLabelStyle("Arial", 8, Chart.TextColor, -90);
            // Display 1 out of 3 labels on the x-axis. Show minor ticks for remaining labels.
            int count = scans.Count;
            if (count >= 60 && count < 90)
                c.xAxis().setLabelStep(3, 1);
            if (count >= 90 && count < 120)
                c.xAxis().setLabelStep(4, 1);
            if (count >= 120)
                c.xAxis().setLabelStep(5, 1);

            LineLayer layer = c.addLineLayer2();
            if (count >= 40)
            {
                layer.setLineWidth(2);
                layer.setFastLineMode();
                layer.addDataSet(scans.Select(a => a.GetReactionTime()).ToArray(), 0x1A237E).setDataSymbol(Chart.SquareShape, 5, 0x1A237E, 0x1A237E);
            }
            else
            {
                layer.setLineWidth(3);
                layer.setFastLineMode();
                layer.setDataLabelFormat("{value|2}");
                layer.setDataLabelStyle("Arial", 10);
                layer.addDataSet(scans.Select(a => a.GetReactionTime()).ToArray(), 0x1A237E).setDataSymbol(Chart.SquareShape, 8, 0x1A237E, 0x1A237E);
            }
            layer.setHTMLImageMap("", "", "title='{value} ms at {xLabel}'");

            return c;
        }

        public List<XYChart> XYChartList
        {
            get
            {
                if (this.Batches == null) return null;
                List<XYChart> XYChartList = new List<XYChart>();
                foreach (var batch in this.Batches)
                {
                    List<int> idList = batch.Value.Select(a => int.Parse(a.GetFileInfo().ScanID)).ToList();
                    XYChartList.Add(SingleBatchXYChartGenerator(batch.Value, batch.Key, idList.Min(), idList.Max(), idList.Count));
                }
                return XYChartList;
            }
        }
    }
}
