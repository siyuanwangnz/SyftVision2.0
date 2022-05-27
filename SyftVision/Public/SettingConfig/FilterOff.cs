using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Public.SettingConfig
{
    public class FilterOff
    {
        public string Wildcard { get; set; } = "";
        public string Regular => "^" + Regex.Escape(Wildcard).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        public bool IsMatched(string name)
        {
            return Regex.IsMatch(name, Regular);
        }
    }
}
