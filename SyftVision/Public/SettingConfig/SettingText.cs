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
        public List<string> TextList { get; set; } = new List<string>() { "" };
        public string ReferText { get; set; } = "";
        //public void TextUpdate(XElement rootNode)
        //{
        //    Text = rootNode.Value;
        //}
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
    }
}
