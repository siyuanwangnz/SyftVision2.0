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
    public abstract class OverlapLineXYFactory2 : XYFactory
    {
        public override List<XYItem> GetXYItemList(ChartProp chartProp, List<ScanFile> scanFileList)
        {
            List<XYItem> xyItemList = new List<XYItem>();
            var colorEmu = MyColor.Pool.GetEnumerator();
            foreach (var scanFile in scanFileList)
            {
                colorEmu.MoveNext();
                xyItemList.Add(new XYItem(new XYLegend(scanFile.File, colorEmu.Current), new List<XYItem.Layer>() { GetLayer(scanFile) }));
            }
            return xyItemList;
        }

        public abstract XYItem.Layer GetLayer(ScanFile scanFile);
    }
}
