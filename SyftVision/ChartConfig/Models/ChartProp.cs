using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.Models
{
    public class ChartProp
    {
        public enum Type
        {
            Bar,
            Line,
        }
        public ChartProp(Type chartType, string name)
        {
            ChartType = chartType;
            Name = name;
        }
        public Type ChartType { get; }
        public string Name { get; }
        public string FullName => ChartType + " - " + Name;
        public bool LimitEnable => ChartType == Type.Bar ? true : false;
    }
}
