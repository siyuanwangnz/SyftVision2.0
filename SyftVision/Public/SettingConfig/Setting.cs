using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.SettingConfig
{
    public class Setting
    {
        public Setting(string name, string content)
        {
            Name = name;
            Content = content;
            TypeList = SettingType.ReferList;
            Type = GetSettingType();
        }
        public Setting(XElement rootNode)
        {
            TypeList = SettingType.ReferList;
            try
            {
                Name = rootNode.Attribute("Name").Value;
                Type = TypeList.Single(a => a.Name == rootNode.Attribute("Type").Value);
                switch (Type.Name)
                {
                    case "Map":
                        if (rootNode.Elements("Map").Count() == 0) break;
                        MapSetList = new ObservableCollection<SettingMap>();
                        foreach (var map in rootNode.Elements("Map"))
                        {
                            SettingMap settingMap = new SettingMap();
                            settingMap.UpdateKeyAndValue(map);
                            MapSetList.Add(settingMap);
                        }
                        break;
                    case "Table":
                        if (rootNode.Elements("Table").Count() == 0) break;
                        TableSetList = new ObservableCollection<SettingTable>();
                        foreach (var table in rootNode.Elements("Table"))
                        {
                            SettingTable settingTable = new SettingTable();
                            settingTable.UpdateKeyAndValue(table);
                            TableSetList.Add(settingTable);
                        }
                        break;
                    case "OnOff":
                        if (rootNode.Element("OnOff") == null) break;
                        OnOff = new SettingOnOff();
                        OnOff.ReferOnOffUpdate(rootNode.Element("OnOff"));
                        break;
                    case "Value":
                        if (rootNode.Element("Value") == null) break;
                        Value = new SettingValue();
                        Value.LimitUpdate(rootNode.Element("Value"));
                        break;
                    case "Text":
                        if (rootNode.Element("Text") == null) break;
                        Text = new SettingText();
                        Text.ReferTextUpdate(rootNode.Element("Text"));
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Name { get; }
        public string Content { get; set; } = "";
        public ObservableCollection<SettingType> TypeList { get; }
        public SettingType Type { get; set; }
        private SettingType GetSettingType()
        {
            if (Content == "") // Empty content
            {
                if (Name.Contains("Map")) return TypeList.Single(a => a.Name == "Map");
                else if (Name.Contains("Table")) return TypeList.Single(a => a.Name == "Table");
                else return TypeList.Single(a => a.Name == "Text");
            }
            else
            {
                try
                {
                    double.Parse(Content);
                }
                catch (Exception)
                {
                    if (Regex.IsMatch(Content, @"^\(.*\)$")) return TypeList.Single(a => a.Name == "Map");
                    else if (Content.Contains(",") && Content.Contains(";")) return TypeList.Single(a => a.Name == "Table");
                    else if (Content == "false" || Content == "true") return TypeList.Single(a => a.Name == "OnOff");
                    else return TypeList.Single(a => a.Name == "Text");
                }
                return TypeList.Single(a => a.Name == "Value");
            }
        }
        public ObservableCollection<SettingMap> MapSetList { get; set; }
        public ObservableCollection<SettingTable> TableSetList { get; set; }
        public SettingOnOff OnOff { get; set; }
        public SettingValue Value { get; set; }
        public SettingText Text { get; set; }
        public XElement XMLGeneration()
        {
            XElement rootNode = new XElement("Setting", new XAttribute("Name", Name), new XAttribute("Type", Type.Name));
            switch (Type.Name)
            {
                case "Map":
                    if (MapSetList != null)
                    {
                        foreach (var map in MapSetList)
                            rootNode.Add(map.XMLGeneration());
                    }
                    break;
                case "Table":
                    if (TableSetList != null)
                    {
                        foreach (var table in TableSetList)
                            rootNode.Add(table.XMLGeneration());
                    }
                    break;
                case "OnOff":
                    if (OnOff != null)
                    {
                        rootNode.Add(OnOff.XMLGeneration());
                    }
                    break;
                case "Value":
                    if (Value != null)
                    {
                        rootNode.Add(Value.XMLGeneration());
                    }
                    break;
                case "Text":
                    if (Text != null)
                    {
                        rootNode.Add(Text.XMLGeneration());
                    }
                    break;
            }
            return rootNode;
        }
        public static ObservableCollection<Setting> GetSettingList(XElement rootNode, ObservableCollection<FilterOff> filterOffList)
        {
            try
            {
                List<FilterOff> _filterOffList = filterOffList.Where(a => a.Wildcard != "").ToList();
                List<Setting> settingList = new List<Setting>();
                foreach (var settingNode in rootNode.Element("settings").Elements("setting"))
                {
                    string name = settingNode.Attribute("name").Value;
                    string content = settingNode.Value;

                    if (_filterOffList.Select(a => a.IsMatched(name)).ToList().Contains(true)) continue;

                    settingList.Add(new Setting(name, content));
                }
                settingList.Sort((a, b) => a.Name.CompareTo(b.Name));
                return new ObservableCollection<Setting>(settingList);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
