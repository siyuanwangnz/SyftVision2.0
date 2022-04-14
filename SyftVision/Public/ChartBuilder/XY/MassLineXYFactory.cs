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
    public class MassLineXYFactory : XYFactory
    {
        public override List<XYItem> GetXYItemList(ChartProp chartProp, List<ScanFile> scanFileList)
        {
            List<XYItem> xyItemList = new List<XYItem>();
            var colorEmu = MyColor.Pool.GetEnumerator();
            foreach (var scanFile in scanFileList)
            {
                colorEmu.MoveNext();
                List<string> labelList = new List<string>();
                List<double> valueList = new List<double>();
                foreach (var component in chartProp.ComponentList)
                {
                    Mass_Data massData = scanFile.Scan.GetMass_Data(component.Reagent, component.Production, chartProp.ScanPhase);
                    if (massData != null)
                    {
                        var massCPSDic = massData.GetMassCPSDic();
                        labelList.AddRange(massCPSDic.Keys.ToList());
                        valueList.AddRange(massCPSDic.Values.ToList());
                    }
                }
                xyItemList.Add(new XYItem(new XYLegend(scanFile.File, colorEmu.Current), new XYItem.Layer(labelList, valueList)));
            }
            return xyItemList;
        }
    }
}
