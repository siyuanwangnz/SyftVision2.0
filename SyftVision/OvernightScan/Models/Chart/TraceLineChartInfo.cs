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
    class TraceLineChartInfo : ChartInfo
    {
        public enum Type
        {
            CPS,
            Conc
        }

        public TraceLineChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, Type lineType, bool lastBatchSelectionEnable = true)
            : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
            LineType = lineType;
            XYData = GetXYData();
        }

        public Type LineType { get; private set; }

        private Dictionary<string, List<LineXY>> XYData { get; set; }

        public Dictionary<string, List<LineXY>> GetXYData()
        {
            if (this.ChartConfig == null || this.Batches == null) return null;

            Dictionary<string, List<LineXY>> batches = new Dictionary<string, List<LineXY>>();
            foreach (var batch in this.Batches)//batch - scans
            {
                List<LineXY> components = new List<LineXY>();
                foreach (var rpCode in this.ChartConfig.ComponentInfoList.Select(a => a.RPCode).ToList())
                {
                    string reagent = "";
                    string product = "";
                    List<double> Y = new List<double>();
                    List<string> Label = new List<string>();
                    foreach (var scan in batch.Value)//scans - scan
                    {
                        RP_Data rpData;
                        switch (LineType)
                        {
                            case Type.CPS:
                                rpData = scan.GetRP_Data(rpCode, this.ChartConfig.ScanPhase, Scan.FastMode.CPS);
                                if (!rpData.IsAvailable)
                                {
                                    Y.Add(rpData.CPSMean());
                                    break;
                                }
                                reagent = rpData.Reagent;
                                product = rpData.Product;
                                Y.Add(rpData.CPSMean());
                                break;
                            case Type.Conc:
                                rpData = scan.GetRP_Data(rpCode, this.ChartConfig.ScanPhase, Scan.FastMode.Conc);
                                if (!rpData.IsAvailable)
                                {
                                    Y.Add(rpData.ConcMean());
                                    break;
                                }
                                reagent = rpData.Reagent;
                                product = rpData.Product;
                                Y.Add(rpData.ConcMean());
                                break;
                            default:
                                break;
                        }
                        Label.Add($"{scan.GetFileInfo().ScanDate}\r\n(ID - {scan.GetFileInfo().ScanID})");
                    }
                    if (Y.Count != 0 && Label.Count != 0)
                        components.Add(new LineXY(reagent, product, -1, null, Y, Label));
                }
                batches.Add(batch.Key, components);
            }
            return batches;
        }

        private XYChart SingleBatchXYChartGenerator(List<LineXY> lineList, string batchName, int firstScanID, int lastScanID, int scanCount)
        {
            if (this.XYData == null) return null;

            XYChart c = new XYChart(1114, 650, 0xccccff);
            string unit = "";
            switch (LineType)
            {
                case Type.CPS:
                    c.setPlotArea(95, 50, c.getWidth() - 125, c.getHeight() - 180, 0xffffff, -1, Chart.Transparent, 0x40dddddd, 0x40dddddd);
                    // Add a legend box 
                    LegendBox b1 = c.addLegend(95, 50, false, "Arial Bold", 10);
                    //b.setAlignment(Chart.Left);
                    b1.setBackground(Chart.Transparent);
                    b1.setKeyBorder(Chart.SameAsMainColor);
                    unit = "cps";
                    break;
                default:
                case Type.Conc:
                    c.setPlotArea(65, 50, c.getWidth() - 95, c.getHeight() - 180, 0xffffff, -1, Chart.Transparent, 0x40dddddd, 0x40dddddd);
                    // Add a legend box 
                    LegendBox b2 = c.addLegend(65, 50, false, "Arial Bold", 10);
                    //b.setAlignment(Chart.Left);
                    b2.setBackground(Chart.Transparent);
                    b2.setKeyBorder(Chart.SameAsMainColor);
                    unit = "ppb";
                    break;
            }
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
            c.yAxis().setTitle(unit, "Arial Bold", 10);
            c.xAxis().setTitle("scan date", "Arial Bold", 10);
            c.xAxis().setMargin(10, 10);
            c.setNumberFormat(',');

            foreach (var line in lineList)
            {
                c.xAxis().setLabels(line.Label.ToArray());
                c.xAxis().setLabelStyle("Arial", 8, Chart.TextColor, -90);

                LineLayer layer = c.addLineLayer2();
                layer.setLineWidth(3);
                layer.setFastLineMode();
                layer.addDataSet(line.Y.ToArray(), line.Color, $"{line.Reagent}/{line.Product}");
                switch (LineType)
                {
                    case Type.CPS:
                        layer.setHTMLImageMap("", "", "title='{value} cps ({dataSetName}) at {xLabel}'");
                        break;
                    case Type.Conc:
                        layer.setHTMLImageMap("", "", "title='{value} ppb ({dataSetName}) at {xLabel}'");
                        break;
                    default:
                        break;
                }
                
            }
            // Set step of labels
            int count = (int)Math.Round((double)lineList.Max(a => a.Label.Count) / 25);
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
