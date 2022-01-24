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
    class CurrentLineChartInfo : ChartInfo
    {
        public enum Type
        {
            UPS,
            DWS
        }

        public CurrentLineChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, Type currentType, bool lastBatchSelectionEnable = true)
           : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
            CurrentType = currentType;
            XYData = GetXYData();
        }

        public Type CurrentType { get; private set; }

        private Dictionary<string, Dictionary<string, List<LineXY>>> XYData { get; set; }

        public Dictionary<string, Dictionary<string, List<LineXY>>> GetXYData()
        {
            if (this.ChartConfig == null || this.Batches == null) return null;

            Dictionary<string, Dictionary<string, List<LineXY>>> batches = new Dictionary<string, Dictionary<string, List<LineXY>>>();
            foreach (var batch in this.Batches)//batches - batch
            {
                Dictionary<string, List<LineXY>> scans = new Dictionary<string, List<LineXY>>();
                foreach (var scan in batch.Value)//scans -scan
                {
                    List<LineXY> components = new List<LineXY>();
                    foreach (var componentInfo in this.ChartConfig.ComponentInfoList)
                    {
                        RC_Data rcData = scan.GetRC_Data(componentInfo.Reagent);
                        if (rcData == null) continue;
                        switch (CurrentType)
                        {
                            default:
                            case Type.UPS:
                                components.Add(new LineXY(rcData.Reagent, rcData.Product, componentInfo.Color, rcData.CurTimeList(), rcData.UPSCurList(), null));
                                break;
                            case Type.DWS:
                                components.Add(new LineXY(rcData.Reagent, rcData.Product, componentInfo.Color, rcData.CurTimeList(), rcData.DWSCurList(), null));
                                break;
                        }
                    }
                    if (components.Count != 0)
                        scans.Add(scan.GetFileInfo().FileName, components);
                }
                batches.Add(batch.Key, scans);
            }
            return batches;
        }

        private XYChart SingleBatchXYChartGenerator(Dictionary<string, List<LineXY>> scans, string batchName, int firstScanID, int lastScanID, int scanCount)
        {
            if (XYData == null) return null;

            XYChart c = new XYChart(1114, 650, 0xccccff);

            c.setPlotArea(60, 50, c.getWidth() - 90, c.getHeight() - 110, 0xffffff, -1, Chart.Transparent, 0x40dddddd, 0x40dddddd);

            // Enable clipping mode to clip the part of the data that is outside the plot area.
            c.setClipping();

            // Add a title to the chart using 15pt Arial Bold font
            switch (CurrentType)
            {
                case Type.UPS:
                    c.addTitle(Chart.TopCenter, $"{this.ChartConfig.ChartName} - {this.ChartConfig.SubChartName} UPS Current", "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);
                    c.yAxis().setTitle("nA", "Arial Bold", 10);
                    break;
                case Type.DWS:
                    c.addTitle(Chart.TopCenter, $"{this.ChartConfig.ChartName} - {this.ChartConfig.SubChartName} DWS Current", "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);
                    c.yAxis().setTitle("pA", "Arial Bold", 10);
                    break;
                default:
                    break;
            }

            // Add a text box
            TextBox b = c.addText(557, 30, $"Batch: {batchName}, ID: {firstScanID} - {lastScanID} ({scanCount} Scans)", "Arial Bold", 10, 0x4A235A);
            b.setAlignment(Chart.TopCenter);
            b.setBackground(Chart.Transparent);
            //// Add a legend box at (450, 40) (top right corner of the chart) with vertical layout
            //// and 8 pts Arial Bold font. Set the background color to semi-transparent grey.
            LegendBox b1 = c.addLegend(60, 50, false, "Arial Bold", 10);
            b1.setBackground(Chart.Transparent);

            foreach (var component in scans.Values?.First())
            {
                b1.addKey($"{component.Reagent}/{component.Product}", component.Color);
            }
            b1.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 10pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // Add axis title using 10pt Arial Bold font
            c.xAxis().setTitle("ms", "Arial Bold", 10);
            //
            // Add layer
            // 
            foreach (var scan in scans)
            {
                foreach (var component in scan.Value)
                {
                    LineLayer layer = c.addLineLayer2();
                    layer.setLineWidth(1);
                    layer.setFastLineMode();

                    layer.setXData(component.X.ToArray());
                    layer.addDataSet(component.Y.ToArray(), component.Color, $"({component.Reagent}/{component.Product})(Scan: {scan.Key})");
                    layer.setLegend(Chart.NoLegend);

                    layer.setHTMLImageMap("", "", "title='{value} nA at {x} ms {dataSetName}'");
                }
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
