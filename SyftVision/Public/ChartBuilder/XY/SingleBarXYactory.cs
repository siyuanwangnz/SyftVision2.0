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
    public abstract class SingleBarXYactory : XYFactory
    {
        public override List<XYItem> GetXYItemList(ChartProp chartProp, List<ScanFile> scanFileList)
        {
            List<string> labelList = chartProp.ComponentList.Select(a => a.CompoundEnable ? a.Compound : $"{a.Reagent}/{a.Production}").ToList();

            List<double> valueList = new List<double>();
            foreach (var component in chartProp.ComponentList)
                valueList.Add(GetValue(in scanFileList, in component, in chartProp));

            return new List<XYItem>() { new XYItem(new XYLegend("N/A", MyColor.Pool.First()), new XYItem.Layer(labelList, valueList)) };
        }

        public abstract double GetValue(in List<ScanFile> scanFileList, in Component component, in ChartProp chartProp);
    }
}
