using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SettingConfig
{
    public class SettingValue
    {
        public SettingValue() { }
        public SettingValue(string content)
        {
            Value = double.Parse(content);
        }
        public double Value { get; set; }
        public double UnderLimit { get; set; }
        public double UpperLimit { get; set; }
    }
}
