using System;
using System.Collections.Generic;

namespace SettingCheck.Models
{
    class UPSPhaseSettings
    {
        public string Unit { get; set; } = "";

        public Setting Lens_14 { get; set; }
        public Setting Lens_1 { get; set; }
        public Setting Lens_2 { get; set; }
        public Setting Lens_3 { get; set; }
        public Setting Lens_4 { get; set; }
        public Setting Prefilter { get; set; }
        public Setting Axial_Bias { get; set; }
        public Setting Lens_5 { get; set; }
        public Setting Lens_6 { get; set; }
        public Setting Lens_E_L { get; set; }
        public Setting Lens_E_H { get; set; }
        public Setting Flowtube { get; set; }

        public double UPS_Extraction_Lens_Gradient_Setting { get; set; } = 0;

        public List<Setting> UPSPhaseList
        {
            get
            {
                List<Setting> temp = new List<Setting>();
                if (Lens_14 != null) temp.Add(Lens_14);
                if (Lens_1 != null) temp.Add(Lens_1);
                if (Lens_2 != null) temp.Add(Lens_2);
                if (Lens_3 != null) temp.Add(Lens_3);
                if (Lens_4 != null) temp.Add(Lens_4);
                if (Prefilter != null) temp.Add(Prefilter);
                if (Axial_Bias != null) temp.Add(Axial_Bias);
                if (Lens_5 != null) temp.Add(Lens_5);
                if (Lens_6 != null) temp.Add(Lens_6);
                if (Lens_E_L != null) temp.Add(Lens_E_L);
                if (Lens_E_H != null) temp.Add(Lens_E_H);
                if (Flowtube != null) temp.Add(Flowtube);

                return temp;
            }
        }

        public bool UPS_Extraction_Lens_Gradient_Check
        {
            get
            {
                if (Lens_14 == null || Lens_1 == null) return false;
                if (Math.Abs(Lens_14.SettingValue - Lens_1.SettingValue) <= UPS_Extraction_Lens_Gradient_Setting)
                    return true;
                else
                    return false;
            }
        }

        public bool UPS_Einzel_Stack_Check
        {
            get
            {
                if (Lens_2 == null || Lens_4 == null || Lens_3 == null) return false;
                if (Lens_2.SettingValue == Lens_4.SettingValue && Math.Abs(Lens_3.SettingValue) >= Math.Abs(Lens_2.SettingValue))
                    return true;
                else
                    return false;
            }
        }
    }
}
