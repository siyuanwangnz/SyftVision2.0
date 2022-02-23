using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.Models.Components
{
    public class ACComponent : AC, Limit
    {
        public ACComponent(string analyteConc, bool limitEnable, double limit = 0)
        {
            AnalyteConc = analyteConc;
            LimitEnable = limitEnable;
            Limit = limit;
        }
        public string AnalyteConc { get; set; }
        public double Limit { get; set; }
        public bool LimitEnable { get; set; }
    }
}
