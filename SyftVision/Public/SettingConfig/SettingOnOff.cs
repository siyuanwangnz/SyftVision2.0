using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SettingConfig
{
    public class SettingOnOff
    {
        public SettingOnOff() { }
        public SettingOnOff(string content)
        {
            if (content == "true") OnOff = true;
            else OnOff = false;
        }
        public bool OnOff { get; set; }
        public bool ReferOnOff { get; set; }
    }
}
