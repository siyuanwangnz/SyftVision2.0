using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyftXML
{
    /// <summary>
    /// analyte concentration, concentration data, analyte lod data of single compound (sort by compound name) 
    /// </summary>
    public class AC_Data
    {
        public AC_Data(string compound, Dictionary<string, List<C_Datum>> c_DatumListDic)
        {
            Compound = compound;
            C_DatumListDic = c_DatumListDic;
        }
        /// <summary>
        /// compound name: "perfluorobenzene"
        /// </summary>
        public string Compound { get; private set; }
        /// <summary>
        /// Dictionary &lt;"reagent/formula", list of datum node data under concentration node&gt;
        /// </summary>
        public Dictionary<string, List<C_Datum>> C_DatumListDic { get; private set; }

        /// <summary>
        /// get all components' list of concentration (sort by reagent and formula) under compound
        /// </summary>
        /// <returns>Dictionary &lt;"reagent/formula", list of concentration&gt;</returns>
        public Dictionary<string, List<double>> ConcListDic()
        {
            Dictionary<string, List<double>> ConcListDic = new Dictionary<string, List<double>>();
            foreach (var C_DatumList in C_DatumListDic)
            {
                ConcListDic.Add(C_DatumList.Key, C_DatumList.Value.Select(a => a.ConcPPB).ToList());
            }
            return ConcListDic;
        }
        /// <summary>
        /// get all components' list of concentration time (sort by reagent and formula) under compound
        /// </summary>
        /// <returns>Dictionary &lt;"reagent/formula", list of concentration time&gt;</returns>
        public Dictionary<string, List<double>> ConcTimeListDic()
        {
            Dictionary<string, List<double>> ConcTimeListDic = new Dictionary<string, List<double>>();
            foreach (var C_DatumList in C_DatumListDic)
            {
                ConcTimeListDic.Add(C_DatumList.Key, C_DatumList.Value.Select(a => a.Time).ToList());
            }
            return ConcTimeListDic;

        }
        /// <summary>
        /// get all mean of each components' concentration (sort by reagent and formula) under compound
        /// </summary>
        /// <returns>Dictionary &lt;"reagent/formula", mean of concentration&gt;</returns>
        public Dictionary<string, double> ConcMeanDic()
        {
            Dictionary<string, double> ConcMeanDic = new Dictionary<string, double>();
            foreach (var ConcList in ConcListDic())
            {
                ConcMeanDic.Add(ConcList.Key, Statistics.Mean(ConcList.Value));
            }
            return ConcMeanDic;
        }
        /// <summary>
        /// get all median of each components' concentration (sort by reagent and formula) under compound
        /// </summary>
        /// <returns>Dictionary &lt;"reagent/formula", median of concentration&gt;</returns>
        public Dictionary<string, double> ConcMedianDic()
        {
            Dictionary<string, double> ConcMedianDic = new Dictionary<string, double>();
            foreach (var ConcList in ConcListDic())
            {
                ConcMedianDic.Add(ConcList.Key, Statistics.Median(ConcList.Value));
            }
            return ConcMedianDic;
        }

        /// <summary>
        /// get filtered components' list of concentration (sort by reagent and formula) under compound
        /// </summary>
        /// <returns>Dictionary &lt;"reagent/formula", list of concentration&gt;</returns>
        public Dictionary<string, List<double>> AConcListDic()
        {
            double min = ConcMeanDic().Values.ToList().Min();
            double upperlimit = min * 1.2;
            double lowerlimit = min * 0.8;

            Dictionary<string, List<double>> AConcListDic = new Dictionary<string, List<double>>(0);
            foreach (var ConcList in ConcListDic())
            {
                if (Statistics.Mean(ConcList.Value) >= lowerlimit && Statistics.Mean(ConcList.Value) <= upperlimit)
                    AConcListDic.Add(ConcList.Key, ConcList.Value);
            }
            return AConcListDic;
        }
        /// <summary>
        /// get filtered mean of each components' concentration (sort by reagent and formula) under compound
        /// </summary>
        /// <returns>Dictionary &lt;"reagent/formula", mean of concentration&gt;</returns>
        public Dictionary<string, double> AConcMeanDic()
        {
            Dictionary<string, double> AConcMeanDic = new Dictionary<string, double>();
            foreach (var ConcList in AConcListDic())
            {
                AConcMeanDic.Add(ConcList.Key, Statistics.Mean(ConcList.Value));
            }
            return AConcMeanDic;
        }
        /// <summary>
        /// get filtered median of each components' concentration (sort by reagent and formula) under compound
        /// </summary>
        /// <returns>Dictionary &lt;"reagent/formula", median of concentration&gt;</returns>
        public Dictionary<string, double> AConcMedianDic()
        {
            Dictionary<string, double> AConcMedianDic = new Dictionary<string, double>();
            foreach (var ConcList in AConcListDic())
            {
                AConcMedianDic.Add(ConcList.Key, Statistics.Median(ConcList.Value));
            }
            return AConcMedianDic;
        }


        /// <summary>
        /// get mean of analyte concentration
        /// </summary>
        /// <returns>mean of analyte concentration</returns>
        public double AConcMean() => Statistics.Mean(AConcMeanDic().Values.ToList());

        /// <summary>
        /// get list of analyte concentration of single phase
        /// </summary>
        /// <returns>list of analyte concentration</returns>
        public List<double> AConcList_SinglePahse()
        {
            List<double> AConcList = new List<double>();

            var count = AConcListDic()?.First().Value.Count;
            for (int i = 0; i < count; i++)
            {
                List<double> pointList = new List<double>();
                foreach (var item in AConcListDic())
                {
                    pointList.Add(item.Value[i]);
                }
                AConcList.Add(Statistics.Mean(pointList));
            }
            return AConcList;
        }
        /// <summary>
        /// get list of analyte concentration time of single phase
        /// </summary>
        /// <returns>list of analyte concentration time</returns>
        public List<double> AConcTimeList_SinglePahse() => ConcTimeListDic().First().Value;
        /// <summary>
        /// get lod of analyte concentration of single phase
        /// </summary>
        /// <returns></returns>
        public double LOD_SinglePahse() => 3 * Statistics.StandardDeviation(AConcList_SinglePahse()) / Math.Sqrt(AConcList_SinglePahse().Count);
        /// <summary>
        /// get median of analyte concentration
        /// </summary>
        /// <returns>median of analyte concentration</returns>
        public double AConcMedian_SinglePahse() => Statistics.Median(AConcList_SinglePahse());
    }
}
