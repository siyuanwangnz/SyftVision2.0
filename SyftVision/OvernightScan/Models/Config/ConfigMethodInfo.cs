using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class ConfigMethodInfo
    {
        public ConfigMethodInfo(string name, List<MarkInfo> markList)
        {
            Name = name;
            MarkList = markList;
        }

        public string Name { get; private set; }
        public string NameCode { get => Name?.ToLower().Replace(" ", "") ?? ""; }

        public List<MarkInfo> MarkList { get; private set; }
        public List<string> MarkCodeList { get => MarkList.Select(a => a.MarkCode).ToList(); }
    }
}
