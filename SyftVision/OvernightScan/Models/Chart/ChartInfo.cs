using OvernightScan.Services;
using Public.Global;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class ChartInfo
    {
        public ChartInfo(Global.BatchType batchType, string chartName, string subCharName, Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, bool lastBatchSelectionEnable)
        {
            ChartConfig = GetConfigChart.BatchChart(batchType, chartName, subCharName);
            Batches = GetScansData.XML(targetScanInfoListDic, ChartConfig, lastBatchSelectionEnable);
        }
        public ConfigChartInfo ChartConfig { get; private set; }
        public Dictionary<string, List<Scan>> Batches { get; private set; }
    }
}
