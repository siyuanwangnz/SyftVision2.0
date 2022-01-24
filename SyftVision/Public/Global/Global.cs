using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.Global
{
    public class Global
    {
        public enum InstrumentType
        {
            SPIS,
            DPIS,
            Infinity
        }

        public enum BatchType
        {
            //SPIS
            SPIS_Overnight,
            SPIS_OnOff,
            SPIS_EOV,
            SPIS_Weekend,
            //DPIS
            DPIS_Coarse,
            DPIS_Fine,
            DPIS_EOV,
            DPIS_ColdStart,
            DPIS_SensAndImpu,
            //Infinity
            Infinity,
            Infinity_ColdStart,
            Infinity_EOB,
            Infinity_SensAndImpu,
        }

        public static List<int> ColorPool = new List<int>() { 0x5588bb, 0xee9944, 0x66bbbb, 0xaa6644, 0x99bb55, 0x444466, 0xbb5555, 0xA569BD, 0xF7DC6F, 0x808B96, 0x85C1E9, 0xFAD7A0, 0xA3E4D7, 0x59866, 0x99A3A4, 0xA9DFBF, 0xBA4A00, 0xD2B4DE, 0xF9E79F, 0xE5E7E9 };

        public static string UserName { get; set; }

        public const string PASSWORD = "Syft2002";
        public static string Password { get; set; } = PASSWORD;
    }
}
