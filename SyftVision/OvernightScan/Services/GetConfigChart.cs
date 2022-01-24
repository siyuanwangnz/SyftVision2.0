using OvernightScan.Models;
using Public.Global;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OvernightScan.Services
{
    class GetConfigChart
    {
        public static ConfigChartInfo BatchChart(Global.BatchType batchType, string chartName, string subCharName)
        {
            XElement rootNode = GetRootNode.BatchConfigRootNode(batchType);
            if (rootNode == null) return null;
            IEnumerable<XElement> targetNodes =
                from target in rootNode.Descendants("Chart")
                where target.Attribute("name").Value.ToLower().Replace(" ", "") == chartName.ToLower().Replace(" ", "")
                from subtarget in target.Descendants("SubChart")
                where subtarget.Attribute("name").Value.ToLower().Replace(" ", "") == subCharName.ToLower().Replace(" ", "")
                select subtarget;

            foreach (var node in targetNodes)
            {
                string phase = node.Attribute("phase")?.Value;
                string accept = node.Attribute("accept")?.Value;
                List<string> markCodeList = new List<string>();
                if (node.Elements("Mark").Count() != 0)
                {
                    foreach (var item in node.Elements("Mark"))
                    {
                        markCodeList.Add($"{item.Attribute("main")?.Value}{item.Attribute("sub")?.Value}".ToLower().Replace(" ", ""));
                    }
                }

                List<ComponentInfo> rpInfoList = new List<ComponentInfo>();
                if (node.Elements("Components").Count() != 0)
                {
                    foreach (var item in node.Elements("Components").Elements("Component"))
                    {
                        string max = item.Attribute("max")?.Value;
                        string min = item.Attribute("min")?.Value;
                        Random random = new Random();
                        int color = Color.FromArgb(0, random.Next(200), random.Next(200), random.Next(200)).ToArgb();
                        Thread.Sleep(50);
                        rpInfoList.Add(new ComponentInfo(item.Attribute("reagent")?.Value, item.Attribute("product")?.Value, color, max, min));
                    }
                }
                return new ConfigChartInfo(chartName, subCharName, phase, accept, markCodeList, rpInfoList);
            }
            return null;
        }
    }
}
