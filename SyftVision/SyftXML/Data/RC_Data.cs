using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyftXML
{
    /// <summary>
    /// ups and dws current data of single reagent
    /// </summary>
    public class RC_Data
    {
        public RC_Data(string reagent, string product, List<M_Datum> m_DatumList)
        {
            Reagent = reagent;
            Product = product;
            M_DatumList = m_DatumList;
        }
        /// <summary>
        /// reagent name: "H3O+"
        /// </summary>
        public string Reagent { get; private set; }
        /// <summary>
        /// product name: "19"
        /// </summary>
        public string Product { get; private set; }
        /// <summary>
        /// list of datum node data under measurement node
        /// </summary>
        public List<M_Datum> M_DatumList { get; private set; }
        /// <summary>
        /// get list of ups current 
        /// </summary>
        /// <returns>list of ups current </returns>
        public List<double> UPSCurList() => M_DatumList.Select(a => a.UPSCurrentnA).ToList();
        /// <summary>
        /// get list of dws current 
        /// </summary>
        /// <returns>list of dws current </returns>
        public List<double> DWSCurList() => M_DatumList.Select(a => a.DWSCurrentpA).ToList();
        /// <summary>
        /// get list of current time
        /// </summary>
        /// <returns>list of current time</returns>
        public List<double> CurTimeList() => M_DatumList.Select(a => a.Time).ToList();
    }
}
