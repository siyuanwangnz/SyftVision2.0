using Public.Chart;
using Public.Chart.XY;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartConfig
{
    public class ChartType
    {
        public static ObservableCollection<ChartType> ReferList = new ObservableCollection<ChartType>() {
            new ChartType("Sensitivities", new Component(true, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),
            new ChartType("Impurities", new Component(true, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),

            new ChartType("LODs_Conc", new Component(true, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),
            new ChartType("LODs_AConc", new Component(true, Component.Mode.C), new TestChartFactory(new TestXYFactory())),

            new ChartType("AConc", new Component(true, Component.Mode.C), new TestChartFactory(new TestXYFactory())),

            new ChartType("RSD_Conc", new Component(true, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),
            new ChartType("RSD_CPS", new Component(true, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),

            new ChartType("DEV_CPS", new Component(true, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),
            new ChartType("DEV_Conc", new Component(true, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),

            new ChartType("Overlap_Conc", new Component(false, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),
            new ChartType("Overlap_CPS", new Component(false, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),

            new ChartType("Trace_Conc", new Component(false, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),
            new ChartType("Trace_CPS", new Component(false, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),

            new ChartType("Current_UPS", new Component(false, Component.Mode.R), new TestChartFactory(new TestXYFactory())),
            new ChartType("Current_DWS", new Component(false, Component.Mode.R), new TestChartFactory(new TestXYFactory())),

            new ChartType("Mass", new Component(false, Component.Mode.RP), new TestChartFactory(new TestXYFactory())),

            new ChartType("ICF", new Component(false, Component.Mode.Null), new TestChartFactory(new TestXYFactory())),
            new ChartType("Injection", new Component(false, Component.Mode.Null), new TestChartFactory(new TestXYFactory())),
            new ChartType("Reaction Time", new Component(false, Component.Mode.Null), new TestChartFactory(new TestXYFactory())),
        };
        public ChartType(string name, Component component, ChartFactory chartFactory)
        {
            Name = name;
            Component = component;
            ChartFactory = chartFactory;
        }
        public string Name { get; }
        public Component Component { get; }
        public ChartFactory ChartFactory { get; }
        public string FullName => Component.LimitEnable == true ? "Bar#" + Name : "Line#" + Name;
    }
}
