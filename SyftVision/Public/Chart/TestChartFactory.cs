using ChartDirector;
using Public.Chart.XY;
using Public.ChartConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.Chart
{
    public class TestChartFactory : ChartFactory
    {
        public TestChartFactory(XYFactory xyFactory) : base(xyFactory)
        {
        }

        public override BaseChart SetChart(ChartProp chartProp, List<XYItem> xyItemList)
        {
            throw new NotImplementedException();
        }
    }
}
