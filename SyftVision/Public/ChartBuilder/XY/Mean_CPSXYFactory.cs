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
    public class Mean_CPSXYFactory : MultiBarXYFactory
    {
        public override double GetValue(in ScanFile scanFile, in Component component, in ChartProp chartProp)
        {
            RP_Data rpData = scanFile.Scan.GetRP_Data(component.Reagent + component.Production, chartProp.ScanPhase, Scan.FastMode.Conc);
            return rpData.IsAvailable ? rpData.CPSMean() : 0;
        }
    }
}
