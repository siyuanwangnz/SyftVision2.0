using Public.ChartConfig;
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
        public BatchProp(string tittle, string subTittle, ObservableCollection<Method> methodsList, ObservableCollection<ChartProp> chartPropList)
        {
            Tittle = tittle;
            SubTittle = subTittle;
            MethodsList = methodsList;
            ChartPropList = chartPropList;
        }
        public BatchProp(XElement rootNode)
        {
            try
            {
                Tittle = rootNode.Attribute("Tittle")?.Value;
                SubTittle = rootNode.Attribute("SubTittle")?.Value;

                MethodsList = new ObservableCollection<Method>();
                foreach (var methodNode in rootNode.Elements("Method"))
                {
                    Method method = new Method();
                    method.MethodName = methodNode.Attribute("MethodName")?.Value;

                    foreach (var chartCodeNode in methodNode.Elements("ChartCode"))
                        method.ChartCodeList.Add(chartCodeNode?.Value);

                    MethodsList.Add(method);
                }

                ChartPropList = new ObservableCollection<ChartProp>();
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
        public string FileName => $"{SubTittle}.xml";
        public ObservableCollection<Method> MethodsList { get; private set; }
        public ObservableCollection<ChartProp> ChartPropList { get; private set; }

        public XElement XMLGeneration()
        {
            XElement rootNode = new XElement("Batch",
                new XAttribute("Tittle", Tittle),
                new XAttribute("SubTittle", SubTittle));

            if (MethodsList != null)
            {
                foreach (var method in MethodsList)
                {
                    XElement methodElement = new XElement(new XElement("Method",
                        new XAttribute("MethodName", method.MethodName)));

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
