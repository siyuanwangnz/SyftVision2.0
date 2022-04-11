using BatchAnalysis.Services;
using Public.BatchConfig;
using Public.ChartConfig;
using Public.Instrument;
using Public.SFTP;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.Models
{
    public class SyftDataHub
    {
        public SyftDataHub(BatchProp batchProp, SyftMatch match)
        {
            BatchProp = batchProp;
            MatchedBatchList = match.GetMatchedBatchList(BatchProp);
        }
        public BatchProp BatchProp { get; }
        public List<MatchedBatch> MatchedBatchList { get; }
        public List<MatchedBatch> SelectedBatchList { get => MatchedBatchList.Where(a => a.IsChecked == true).ToList(); }
        public int ScanCount { get => SelectedBatchList.Count * BatchProp.MethodList.Count; }
        // Syft scan list
        public List<SyftScan> SyftScanList { get; private set; }
        public List<SyftScan> GetSyftScanList(Action progress)
        {
            SyftScanList = new List<SyftScan>();
            foreach (var batch in SelectedBatchList)
            {
                foreach (var scan in batch.ScanList)
                {
                    Scan _scan = new Scan(scan.FullLocalFilePath);
                    SyftScanList.Add(new SyftScan(scan.File, _scan.Status(), _scan.Result()));
                    progress.Invoke();
                }
            }
            return SyftScanList;
        }
        // Syft info list
        public List<SyftInfo> SyftInfoList { get; private set; }
        public List<SyftInfo> GetSyftInfoList()
        {
            SyftInfoList = new List<SyftInfo>();
            Scan scan = new Scan(SelectedBatchList.First().ScanList.First().FullLocalFilePath);
            var InstruInfo = scan.GetInstrumentInfo();
            SyftInfoList.Add(new SyftInfo("Instrument", "Number", InstruInfo.Number));
            SyftInfoList.Add(new SyftInfo("Instrument", "Mode", InstruInfo.Model));
            SyftInfoList.Add(new SyftInfo("Instrument", "Serial Number", InstruInfo.SN));
            SyftInfoList.Add(new SyftInfo("Instrument", "Kiosk Version", InstruInfo.KioskVersion));
            SyftInfoList.Add(new SyftInfo("Batch", "Name", BatchProp.Name));
            SyftInfoList.Add(new SyftInfo("Batch", "Number", SelectedBatchList.First().Name));

            SyftInfoList = SyftInfoList.OrderBy(a => a.Category).ThenBy(e => e.Item).ToList();
            return SyftInfoList;
        }
        // Syft chart list
        public List<SyftChart> SyftChartList { get; private set; }
        public List<SyftChart> GetSyftChartList(Action progress)
        {
            SyftChartList = new List<SyftChart>();
            foreach (var chartProp in BatchProp.ChartPropList)
            {
                SyftChartList.Add(new SyftChart(chartProp, GetMarkedScanFileList(chartProp.HashCode)));
                progress.Invoke();
            }
            return SyftChartList;

        }
        private List<ScanFile> GetMarkedScanFileList(int chartHashCode)
        {
            List<ScanFile> scanFileList = new List<ScanFile>();
            List<int> indexList = GetMarkedIndexList(chartHashCode);
            foreach (var batch in SelectedBatchList)
            {
                foreach (var index in indexList)
                {
                    scanFileList.Add(batch.ScanList[index]);
                }
            }
            return scanFileList;
        }
        private List<int> GetMarkedIndexList(int chartHashCode)
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < BatchProp.MethodList.Count; i++)
            {
                if (BatchProp.MethodList[i].ChartHashCodeList.Contains(chartHashCode))
                    indexList.Add(i);
            }
            return indexList;
        }


    }
}
