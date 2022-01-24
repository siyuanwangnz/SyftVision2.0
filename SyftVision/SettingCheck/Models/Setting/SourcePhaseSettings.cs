using System.Collections.Generic;

namespace SettingCheck.Models
{
    class SourcePhaseSettings
    {
        public string Unit { get; set; } = "";
        public Setting PosWet { get; set; }
        public Setting NetWet { get; set; }
        public Setting NetDry { get; set; }
        public List<Setting> SourPhaseList
        {
            get
            {
                List<Setting> temp = new List<Setting>();

                if (PosWet != null) temp.Add(PosWet);
                if (NetWet != null) temp.Add(NetWet);
                if (NetDry != null) temp.Add(NetDry);

                return temp;
            }
        }
    }
}
