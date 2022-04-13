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
    public abstract class BarXYFactory : XYFactory
    {
        public override List<XYItem> GetXYItemList(ChartProp chartProp, List<ScanFile> scanFileList)
        {
            List<XYItem> xyItemList = new List<XYItem>();
            List<string> labelList = chartProp.ComponentList.Select(a => a.CompoundEnable ? a.Compound : $"{a.Reagent}/{a.Production}").ToList();
            var colorEmu = MyColor.Pool.GetEnumerator();
            foreach (var scanFile in scanFileList)
            {
                colorEmu.MoveNext();
                List<double> valueList = new List<double>();
                foreach (var component in chartProp.ComponentList)
                    valueList.Add(GetValue(in scanFile, in component, in chartProp));
                xyItemList.Add(new XYItem(new XYLegend(scanFile.File, colorEmu.Current), new XYItem.Layer(labelList, valueList)));
            }
            return xyItemList;
        }

        public abstract double GetValue(in ScanFile scanFile, in Component component, in ChartProp chartProp);
    }
}
