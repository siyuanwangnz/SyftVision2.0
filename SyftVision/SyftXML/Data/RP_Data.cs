using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyftXML
{
    /// <summary>
    /// cps data, concentartion data, sensitivity data, ... of single component (sort by reagent and product)
    /// </summary>
    public class RP_Data
    {
        public RP_Data(string reagent, string product, string r_product, List<M_Datum> m_DatumList, List<M_Datum> r_m_DatumList, List<C_Datum> c_DatumList)
        {
            Reagent = reagent;
            Product = product;
            R_Product = r_product;
            M_DatumList = m_DatumList;
            R_M_DatumList = r_m_DatumList;
            C_DatumList = c_DatumList;
        }
        /// <summary>
        /// Check if object is available 
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                if (Reagent == "" || Product == "")
                    return false;
                else
                    return true;
            }
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
        /// corresponding reagent's product name: "19"
        /// </summary>
        public string R_Product { get; private set; }
        /// <summary>
        /// string code of reagent name add product name that remove space and convert to lower character: h3o+29
        /// </summary>
        public string RPCode { get => $"{Reagent}{Product}".ToLower().Replace(" ", ""); }

        /// <summary>
        /// list of datum node data under measurement node
        /// </summary>
        public List<M_Datum> M_DatumList { get; private set; }
        /// <summary>
        /// list of datum node data under measurement node of corresponding reagent 
        /// </summary>
        public List<M_Datum> R_M_DatumList { get; private set; }
        /// <summary>
        /// list of datum node data under concentration node
        /// </summary>
        public List<C_Datum> C_DatumList { get; private set; }
        /// <summary>
        /// get list of cps
        /// </summary>
        /// <returns>list of cps</returns>
        public List<double> CPSList() => M_DatumList.Select(a => a.CPS).ToList();
        /// <summary>
        /// get list of cps time
        /// </summary>
        /// <returns>list of cps time</returns>
        public List<double> CPSTimeList() => M_DatumList.Select(a => a.Time).ToList();
        /// <summary>
        /// get list of corresponding reagent cps 
        /// </summary>
        /// <returns>list of correspoding reagent cps</returns>
        public List<double> R_CPSList() => R_M_DatumList.Select(a => a.CPS).ToList();
        /// <summary>
        /// get list of corresponding reagent cps time
        /// </summary>
        /// <returns>list of corresponding reagent cps time</returns>
        public List<double> R_CPSTimeList() => R_M_DatumList.Select(a => a.Time).ToList();
        /// <summary>
        /// get list of concentration
        /// </summary>
        /// <returns>list of concentration</returns>
        public List<double> ConcList() => C_DatumList?.Select(a => a.ConcPPB).ToList();
        /// <summary>
        /// get list of concentration time
        /// </summary>
        /// <returns>list of concentration time</returns>
        public List<double> ConcTimeList() => C_DatumList?.Select(a => a.Time).ToList();

        /// <summary>
        /// get mean of cps
        /// </summary>
        /// <returns>mean of cps</returns>
        public double CPSMean() => Statistics.Mean(CPSList());
        /// <summary>
        /// get median of cps
        /// </summary>
        /// <returns>median of cps</returns>
        public double CPSMedian() => Statistics.Median(CPSList());
        /// <summary>
        /// get mean of corresponding reagent cps 
        /// </summary>
        /// <returns>mean of corresponding reagent cps</returns>
        public double R_CPSMean() => Statistics.Mean(R_CPSList());
        /// <summary>
        /// get median of corresponding reagent cps 
        /// </summary>
        /// <returns>median of corresponding reagent cps </returns>
        public double R_CPSMedian() => Statistics.Median(R_CPSList());
        /// <summary>
        /// get mean of concentration
        /// </summary>
        /// <returns>mean of concentration</returns>
        public double ConcMean() => Statistics.Mean(ConcList());
        /// <summary>
        /// get median of concentration
        /// </summary>
        /// <returns>median of concentration</returns>
        public double ConcMedian() => Statistics.Median(ConcList());

        /// <summary>
        /// get sensitivity of cps and concrentration
        /// </summary>
        /// <returns>sensitivity</returns>
        public double Sensitivity() => CPSMean() / ConcMean();
        /// <summary>
        /// get impurity ratio of cps
        /// </summary>
        /// <returns>impurity ratio: 0.05 = 5%</returns>
        public double Impurity() => CPSMean() / R_CPSMean();
        /// <summary>
        /// get lod of concentration
        /// </summary>
        /// <returns>lod</returns>
        public double LOD() => 3 * Statistics.StandardDeviation(ConcList()) / Math.Sqrt(ConcList().Count);
    }
}
