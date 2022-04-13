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
    public class Overlap_ConcXYFactory : OverlapLineXYFactory
    {
        public override void AddLayer(List<XYItem.Layer> layerList, in Component component, in ChartProp chartProp, in ScanFile scanFile)
        {
            RP_Data rpData = scanFile.Scan.GetRP_Data(component.Reagent + component.Production, chartProp.ScanPhase, Scan.FastMode.Conc);
            if (rpData.IsAvailable) layerList.Add(new XYItem.Layer(rpData.ConcTimeList(), rpData.ConcList(), scanFile.File));
        }
    }
}
