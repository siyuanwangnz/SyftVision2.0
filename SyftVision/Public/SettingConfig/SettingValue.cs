using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.SettingConfig
{
    public class SettingValue
    {
        public List<double> ValueList { get; set; } = new List<double>() { 0 };
        public double UnderLimit { get; set; }
        public double UpperLimit { get; set; }
        //public void ValueUpdate(XElement rootNode)// TODO: need change
        //{
        //    try
        //    {
        //        Value = double.Parse(rootNode.Value);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        public void LimitUpdate(XElement rootNode)
        {
            UnderLimit = double.Parse(rootNode.Attribute("UnderLimit").Value);
            UpperLimit = double.Parse(rootNode.Attribute("UpperLimit").Value);
        }
        public XElement XMLGeneration()
        {
            return new XElement("Value", new XAttribute("UnderLimit", UnderLimit), new XAttribute("UpperLimit", UpperLimit));
        }
        public static double GetValue(string content)
        {
            try
            {
                return double.Parse(content);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
