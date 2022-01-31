using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.Global
{
    public static class MyMath
    {
        public static double RSD(List<double> sample)
        {
            if (sample.Count == 0) return 0;
            return Statistics.StandardDeviation(sample) / Statistics.Mean(sample);
        }

        public static double Deviation(double baseValue, double compareValue)
        {
            if (baseValue == 0) return 0;
            return Math.Abs(compareValue / baseValue - 1);
        }
    }
}
