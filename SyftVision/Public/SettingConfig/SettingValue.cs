﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SettingConfig
{
    public class SettingValue
    {
        public double Value { get; set; }
        public double UnderLimit { get; set; }
        public double UpperLimit { get; set; }
        public static double GetValue(string content)
        {
            try
            {
                return double.Parse(content);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
