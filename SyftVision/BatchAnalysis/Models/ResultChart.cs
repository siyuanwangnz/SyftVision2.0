﻿using ChartDirector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.Models
{
    public class ResultChart
    {
        public ResultChart(BaseChart chart)
        {
            Chart = chart;
        }
        public BaseChart Chart { get; }
    }
}
