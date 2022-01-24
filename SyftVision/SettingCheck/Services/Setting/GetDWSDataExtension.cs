using System;
using System.Windows;
using System.Xml.Linq;
using Public.Global;
using SettingCheck.Models;

namespace SettingCheck.Services
{
    static class GetDWSDataExtension
    {
        #region Get data from scan file 
        //Setting check with range check
        public static DWSSettings GetDWSData(this DWSSettings dwsSettings, XElement ScanRootNode, XElement ConfigRootNode, Global.InstrumentType type)
        {
            try
            {
                GetDWSSettings(ref dwsSettings, ScanRootNode, type);
                GetDWSRanges(ref dwsSettings, ConfigRootNode, type);
                return dwsSettings;
            }
            catch (Exception ex)
            {
                throw new Exception($"DWS Data: {ex.Message}");
            }
        }

        private static void GetDWSSettings(ref DWSSettings dwsSettings, XElement ScanRootNode, Global.InstrumentType type)
        {
            try
            {
                bool isMassDrivenActive;
                switch (type)
                {
                    case Global.InstrumentType.SPIS:
                        dwsSettings.Lens_E = new Setting("Lens E", GetSetting.SPIS(ScanRootNode, SPISLens.DWSLensE,out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_1 = new Setting("Lens 1", GetSetting.SPIS(ScanRootNode, SPISLens.DWSLens1, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_IG_Bias = new Setting("Lens IGB", GetSetting.SPIS(ScanRootNode, SPISLens.DWSIGBias, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_IG_RF = new Setting("Lens RF", GetSetting.SPIS(ScanRootNode, SPISLens.DWSIGRF, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_2 = new Setting("Lens 2", GetSetting.SPIS(ScanRootNode, SPISLens.DWSLens2, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_3 = new Setting("Lens 3", GetSetting.SPIS(ScanRootNode, SPISLens.DWSLens3, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_4 = new Setting("Lens 4", GetSetting.SPIS(ScanRootNode, SPISLens.DWSLens4, out isMassDrivenActive), isMassDrivenActive);
                        break;
                    case Global.InstrumentType.DPIS:
                    case Global.InstrumentType.Infinity:
                        dwsSettings.Lens_E = new Setting("Lens E", GetSetting.DPIS(ScanRootNode, DPISLens.DWSLensE, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_1 = new Setting("Lens 1", GetSetting.DPIS(ScanRootNode, DPISLens.DWSLens1, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_IG_Bias = new Setting("Lens IGB", GetSetting.DPIS(ScanRootNode, DPISLens.DWSIGBias, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_IG_RF = new Setting("Lens RF", GetSetting.DPIS(ScanRootNode, DPISLens.DWSIGRF, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_2 = new Setting("Lens 2", GetSetting.DPIS(ScanRootNode, DPISLens.DWSLens2, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_3 = new Setting("Lens 3", GetSetting.DPIS(ScanRootNode, DPISLens.DWSLens3, out isMassDrivenActive), isMassDrivenActive);
                        dwsSettings.Lens_4 = new Setting("Lens 4", GetSetting.DPIS(ScanRootNode, DPISLens.DWSLens4, out isMassDrivenActive), isMassDrivenActive);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Scan Data Collection failed，Reason：{ex.Message}");
            }
        }
        #endregion

        private static void GetDWSRanges(ref DWSSettings dwsSettings, XElement ConfigRootNode, Global.InstrumentType type)
        {
            try
            {
                switch (type)
                {
                    case Global.InstrumentType.SPIS:
                    case Global.InstrumentType.DPIS:
                        dwsSettings.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS");

                        dwsSettings.Lens_E.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_E", "Minimum"));
                        dwsSettings.Lens_E.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_E", "Maximum"));

                        dwsSettings.Lens_1.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Len_1", "Minimum"));
                        dwsSettings.Lens_1.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Len_1", "Maximum"));


                        dwsSettings.Lens_IG_Bias.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_IG_Bias", "Minimum"));
                        dwsSettings.Lens_IG_Bias.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_IG_Bias", "Maximum"));

                        dwsSettings.Lens_IG_RF.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_IG_RF", "Minimum"));
                        dwsSettings.Lens_IG_RF.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_IG_RF", "Maximum"));

                        dwsSettings.Lens_2.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_2", "Minimum"));
                        dwsSettings.Lens_2.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_2", "Maximum"));

                        dwsSettings.Lens_3.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_3", "Minimum"));
                        dwsSettings.Lens_3.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "Lens_3", "Maximum"));

                        dwsSettings.Lens_4.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "lens_4", "Minimum"));
                        dwsSettings.Lens_4.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_DPIS_DWS", "lens_4", "Maximum"));
                        break;
                    case Global.InstrumentType.Infinity:
                        dwsSettings.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Infinity_DWS");

                        dwsSettings.Lens_E.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_E", "Minimum"));
                        dwsSettings.Lens_E.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_E", "Maximum"));

                        dwsSettings.Lens_1.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Len_1", "Minimum"));
                        dwsSettings.Lens_1.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Len_1", "Maximum"));


                        dwsSettings.Lens_IG_Bias.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_IG_Bias", "Minimum"));
                        dwsSettings.Lens_IG_Bias.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_IG_Bias", "Maximum"));

                        dwsSettings.Lens_IG_RF.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_IG_RF", "Minimum"));
                        dwsSettings.Lens_IG_RF.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_IG_RF", "Maximum"));

                        dwsSettings.Lens_2.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_2", "Minimum"));
                        dwsSettings.Lens_2.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_2", "Maximum"));

                        dwsSettings.Lens_3.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_3", "Minimum"));
                        dwsSettings.Lens_3.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "Lens_3", "Maximum"));

                        dwsSettings.Lens_4.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "lens_4", "Minimum"));
                        dwsSettings.Lens_4.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Infinity_DWS", "lens_4", "Maximum"));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Config Data Collection failed，Reason：{ex.Message}");
            }
        }
    }
}
