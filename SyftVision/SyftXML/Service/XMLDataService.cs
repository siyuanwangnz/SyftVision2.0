using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyftXML
{
    class XMLDataService
    {
        /// <summary>
        /// get node value by attribute name
        /// </summary>
        /// <param name="rootNode">root node</param>
        /// <param name="attributeName">attribute name: "job.filename"</param>
        /// <returns>node value: "infinity +- performance check_V5", not found return null</returns>
        public static string GetSettingNodeValueByAttributeName(XElement rootNode, string attributeName)
        {
            if (rootNode == null) return "";

            IEnumerable<XElement> targetNodes =
                from target in rootNode.Descendants("setting")
                where target?.Attribute("name")?.Value == attributeName
                select target;

            foreach (var node in targetNodes)
            {
                return node.Value;
            }
            return null;
        }
        /// <summary>
        /// get formula by reagent and product
        /// </summary>
        /// <param name="rootNode">root node</param>
        /// <param name="reagent">reagent: "H3O+"</param>
        /// <param name="product">product: "93"</param>
        /// <returns>formula: "(C-12)5(C-13)F5O-", not found return null</returns>
        public static string GetFormulaByReagentandProduct(XElement rootNode, string reagent, string product)
        {
            if (rootNode == null) return "";


            IEnumerable<XElement> targetNodes =
                from target in rootNode.Descendants("compounds").Descendants("Precursor")
                where target?.Attribute("Ion")?.Value == reagent
                from subTarget in target.Descendants("Product")
                where Regex.Match(subTarget?.Element("Mass")?.Value, @"\-?(\d*)").Groups[1].Value == product
                select subTarget;

            foreach (var node in targetNodes)
            {
                return node?.Attribute("Formulae")?.Value;
            }
            return null;
        }


    }
}
