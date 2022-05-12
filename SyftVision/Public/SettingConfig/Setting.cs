using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SettingConfig
{
    public class Setting
    {
        public enum Type { Text, Value, OnOff, Table, Map }
        public string Name { get; set; } = "";
        public string Content { get; set; } = "";
        public Type TypeSet { get; set; } = Type.Text;
    }
}
