using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.SettingConfig
{
    public class SettingOnOff
    {
        public bool OnOff { get; set; }
        public bool ReferOnOff { get; set; }
        public void OnOffUpdate(XElement rootNode)
        {
            OnOff = rootNode.Value == "true";
        }
        public void ReferOnOffUpdate(XElement rootNode)
        {
            ReferOnOff = rootNode.Attribute("ReferOnOff").Value == "true";
        }
        public XElement XMLGeneration()
        {
            return new XElement("OnOff", new XAttribute("ReferOnOff", ReferOnOff));
        }
        public static bool GetOnOff(string content)
        {
            if (content == "true") return true;
            return false;
        }
    }
}
