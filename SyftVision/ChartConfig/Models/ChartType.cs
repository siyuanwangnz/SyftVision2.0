﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.Models
{
    public class ChartType
    {
        public static ObservableCollection<ChartType> ReferList = new ObservableCollection<ChartType>() {
                new ChartType("Sensitivities",new Component(true)),
                new ChartType("Impurities",new Component(true)),

                new ChartType("LODs_Conc",new Component(true)),
                new ChartType("LODs_AConc",new Component(true,Component.Type.CompoundOnly)),

                new ChartType("AConc",new Component(true,Component.Type.CompoundOnly)),

                new ChartType("RSD_Conc",new Component(true)),
                new ChartType("RSD_CPS",new Component(true)),

                new ChartType("DEV_CPS",new Component(true)),
                new ChartType("DEV_Conc",new Component(true)),

                new ChartType("Overlap_Conc",new Component(false)),
                new ChartType("Overlap_CPS",new Component(false)),

                new ChartType("Trace_Conc",new Component(false)),
                new ChartType("Trace_CPS",new Component(false)),

                new ChartType("Current_UPS",new Component(false,Component.Type.ReagentOnly)),
                new ChartType("Current_DWS",new Component(false,Component.Type.ReagentOnly)),

                new ChartType("Mass",new Component(false))
        };
        public ChartType(string name, Component component)
        {
            Name = name;
            Component = component;
        }
        public string Name { get; }
        public Component Component { get; }
        public string FullName => Component.LimitEnable == true ? "Bar" + " - " + Name : "Line" + " - " + Name;
    }
}