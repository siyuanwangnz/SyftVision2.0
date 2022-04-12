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
    public class RSD_ConcXYFactory : SingleBarXYactory
    {
        public override double GetValue(in List<ScanFile> scanFileList, in Component component, in ChartProp chartProp)
        {
            List<double> meanList = new List<double>();
            foreach (var scanFile in scanFileList)
            {
                RP_Data rpData = scanFile.Scan.GetRP_Data(component.Reagent + component.Production, chartProp.ScanPhase, Scan.FastMode.Conc);
                if (rpData.IsAvailable) meanList.Add(rpData.ConcMean());
            }
            return MyMath.RSD(meanList);
        }
    }
}
