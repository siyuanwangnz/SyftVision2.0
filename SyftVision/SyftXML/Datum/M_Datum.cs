using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyftXML
{
    /// <summary>
    /// datum node data under measurement node
    /// </summary>
    public class M_Datum
    {
        public M_Datum(XElement node)
        {
            Time = double.Parse(node?.Attribute("time")?.Value ?? "0");
            Reagent = node?.Attribute("reagent")?.Value ?? "";
            Product = node?.Attribute("product")?.Value ?? "";
            Period = double.Parse(node?.Attribute("period")?.Value ?? "0");
            UPSCurrent = double.Parse(node?.Attribute("ups")?.Value ?? "0");
            DWSCurrent = double.Parse(node?.Attribute("dws")?.Value ?? "0");
            FlowTubeTemperature = double.Parse(node?.Attribute("flowtubeTemperature")?.Value ?? "0");
            FlowTubePressure = double.Parse(node?.Attribute("flowtubePressure")?.Value ?? "0");
            CarrierFlow = double.Parse(node?.Attribute("carrierFlow")?.Value ?? "0");
            SampleFlow = double.Parse(node?.Attribute("sampleFlow")?.Value ?? "0");
            ReactionTime = double.Parse(node?.Attribute("reactionTime")?.Value ?? "0");
            ICF = double.Parse(node?.Attribute("icf")?.Value ?? "0");
            AF = double.Parse(node?.Attribute("af")?.Value ?? "0");
            DF = double.Parse(node?.Attribute("df")?.Value ?? "0");
            Value = double.Parse(node?.Value);
        }
        /// <summary>
        /// time, unit: ms
        /// </summary>
        public double Time { get; private set; }
        /// <summary>
        /// reagent name: "H3O+"
        /// </summary>
        public string Reagent { get; private set; }
        /// <summary>
        /// product name: "93.00"
        /// </summary>
        public string Product { get; private set; }
        public double Period { get; private set; }
        public double UPSCurrent { get; private set; }
        public double DWSCurrent { get; private set; }
        public double FlowTubeTemperature { get; private set; }
        public double FlowTubePressure { get; private set; }
        public double CarrierFlow { get; private set; }
        public double SampleFlow { get; private set; }
        public double ReactionTime { get; private set; }
        public double ICF { get; private set; }
        public double AF { get; private set; }
        public double DF { get; private set; }
        public double Value { get; private set; }

        /// <summary>
        /// calculated cps
        /// </summary>
        public double CPS { get => Math.Round(Value * (1000000 / Period)); }
        /// <summary>
        /// ups current, unit: nA
        /// </summary>
        public double UPSCurrentnA { get => UPSCurrent * 1000000000; }
        /// <summary>
        /// dws current, unit:pA
        /// </summary>
        public double DWSCurrentpA { get => DWSCurrent * 1000000000000; }
        /// <summary>
        /// string code of reagent name add product name that remove space and convert to lower character: h3o+29
        /// </summary>
        public string RPCode { get => $"{Reagent}{Product.Substring(0, Product.IndexOf("."))}".ToLower().Replace(" ", ""); }
        /// <summary>
        /// raw string code of reagent name add product name that remove space and convert to lower character: h3o+29.00
        /// </summary>
        public string RPRawCode { get => $"{Reagent}{Product}".ToLower().Replace(" ", ""); }
    }
}
