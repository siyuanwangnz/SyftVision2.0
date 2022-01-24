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
    class DevBarChartInfo: ChartInfo
    {
        public enum Type
        {
            CPS,
            Sens
        }

        public DevBarChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, Type devType, bool lastBatchSelectionEnable = true)
           : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
            DevType = devType;
            XYData = GetXYData();
        }

        public Type DevType { get; private set; }

        private Dictionary<string, List<BarXY>> XYData { get; set; }

        public Dictionary<string, List<BarXY>> GetXYData()
        {
            if (this.ChartConfig == null || this.Batches == null) return null;

            Dictionary<string, List<BarXY>> batches = new Dictionary<string, List<BarXY>>();

            foreach (var batch in this.Batches)//batchs - batch
            {
                List<BarXY> components = new List<BarXY>();
                foreach (var componentInfo in this.ChartConfig.ComponentInfoList)
                {
                    RP_Data firstRPData;
                    RP_Data lastRPData;
                    switch (DevType)
                    {
                        case Type.CPS:
                            firstRPData = batch.Value.First().GetRP_Data(componentInfo.RPCode, this.ChartConfig.ScanPhase, Scan.FastMode.CPS);
                            lastRPData = batch.Value.Last().GetRP_Data(componentInfo.RPCode, this.ChartConfig.ScanPhase, Scan.FastMode.CPS);
                            if (!firstRPData.IsAvailable || !lastRPData.IsAvailable) continue;
                            components.Add(new BarXY(firstRPData.Reagent, firstRPData.Product, MyMath.Deviation(firstRPData.CPSMean(), lastRPData.CPSMean()), componentInfo.Max, componentInfo.Min, this.ChartConfig.Accept));
                            break;
                        case Type.Sens:
                            firstRPData = batch.Value.First().GetRP_Data(componentInfo.RPCode, this.ChartConfig.ScanPhase, Scan.FastMode.Sensitivity);
                            lastRPData = batch.Value.Last().GetRP_Data(componentInfo.RPCode, this.ChartConfig.ScanPhase, Scan.FastMode.Sensitivity);
                            if (!firstRPData.IsAvailable || !lastRPData.IsAvailable) continue;
                            components.Add(new BarXY(firstRPData.Reagent, firstRPData.Product, MyMath.Deviation(firstRPData.Sensitivity(), lastRPData.Sensitivity()), componentInfo.Max, componentInfo.Min, this.ChartConfig.Accept));
                            break;
                        default:
                            break;
                    }
                }
                batches.Add(batch.Key, components);
            }
            return batches;
        }

        private XYChart SingleBatchXYChartGenerator(List<BarXY> scans, string batchName, int firstScanID, int lastScanID, int scanCount)
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

            // Set the x and y axis stems to transparent and the label font to 10pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);
            // Set the y axis label format to nn%
            c.yAxis().setLabelFormat("{value}%");

            // Add axis title using 10pt Arial Bold font
            c.yAxis().setTitle("precentage", "Arial Bold", 10);
            c.xAxis().setTitle("component", "Arial Bold", 10);

            //
            // Add layer
            // 
            // Set the x axis labels
            c.xAxis().setLabels(scans.Select(a => a.X).ToArray());
            // Draw the ticks between label positions (instead of at label positions)
            c.xAxis().setTickOffset(0.5);
            //mark
            if (scans?[0].Max != null)
            {
                BoxWhiskerLayer markLayer2 = c.addBoxWhiskerLayer(null, null, null, null, scans.Select(a => Math.Round(Double.Parse(a.Max) * 100, 2)).ToArray(), -1, 0xff8080);
                markLayer2.setLineWidth(3);
                markLayer2.setDataGap(0);

                markLayer2.setHTMLImageMap("", "", "title='Upper Limit: {med} % at {xLabel} - Value should be under this level!'");
            }
            //Add data and matched color
            BarLayer layer = c.addBarLayer3(scans.Select(a => Math.Round(a.Y * 100, 2)).ToArray(), scans.Select(a => a.Color).ToArray());
            // Use soft lighting effect with light direction from the left
            //layer.setBorderColor(Chart.Transparent, Chart.softLighting(Chart.Left));
            // Display labela on top of bars using 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);
            layer.setAggregateLabelFormat("{value}%");
            layer.setHTMLImageMap("", "", "title='{value} % at {xLabel}'");

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
