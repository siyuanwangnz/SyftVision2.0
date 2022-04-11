using Public.ChartConfig;
using Public.Global;
using Public.Instrument;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public class MultiBarXYFactory : XYFactory
    {
        public override List<XYItem> GetXYItemList(ChartProp chartProp, List<ScanFile> scanFileList)
        {
            List<XYItem> xyItemList = new List<XYItem>();
            List<string> labelList = chartProp.ComponentList.Select(a => $"{a.Reagent}/{a.Production}").ToList();
            var colorEmu = MyColor.Pool.GetEnumerator();
            foreach (var scanFile in scanFileList)
            {
                colorEmu.MoveNext();
                List<double> valueList = new List<double>();
                foreach (var component in chartProp.ComponentList)
                {
                    RP_Data rpData = scanFile.Scan.GetRP_Data($"{component.Reagent}{component.Production}", chartProp.ScanPhase, Scan.FastMode.Sensitivity);
                    valueList.Add(rpData.IsAvailable ? rpData.Sensitivity() : 0);
                }
                xyItemList.Add(new XYItem(new XYLegend(scanFile.File, colorEmu.Current), new XYItem.Layer(labelList, valueList)));
            }
            return xyItemList;
        }
    }
}
