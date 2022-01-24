using ChartDirector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingCheck.Models
{
    static class PNGGeneratorExtension
    {
        public static string SavePNGChart(this BaseChart chart)
        {
            if (!Directory.Exists("./temp/Setting_Check/chart_temp"))
            {
                Directory.CreateDirectory("./temp/Setting_Check/chart_temp");
            }
            string path = $"./temp/Setting_Check/chart_temp/{chart.GetHashCode()}.png";
            if (chart.makeChart(path))
                return path;
            return "";
        }
    }
}
