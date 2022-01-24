using ChartDirector;
using Public.Global;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class MassLineChartInfo : ChartInfo
    {
        public MassLineChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, bool lastBatchSelectionEnable = true)
            : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
            XYData = GetXYData();
        }

        private Dictionary<string, Dictionary<string, LineXY>> XYData { get; set; }

        public Dictionary<string, Dictionary<string, LineXY>> GetXYData()
        {
            if (this.ChartConfig == null || this.Batches == null) return null;

            Dictionary<string, Dictionary<string, LineXY>> batches = new Dictionary<string, Dictionary<string, LineXY>>();
            foreach (var batch in this.Batches)//batchs - batch
            {
                Dictionary<string, LineXY> scans = new Dictionary<string, LineXY>();
                foreach (var scan in batch.Value)//scans - scan
                {
                    List<string> X = new List<string>();
                    List<double> Y = new List<double>();
                    foreach (var componentInfo in this.ChartConfig.ComponentInfoList)
                    {
                        Mass_Data massData = scan.GetMass_Data(componentInfo.Reagent, componentInfo.Product, this.ChartConfig.ScanPhase);
                        if (massData == null) continue;
                        X.AddRange(massData.CPSMassList());
                        Y.AddRange(massData.CPSList());
                    }
                    scans.Add(scan.GetFileInfo().FileName, new LineXY(null, null, -1, null, Y, X));
                }
                batches.Add(batch.Key, scans);
            }
            return batches;
        }

        private XYChart SingleBatchXYChartGenerator(Dictionary<string, LineXY> scans, string batchName, int firstScanID, int lastScanID, int scanCount)
        {
            if (XYData == null) return null;

            XYChart c = new XYChart(1114, 650, 0xccccff);

            c.setPlotArea(95, 50, c.getWidth() - 125, c.getHeight() - 150, 0xffffff, -1, Chart.Transparent, 0x40dddddd, 0x40dddddd);

            // Enable clipping mode to clip the part of the data that is outside the plot area.
            c.setClipping();

            // Add a title to the chart using 15pt Arial Bold font
            c.addTitle(Chart.TopCenter, $"{this.ChartConfig.ChartName} - {this.ChartConfig.SubChartName}", "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);

            // Add a text box
            TextBox b = c.addText(557, 30, $"Batch: {batchName}, ID: {firstScanID} - {lastScanID} ({scanCount} Scans)", "Arial Bold", 10, 0x4A235A);
            b.setAlignment(Chart.TopCenter);
            b.setBackground(Chart.Transparent);

            // Set the x and y axis stems to transparent and the label font to 10pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // Add axis title using 10pt Arial Bold font
            c.yAxis().setTitle("cps", "Arial Bold", 10);
            c.xAxis().setTitle("mass", "Arial Bold", 10);
            c.setNumberFormat(',');

            // Add layer
            foreach (var scan in scans)
            {
                c.xAxis().setLabels(scan.Value.Label.ToArray());
                c.xAxis().setLabelStyle("Arial", 8, Chart.TextColor, -90);

                LineLayer layer = c.addLineLayer2();
                layer.setLineWidth(1);
                layer.setFastLineMode();
                layer.addDataSet(scan.Value.Y.ToArray(), scan.Value.Color, $"Scan: {scan.Key}");
                layer.setHTMLImageMap("", "", "title='{value} cps at {xLabel} mass ({dataSetName})'");
            }

            // Set step of labels
            int count = (int)Math.Round((double)scans.Values.Max(a => a.Label.Count) / 25);
            c.xAxis().setLabelStep(count, 1);

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
