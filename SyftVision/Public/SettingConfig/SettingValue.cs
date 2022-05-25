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
        public List<double> ValueList { get; private set; } = new List<double>() { 0 };
        public double UnderLimit { get; set; }
        public double UpperLimit { get; set; }
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
        public void SetValue(string content)
        {
            ValueList.Clear();
            ValueList.Add(GetValue(content));
        }
        public void SetValue(double value)
        {
            ValueList.Clear();
            ValueList.Add(value);
        }
        public void AddValue(string content)
        {
            ValueList.Add(GetValue(content));
        }
        public void AddValue(double value)
        {
            ValueList.Add(value);
        }

    }
}
