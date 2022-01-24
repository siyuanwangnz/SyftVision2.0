using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class ComponentInfo
    {
        public ComponentInfo(string reagent, string product, int color, string max, string min)
        {
            Reagent = reagent;
            Product = product;
            Color = color;
            Max = max;
            Min = min;
        }
        public string Reagent { get; private set; }
        public string Product { get; private set; }
        public int Color { get; private set; }
        public string RPCode { get => $"{Reagent}{Product}".ToLower().Replace(" ", ""); }
        public string Max { get; private set; }
        public string Min { get; private set; }
    }
}
