using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.Models
{
    public class ChartType
    {
        public ChartType(string name, Component component)
        {
            Name = name;
            Component = component;
        }
        public string Name { get; }
        public Component Component { get; }
        public string FullName => Component.LimitEnable == true ? "Bar" + " - " + Name : "Line" + " - " + Name;
    }
}
