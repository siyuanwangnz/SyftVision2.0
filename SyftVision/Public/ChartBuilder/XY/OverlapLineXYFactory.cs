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
    public abstract class OverlapLineXYFactory : XYFactory
    {
        public override List<XYItem> GetXYItemList(ChartProp chartProp, List<ScanFile> scanFileList)
        {
            List<XYItem> xyItemList = new List<XYItem>();
            var colorEmu = MyColor.Pool.GetEnumerator();
            foreach (var component in chartProp.ComponentList)
            {
                colorEmu.MoveNext();
                List<XYItem.Layer> layerList = new List<XYItem.Layer>();
                foreach (var scanFile in scanFileList)
                    AddLayer(layerList, component, chartProp, scanFile);
                xyItemList.Add(new XYItem(new XYLegend(!component.ProductionEnable ? component.Reagent : $"{component.Reagent}/{component.Production}", colorEmu.Current), layerList));
            }
            return xyItemList;
        }

        public abstract void AddLayer(List<XYItem.Layer> layerList, in Component component, in ChartProp chartProp, in ScanFile scanFile);
    }
}
