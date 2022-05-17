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
        public string Text { get; set; } = "";
        public string ReferText { get; set; } = "";
        public void TextUpdate(XElement rootNode)
        {
            Text = rootNode.Value;
        }
        public void ReferTextUpdate(XElement rootNode)
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
