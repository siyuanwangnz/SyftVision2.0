using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public string Name { get; }
        public string Content { get; }
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

    }
}
