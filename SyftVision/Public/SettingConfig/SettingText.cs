using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.SettingConfig
{
    public class SettingText
    {
        public List<string> TextList { get; private set; } = new List<string>() { "" };
        public string ReferText { get; set; } = "";
        public void LimitUpdate(XElement rootNode)
        {
            ReferText = rootNode.Attribute("ReferText").Value;
        }
        public XElement XMLGeneration()
        {
            return new XElement("Text", new XAttribute("ReferText", ReferText));
        }
        public static string GetText(string content)
        {
            return content;
        }
        public void SetText(string content)
        {
            TextList.Clear();
            TextList.Add(GetText(content));
        }
        public void AddText(string content)
        {
            TextList.Add(GetText(content));
        }
    }
}
