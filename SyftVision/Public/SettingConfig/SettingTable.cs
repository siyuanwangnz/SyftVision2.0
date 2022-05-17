using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.SettingConfig
{
    public class SettingTable
    {
        public double Key { get; set; }
        public SettingValue Value { get; set; } = new SettingValue();
        public void UpdateKeyAndValue(XElement rootNode)
        {
            Key = double.Parse(rootNode.Attribute("Key").Value);
            Value.LimitUpdate(rootNode.Element("Value"));
        }
        public XElement XMLGeneration()
        {
            return new XElement("Table", new XAttribute("Key", Key), Value.XMLGeneration());
        }
        public static ObservableCollection<SettingTable> GetTableSetList(string content)
        {
            ObservableCollection<SettingTable> tableSetList = new ObservableCollection<SettingTable>();

            while (content.Contains(";"))
            {
                //Get xxx,xxx;
                string temp = content.Substring(0, content.IndexOf(";") + 1);
                //Add xxx,xxx; to list
                try
                {
                    SettingTable settingTable = new SettingTable();
                    settingTable.Key = double.Parse(temp.Substring(0, temp.IndexOf(",")));
                    settingTable.Value.Value = double.Parse(temp.Substring(temp.IndexOf(",") + 1, temp.IndexOf(";") - temp.IndexOf(",") - 1));
                    tableSetList.Add(settingTable);
                }
                catch (Exception)
                {

                }
                //Trim off front xxx,xxx;
                content = content.Remove(0, content.IndexOf(";") + 1);
            }

            if (tableSetList.Count == 0) tableSetList.Add(new SettingTable());

            return tableSetList;
        }
    }
}
