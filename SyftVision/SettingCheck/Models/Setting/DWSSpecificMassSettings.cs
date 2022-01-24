using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SettingCheck.Models
{
    class DWSSpecificMassSettings
    {
        public string Unit { get; set; } = "";

        public List<Setting> MassList_Pos { get; set; }
        public List<Setting> MassList_Neg { get; set; }

        public bool Mirror_Check
        {
            get
            {
                foreach (var setting in MassList_Pos)
                {
                    Setting temp = MassList_Neg?.Find(a => Math.Abs(double.Parse(a.Name)) == Math.Abs(double.Parse(setting.Name)));
                    if (temp == null) continue;
                    else
                    {
                        if (Math.Abs(temp.SettingValue) != Math.Abs(setting.SettingValue))
                            return false;
                    }
                    return true;
                }
                return false;
            }
        }

        public bool Linearity_Check
        {
            get
            {
                if (MassList_Pos.Count <= 0)
                    return false;
                for (int i = 0; i < MassList_Pos.Count - 1; i++)
                {
                    if (Math.Abs(MassList_Pos[i].SettingValue) > Math.Abs(MassList_Pos[i + 1].SettingValue))
                        return false;
                }
                if (MassList_Neg.Count > 0)
                {
                    for (int i = 0; i < MassList_Neg.Count - 1; i++)
                    {
                        if (Math.Abs(MassList_Neg[i].SettingValue) < Math.Abs(MassList_Neg[i + 1].SettingValue))
                            return false;
                    }
                }
                return true;
            }
        }
    }
}
