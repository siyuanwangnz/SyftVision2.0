using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Public.ChartBuilder.XY
{
    public class XYLegend
    {
        public XYLegend(string content, int color)
        {
            Content = content;
            Color = color;
        }
        public string Content { get; }
        public int Color { get; }
        public Brush Brush { get => (Brush)new BrushConverter().ConvertFromString($"#{Color.ToString("X")}"); }
    }
}
