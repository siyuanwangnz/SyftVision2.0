using Public.Instrument;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public class InjectionXYFactory : OverlapLineXYFactory2
    {
        public override XYItem.Layer GetLayer(ScanFile scanFile)
        {
            Inj_Data inj_DaTa = scanFile.Scan.GetInj_Data();
            return new XYItem.Layer(inj_DaTa.MassList(), inj_DaTa.UPSCurList());
        }
    }
}
