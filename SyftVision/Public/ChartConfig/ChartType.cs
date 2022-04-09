using Public.Chart;
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
            new ChartType("Sensitivities", new Component(true, Component.Mode.RP), new ChartFactory()),
            new ChartType("Impurities", new Component(true, Component.Mode.RP), new ChartFactory()),

            new ChartType("LODs_Conc", new Component(true, Component.Mode.RP), new ChartFactory()),
            new ChartType("LODs_AConc", new Component(true, Component.Mode.C), new ChartFactory()),

            new ChartType("AConc", new Component(true, Component.Mode.C), new ChartFactory()),

            new ChartType("RSD_Conc", new Component(true, Component.Mode.RP), new ChartFactory()),
            new ChartType("RSD_CPS", new Component(true, Component.Mode.RP), new ChartFactory()),

            new ChartType("DEV_CPS", new Component(true, Component.Mode.RP), new ChartFactory()),
            new ChartType("DEV_Conc", new Component(true, Component.Mode.RP), new ChartFactory()),

            new ChartType("Overlap_Conc", new Component(false, Component.Mode.RP), new ChartFactory()),
            new ChartType("Overlap_CPS", new Component(false, Component.Mode.RP), new ChartFactory()),

            new ChartType("Trace_Conc", new Component(false, Component.Mode.RP), new ChartFactory()),
            new ChartType("Trace_CPS", new Component(false, Component.Mode.RP), new ChartFactory()),

            new ChartType("Current_UPS", new Component(false, Component.Mode.R), new ChartFactory()),
            new ChartType("Current_DWS", new Component(false, Component.Mode.R), new ChartFactory()),

            new ChartType("Mass", new Component(false, Component.Mode.RP), new ChartFactory()),

            new ChartType("ICF", new Component(false, Component.Mode.Null), new ChartFactory()),
            new ChartType("Injection", new Component(false, Component.Mode.Null), new ChartFactory()),
            new ChartType("Reaction Time", new Component(false, Component.Mode.Null), new ChartFactory()),
        };
        public ChartType(string name, Component component, IChartFactory chartFactory)
        {
            Name = name;
            Component = component;
            ChartFactory = chartFactory;
        }
        public string Name { get; }
        public Component Component { get; }
        public IChartFactory ChartFactory { get; }
        public string FullName => Component.LimitEnable == true ? "Bar#" + Name : "Line#" + Name;
    }
}
