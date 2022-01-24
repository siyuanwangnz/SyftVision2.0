using MathNet.Numerics.Statistics;
using Public.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    class LineXY
    {
        public LineXY(string reagent, string product, int color, List<double> x, List<double> y, List<string> label)
        {
            Reagent = reagent;
            Product = product;
            Color = color;
            X = x;
            Y = y;
            Label = label;
        }
        public string Reagent { get; private set; }
        public string Product { get; private set; }
        public int Color { get; private set; }
        public List<double> X { get; private set; }
        public List<double> Y { get; private set; }
        public List<string> Label { get; private set; }
    }
}
