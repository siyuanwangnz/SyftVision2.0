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
        public List<bool> OnOffList { get; private set; } = new List<bool>() { false };
        public bool ReferOnOff { get; set; }
        public void LimitUpdate(XElement rootNode)
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
        public void SetOnOff(string content)
        {
            OnOffList.Clear();
            OnOffList.Add(GetOnOff(content));
        }
        public void AddOnOff(string content)
        {
            OnOffList.Add(GetOnOff(content));
        }
    }
}
