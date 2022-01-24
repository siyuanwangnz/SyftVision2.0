using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class ConfigChartInfo
    {
        public ConfigChartInfo(string chartName, string subChartName, string phase, string accept, List<string> markCodeList, List<ComponentInfo> componentInfoList)
        {
            ChartName = chartName;
            SubChartName = subChartName;
            Phase = phase;
            Accept = accept;
            MarkCodeList = markCodeList;
            ComponentInfoList = componentInfoList;
        }
        public string ChartName { get; private set; }
        public string SubChartName { get; private set; }
        public string Phase { get; private set; }
        public string Accept { get; private set; }
        public List<string> MarkCodeList { get; private set; }
        public List<ComponentInfo> ComponentInfoList { get; private set; }
        public Scan.Phase ScanPhase
        {
            get
            {
                Scan.Phase phase = Scan.Phase.All;
                if (Phase == null) return phase;
                switch (Phase)
                {
                    default:
                    case "Sample":
                        phase = Scan.Phase.Sample;
                        break;
                    case "Background":
                        phase = Scan.Phase.Background;
                        break;
                    case "Preparation":
                        phase = Scan.Phase.Preparation;
                        break;
                }
                return phase;
            }
        }
    }
}
