using ChartDirector;
using Public.Global;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class LODsBarChartInfo : ChartInfo
    {
        public LODsBarChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, bool lastBatchSelectionEnable = true)
            : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
            XYData = GetXYData();
        }

        public Dictionary<string, Dictionary<string, List<BarXY>>> XYData { get; set; }

        public Dictionary<string, Dictionary<string, List<BarXY>>> GetXYData()
        {
            if (this.ChartConfig == null || this.Batches == null) return null;

            Dictionary<string, Dictionary<string, List<BarXY>>> batches = new Dictionary<string, Dictionary<string, List<BarXY>>>();
            foreach (var batch in this.Batches)//batches - batch
            {
                Dictionary<string, List<BarXY>> scans = new Dictionary<string, List<BarXY>>();
                foreach (var scan in batch.Value)//scans - scan
                {
                    List<BarXY> components = new List<BarXY>();
                    foreach (var componentInfo in this.ChartConfig.ComponentInfoList)
                    {
                        RP_Data rpData = scan.GetRP_Data(componentInfo.RPCode, this.ChartConfig.ScanPhase, Scan.FastMode.Conc);
                        if (!rpData.IsAvailable)
                        {
                            components.Add(new BarXY(componentInfo.Reagent, componentInfo.Product, 0, componentInfo.Max, componentInfo.Min, this.ChartConfig.Accept));
                            continue;
                        }
                        components.Add(new BarXY(rpData.Reagent, rpData.Product, rpData.LOD(), componentInfo.Max, componentInfo.Min, this.ChartConfig.Accept));
                    }
                    scans.Add(scan.GetFileInfo().FileName, components);
                }
                batches.Add(batch.Key, scans);
            }
            return batches;
        }

        private XYChart SingleBatchXYChartGenerator(Dictionary<string, List<BarXY>> scans, string batchName, int firstScanID, int lastScanID, int scanCount)
        {
            if (XYData == null) return null;

            XYChart c = new XYChart(1114, 650, 0xccccff);

            c.setPlotArea(80, 50, c.getWidth() - 110, c.getHeight() - 110, 0xf8f8f8, 0xffffff);

            // Enable clipping mode to clip the part of the data that is outside the plot area.
            c.setClipping();

            // Add a title to the chart using 15pt Arial Bold font
            c.addTitle(Chart.TopCenter, $"{this.ChartConfig.ChartName} - {this.ChartConfig.SubChartName}", "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);

            // Add a text box
            TextBox b = c.addText(557, 30, $"Batch: {batchName}, ID: {firstScanID} - {lastScanID} ({scanCount} Scans)", "Arial Bold", 10, 0x4A235A);
            b.setAlignment(Chart.TopCenter);
            b.setBackground(Chart.Transparent);
            // Add a legend box at (450, 40) (top right corner of the chart) with vertical layout
            // and 8 pts Arial Bold font. Set the background color to semi-transparent grey.
            LegendBox b1 = c.addLegend(80, 50, false, "Arial Bold", 10);
            b1.setBackground(Chart.Transparent);
            b1.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 10pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // Add axis title using 10pt Arial Bold font
            c.yAxis().setTitle("value", "Arial Bold", 10);
            c.xAxis().setTitle("component", "Arial Bold", 10);
            // Add layer
            // 
            // Add a multi-bar layer with multi data sets
            BarLayer layer = c.addBarLayer2(Chart.Side);
            // Use soft lighting effect with light direction from the left
            //layer.setBorderColor(Chart.Transparent, Chart.softLighting(Chart.Left));
            // Set 50% overlap between bars
            layer.setOverlapRatio(0);
            int i = 0;
            foreach (var scan in scans)
            {
                c.xAxis().setLabels(scan.Value.Select(a => a.X).ToArray());
                layer.addDataSet(scan.Value.Select(a => a.Y).ToArray(), Global.ColorPool[i++], $"{scan.Key}");
            }

            layer.setHTMLImageMap("", "", "title='{value} at {xLabel} (Scan: {dataSetName})'");

            //Add backgroung layer
            BarLayer blayer1 = c.addBarLayer(scans?.First().Value.Select(a => Double.Parse(a.Max)).ToArray(), unchecked((int)0x8080ff80));
            blayer1.setBorderColor(Chart.SameAsMainColor);
            blayer1.setBarGap(0.01);
            blayer1.setHTMLImageMap("", "", "title='Upper Limit: {value} at {xLabel} - Value should be under this level!'");

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
