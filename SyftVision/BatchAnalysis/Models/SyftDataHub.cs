using BatchAnalysis.Services;
using Public.BatchConfig;
using Public.Instrument;
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

    }
}
