using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SettingCheck.Services
{
    static class GetConfigData
    {
        //Range by node
        public static string Range(XElement RootNode, string ModeName, string NodeName, string SubNodeName, string Range)
        {
            if (RootNode == null) return "";

            IEnumerable<XElement> targetNodes =
                   from target in RootNode.Descendants(ModeName)
                   where target.Element(NodeName).Attribute("unit") != null
                   select target.Element(NodeName);

            foreach (var element in targetNodes)
            {
                return $"{element.Element(SubNodeName).Element(Range).Value}";
            }
            return "";
        }

        //Unit by node
        public static string Unit(XElement RootNode, string PhaseName, string NodeName)
        {
            if (RootNode == null) return "";

            IEnumerable<XElement> targetNodes =
                   from target in RootNode.Descendants(PhaseName)
                   where target.Element(NodeName).Attribute("unit") != null
                   select target.Element(NodeName);

            foreach (var element in targetNodes)
            {
                return $"{element.Attribute("unit").Value}";
            }
            return "";
        }
    }
}
