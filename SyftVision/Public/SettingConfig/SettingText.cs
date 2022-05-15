using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SettingConfig
{
    public class SettingText
    {
        public string Text { get; set; } = "";
        public string ReferText { get; set; } = "";
        public static string GetText(string content)
        {
            return content;
        }
    }
}
