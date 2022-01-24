using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyftXML
{
    /// <summary>
    /// datum node data under concentration node
    /// </summary>
    public class C_Datum
    {
        public C_Datum(XElement node)
        {
            Time = double.Parse(node?.Attribute("time")?.Value??"0");
            Reagent = node?.Attribute("reagent")?.Value??"";
            Formula = node?.Attribute("product")?.Value??"";
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
        /// formula name: "(C-12)5(C-13)F5O-"
        /// </summary>
        public string Formula { get; private set; }
        public double Value { get; private set; }

        /// <summary>
        /// concentration, unit: ppb
        /// </summary>
        public double ConcPPB { get => Value * 1000000000; }
        /// <summary>
        /// string code of reagent name add formula name that remove space and convert to lower character: o-f-
        /// </summary>
        public string RFCode { get => $"{Reagent}{Formula}".ToLower().Replace(" ", ""); }
    }
}
