using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.SettingConfig
{
    public class SettingProp
    {
        public string Tittle { get; set; }
        public string SubTittle { get; set; }
        public ObservableCollection<FilterOff> FilterOffList { get; }
        public ObservableCollection<Setting> SettingList { get; set; }
    }
}
