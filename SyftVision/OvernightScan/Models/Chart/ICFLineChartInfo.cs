using ChartDirector;
using OvernightScan.Services;
using Public.Global;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class ICFLineChartInfo : ChartInfo
    {
        public ICFLineChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, bool lastBatchSelectionEnable = true)
            : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
        }

        private XYChart SingleBatchXYChartGenerator(List<Scan> scans, string batchName, int firstScanID, int lastScanID, int scanCount)
        {
            if (this.Batches == null) return null;

            XYChart c = new XYChart(1114, 650, 0xccccff);

            c.setPlotArea(60, 50, c.getWidth() - 90, c.getHeight() - 110, 0xffffff, -1, Chart.Transparent, 0x40dddddd, 0x40dddddd);

            // Enable clipping mode to clip the part of the data that is outside the plot area.
            c.setClipping();

            // Add a title to the chart using 15pt Arial Bold font
            c.addTitle(Chart.TopCenter, $"{this.ChartConfig.ChartName} - {this.ChartConfig.SubChartName}", "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);


            // Add a text box
            TextBox b = c.addText(557, 30, $"Batch: {batchName}, ID: {firstScanID} - {lastScanID} ({scanCount} Scans)", "Arial Bold", 10, 0x4A235A);
            b.setAlignment(Chart.TopCenter);
            b.setBackground(Chart.Transparent);
            //// Add a legend box at (450, 40) (top right corner of the chart) with vertical layout
            //// and 8 pts Arial Bold font. Set the background color to semi-transparent grey.
            LegendBox b1 = c.addLegend(60, 50, false, "Arial Bold", 10);
            b1.setBackground(Chart.Transparent);
            b1.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 10pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);
            c.yAxis().setMargin(20, 20);

            // Add axis title using 10pt Arial Bold font
            c.yAxis().setTitle("factor", "Arial Bold", 10);
            c.xAxis().setTitle("mass", "Arial Bold", 10);
            c.xAxis().setMargin(20, 20);
            //
            // Add layer
            //
            //last icf line
            var lastscan = scans.Last();
            LineLayer layer = c.addLineLayer2();
            layer.setLineWidth(2);
            layer.setFastLineMode();

            layer.setXData(lastscan.GetICF_Table().Keys.ToArray());
            layer.addDataSet(lastscan.GetICF_Table().Values.ToArray(), 0x1A237E, $" {lastscan.GetFileInfo().FileName}").setDataSymbol(Chart.SquareSymbol, 6, 0x1A237E, 0x1A237E);

            layer.setHTMLImageMap("", "", "title='{value} at {x} mass (Scan: {dataSetName})'");

            // Enable data label on the data points
            var t = layer.setDataLabelStyle("Arial Bold", 10);
            t.setPos(0, -10);
            layer.setDataLabelFormat("{value|2}");
            for (int i = 3; i < lastscan.GetICF_Table().Count; i++)
            {
                if (lastscan.GetICF_Table().Keys.ToArray()[i] == -17)
                {
                    layer.addCustomDataLabel(0, i, lastscan.GetICF_Table().Values.ToArray()[i].ToString("0.##"), "Arial Bold", 10).setPos(-20, 0);
                    continue;
                }
                if (lastscan.GetICF_Table().Keys.ToArray()[i] == 19)
                {
                    layer.addCustomDataLabel(0, i, lastscan.GetICF_Table().Values.ToArray()[i].ToString("0.##"), "Arial Bold", 10).setPos(-20, 0);
                    continue;
                }
                if (lastscan.GetICF_Table().Keys.ToArray()[i] == -28)
                {
                    layer.addCustomDataLabel(0, i, lastscan.GetICF_Table().Values.ToArray()[i].ToString("0.##"), "Arial Bold", 10).setPos(7, -5);
                    continue;
                }
                layer.addCustomDataLabel(0, i, lastscan.GetICF_Table().Values.ToArray()[i].ToString("0.##"), "Arial Bold", 10).setPos(0, i % 2 == 1 ? 25 : -10);
            }

            // Add a line layer to the chart with two dark green (338033) data sets, and a line
            // width of 2 pixels
            double[] upperY = { 10, 3.5, 2, 0.8, 0.62, 0.65, 0.8, 1, 1, 9, 15, 3, 1, 1, 0.8, 0.65, 0.62, 0.8, 2, 3.5, 10 };
            double[] lowerY = { 2, 1.5, 1, 0.5, 0.45, 0.48, 0.55, 1, 1, 2, 4, 1.5, 1, 1, 0.55, 0.48, 0.45, 0.5, 1, 1.5, 2 };
            double[] zoneX = { -400, -236, -186, -150, -92, -78, -57, -32, -28, -17, -16, 19, 28, 32, 57, 78, 92, 150, 186, 236, 400 };
            LineLayer lineLayer = c.addLineLayer2();
            lineLayer.addDataSet(upperY, unchecked((int)0x8099ff99), "Target Zone");
            lineLayer.addDataSet(lowerY, unchecked((int)0x8099ff99));
            lineLayer.setXData(zoneX);
            lineLayer.setLineWidth(1);

            c.addInterLineLayer(lineLayer.getLine(0), lineLayer.getLine(1),
            unchecked((int)0x8099ff99), unchecked((int)0x8099ff99));

            // If the spline line gets above the upper zone line, color to area between the lines
            // red (ff0000)
            c.addInterLineLayer(layer.getLine(0), lineLayer.getLine(0), unchecked((int)0x80ff0000),
                Chart.Transparent);

            // If the spline line gets below the lower zone line, color to area between the lines
            // blue (0000ff)
            c.addInterLineLayer(layer.getLine(0), lineLayer.getLine(1), Chart.Transparent,
                unchecked((int)0x800000ff));

            lineLayer.setHTMLImageMap("", "", "title='Limit: {value} at {x} mass'");

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
