using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.Models
{
    public class Component
    {
        public enum Type
        {
            Normal,
            CompoundOnly,
            ReagentOnly
        }
        public Component(bool limitEnable, Type type = Type.Normal)
        {
            LimitEnable = limitEnable;
            ComponentType = type;
        }
        public string Compound { get; set; } = "";
        public bool CompoundEnable => ComponentType == Type.CompoundOnly;
        public string Reagent { get; set; } = "";
        public bool ReagentEnable => ComponentType != Type.CompoundOnly;
        public string Production { get; set; } = "";
        public bool ProductionEnable => ComponentType == Type.Normal;
        public double Limit { get; set; } = 0;
        public bool LimitEnable { get; }
        public Type ComponentType { get; }
        public Component Copy()
        {
            Component tarComponent = new Component(LimitEnable, ComponentType);
            tarComponent.Compound = Compound;
            tarComponent.Reagent = Reagent;
            tarComponent.Production = Production;
            tarComponent.Limit = Limit;
            return tarComponent;
        }
    }
}
