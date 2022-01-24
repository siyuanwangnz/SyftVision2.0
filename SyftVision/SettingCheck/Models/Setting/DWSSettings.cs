using System.Collections.Generic;

namespace SettingCheck.Models
{
    class DWSSettings
    {
        public string Unit { get; set; } = "";

        public Setting Lens_E { get; set; }
        public Setting Lens_1 { get; set; }
        public Setting Lens_IG_Bias { get; set; }
        public Setting Lens_IG_RF { get; set; }
        public Setting Lens_2 { get; set; }
        public Setting Lens_3 { get; set; }
        public Setting Lens_4 { get; set; }
        public List<Setting> DWSList
        {
            get
            {
                List<Setting> temp = new List<Setting>();

                if (Lens_E != null) temp.Add(Lens_E);
                if (Lens_1 != null) temp.Add(Lens_1);
                if (Lens_IG_Bias != null) temp.Add(Lens_IG_Bias);
                if (Lens_IG_RF != null) temp.Add(Lens_IG_RF);
                if (Lens_2 != null) temp.Add(Lens_2);
                if (Lens_3 != null) temp.Add(Lens_3);
                if (Lens_4 != null) temp.Add(Lens_4);

                return temp;
            }
        }
    }
}
