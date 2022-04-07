using BatchAnalysis.Models;
using Public.BatchConfig;
using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.Services
{
    public class SyftMatch
    {
        public SyftMatch(List<ScanFile> scanFileList, string matchLevel)
        {
            ScanFileList = scanFileList;
            MatchLevel = matchLevel;
        }

        public List<ScanFile> ScanFileList { get; }
        public string MatchLevel { get; }
        public List<MatchedBatch> GetMatchedBatchList(BatchProp batchProp)
        {
            List<MatchedBatch> matchedBatchList = new List<MatchedBatch>();

            List<int> indexList = new List<int>();
            switch (MatchLevel)
            {
                case "High":
                    indexList = GetHighLevelMatchedIndex(ScanFileList.Select(a => a.NameHashCode).ToList(),
                        batchProp.MethodList.Select(a => a.NameHashCode).ToList());
                    foreach (var index in indexList)
                        matchedBatchList.Add(new MatchedBatch(ScanFileList.GetRange(index, batchProp.MethodList.Count)));
                    break;
                case "Low":
                    indexList = GetLowLevelMatchedIndex(ScanFileList.Select(a => a.NameHashCode).ToList(),
                        batchProp.MethodList.Select(a => a.NameHashCode).ToList());
                    foreach (var index in indexList)
                    {
                        List<ScanFile> scanFileList = ScanFileList.GetRange(index, batchProp.MethodList.Count);
                        ReorderScanFileList(scanFileList, batchProp);
                        matchedBatchList.Add(new MatchedBatch(scanFileList));
                    }
                    break;
                default:
                    break;
            }

            return matchedBatchList;
        }

        private static List<int> GetHighLevelMatchedIndex(List<int> sourceList, List<int> referenceList)
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < sourceList.Count - referenceList.Count + 1; i++)
            {
                if (sourceList.Skip(i).Take(referenceList.Count).SequenceEqual(referenceList))
                    indexList.Add(i);
            }
            return indexList;
        }

        private static List<int> GetLowLevelMatchedIndex(List<int> sourceList, List<int> referenceList)
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < sourceList.Count - referenceList.Count + 1; i++)
            {
                if (sourceList[i] == referenceList[0])
                {
                    if (sourceList.Skip(i).Take(referenceList.Count).OrderBy(e => e).SequenceEqual(referenceList.OrderBy(e => e)))
                        indexList.Add(i);
                }
            }
            return indexList;
        }

        private void ReorderScanFileList(List<ScanFile> scanFileList, BatchProp batchProp)
        {
            int count = scanFileList.Count;
            for (int i = 0; i < count; i++)
            {
                if (scanFileList[i].NameHashCode != batchProp.MethodList[i].NameHashCode)
                {
                    int index = scanFileList.FindIndex(i, count - i, e => e.NameHashCode == batchProp.MethodList[i].NameHashCode);

                    scanFileList.Insert(i, scanFileList[index]);

                    scanFileList.RemoveAt(index + 1);
                }

            }
        }

    }
}
