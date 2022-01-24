using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using ChartDirector;
using System.Text.RegularExpressions;
using SyftXML;

namespace SettingCheck.Models
{
    class ChartGenerator
    {
        public static BaseChart GetCompareChart(string title, string unit, int width, int height, bool legendposition, Dictionary<string, List<Setting>> settingsDic)
        {
            // Create a XYChart object of size 580 x 280 pixels
            XYChart c = new XYChart(width, height);

            // Add a title to the chart using 14pt Arial Bold Italic font
            c.addTitle(title, "Arial Bold Italic", 14);

            // Set the plot area at (50, 50) and of size 500 x 200. Use two alternative background
            // colors (f8f8f8 and ffffff)
            c.setPlotArea(50, 30, width - 70, height - 70, 0xf8f8f8, 0xffffff);

            // Add a legend box at (50, 25) using horizontal layout. Use 8pt Arial as font, with
            // transparent background.
            LegendBox b = c.addLegend(45, legendposition ? 25 : height - 35, false, "Arial", 8);
            b.setBackground(Chart.Transparent);
            b.setKeyBorder(Chart.Transparent);
            //b.setCols(1);
            if (!legendposition) b.setAlignment(Chart.BottomLeft);

            // Draw the ticks between label positions (instead of at label positions)
            c.xAxis().setTickOffset(0.5);

            // Add a multi-bar layer with multi data sets
            BarLayer layer = c.addBarLayer2(Chart.Side);
            string[] NameArray = null;
            double[] SettingValueArray = null;
            foreach (var item in settingsDic)
            {
                // The data for the bar chart
                try
                {
                    if (NameArray == null)
                    {
                        // Set the x axis labels
                        NameArray = item.Value.Select(t =>
                        {
                            return Regex.IsMatch(t.Name, @"^\d+$") ? $"Mass {t.Name}" : t.Name;
                        }
                        ).ToArray();
                        c.xAxis().setLabels(NameArray);
                    }
                    //Set the y axis data
                    SettingValueArray = item.Value.Select(t => t.SettingValue).ToArray();
                    //Get color
                    //Match match = Regex.Match(item.Key, @"\((\d.*)\)");
                    Match match = Regex.Match(item.Key, @"\((\d{4,}|\-1)\)");
                    int color = int.Parse(match.Groups[1].Value); //Groups[0]: full string, Groups[1]: selected() string 
                    //Get instrument name
                    string instrumentName = item.Key.Substring(0, item.Key.LastIndexOf("("));
                    layer.addDataSet(SettingValueArray, color, instrumentName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR");
                    NameArray = new string[] { "null" };
                    SettingValueArray = new double[] { 0 };
                }
            }

            // Display labela on top of bars using 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 8);

            // Set 50% overlap between bars
            layer.setOverlapRatio(0);

            // Tool tip for the bar layer
            layer.setHTMLImageMap("", "", "title='{value} at {xLabel} ({dataSetName})'");

            // Add a title to the y-axis
            c.yAxis().setTitle(unit);

            //mark1
            BoxWhiskerLayer markLayer1 = c.addBoxWhiskerLayer(null, null, null, null, settingsDic.Last().Value.Select(t => t.MinimumValue).ToArray(), -1, 0x8080ff);
            markLayer1.setLineWidth(3);
            markLayer1.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer1.setHTMLImageMap("", "", "title='Lower Limit: {med} at {xLabel}'");

            //mark2
            BoxWhiskerLayer markLayer2 = c.addBoxWhiskerLayer(null, null, null, null, settingsDic.Last().Value.Select(t => t.MaximumValue).ToArray(), -1, 0xff8080);
            markLayer2.setLineWidth(3);
            markLayer2.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer2.setHTMLImageMap("", "", "title='Upper Limit: {med} at {xLabel}'");

            // Output the chart
            return c;

            //include tool tip for the chart
            //viewer.ImageMap = c.getHTMLImageMap("clickable", "",
            //    "title='{xLabel} Revenue on {dataSetName}: {value} millions'");
        }

        public static BaseChart GetLimitColorChart(string title, string unit, int width, int height, List<Setting> settingslist)
        {
            // Create a XYChart object of size 580 x 280 pixels
            XYChart c = new XYChart(width, height);

            // Add a title to the chart using 14pt Arial Bold Italic font
            c.addTitle(title, "Arial Bold Italic", 14);

            // Set the plot area at (50, 50) and of size 500 x 200. Use two alternative background
            // colors (f8f8f8 and ffffff)
            c.setPlotArea(50, 30, width - 70, height - 70, 0xf8f8f8, 0xffffff);

            // Add a legend box at (50, 25) using horizontal layout. Use 8pt Arial as font, with
            // transparent background.
            c.addLegend(50, 25, false, "Arial", 8).setBackground(Chart.Transparent);

            // Set the x axis labels
            c.xAxis().setLabels(settingslist.Select(t => t.Name).ToArray());

            // Draw the ticks between label positions (instead of at label positions)
            c.xAxis().setTickOffset(0.5);

            //Add data and matched color
            BarLayer layer = c.addBarLayer3(settingslist.Select(t => t.SettingValue).ToArray(), settingslist.Select(t => t.Color).ToArray());

            // Display labela on top of bars using 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            //mark1
            BoxWhiskerLayer markLayer1 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MinimumValue).ToArray(), -1, 0x8080ff);
            markLayer1.setLineWidth(3);
            markLayer1.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer1.setHTMLImageMap("", "", "title='Lower Limit: {med} at {xLabel}'");

            //mark2
            BoxWhiskerLayer markLayer2 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MaximumValue).ToArray(), -1, 0xff8080);
            markLayer2.setLineWidth(3);
            markLayer2.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer2.setHTMLImageMap("", "", "title='Upper Limit: {med} at {xLabel}'");

            // Set 50% overlap between bars
            //layer.setOverlapRatio(0.5);

            // Tool tip for the bar layer
            layer.setHTMLImageMap("", "", "title='{value} at {xLabel}'");

            // Add a title to the y-axis
            c.yAxis().setTitle(unit);

            // Output the chart
            return c;

            //include tool tip for the chart
            //viewer.ImageMap = c.getHTMLImageMap("clickable", "",
            //    "title='{xLabel} Revenue on {dataSetName}: {value} millions'");

        }

        public static BaseChart GetUPSPhaseLimitColorChart(string title, string unit, bool extraction, bool einzel, int width, int height, List<Setting> settingslist)
        {
            // Create a XYChart object of size 580 x 280 pixels
            XYChart c = new XYChart(width, height);

            // Add a title to the chart using 14pt Arial Bold Italic font
            c.addTitle(title, "Arial Bold Italic", 14);

            // Set the plot area at (50, 50) and of size 500 x 200. Use two alternative background
            // colors (f8f8f8 and ffffff)
            c.setPlotArea(50, 30, width - 70, height - 70, 0xf8f8f8, 0xffffff);

            // Add a legend box at (50, 25) using horizontal layout. Use 8pt Arial as font, with
            // transparent background.
            LegendBox legendBox = c.addLegend(50, 30, false, "Arial Bold Italic", 12);
            legendBox.setBackground(Chart.Transparent);

            // Add entries to the legend box
            int ExtractionColor = extraction ? 0x80ff80 : 0xff8080;
            int EinzelColor = einzel ? 0x80ff80 : 0xff8080;
            legendBox.addKey("UPS Extraction Lens Gradient Check", unchecked((int)ExtractionColor));
            legendBox.addKey("UPS Einzel Stack Check", unchecked((int)EinzelColor));

            // Set the x axis labels
            c.xAxis().setLabels(settingslist.Select(t => t.Name).ToArray());

            // Draw the ticks between label positions (instead of at label positions)
            c.xAxis().setTickOffset(0.5);

            //Add data and matched color
            BarLayer layer = c.addBarLayer3(settingslist.Select(t => t.SettingValue).ToArray(), settingslist.Select(t => t.Color).ToArray());

            // Display labela on top of bars using 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            //mark1
            BoxWhiskerLayer markLayer1 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MinimumValue).ToArray(), -1, 0x8080ff);
            markLayer1.setLineWidth(3);
            markLayer1.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer1.setHTMLImageMap("", "", "title='Lower Limit: {med} at {xLabel}'");

            //mark2
            BoxWhiskerLayer markLayer2 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MaximumValue).ToArray(), -1, 0xff8080);
            markLayer2.setLineWidth(3);
            markLayer2.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer2.setHTMLImageMap("", "", "title='Upper Limit: {med} at {xLabel}'");

            // Set 50% overlap between bars
            //layer.setOverlapRatio(0.5);

            // Tool tip for the bar layer
            layer.setHTMLImageMap("", "", "title='{value} at {xLabel}'");

            // Add a title to the y-axis
            c.yAxis().setTitle(unit);

            // Output the chart
            return c;

            //include tool tip for the chart
            //viewer.ImageMap = c.getHTMLImageMap("clickable", "",
            //    "title='{xLabel} Revenue on {dataSetName}: {value} millions'");
        }
        public static BaseChart GetUPSPhaseLimitColorChart(string title, string unit, int width, int height, List<Setting> settingslist)
        {
            // Create a XYChart object of size 580 x 280 pixels
            XYChart c = new XYChart(width, height);

            // Add a title to the chart using 14pt Arial Bold Italic font
            c.addTitle(title, "Arial Bold Italic", 14);

            // Set the plot area at (50, 50) and of size 500 x 200. Use two alternative background
            // colors (f8f8f8 and ffffff)
            c.setPlotArea(50, 30, width - 70, height - 70, 0xf8f8f8, 0xffffff);

            // Set the x axis labels
            c.xAxis().setLabels(settingslist.Select(t => t.Name).ToArray());

            // Draw the ticks between label positions (instead of at label positions)
            c.xAxis().setTickOffset(0.5);

            //Add data and matched color
            BarLayer layer = c.addBarLayer3(settingslist.Select(t => t.SettingValue).ToArray(), settingslist.Select(t => t.Color).ToArray());

            // Display labela on top of bars using 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            //mark1
            BoxWhiskerLayer markLayer1 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MinimumValue).ToArray(), -1, 0x8080ff);
            markLayer1.setLineWidth(3);
            markLayer1.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer1.setHTMLImageMap("", "", "title='Lower Limit: {med} at {xLabel}'");

            //mark2
            BoxWhiskerLayer markLayer2 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MaximumValue).ToArray(), -1, 0xff8080);
            markLayer2.setLineWidth(3);
            markLayer2.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer2.setHTMLImageMap("", "", "title='Upper Limit: {med} at {xLabel}'");

            // Set 50% overlap between bars
            //layer.setOverlapRatio(0.5);

            // Tool tip for the bar layer
            layer.setHTMLImageMap("", "", "title='{value} at {xLabel}'");

            // Add a title to the y-axis
            c.yAxis().setTitle(unit);

            // Output the chart
            return c;

            //include tool tip for the chart
            //viewer.ImageMap = c.getHTMLImageMap("clickable", "",
            //    "title='{xLabel} Revenue on {dataSetName}: {value} millions'");
        }
        public static BaseChart GetDWSLensLimitColorChart(string title, string unit, bool linearity, bool mirror, int width, int height, List<Setting> settingslist)
        {
            // Create a XYChart object of size 580 x 280 pixels
            XYChart c = new XYChart(width, height);

            // Add a title to the chart using 14pt Arial Bold Italic font
            c.addTitle(title, "Arial Bold Italic", 14);

            // Set the plot area at (50, 50) and of size 500 x 200. Use two alternative background
            // colors (f8f8f8 and ffffff)
            c.setPlotArea(50, 30, width - 70, height - 70, 0xf8f8f8, 0xffffff);

            // Add a legend box at (50, 25) using horizontal layout. Use 8pt Arial as font, with
            // transparent background.
            LegendBox legendBox = c.addLegend(50, 170, false, "Arial Bold Italic", 10);
            legendBox.setBackground(Chart.Transparent);

            // Add entries to the legend box
            int LinearityColor = linearity ? 0x80ff80 : 0xff8080;
            int MirrorColor = mirror ? 0x80ff80 : 0xff8080;
            legendBox.addKey("DWS Specific Lens Mirror Check", unchecked((int)MirrorColor));
            legendBox.addKey("DWS Specific Lens Linearity Check", unchecked((int)LinearityColor));

            // Set the x axis labels
            c.xAxis().setLabels(settingslist.Select(t => $"Mass {t.Name}").ToArray());

            // Draw the ticks between label positions (instead of at label positions)
            c.xAxis().setTickOffset(0.5);

            //Add data and matched color
            BarLayer layer = c.addBarLayer3(settingslist.Select(t => t.SettingValue).ToArray(), settingslist.Select(t => t.Color).ToArray());

            // Display labela on top of bars using 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            //mark1
            BoxWhiskerLayer markLayer1 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MinimumValue).ToArray(), -1, 0x8080ff);
            markLayer1.setLineWidth(3);
            markLayer1.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer1.setHTMLImageMap("", "", "title='Lower Limit: {med} at {xLabel}'");

            //mark2
            BoxWhiskerLayer markLayer2 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MaximumValue).ToArray(), -1, 0xff8080);
            markLayer2.setLineWidth(3);
            markLayer2.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer2.setHTMLImageMap("", "", "title='Upper Limit: {med} at {xLabel}'");

            // Set 50% overlap between bars
            //layer.setOverlapRatio(0.5);

            // Tool tip for the bar layer
            layer.setHTMLImageMap("", "", "title='{value} at {xLabel}'");

            // Add a title to the y-axis
            c.yAxis().setTitle(unit);

            // Output the chart
            return c;
        }
        public static BaseChart GetDWSLensLimitColorChart(string title, string unit, bool linearity, int width, int height, List<Setting> settingslist)
        {
            // Create a XYChart object of size 580 x 280 pixels
            XYChart c = new XYChart(width, height);

            // Add a title to the chart using 14pt Arial Bold Italic font
            c.addTitle(title, "Arial Bold Italic", 14);

            // Set the plot area at (50, 50) and of size 500 x 200. Use two alternative background
            // colors (f8f8f8 and ffffff)
            c.setPlotArea(50, 30, width - 70, height - 70, 0xf8f8f8, 0xffffff);

            // Add a legend box at (50, 25) using horizontal layout. Use 8pt Arial as font, with
            // transparent background.
            LegendBox legendBox = c.addLegend(50, 170, false, "Arial Bold Italic", 10);
            legendBox.setBackground(Chart.Transparent);

            // Add entries to the legend box
            int LinearityColor = linearity ? 0x80ff80 : 0xff8080;
            legendBox.addKey("DWS Specific Lens Linearity Check", unchecked((int)LinearityColor));

            // Set the x axis labels
            c.xAxis().setLabels(settingslist.Select(t => $"Mass {t.Name}").ToArray());

            // Draw the ticks between label positions (instead of at label positions)
            c.xAxis().setTickOffset(0.5);

            //Add data and matched color
            BarLayer layer = c.addBarLayer3(settingslist.Select(t => t.SettingValue).ToArray(), settingslist.Select(t => t.Color).ToArray());

            // Display labela on top of bars using 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            //mark1
            BoxWhiskerLayer markLayer1 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MinimumValue).ToArray(), -1, 0x8080ff);
            markLayer1.setLineWidth(3);
            markLayer1.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer1.setHTMLImageMap("", "", "title='Lower Limit: {med} at {xLabel}'");

            //mark2
            BoxWhiskerLayer markLayer2 = c.addBoxWhiskerLayer(null, null, null, null, settingslist.Select(t => t.MaximumValue).ToArray(), -1, 0xff8080);
            markLayer2.setLineWidth(3);
            markLayer2.setDataGap(0.1);

            // Tool tip for the mark layer
            markLayer2.setHTMLImageMap("", "", "title='Upper Limit: {med} at {xLabel}'");

            // Set 50% overlap between bars
            //layer.setOverlapRatio(0.5);

            // Tool tip for the bar layer
            layer.setHTMLImageMap("", "", "title='{value} at {xLabel}'");

            // Add a title to the y-axis
            c.yAxis().setTitle(unit);

            // Output the chart
            return c;
        }

        public static XYChart GetICFChart(int width, int height, Scan scan)
        {
            int icfCount = scan.GetICF_Table().Count;

            XYChart c = new XYChart(width, height);

            c.setPlotArea(50, 30, width - 70, height - 90, 0xffffff, -1, -1, 0x40dddddd, 0x40dddddd);

            // Enable clipping mode to clip the part of the data that is outside the plot area.
            c.setClipping();

            // Add a title to the chart using 15pt Arial Bold font
            c.addTitle("ICF", "Arial Bold Italic", 14);

            //// Add a legend box at (450, 40) (top right corner of the chart) with vertical layout
            //// and 8 pts Arial Bold font. Set the background color to semi-transparent grey.
            LegendBox b1 = c.addLegend(50, 30, false, "Arial Bold Italic", 12);
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
            c.xAxis().setMargin(icfCount <= 14 ? 60 : 20, icfCount <= 14 ? 0 : 20);
            //
            // Add layer
            //
            LineLayer layer = c.addLineLayer2();
            layer.setLineWidth(2);
            layer.setFastLineMode();

            if (icfCount <= 14)
            {
                Dictionary<double, double> icfDic = new Dictionary<double, double>();
                foreach (var item in scan.GetICF_Table())
                    if (item.Key >= 19) icfDic.Add(item.Key, item.Value);

                layer.setXData(icfDic.Keys.ToArray());
                layer.addDataSet(icfDic.Values.ToArray(), 0x1A237E, $" {scan.GetFileInfo().FileName}").setDataSymbol(Chart.SquareSymbol, 6, 0x1A237E, 0x1A237E);
                // Enable data label on the data points
                layer.setDataLabelStyle("Arial Bold", 10).setPos(0, -10);
                //t.setBackground(0xf8f8f8, 0xf8f8f8);
                layer.setDataLabelFormat("{value|2}");
                for (int i = 0; i < icfDic.Count; i++)
                {
                    if (icfDic.Keys.ToArray()[i] == 28) layer.addCustomDataLabel(0, i, icfDic.Values.ToArray()[i].ToString("0.00"), "Arial Bold", 10).setPos(0, 25);
                }
            }
            else
            {
                layer.setXData(scan.GetICF_Table().Keys.ToArray());
                layer.addDataSet(scan.GetICF_Table().Values.ToArray(), 0x1A237E, $" {scan.GetFileInfo().FileName}").setDataSymbol(Chart.SquareSymbol, 6, 0x1A237E, 0x1A237E);
                // Enable data label on the data points
                layer.setDataLabelStyle("Arial Bold", 10).setPos(0, -10);
                //t.setBackground(0xf8f8f8, 0xf8f8f8);
                layer.setDataLabelFormat("{value|2}");
                for (int i = 3; i < icfCount; i++)
                {
                    if (scan.GetICF_Table().Keys.ToArray()[i] == -17)
                    {
                        layer.addCustomDataLabel(0, i, scan.GetICF_Table().Values.ToArray()[i].ToString("0.##"), "Arial Bold", 10).setPos(-20, 0);
                        continue;
                    }
                    if (scan.GetICF_Table().Keys.ToArray()[i] == 19)
                    {
                        layer.addCustomDataLabel(0, i, scan.GetICF_Table().Values.ToArray()[i].ToString("0.##"), "Arial Bold", 10).setPos(-20, 0);
                        continue;
                    }
                    if (scan.GetICF_Table().Keys.ToArray()[i] == -28)
                    {
                        layer.addCustomDataLabel(0, i, scan.GetICF_Table().Values.ToArray()[i].ToString("0.##"), "Arial Bold", 10).setPos(7, -5);
                        continue;
                    }
                    layer.addCustomDataLabel(0, i, scan.GetICF_Table().Values.ToArray()[i].ToString("0.##"), "Arial Bold", 10).setPos(0, i % 2 == 1 ? 25 : -10);
                }
            }

            layer.setHTMLImageMap("", "", "title='{value} at {x} mass (Scan: {dataSetName})'");

            // Add a line layer to the chart with two dark green (338033) data sets, and a line
            // width of 2 pixels
            double[] spisUpperY = { 3, 1, 1, 0.8, 0.65, 0.62, 0.8, 2, 3.5, 10 };
            double[] spisLowerY = { 1.5, 1, 1, 0.55, 0.48, 0.45, 0.5, 1, 1.5, 2 };
            double[] spisZoneX = { 19, 28, 32, 57, 78, 92, 150, 186, 236, 400 };

            double[] upperY = { 10, 3.5, 2, 0.8, 0.62, 0.65, 0.8, 1, 1, 9, 15, 3, 1, 1, 0.8, 0.65, 0.62, 0.8, 2, 3.5, 10 };
            double[] lowerY = { 2, 1.5, 1, 0.5, 0.45, 0.48, 0.55, 1, 1, 2, 4, 1.5, 1, 1, 0.55, 0.48, 0.45, 0.5, 1, 1.5, 2 };
            double[] zoneX = { -400, -236, -186, -150, -92, -78, -57, -32, -28, -17, -16, 19, 28, 32, 57, 78, 92, 150, 186, 236, 400 };
            LineLayer lineLayer = c.addLineLayer2();
            lineLayer.addDataSet(icfCount <= 14 ? spisUpperY : upperY, unchecked((int)0x8099ff99), "Target Zone");
            lineLayer.addDataSet(icfCount <= 14 ? spisLowerY : lowerY, unchecked((int)0x8099ff99));
            lineLayer.setXData(icfCount <= 14 ? spisZoneX : zoneX);
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
        public static XYChart GetICFCompareChart(int width, int height, Dictionary<string, Scan> scanDic, bool isSPIS = false)
        {
            XYChart c = new XYChart(width, height);

            c.setPlotArea(50, 30, width - 70, height - 90, 0xffffff, -1, -1, 0x40dddddd, 0x40dddddd);

            // Enable clipping mode to clip the part of the data that is outside the plot area.
            c.setClipping();

            // Add a title to the chart using 15pt Arial Bold font
            c.addTitle("ICF", "Arial Bold Italic", 14);

            //// Add a legend box at (450, 40) (top right corner of the chart) with vertical layout
            //// and 8 pts Arial Bold font. Set the background color to semi-transparent grey.
            LegendBox b1 = c.addLegend(50, 30, false, "Arial", 8);
            b1.setBackground(Chart.Transparent);
            b1.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 10pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // Add axis title using 10pt Arial Bold font
            c.yAxis().setTitle("factor", "Arial Bold", 10);
            c.xAxis().setTitle("mass", "Arial Bold", 10);
            c.xAxis().setMargin(isSPIS ? 60 : 20, isSPIS ? 0 : 20);
            //
            // Add layer
            //
            foreach (var scan in scanDic)
            {
                LineLayer layer = c.addLineLayer2();
                layer.setLineWidth(2);
                layer.setFastLineMode();

                //Get color
                Match match = Regex.Match(scan.Key, @"\((\d{4,}|\-1)\)");
                int color = int.Parse(match.Groups[1].Value); //Groups[0]: full string, Groups[1]: selected() string 
                //Get instrument name
                string instrumentName = scan.Key.Substring(0, scan.Key.LastIndexOf("("));

                if (isSPIS)
                {
                    Dictionary<double, double> icfDic = new Dictionary<double, double>();
                    foreach (var item in scan.Value.GetICF_Table())
                        if (item.Key >= 19) icfDic.Add(item.Key, item.Value);

                    layer.setXData(icfDic.Keys.ToArray());
                    layer.addDataSet(icfDic.Values.ToArray(), color, instrumentName).setDataSymbol(Chart.SquareSymbol, 6, color, color);
                }
                else
                {
                    layer.setXData(scan.Value.GetICF_Table().Keys.ToArray());
                    layer.addDataSet(scan.Value.GetICF_Table().Values.ToArray(), color, instrumentName).setDataSymbol(Chart.SquareSymbol, 6, color, color);
                }

                layer.setHTMLImageMap("", "", "title='{value} at {x} mass (Scan: {dataSetName})'");
            }

            // Add a line layer to the chart with two dark green (338033) data sets, and a line
            // width of 2 pixels
            double[] spisUpperY = { 3, 1, 1, 0.8, 0.62, 0.8, 2, 3.5, 10 };
            double[] spisLowerY = { 1.5, 1, 1, 0.55, 0.45, 0.5, 1, 1.5, 2 };
            double[] spisZoneX = { 19, 28, 32, 57, 92, 150, 186, 236, 400 };

            double[] upperY = { 10, 3.5, 2, 0.8, 0.62, 0.8, 1, 1, 9, 15, 3, 1, 1, 0.8, 0.62, 0.8, 2, 3.5, 10 };
            double[] lowerY = { 2, 1.5, 1, 0.5, 0.45, 0.55, 1, 1, 2, 4, 1.5, 1, 1, 0.55, 0.45, 0.5, 1, 1.5, 2 };
            double[] zoneX = { -400, -236, -186, -150, -92, -57, -32, -28, -17, -16, 19, 28, 32, 57, 92, 150, 186, 236, 400 };
            LineLayer lineLayer = c.addLineLayer2();
            lineLayer.addDataSet(isSPIS ? spisUpperY : upperY, unchecked((int)0x8099ff99), "Target Zone");
            lineLayer.addDataSet(isSPIS ? spisLowerY : lowerY, unchecked((int)0x8099ff99));
            lineLayer.setXData(isSPIS ? spisZoneX : zoneX);
            lineLayer.setLineWidth(1);

            c.addInterLineLayer(lineLayer.getLine(0), lineLayer.getLine(1),
            unchecked((int)0x8099ff99), unchecked((int)0x8099ff99));

            lineLayer.setHTMLImageMap("", "", "title='Limit: {value} at {x} mass'");

            return c;

        }
    }
}
