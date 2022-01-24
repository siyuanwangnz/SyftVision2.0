using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingCheck.Models
{
    public class SPISLens
    {
        //Source
        public const string SourcePressure = "source.pressure.controller.target";
        public const string MicrowavePower = "source.microwave.power.target";
        //UPS
        public const string UPSLens1 = "UPS_LENS1";
        public const string UPSLens2 = "UPS_LENS2";
        public const string UPSLens3 = "UPS_LENS3";
        public const string UPSLens4 = "UPS_LENS4";
        public const string UPSPrefilter = "UPS_LENS_QUAD_PREFILTER";
        public const string UPSAxialBias = "UPS_LENS_QUAD_AXIAL_BIAS";
        public const string UPSLens5 = "UPS_LENS5";
        public const string UPSLens6 = "UPS_LENS6";
        public const string UPSLensE = "UPS_LENSE";
        public const string UPSFlowTube = "UPS_LENS_FT";
        //DWS
        public const string DWSLensE = "DWS_LENSE";
        public const string DWSLens1 = "DWS_LENS1";
        public const string DWSIGBias = "DWS_LENS_IG_BIAS";
        public const string DWSIGRF = "DWS_LENS_IG_RF";
        public const string DWSLens2 = "DWS_LENS2";
        public const string DWSLens3 = "DWS_LENS3";
        public const string DWSLens4 = "DWS_LENS4";
        //DWS Specific
        public const string DWSPrefilter = "DWS_LENS_QUAD_PREFILTER";
        public const string DWSAxialBias = "DWS_LENS_QUAD_AXIAL_BIAS";
        public const string DWSLens5 = "DWS_LENS5";
        //Detection
        public const string Detector = "dws.detector.voltage.target";
        public const string Discriminator = "dws.detector.discrimator.target";
        public const string SettleTime = "scanner.dws.quad.settle_time";

        //Fix Volt Setting
        public const string FixVolt = ".voltage.target";
        //Mass Table
        public const string MassTable = ".converter";
        //Mass Driven
        public const string MassDriven = ".mass_dependent";
    }
}
