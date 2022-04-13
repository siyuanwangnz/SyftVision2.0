using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public class ICFXYFactory : OverlapLineXYFactory2
    {
        public override XYItem.Layer GetLayer(ScanFile scanFile)
        {
            Dictionary<double, double> icfTable = scanFile.Scan.GetICF_Table();
            return new XYItem.Layer(icfTable.Keys.ToList(), icfTable.Values.ToList());
        }
    }
}
