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
    public class Mean_AConcXYFactory : MultiBarXYFactory
    {
        public override double GetValue(in ScanFile scanFile, in Component component, in ChartProp chartProp)
        {
            AC_Data acData = scanFile.Scan.GetAC_Data(component.Compound, chartProp.ScanPhase);
            return acData.IsAvailable ? acData.AConcMean() : 0;
        }
    }
}
