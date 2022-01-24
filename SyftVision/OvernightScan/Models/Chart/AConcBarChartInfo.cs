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
    class AConcBarChartInfo : ChartInfo
    {
        public enum Type
        {
            Compounds75,
            Compounds51_52
        }

        public AConcBarChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, Type type, bool lastBatchSelectionEnable = true)
            : base(batchType, chartName, subCharName, targetScanInfoListDic, lastBatchSelectionEnable)
        {
            CompoundType = type;
            XYData = GetXYData();
        }

        public Type CompoundType { get; private set; }
        public string MeanUpperLimit
        {
            get
            {
                switch (CompoundType)
                {
                    default:
                    case Type.Compounds75:
                        return "1";
                    case Type.Compounds51_52:
                        return "2";
                }

            }
        }
        public string LODUpperLimit
        {
            get
            {
                switch (CompoundType)
                {
                    default:
                    case Type.Compounds75:
                        return "0.1";
                    case Type.Compounds51_52:
                        return "0.5";
                }

            }
        }

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, BarXY>>>> XYData { get; set; }

        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, BarXY>>>> GetXYData()
        {
            if (this.ChartConfig == null || this.Batches == null) return null;
            //batches - batch -> scans - scan -> components - component -> lod and conc
            Dictionary<string, Dictionary<string, Dictionary<string, List<BarXY>>>> batches = new Dictionary<string, Dictionary<string, Dictionary<string, List<BarXY>>>>();
            foreach (var batch in this.Batches)//batches - batch
            {
                Dictionary<string, Dictionary<string, List<BarXY>>> scans = new Dictionary<string, Dictionary<string, List<BarXY>>>();
                foreach (var scan in batch.Value)//scans - scan
                {
                    Dictionary<string, List<BarXY>> components = new Dictionary<string, List<BarXY>>();

                    List<AC_Data> acDatalist = scan.GetAC_DataList(this.ChartConfig.ScanPhase);
                    if (acDatalist == null) continue;
                    foreach (var acData in acDatalist)
                    {
                        List<BarXY> results = new List<BarXY>();
                        var componentInfo = this.ChartConfig.ComponentInfoList.Find(a => a.Reagent.ToLower().Replace(" ", "") == acData.Compound.ToLower().Replace(" ", ""));
                        if (componentInfo == null)
                        {
                            results.Add(new BarXY("LOD", null, acData.LOD_SinglePahse(), LODUpperLimit, null, "Under"));
                            results.Add(new BarXY("Analyte Concentration", null, acData.AConcMean(), MeanUpperLimit, null, "Under"));
                        }
                        else
                        {
                            results.Add(new BarXY("LOD", null, acData.LOD_SinglePahse(), componentInfo.Min, null, "Under"));
                            results.Add(new BarXY("Analyte Concentration", null, acData.AConcMean(), componentInfo.Max, null, "Under"));
                        }
                        components.Add(acData.Compound, results);
                    }
                    scans.Add(scan.GetFileInfo().FileName, components);
                }
                batches.Add(batch.Key, scans);
            }
            //remove pass components
            foreach (var batch in batches)
            {
                if (batch.Value.Count == 0) continue;
                List<string> componentList = batch.Value.First().Value.Keys.ToList();
                foreach (var component in componentList)
                {
                    bool pass = false;
                    foreach (var scan in batch.Value)
                    {
                        //[0]: lod, [1]: analyte concentration,
                        if (scan.Value[component][0].Color == 0x80ff80 && scan.Value[component][1].Color == 0x80ff80)
                        {
                            pass = true;
                            break;
                        }
                        else
                            pass = false;
                    }
                    if (pass == true)
                    {
                        foreach (var scan in batch.Value)
                        {
                            scan.Value.Remove(component);
                        }
                    }
                }
            }
            //batches - batch -> lod and conc - lod or conc -> components - component -> scans - scan 
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, BarXY>>>> newBatches = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, BarXY>>>>();
            foreach (var batch in batches)
            {
                if (batch.Value.Count == 0) continue;
                Dictionary<string, Dictionary<string, Dictionary<string, BarXY>>> lod_conc = new Dictionary<string, Dictionary<string, Dictionary<string, BarXY>>>();
                for (int i = 0; i < 2; i++)
                {
                    Dictionary<string, Dictionary<string, BarXY>> componentList = new Dictionary<string, Dictionary<string, BarXY>>();

                    List<string> componentKeys = batch.Value.First().Value.Keys.ToList();
                    foreach (var componentKey in componentKeys)
                    {
                        Dictionary<string, BarXY> newScan = new Dictionary<string, BarXY>();
                        foreach (var scan in batch.Value)
                        {
                            newScan.Add(scan.Key, scan.Value[componentKey][i]);
                        }
                        componentList.Add(componentKey, newScan);
                    }

                    string temp;
                    switch (i)
                    {
                        default:
                        case 0:
                            temp = "LOD";
                            break;
                        case 1:
                            temp = "Conc";
                            break;
                    }
                    lod_conc.Add(temp, componentList);
                }
                newBatches.Add(batch.Key, lod_conc);
            }
            return newBatches;
        }

        private XYChart SingleBatchXYChartGenerator(Dictionary<string, Dictionary<string, Dictionary<string, BarXY>>> conc_lod, string batchName, int firstScanID, int lastScanID, int scanCount)
        {
            if (XYData == null) return null;

            XYChart c = new XYChart(1114, 650, 0xccccff);

            if (conc_lod.First().Value.Count == 0)
            {
                c.addTitle(Chart.TopCenter, $"{this.ChartConfig.ChartName} - {this.ChartConfig.SubChartName}", "Times New Roman Bold Italic", 16).setMargin2(0, 0, 5, 0);

                TextBox b = c.addText(557, 30, $"Batch: {batchName}, ID: {firstScanID} - {lastScanID} ({scanCount} Scans)", "Arial Bold", 10, 0x4A235A);
                b.setAlignment(Chart.TopCenter);
                b.setBackground(Chart.Transparent);

                TextBox b1 = c.addText(207, 65, $"All Compounds Background Check Pass", "Arial Bold", 25);
                b1.setHeight(70);
                b1.setWidth(700);
                b1.setAlignment(Chart.Center);
                b1.setBackground(unchecked((int)0x80f8f8f8));
            }
            else
            {
                c.setPlotArea(70, 135, c.getWidth() - 130, c.getHeight() - 200, 0xf8f8f8, 0xffffff);
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
                LegendBox b1 = c.addLegend(557, 55, false, "Arial Bold", 10);
                b1.setAlignment(Chart.TopCenter);
                b1.setCols(2);
                b1.setBackground(unchecked((int)0x80f8f8f8));
                //b1.setKeyBorder(Chart.SameAsMainColor);

                // Set the x and y axis stems to transparent and the label font to 10pt Arial
                c.xAxis().setColors(Chart.Transparent);
                c.yAxis().setColors(Chart.Transparent);
                c.xAxis().setLabelStyle("Arial", 10);
                c.yAxis().setLabelStyle("Arial", 10);

                // Add axis title using 10pt Arial Bold font
                c.yAxis().setTitle("ppb", "Arial Bold", 10);
                c.xAxis().setTitle("compound", "Arial Bold", 10);

                //mark lod
                BoxWhiskerLayer markLayer1 = c.addBoxWhiskerLayer(null, null, null, null, conc_lod.First().Value.Select(a => double.Parse(a.Value.First().Value.Max)).ToArray(), -1, 0xff8080);
                markLayer1.setLineWidth(3);
                markLayer1.setDataGap(0);
                markLayer1.setHTMLImageMap("", "", "title='LOD Upper Limit: {med} at {xLabel} - Value should be under this level!'");
                //set lod and conc shap
                int[] barShaps = new int[] { Chart.PolygonShape(6), Chart.SquareShape };
                int barShapsPos = 0;

                foreach (var lodandconc in conc_lod)
                {
                    // Add layer
                    // Add a multi-bar layer with multi data sets
                    BarLayer layer = c.addBarLayer2(Chart.Side, 10);
                    // Display labela on top of bars using 12pt Arial font
                    layer.setAggregateLabelStyle("Arial", 10);
                    layer.setAggregateLabelFormat("{value|3}");
                    // Set 50% overlap between bars
                    layer.setOverlapRatio(0);

                    c.xAxis().setLabels(lodandconc.Value.Keys.ToArray());

                    if (lodandconc.Value.Keys.Count <= 8) layer.setBarWidth(100);

                    int i = 0;
                    foreach (var scan in lodandconc.Value.Values?.First().Keys.ToList())
                    {
                        layer.addDataSet(lodandconc.Value.Values.Select(a => a[scan].Y).ToArray(), Global.ColorPool[i++] + unchecked((int)0x80000000), $"({lodandconc.Key}) (Scan: {scan})");
                    }
                    layer.setBarShape(barShaps[barShapsPos++]);
                    layer.setHTMLImageMap("", "", "title='{value} at {xLabel} {dataSetName}'");
                }
                //mark conc
                BoxWhiskerLayer markLayer2 = c.addBoxWhiskerLayer(null, null, null, null, conc_lod.Last().Value.Select(a => double.Parse(a.Value.First().Value.Max)).ToArray(), -1, 0xff8080);
                markLayer2.setLineWidth(3);
                markLayer2.setDataGap(0);
                markLayer2.setHTMLImageMap("", "", "title='Conc Upper Limit: {med} at {xLabel} - Value should be under this level!'");
            }
            return c;
        }

        public List<XYChart> XYChartList
        {
            get
            {
                if (this.XYData == null || this.XYData.Count == 0) return new List<XYChart>() { null, null, null, null, null };
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
