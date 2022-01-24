using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyftXML
{
    /// <summary>
    /// struture for storage reagent to product
    /// </summary>
    public struct R2P
    {
        public R2P(string reagent, string product)
        {
            Reagent = reagent;
            Product = product;
        }
        public string Reagent;
        public string Product;
        /// <summary>
        /// string code of reagent name add product name that remove space and convert to lower character: h3o+29
        /// </summary>
        public string RPCode { get => $"{Reagent}{Product}".ToLower().Replace(" ", ""); }
        /// <summary>
        /// string code of reagent name that remove space and convert to lower character: o-
        /// </summary>
        public string RCode { get => $"{Reagent}".ToLower().Replace(" ", ""); }
    }
    /// <summary>
    /// reagent to product table: find reagent's product by reagent
    /// </summary>
    public static class R2P_Table
    {
        public static R2P[] Content = {
            new R2P("O-","16"),
            new R2P("OH-","17"),
            new R2P("O2-","32"),
            new R2P("NO2-","46"),
            new R2P("NO3-","62"),
            new R2P("H3O+","19"),
            new R2P("NO+","30"),
            new R2P("O2+","32")
        };
    }
}
