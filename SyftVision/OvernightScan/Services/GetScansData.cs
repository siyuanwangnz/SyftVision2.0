using OvernightScan.Models;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OvernightScan.Services
{
    class GetScansData
    {
        //Dictionary: batch - scans
        public static Dictionary<string, List<Scan>> XML(Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic, ConfigChartInfo configChartInfo, bool lastBatchSelectionEnable)
        {
            Dictionary<string, List<Scan>> batches = new Dictionary<string, List<Scan>>();

            if (targetScanInfoListDic.Count == 0) return null;
            if (configChartInfo == null) return null;

            List<string> markCodeList = configChartInfo.MarkCodeList;

            if (lastBatchSelectionEnable)
            {
                List<Scan> scanList = new List<Scan>();
                var lastBatchKey = targetScanInfoListDic.Keys.Last();
                var lastBatchValue = targetScanInfoListDic.Values.Last();
                foreach (var targetScanInfo in lastBatchValue)
                {
                    foreach (var item in targetScanInfo.MarkList)
                    {
                        if (markCodeList.Contains(item.MarkCode))
                            scanList.Add(new Scan($"./temp/Batch_Analysis/{lastBatchKey}/{item.Main}/{item.Sub}/{targetScanInfo.FileName}"));
                    }
                }
                batches.Add(lastBatchKey, scanList);
            }
            else
            {
                foreach (var batch in targetScanInfoListDic)
                {
                    List<Scan> scanList = new List<Scan>();
                    foreach (var targetScanInfo in batch.Value)
                    {
                        foreach (var item in targetScanInfo.MarkList)
                        {
                            if (markCodeList.Contains(item.MarkCode))
                                scanList.Add(new Scan($"./temp/Batch_Analysis/{batch.Key}/{item.Main}/{item.Sub}/{targetScanInfo.FileName}"));
                        }
                    }
                    batches.Add(batch.Key, scanList);
                }
            }
            return batches;
        }
    }
}
