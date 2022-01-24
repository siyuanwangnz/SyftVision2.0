using ChartDirector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    static class PNGGeneratorExtension
    {
        public static List<string> SavePNGChart(this List<XYChart> chartList)
        {
            List<string> pathList = new List<string>();
            chartList.ForEach(a =>
            {
                if (!Directory.Exists("./temp/Batch_Analysis/chart_temp"))
                {
                    Directory.CreateDirectory("./temp/Batch_Analysis/chart_temp");
                }
                string path = $"./temp/Batch_Analysis/chart_temp/{a?.GetHashCode()}.png";
                if (a.makeChart(path))
                    pathList.Add(path);
            }
            );
            return pathList;
        }

        public static string SavePNGChart(this BaseChart chart)
        {
            if (!Directory.Exists("./temp/Batch_Analysis/chart_temp"))
            {
                Directory.CreateDirectory("./temp/Batch_Analysis/chart_temp");
            }
            string path = $"./temp/Batch_Analysis/chart_temp/{chart?.GetHashCode()}.png";
            if (chart.makeChart(path))
                return path;
            return "";
        }
    }
}
