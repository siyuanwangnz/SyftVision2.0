using Public.ChartConfig;
using Public.Global;
using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public abstract class TraceLineXYFactory : XYFactory
    {
        public override List<XYItem> GetXYItemList(ChartProp chartProp, List<ScanFile> scanFileList)
        {
            List<XYItem> xyItemList = new List<XYItem>();
            List<string> labelList = scanFileList.Select(a => a.Date_Time).ToList();
            var colorEmu = MyColor.Pool.GetEnumerator();
            foreach (var component in chartProp.ComponentList)
            {
                colorEmu.MoveNext();
                List<double> valueList = new List<double>();
                foreach (var scanFile in scanFileList)
                    valueList.Add(GetValue(in scanFile, in component, in chartProp));
                xyItemList.Add(new XYItem(new XYLegend($"{component.Reagent}/{component.Production}", colorEmu.Current), new XYItem.Layer(labelList, valueList)));
            }
            return xyItemList;
        }
        public abstract double GetValue(in ScanFile scanFile, in Component component, in ChartProp chartProp);
    }
}
