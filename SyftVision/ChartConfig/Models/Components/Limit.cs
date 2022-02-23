using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.Models.Components
{
    public interface Limit
    {
        double Limit { get; set; }
        bool LimitEnable { get; set; }
    }
}
