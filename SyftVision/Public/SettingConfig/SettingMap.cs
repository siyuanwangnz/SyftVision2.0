using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.SettingConfig
{
    public class SettingMap
    {
        public string Key { get; set; } = "";
        public SettingValue Value { get; set; } = new SettingValue();
        public void UpdateKeyAndValue(XElement rootNode)
        {
            Key = rootNode.Attribute("Key").Value;
            Value.LimitUpdate(rootNode.Element("Value"));
        }
        public XElement XMLGeneration()
        {
            return new XElement("Map", new XAttribute("Key", Key), Value.XMLGeneration());
        }
        public static ObservableCollection<SettingMap> GetMapSetList(string content)
        {
            ObservableCollection<SettingMap> mapSetList = new ObservableCollection<SettingMap>();

            while (content.Contains(")"))
            {
                //Get (xxx,xxx,xxx)
                string temp = content.Substring(0, content.IndexOf(")") + 1);
                //Add (xxx,xxx,xxx) to list
                try
                {
                    SettingMap settingMap = new SettingMap();
                    settingMap.Key = temp.Substring(temp.IndexOf("(") + 1, temp.LastIndexOf(",") - 1);
                    settingMap.Value.Value = double.Parse(temp.Substring(temp.LastIndexOf(",") + 1, temp.IndexOf(")") - temp.LastIndexOf(",") - 1));
                    mapSetList.Add(settingMap);
                }
                catch (Exception)
                {

                }
                //Trim off front xxx,xxx;
                content = content.Remove(0, content.IndexOf(")") + 1);
            }

            if (mapSetList.Count == 0) mapSetList.Add(new SettingMap());

            return mapSetList;

        }
    }
}
