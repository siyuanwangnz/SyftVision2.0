using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SettingConfig
{
    public class SettingTable<T>
    {
        public T Key { get; set; }
        public SettingValue SyftValue { get; set; }
    }
}
