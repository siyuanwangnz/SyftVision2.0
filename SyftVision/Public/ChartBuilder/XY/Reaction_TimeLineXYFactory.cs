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
    public class Reaction_TimeLineXYFactory : XYFactory
    {
        public override List<XYItem> GetXYItemList(ChartProp chartProp, List<ScanFile> scanFileList)
        {
            return new List<XYItem>()
            {
                new XYItem
                (
                    new XYLegend("Reaction Time",MyColor.Pool.First()),

                    new XYItem.Layer(scanFileList.Select(a=>a.Date_Time).ToList(),
                                     scanFileList.Select(a=>a.Scan.GetReactionTime()).ToList())
                )
            };
        }
    }
}
