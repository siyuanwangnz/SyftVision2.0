﻿using Public.ChartConfig;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.BatchConfig
{
    public class BatchProp
    {
        public BatchProp(string tittle, string subTittle, List<Method> methodList, List<ChartProp> chartPropList)
        {
            Tittle = tittle;
            SubTittle = subTittle;
            MethodList = methodList.Where(a => !(string.IsNullOrEmpty(a.Name) || string.IsNullOrWhiteSpace(a.Name))).ToList();
            ChartPropList = chartPropList;
        }
        public BatchProp(XElement rootNode)
        {
            try
            {
                Tittle = rootNode.Attribute("Tittle").Value;
                SubTittle = rootNode.Attribute("SubTittle").Value;

                MethodList = new List<Method>();
                foreach (var methodNode in rootNode.Elements("Method"))
                {
                    Method method = new Method();
                    method.Name = methodNode.Attribute("Name").Value;

                    foreach (var chartCodeNode in methodNode.Elements("ChartCode"))
                        method.ChartCodeList.Add(chartCodeNode.Value);

                    MethodList.Add(method);
                }

                ChartPropList = new List<ChartProp>();
                foreach (var chartNode in rootNode.Elements("Chart"))
                    ChartPropList.Add(new ChartProp(chartNode));

            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Tittle { get; private set; }
        public string SubTittle { get; private set; }
        public string Name => $"{Tittle}-{SubTittle}";
        public string File => $"{SubTittle}.xml";
        public List<Method> MethodList { get; private set; }
        public List<ChartProp> ChartPropList { get; private set; }

        public XElement XMLGeneration()
        {
            XElement rootNode = new XElement("Batch",
                new XAttribute("Tittle", Tittle),
                new XAttribute("SubTittle", SubTittle));

            if (MethodList != null)
            {
                foreach (var method in MethodList)
                {
                    XElement methodElement = new XElement(new XElement("Method",
                        new XAttribute("Name", method.Name)));

                    foreach (var chartCode in method.ChartCodeList)
                        methodElement.Add(new XElement("ChartCode", chartCode));

                    rootNode.Add(methodElement);
                }
            }

            if (ChartPropList != null)
            {
                foreach (var chartProp in ChartPropList)
                    rootNode.Add(chartProp.XMLGeneration());
            }

            return rootNode;
        }
    }
}
