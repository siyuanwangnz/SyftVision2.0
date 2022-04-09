using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartConfig
{
    public class Component
    {
        public enum Mode { RP, C, R, Null }
        public Component(bool limitEnable, Mode mode)
        {
            LimitEnable = limitEnable;
            ModeSet = mode;
        }
        public string Compound { get; set; } = "";
        public bool CompoundEnable => ModeSet == Mode.C;
        public string Reagent { get; set; } = "";
        public bool ReagentEnable => ModeSet == Mode.RP || ModeSet == Mode.R;
        public string Production { get; set; } = "";
        public bool ProductionEnable => ModeSet == Mode.RP;
        public double Limit { get; set; } = 0;
        public bool LimitEnable { get; }
        public Mode ModeSet { get; }
        public Component Copy()
        {
            Component tarComponent = new Component(LimitEnable, ModeSet);
            tarComponent.Compound = Compound;
            tarComponent.Reagent = Reagent;
            tarComponent.Production = Production;
            tarComponent.Limit = Limit;
            return tarComponent;
        }
    }
}
