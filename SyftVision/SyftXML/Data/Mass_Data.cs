using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyftXML
{
    public class Mass_Data
    {
        public Mass_Data(string reagent, string product, List<M_Datum> m_DataList)
        {
            Reagent = reagent;
            Product = product;
            M_DatumList = m_DataList;
        }
        /// <summary>
        /// reagent name: "H3O+"
        /// </summary>
        public string Reagent { get; private set; }
        /// <summary>
        /// prodcut name: "93"
        /// </summary>
        public string Product { get; private set; }
        /// <summary>
        /// string code of reagent name add product name that remove space and convert to lower character: h3o+29
        /// </summary>
        public string RPCode { get => $"{Reagent}{Product}".ToLower().Replace(" ", ""); }
        /// <summary>
        /// list of datum node data under measurement node
        /// </summary>
        public List<M_Datum> M_DatumList { get; private set; }

        /// <summary>
        /// get dictionary<mass, average of cps> of mass peak
        /// </summary>
        /// <returns>dictionary<mass, average of cps></returns>
        public Dictionary<string, double> GetMassCPSDic()
        {
            List<string> rpList = new List<string>();
            double dproduct = double.Parse(Product) - 0.5;
            while (dproduct <= double.Parse(Product) + 0.5)
            {
                rpList.Add($"{Reagent}{dproduct:N}");
                dproduct = dproduct + 0.05;
            }

            Dictionary<string, double> massCPSDic = new Dictionary<string, double>();
            foreach (var rp in rpList)
            {
                string rpCode = rp.ToLower().Replace(" ", "");
                double cps = Statistics.Mean(M_DatumList.FindAll(a => a.RPRawCode == rpCode).Select(a => a.CPS).ToList());
                if (!double.Equals(cps, double.NaN))
                    massCPSDic.Add(rp, cps);
            }
            return massCPSDic;
        }
    }
}
