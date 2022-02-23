using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.Models.Components
{
    public class RPComponent : RP, Limit
    {
        public RPComponent(string reagent, string product, bool limitEnable, double limit = 0)
        {
            Reagent = reagent;
            Production = product;
            LimitEnable = limitEnable;
            Limit = limit;
        }
        public string Reagent { get; set; }
        public string Production { get; set; }
        public double Limit { get; set; }
        public bool LimitEnable { get; set; }
    }
}
