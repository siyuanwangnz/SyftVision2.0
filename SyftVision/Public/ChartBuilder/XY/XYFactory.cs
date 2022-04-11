using Public.ChartConfig;
using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public abstract class XYFactory
    {
        public abstract List<XYItem> GetXYItemList(ChartProp chartProp, List<ScanFile> scanFileList);
    }
}
