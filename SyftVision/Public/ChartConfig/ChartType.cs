﻿using Public.ChartBuilder;
using Public.ChartBuilder.XY;
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
            new ChartType("Sensitivity", new Component(true, Component.Mode.RP), new BarChartFactory(new SensitivityXYFactory())),
            new ChartType("Impurity", new Component(true, Component.Mode.RP), new BarChartFactory(new ImpurityXYFactory())),

            new ChartType("LODs_Conc", new Component(true, Component.Mode.RP), new BarChartFactory(new LODs_ConcXYFactory())),
            new ChartType("LODs_AConc", new Component(true, Component.Mode.C), new BarChartFactory(new LODs_AConcXYFactory())),

            new ChartType("Mean_Conc", new Component(true, Component.Mode.RP), new BarChartFactory(new Mean_ConcXYFactory())),
            new ChartType("Mean_AConc", new Component(true, Component.Mode.C), new BarChartFactory(new Mean_AConcXYFactory())),

            new ChartType("RSD_Conc", new Component(true, Component.Mode.RP), new BarChartFactory(new RSD_ConcXYFactory())),
            new ChartType("RSD_CPS", new Component(true, Component.Mode.RP), new BarChartFactory(new RSD_CPSXYFactory())),

            new ChartType("DEV_Conc", new Component(true, Component.Mode.RP), new BarChartFactory(new DEV_ConcXYFactory())),
            new ChartType("DEV_CPS", new Component(true, Component.Mode.RP), new BarChartFactory(new DEV_CPSXYFactory())),
            new ChartType("DEV_Sensitivity", new Component(true, Component.Mode.RP), new BarChartFactory(new DEV_SensitivityXYFactory())),

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
