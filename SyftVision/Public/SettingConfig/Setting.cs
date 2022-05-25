using Public.Instrument;
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
            ContentList[0] = content;
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
                        MapSetList = new List<SettingMap>();
                        foreach (var map in rootNode.Elements("Map"))
                        {
                            SettingMap settingMap = new SettingMap();
                            settingMap.LimitUpdate(map);
                            MapSetList.Add(settingMap);
                        }
                        break;
                    case "Table":
                        if (rootNode.Elements("Table").Count() == 0) break;
                        TableSetList = new List<SettingTable>();
                        foreach (var table in rootNode.Elements("Table"))
                        {
                            SettingTable settingTable = new SettingTable();
                            settingTable.LimitUpdate(table);
                            TableSetList.Add(settingTable);
                        }
                        break;
                    case "OnOff":
                        if (rootNode.Element("OnOff") == null) break;
                        OnOff = new SettingOnOff();
                        OnOff.LimitUpdate(rootNode.Element("OnOff"));
                        break;
                    case "Value":
                        if (rootNode.Element("Value") == null) break;
                        Value = new SettingValue();
                        Value.LimitUpdate(rootNode.Element("Value"));
                        break;
                    case "Text":
                        if (rootNode.Element("Text") == null) break;
                        Text = new SettingText();
                        Text.LimitUpdate(rootNode.Element("Text"));
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Name { get; }
        public List<string> ContentList { get; private set; } = new List<string>() { "" };
        public List<ScanFile> ScanList { get; private set; } = new List<ScanFile>();
        public List<SettingType> TypeList { get; }
        public SettingType Type { get; set; }
        private SettingType GetSettingType()
        {
            if (ContentList[0] == "") // Empty content
            {
                if (Name.Contains("Map")) return TypeList.Single(a => a.Name == "Map");
                else if (Name.Contains("Table")) return TypeList.Single(a => a.Name == "Table");
                else return TypeList.Single(a => a.Name == "Text");
            }
            else
            {
                try
                {
                    double.Parse(ContentList[0]);
                }
                catch (Exception)
                {
                    if (Regex.IsMatch(ContentList[0], @"^\(.*\)$")) return TypeList.Single(a => a.Name == "Map");
                    else if (ContentList[0].Contains(",") && ContentList[0].Contains(";")) return TypeList.Single(a => a.Name == "Table");
                    else if (ContentList[0] == "false" || ContentList[0] == "true") return TypeList.Single(a => a.Name == "OnOff");
                    else return TypeList.Single(a => a.Name == "Text");
                }
                return TypeList.Single(a => a.Name == "Value");
            }
        }
        public List<SettingMap> MapSetList { get; set; }
        public List<SettingTable> TableSetList { get; set; }
        public SettingOnOff OnOff { get; set; }
        public SettingValue Value { get; set; }
        public SettingText Text { get; set; }
        public bool IsOut { get; set; }
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
        public bool IsInvalid()
        {
            switch (Type.Name)
            {
                case "Map":
                    if (MapSetList == null) return true;
                    break;
                case "Table":
                    if (TableSetList == null) return true;
                    break;
                case "OnOff":
                    if (OnOff == null) return true;
                    break;
                case "Value":
                    if (Value == null) return true;
                    break;
                case "Text":
                    if (Text == null) return true;
                    break;
            }
            return false;
        }
        public static List<Setting> GetSettingList(XElement rootNode, List<FilterOff> filterOffList)
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
                return settingList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SetContent(string content, ScanFile scanFile)
        {
            ContentList.Clear();
            ContentList.Add(content);
            ScanList.Clear();
            ScanList.Add(scanFile);

            switch (Type.Name)
            {
                case "Map":
                    List<SettingMap> newMapSetList = SettingMap.GetMapSetList(content);
                    foreach (var map in MapSetList)
                    {
                        double d;
                        try
                        {
                            d = newMapSetList.Single(a => a.Key == map.Key).Value.ValueList[0];
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        map.Value.SetValue(d);
                    }
                    IsOut = MapSetList.Select(a => a.Value.IsOut).ToList().Contains(true) ? true : false;
                    break;
                case "Table":
                    List<SettingTable> newTableSetList = SettingTable.GetTableSetList(content);
                    foreach (var table in TableSetList)
                    {
                        double d;
                        try
                        {
                            d = newTableSetList.Single(a => a.Key == table.Key).Value.ValueList[0];
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        table.Value.SetValue(d);
                    }
                    IsOut = TableSetList.Select(a => a.Value.IsOut).ToList().Contains(true) ? true : false;
                    break;
                case "OnOff":
                    OnOff.SetOnOff(content);
                    IsOut = OnOff.IsOut;
                    break;
                case "Value":
                    Value.SetValue(content);
                    IsOut = Value.IsOut;
                    break;
                case "Text":
                    Text.SetText(content);
                    IsOut = Text.IsOut;
                    break;
            }
        }
        public void AddContent(string content, ScanFile scanFile)
        {
            ContentList.Add(content);
            ScanList.Add(scanFile);

            switch (Type.Name)
            {
                case "Map":
                    List<SettingMap> newMapSetList = SettingMap.GetMapSetList(content);
                    foreach (var map in MapSetList)
                    {
                        double d;
                        try
                        {
                            d = newMapSetList.Single(a => a.Key == map.Key).Value.ValueList[0];
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        map.Value.AddValue(d);
                    }
                    break;
                case "Table":
                    List<SettingTable> newTableSetList = SettingTable.GetTableSetList(content);
                    foreach (var table in TableSetList)
                    {
                        double d;
                        try
                        {
                            d = newTableSetList.Single(a => a.Key == table.Key).Value.ValueList[0];
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        table.Value.AddValue(d);
                    }
                    break;
                case "OnOff":
                    OnOff.AddOnOff(content);
                    break;
                case "Value":
                    Value.AddValue(content);
                    break;
                case "Text":
                    Text.AddText(content);
                    break;
            }
        }
    }
}
