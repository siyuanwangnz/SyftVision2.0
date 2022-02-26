using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChartConfig.Models
{
    public class ChartProp
    {
        public ChartProp(ChartType chartType, string tittle, string subTittle, string expectedRange, string phase, ObservableCollection<Component> componentsList)
        {
            ChartType = chartType;
            Tittle = tittle;
            SubTittle = subTittle;
            ExpectedRange = expectedRange;
            Phase = phase;
            ComponentsList = componentsList;
        }
        public ChartProp(XElement rootNode)
        {

        }
        public ChartType ChartType { get; private set; }
        public string Tittle { get; private set; }
        public string SubTittle { get; private set; }
        public string ExpectedRange { get; private set; }
        public string Phase { get; private set; }
        public ObservableCollection<Component> ComponentsList { get; private set; }

        public XDocument XMLGeneration()
        {
            XElement rootNode = new XElement("Chart",
                new XAttribute("ChartType", ChartType?.Name),
                new XAttribute("Tittle", Tittle),
                new XAttribute("SubTittle", SubTittle),
                new XAttribute("ExpectedRange", ExpectedRange),
                new XAttribute("Phase", Phase));

            if (ComponentsList != null)
            {
                foreach (var component in ComponentsList)
                {
                    rootNode.Add(new XElement("Component",
                        new XAttribute("Compound", component.Compound),
                        new XAttribute("Reagent", component.Reagent),
                        new XAttribute("Production", component.Production),
                        new XAttribute("Limit", component.Limit)));
                }
            }

            return new XDocument(rootNode);
        }
    }
}
