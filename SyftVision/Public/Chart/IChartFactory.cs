using ChartDirector;
using Public.ChartConfig;
using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.Chart
{
    public interface IChartFactory
    {
        IXY GetXY(List<ScanFile> scanFileList);
        BaseChart GetChart(ChartProp chartProp);
    }
}
