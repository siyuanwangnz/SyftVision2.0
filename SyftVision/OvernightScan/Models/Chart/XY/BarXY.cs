using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class BarXY
    {
        public BarXY(string reagent, string product, double y, string max, string min, string accept)
        {
            Reagent = reagent;
            Product = product;
            Y = y;
            Max = max;
            Min = min;
            Accept = accept;
        }
        public string Reagent { get; private set; }
        public string Product { get; private set; }
        public double Y { get; set; }
        public string Max { get; set; }
        public string Min { get; set; }
        public string Accept { get; set; }

        public string X { get => $"{Reagent}/{Product}"; }

        public int Color
        {
            get
            {
                int color = 0x5588bb;
                if (Accept == null) return color;
                switch (Accept)
                {
                    case "Above":
                        if (Min == null) return color;
                        if (Y >= double.Parse(Min))
                            color = 0x80ff80;
                        else
                            color = 0xff8080;
                        break;
                    case "Under":
                        if (Max == null) return color;
                        if (Y <= double.Parse(Max))
                            color = 0x80ff80;
                        else
                            color = 0xff8080;
                        break;
                    default:
                        return color;
                }
                return color;
            }
        }
    }
}
