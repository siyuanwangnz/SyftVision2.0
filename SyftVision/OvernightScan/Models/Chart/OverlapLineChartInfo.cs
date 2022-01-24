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
    class OverlapLineChartInfo : ChartInfo
    {
        public enum Type
        {
            CPS,
            Conc
        }

        public OverlapLineChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, Type lineType, bool lastBatchSelectionEnable = true)
            : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
            LineType = lineType;
            XYData = GetXYData();
        }

        public Type LineType { get; private set; }

        private Dictionary<string, Dictionary<string, List<LineXY>>> XYData { get; set; }

        public Dictionary<string, Dictionary<string, List<LineXY>>> GetXYData()
        {
            if (this.ChartConfig == null || this.Batches == null) return null;

            Dictionary<string, Dictionary<string, List<LineXY>>> batches = new Dictionary<string, Dictionary<string, List<LineXY>>>();
            foreach (var batch in this.Batches)//batchs - batch
            {
                Dictionary<string, List<LineXY>> scans = new Dictionary<string, List<LineXY>>();
                foreach (var scan in batch.Value)//scans - scan
                {
                    List<LineXY> components = new List<LineXY>();
                    foreach (var componentInfo in this.ChartConfig.ComponentInfoList)
                    {
                        RP_Data rpData;
                        switch (LineType)
                        {
                            case Type.CPS:
                                rpData = scan.GetRP_Data(componentInfo.RPCode, this.ChartConfig.ScanPhase, Scan.FastMode.CPS);
                                if (!rpData.IsAvailable) continue;
                                components.Add(new LineXY(rpData.Reagent, rpData.Product, componentInfo.Color, rpData.CPSTimeList(), rpData.CPSList(), null));
                                break;
                            case Type.Conc:
                                rpData = scan.GetRP_Data(componentInfo.RPCode, this.ChartConfig.ScanPhase, Scan.FastMode.Conc);
                                if (!rpData.IsAvailable) continue;
                                components.Add(new LineXY(rpData.Reagent, rpData.Product, componentInfo.Color, rpData.ConcTimeList(), rpData.ConcList(), null));
                                break;
                            default:
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

            switch (LineType)
            {
                case Type.CPS:
                    c.setPlotArea(95, 50, c.getWidth() - 125, c.getHeight() - 110, 0xffffff, -1, Chart.Transparent, 0x40dddddd, 0x40dddddd);
                    // Add a title to the chart using 15pt Arial Bold font
                    c.addTitle(Chart.TopCenter, $"{this.ChartConfig.ChartName} - {this.ChartConfig.SubChartName} CPS", "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);
                    //// Add a legend box at (450, 40) (top right corner of the chart) with vertical layout
                    //// and 8 pts Arial Bold font. Set the background color to semi-transparent grey.
                    LegendBox b1 = c.addLegend(95, 50, false, "Arial Bold", 10);
                    b1.setBackground(Chart.Transparent);

                    foreach (var component in scans.Values?.First())
                    {
                        b1.addKey($"{component.Reagent}/{component.Product}", component.Color);
                    }
                    b1.setKeyBorder(Chart.SameAsMainColor);

                    c.yAxis().setTitle("cps", "Arial Bold", 10);
                    break;
                case Type.Conc:
                    c.setPlotArea(65, 50, c.getWidth() - 95, c.getHeight() - 110, 0xffffff, -1, Chart.Transparent, 0x40dddddd, 0x40dddddd);
                    // Add a title to the chart using 15pt Arial Bold font
                    c.addTitle(Chart.TopCenter, $"{this.ChartConfig.ChartName} - {this.ChartConfig.SubChartName} PPB", "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);
                    //// Add a legend box at (450, 40) (top right corner of the chart) with vertical layout
                    //// and 8 pts Arial Bold font. Set the background color to semi-transparent grey.
                    LegendBox b2 = c.addLegend(65, 50, false, "Arial Bold", 10);
                    b2.setBackground(Chart.Transparent);

                    foreach (var component in scans.Values?.First())
                    {
                        b2.addKey($"{component.Reagent}/{component.Product}", component.Color);
                    }
                    b2.setKeyBorder(Chart.SameAsMainColor);

                    c.yAxis().setTitle("ppb", "Arial Bold", 10);
                    break;
                default:
                    break;
            }
            

            // Enable clipping mode to clip the part of the data that is outside the plot area.
            c.setClipping();

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
            c.xAxis().setTitle("ms", "Arial Bold", 10);
            c.setNumberFormat(',');
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
                    layer.addDataSet(component.Y.ToArray(), component.Color, $"({component.Reagent}/{component.Product}) (Scan: {scan.Key})");
                    layer.setLegend(Chart.NoLegend);

                    switch (LineType)
                    {
                        case Type.CPS:
                            layer.setHTMLImageMap("", "", "title='{value} cps at {x} ms {dataSetName}'");
                            break;
                        case Type.Conc:
                            layer.setHTMLImageMap("", "", "title='{value} ppb at {x} ms {dataSetName}'");
                            break;
                        default:
                            break;
                    }
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
