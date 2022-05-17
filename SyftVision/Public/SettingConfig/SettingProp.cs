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
        public SettingProp(string title, string subTitle, ObservableCollection<FilterOff> filterOffList, ObservableCollection<Setting> settingList)
        {
            Tittle = title;
            SubTittle = subTitle;
            FilterOffList = new ObservableCollection<FilterOff>(filterOffList.Where(a => a.Wildcard != ""));
            SettingList = new ObservableCollection<Setting>(settingList.Where(a => !a.IsInvalid()));
        }
        public SettingProp(XElement rootNode)
        {
            try
            {
                Tittle = rootNode.Attribute("Tittle").Value;
                SubTittle = rootNode.Attribute("SubTittle").Value;

                FilterOffList = new ObservableCollection<FilterOff>();
                foreach (var wildcard in rootNode.Element("FilterOff").Elements("Wildcard"))
                {
                    FilterOff filterOff = new FilterOff();
                    filterOff.Wildcard = wildcard.Value;
                    FilterOffList.Add(filterOff);
                }

                SettingList = new ObservableCollection<Setting>();
                foreach (var setting in rootNode.Elements("Setting"))
                {
                    SettingList.Add(new Setting(setting));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Tittle { get; private set; }
        public string SubTittle { get; private set; }
        public string Name => $"{Tittle}-{SubTittle}";
        public string File => $"{SubTittle}.xml";
        public ObservableCollection<FilterOff> FilterOffList { get; private set; }
        public ObservableCollection<Setting> SettingList { get; private set; }
        public XElement XMLGeneration()
        {
            XElement rootNode = new XElement("Settings",
                new XAttribute("Tittle", Tittle),
                new XAttribute("SubTittle", SubTittle));

            if (FilterOffList != null)
            {
                XElement filterOffElement = new XElement("FilterOff");

                foreach (var filterOff in FilterOffList)
                    filterOffElement.Add(new XElement("Wildcard", filterOff.Wildcard));

                rootNode.Add(filterOffElement);
            }

            if (SettingList != null)
            {
                foreach (var setting in SettingList)
                {
                    rootNode.Add(setting.XMLGeneration());
                }
            }

            return rootNode;
        }
    }
}
