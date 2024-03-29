﻿using SyftXML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.ChartConfig
{
    public class ChartProp
    {
        public ChartProp(ChartType chartType, string tittle, string subTittle, string expectedRange, string phase, List<Component> componentList)
        {
            ChartType = chartType;
            Tittle = tittle;
            SubTittle = subTittle;
            ExpectedRange = expectedRange;
            Phase = phase;

            ComponentList = componentList.Where(a =>
            {
                if (a.CompoundEnable == true && (string.IsNullOrEmpty(a.Compound) || string.IsNullOrWhiteSpace(a.Compound))) return false;
                if (a.ReagentEnable == true && (string.IsNullOrEmpty(a.Reagent) || string.IsNullOrWhiteSpace(a.Reagent))) return false;
                if (a.ProductionEnable == true && (string.IsNullOrEmpty(a.Production) || string.IsNullOrWhiteSpace(a.Production))) return false;
                return true;
            }).ToList();
        }
        public ChartProp(XElement rootNode)
        {
            try
            {
                ChartType = ChartType.ReferList.Single(a => a.Name == rootNode.Attribute("ChartType").Value);
                Tittle = rootNode.Attribute("Tittle").Value;
                SubTittle = rootNode.Attribute("SubTittle").Value;
                ExpectedRange = rootNode.Attribute("ExpectedRange").Value;
                Phase = rootNode.Attribute("Phase").Value;

                ComponentList = new List<Component>();
                foreach (var componentNode in rootNode.Elements("Component"))
                {
                    Component component = ChartType.Component.Copy();
                    component.Compound = componentNode.Attribute("Compound").Value;
                    component.Reagent = componentNode.Attribute("Reagent").Value;
                    component.Production = componentNode.Attribute("Production").Value;
                    component.Limit = double.Parse(componentNode.Attribute("Limit").Value);
                    ComponentList.Add(component);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
        public ChartType ChartType { get; private set; }
        public string Tittle { get; private set; }
        public string SubTittle { get; private set; }
        public string Name => $"{Tittle}-{SubTittle}";
        public string File => $"{SubTittle}({ChartType.FullName}).xml";
        public string Code => $"{Name}({ChartType.FullName})";
        public int HashCode => Code.GetHashCode();
        public string ExpectedRange { get; private set; }
        public int ExpectedRangeColor { get => ExpectedRange == "Upper Limit" ? unchecked((int)0x80ff8080) : unchecked((int)0x8080ff80); }
        public string Phase { get; private set; }
        public List<Component> ComponentList { get; private set; }
        public Scan.Phase ScanPhase
        {
            get
            {
                switch (Phase)
                {
                    case "Sample":
                        return Scan.Phase.Sample;
                    case "Background":
                        return Scan.Phase.Background;
                    case "Preparation":
                        return Scan.Phase.Preparation;
                    default:
                        return Scan.Phase.All;
                }
            }
        }

        public XElement XMLGeneration()
        {
            XElement rootNode = new XElement("Chart",
                new XAttribute("ChartType", ChartType.Name),
                new XAttribute("Tittle", Tittle),
                new XAttribute("SubTittle", SubTittle),
                new XAttribute("ExpectedRange", ExpectedRange),
                new XAttribute("Phase", Phase));

            if (ComponentList != null)
            {
                foreach (var component in ComponentList)
                {
                    rootNode.Add(new XElement("Component",
                        new XAttribute("Compound", component.Compound),
                        new XAttribute("Reagent", component.Reagent),
                        new XAttribute("Production", component.Production),
                        new XAttribute("Limit", component.Limit)));
                }
            }

            return rootNode;
        }
    }
}
