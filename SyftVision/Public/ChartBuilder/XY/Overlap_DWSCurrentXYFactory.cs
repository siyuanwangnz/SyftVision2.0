using Public.ChartConfig;
using Public.Instrument;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public class Overlap_DWSCurrentXYFactory : OverlapLineXYFactory
    {
        public override void AddLayer(List<XYItem.Layer> layerList, in Component component, in ChartProp chartProp, in ScanFile scanFile)
        {
            RC_Data rcData = scanFile.Scan.GetRC_Data(component.Reagent);
            if (rcData.IsAvailable) layerList.Add(new XYItem.Layer(rcData.CurTimeList(), rcData.DWSCurList(), scanFile.File));
        }
    }
}
