using System.Collections.Generic;

namespace SettingCheck.Models
{
    class DetectionSetting
    {
        public string Unit { get; set; }
        public Setting Setting { get; set; }
        public List<Setting> SettingList
        {
            get
            {
                List<Setting> temp = new List<Setting>();
                if (Setting != null) temp.Add(Setting);
                return temp;
            }
        }

    }
}
