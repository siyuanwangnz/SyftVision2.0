using ChartDirector;
using MathNet.Numerics.Statistics;
using Public.Global;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class InjectionLineChartInfo : ChartInfo
    {
        public InjectionLineChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, bool lastBatchSelectionEnable = true)
            : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
            XYData = GetXYData();
        }

        private Dictionary<string, Dictionary<string, LineXY>> XYData { get; set; }

        public Dictionary<string, Dictionary<string, LineXY>> GetXYData()
        {
            if (this.ChartConfig == null || this.Batches == null) return null;

            Dictionary<string, Dictionary<string, LineXY>> batches = new Dictionary<string, Dictionary<string, LineXY>>();
            foreach (var batch in this.Batches)//batches - batch
            {
                Dictionary<string, LineXY> scans = new Dictionary<string, LineXY>();
                foreach (var scan in batch.Value)//scans - scan
                {
                    Inj_Data inj_DaTa = scan.GetInj_Data();
                    if (inj_DaTa == null) continue;
                    scans.Add(scan.GetFileInfo().FileName, new LineXY(null, null, 0x1A237E, inj_DaTa.MassList(), inj_DaTa.UPSCurList(), null));
                }
                if (scans.Count != 0)
                    batches.Add(batch.Key, scans);
            }
            return batches;
        }

        private XYChart SingleBatchXYChartGenerator(Dictionary<string, LineXY> scans, string batchName, int firstScanID, int lastScanID, int scanCount)
        {
            if (XYData == null) return null;

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

            // Add axis title using 10pt Arial Bold font
            c.yAxis().setTitle("nA", "Arial Bold", 10);
            c.xAxis().setTitle("mass", "Arial Bold", 10);
            c.xAxis().setMargin(20, 20);

            //Add table
            if (this.ChartConfig.SubChartName == "PosWet")
            {
                List<double> baseline = new List<double>();
                List<double> mass18 = new List<double>();
                List<double> mass19 = new List<double>();
                List<double> mass30 = new List<double>();
                List<double> mass32 = new List<double>();
                foreach (var scan in scans)
                {
                    for (int i = 0; i < scan.Value.X.Count; i++)
                    {
                        if (scan.Value.X[i] <= 15 && scan.Value.X[i] >= 11)
                            baseline.Add(scan.Value.Y[i]);
                    }
                    for (int i = 0; i < scan.Value.X.Count; i++)
                    {
                        if (scan.Value.X[i] <= 18.1 && scan.Value.X[i] >= 17.9)
                            mass18.Add(scan.Value.Y[i]);
                    }
                    for (int i = 0; i < scan.Value.X.Count; i++)
                    {
                        if (scan.Value.X[i] <= 19.1 && scan.Value.X[i] >= 18.9)
                            mass19.Add(scan.Value.Y[i]);
                    }
                    for (int i = 0; i < scan.Value.X.Count; i++)
                    {
                        if (scan.Value.X[i] <= 30.1 && scan.Value.X[i] >= 29.9)
                            mass30.Add(scan.Value.Y[i]);
                    }
                    for (int i = 0; i < scan.Value.X.Count; i++)
                    {
                        if (scan.Value.X[i] <= 32.1 && scan.Value.X[i] >= 31.9)
                            mass32.Add(scan.Value.Y[i]);
                    }
                }
                var table = c.addTable(c.getWidth() - 40, 145, Chart.Right, 2, 7);
                ChartDirector.TextBox cellStyle = table.getStyle();
                cellStyle.setMargin2(5, 5, 5, 5);
                cellStyle.setFontSize(10);
                cellStyle.setFontStyle("Arial Bold");

                double mbaseline = Math.Round(Statistics.Mean(baseline), 4);
                double mmass18 = Math.Round(Statistics.Mean(mass18), 4) - mbaseline;
                double mmass19 = Math.Round(Statistics.Mean(mass19), 4) - mbaseline;
                double mmass30 = Math.Round(Statistics.Mean(mass30), 4) - mbaseline;
                double mmass32 = Math.Round(Statistics.Mean(mass32), 4) - mbaseline;
                double ratio1819 = Math.Round(mmass18 / mmass19 * 100, 4);
                double diff3032 = Math.Round(Math.Abs(mmass30 - mmass32), 4);

                if (ratio1819 <= 20)
                {
                    table.setText(0, 0, "18/19 Ratio").setBackground(unchecked((int)0x8080ff80), 0x000000);
                    table.setText(1, 0, $"{ratio1819} %").setBackground(unchecked((int)0x8080ff80), 0x000000);
                }
                else
                {
                    table.setText(0, 0, "18/19 Ratio").setBackground(unchecked((int)0x80ff8080), 0x000000);
                    table.setText(1, 0, $"{ratio1819} %").setBackground(unchecked((int)0x80ff8080), 0x000000);
                }

                if (diff3032 <= 2)
                {
                    table.setText(0, 1, "30/32 Diff").setBackground(unchecked((int)0x8080ff80), 0x000000);
                    table.setText(1, 1, $"{diff3032} nA").setBackground(unchecked((int)0x8080ff80), 0x000000);
                }
                else
                {
                    table.setText(0, 1, "30/32 Diff").setBackground(unchecked((int)0x80ff8080), 0x000000);
                    table.setText(1, 1, $"{diff3032} nA").setBackground(unchecked((int)0x80ff8080), 0x000000);
                }

                table.setText(0, 2, "Baseline");
                table.setText(1, 2, $"{mbaseline} nA");
                table.setText(0, 3, "18 Mass");
                table.setText(1, 3, $"{mmass18} nA");
                table.setText(0, 4, "19 Mass");
                table.setText(1, 4, $"{mmass19} nA");
                table.setText(0, 5, "30 Mass");
                table.setText(1, 5, $"{mmass30} nA");
                table.setText(0, 6, "32 Mass");
                table.setText(1, 6, $"{mmass32} nA");
            }
            // Add layer
            foreach (var scan in scans)
            {
                LineLayer layer = c.addLineLayer2();
                layer.setLineWidth(3);
                layer.setFastLineMode();

                layer.setXData(scan.Value.X.ToArray());
                layer.addDataSet(scan.Value.Y.ToArray(), scan.Value.Color, $"{scan.Key}");

                layer.setHTMLImageMap("", "", "title='{value} nA at {x} mass (Scan: {dataSetName})'");
            }
            return c;
        }

        public List<XYChart> XYChartList
        {
            get
            {
                if (this.XYData == null) return null;
                List<XYChart> XYChartList = new List<XYChart>();
                foreach (var batch in this.XYData)
                {
                    List<int> idList = this.Batches[batch.Key].Select(a => int.Parse(a.GetFileInfo().ScanID)).ToList();
                    XYChartList.Add(SingleBatchXYChartGenerator(batch.Value, batch.Key, idList.Min(), idList.Max(), idList.Count));
                }
                return XYChartList;
            }
        }
    }
}
