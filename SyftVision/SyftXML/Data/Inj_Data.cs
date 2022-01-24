using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyftXML
{
    /// <summary>
    /// injection scan data
    /// </summary>
    public class Inj_Data
    {
        public Inj_Data(List<M_Datum> m_DatumList)
        {
            M_DatumList = m_DatumList;
        }
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
        /// get list of mass
        /// </summary>
        /// <returns>list of mass</returns>
        public List<double> MassList() => M_DatumList.Select(a => double.Parse(a.Product)).ToList();
    }
}
