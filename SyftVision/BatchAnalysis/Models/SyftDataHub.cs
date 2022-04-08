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


    }
}
