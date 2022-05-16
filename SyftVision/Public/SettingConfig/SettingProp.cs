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
        public SettingProp(XElement rootNode, ObservableCollection<FilterOff> filterOffList)
        {
            FilterOffList = filterOffList;

            try
            {
                List<Setting> settingList = new List<Setting>();
                foreach (var settingNode in rootNode.Element("settings").Elements("setting"))
                {
                    string name = settingNode.Attribute("name").Value;
                    string content = settingNode.Value;

                    if (FilterOffList.Select(a => a.IsMatched(name)).ToList().Contains(true)) continue;

                    settingList.Add(new Setting(name, content));
                }
                settingList.Sort((a, b) => a.Name.CompareTo(b.Name));
                SettingList = new ObservableCollection<Setting>(settingList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Tittle { get; set; }
        public string SubTittle { get; set; }
        public ObservableCollection<FilterOff> FilterOffList { get; }
        public ObservableCollection<Setting> SettingList { get; set; }
    }
}
