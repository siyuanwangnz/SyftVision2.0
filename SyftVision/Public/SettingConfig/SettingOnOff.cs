using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SettingConfig
{
    public class SettingOnOff
    {
        public bool OnOff { get; set; }
        public bool ReferOnOff { get; set; }
        public static bool GetOnOff(string content)
        {
            if (content == "true") return true;
            return false;
        }
    }
}
