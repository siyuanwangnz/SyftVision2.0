using BatchAnalysis.Services;
using Public.BatchConfig;
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
        public int scanCount { get => SelectedBatchList.Count * BatchProp.MethodList.Count; }
        public List<ScanStatus> GetScanStatusList(Action progress)
        {
            List<ScanStatus> scanStatusList = new List<ScanStatus>();
            foreach (var batch in SelectedBatchList)
            {
                foreach (var scan in batch.ScanList)
                {
                    Scan _scan = new Scan(scan.FullLocalFilePath);
                    scanStatusList.Add(new ScanStatus(scan.File, _scan.Status(), _scan.Result()));
                    progress.Invoke();
                }
            }
            return scanStatusList;
        }
        public List<Info> GetInfoList()
        {
            List<Info> batchInfoList = new List<Info>();
            Scan scan = new Scan(SelectedBatchList.First().ScanList.First().FullLocalFilePath);
            var InstruInfo = scan.GetInstrumentInfo();
            batchInfoList.Add(new Info("Instrument", "Number", InstruInfo.Number));
            batchInfoList.Add(new Info("Instrument", "Mode", InstruInfo.Model));
            batchInfoList.Add(new Info("Instrument", "Serial Number", InstruInfo.SN));
            batchInfoList.Add(new Info("Instrument", "Kiosk Version", InstruInfo.KioskVersion));
            batchInfoList.Add(new Info("Batch", "Name", BatchProp.Name));
            batchInfoList.Add(new Info("Batch", "Number", SelectedBatchList.First().Name));

            return batchInfoList.OrderBy(a => a.Category).ThenBy(e => e.Item).ToList();
        }


    }
}
