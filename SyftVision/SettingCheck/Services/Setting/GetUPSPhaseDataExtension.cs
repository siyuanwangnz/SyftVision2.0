using System;
using System.Windows;
using System.Xml.Linq;
using Public.Global;
using SettingCheck.Models;

namespace SettingCheck.Services
{
    static class GetUPSPhaseDataExtension
    {
        #region PosWet
        //Setting check with range check
        public static UPSPhaseSettings GetPosWetUPSPhaseData(this UPSPhaseSettings upsPhaseSettings, XElement ScanRootNode, XElement ConfigRootNode, Global.InstrumentType type)
        {
            try
            {
                GetPosWetUPSPhaseSettings(ref upsPhaseSettings, ScanRootNode, type);
                GetUPSPhaseRangesMethod(ref upsPhaseSettings, ConfigRootNode, "UPS_PosWet", type);
                return upsPhaseSettings;
            }
            catch (Exception ex)
            {
                throw new Exception($"PosWet UPS Phase Data: {ex.Message}");
            }
        }

        private static void GetPosWetUPSPhaseSettings(ref UPSPhaseSettings upsPhaseSettings, XElement ScanRootNode, Global.InstrumentType type)
        {
            try
            {
                bool isMassDrivenActive;
                switch (type)
                {
                    case Global.InstrumentType.SPIS:
                        upsPhaseSettings.Lens_1 = new Setting("Lens 1", GetSetting.SPIS(ScanRootNode, SPISLens.UPSLens1, out isMassDrivenActive), isMassDrivenActive);
                        upsPhaseSettings.Lens_2 = new Setting("Lens 2", GetSetting.SPIS(ScanRootNode, SPISLens.UPSLens2, out isMassDrivenActive), isMassDrivenActive);
                        upsPhaseSettings.Lens_3 = new Setting("Lens 3", GetSetting.SPIS(ScanRootNode, SPISLens.UPSLens3, out isMassDrivenActive), isMassDrivenActive);
                        upsPhaseSettings.Lens_4 = new Setting("Lens 4", GetSetting.SPIS(ScanRootNode, SPISLens.UPSLens4, out isMassDrivenActive), isMassDrivenActive);
                        upsPhaseSettings.Prefilter = new Setting("PF", GetSetting.SPIS(ScanRootNode, SPISLens.UPSPrefilter, out isMassDrivenActive), isMassDrivenActive);
                        upsPhaseSettings.Axial_Bias = new Setting("AB", GetSetting.SPIS(ScanRootNode, SPISLens.UPSAxialBias, out isMassDrivenActive), isMassDrivenActive);
                        upsPhaseSettings.Lens_5 = new Setting("Lens 5", GetSetting.SPIS(ScanRootNode, SPISLens.UPSLens5, out isMassDrivenActive), isMassDrivenActive);
                        upsPhaseSettings.Lens_6 = new Setting("Lens 6", GetSetting.SPIS(ScanRootNode, SPISLens.UPSLens6, out isMassDrivenActive), isMassDrivenActive);
                        upsPhaseSettings.Lens_E_L = new Setting("Lens E\nLow Mass", GetSetting.SPIS(ScanRootNode, SPISLens.UPSLensE, out isMassDrivenActive, 0), isMassDrivenActive);
                        upsPhaseSettings.Lens_E_H = new Setting("Lens E\nHigh Mass", GetSetting.SPIS(ScanRootNode, SPISLens.UPSLensE, out isMassDrivenActive), isMassDrivenActive);
                        upsPhaseSettings.Flowtube = new Setting("Lens FT", GetSetting.SPIS(ScanRootNode, SPISLens.UPSFlowTube, out isMassDrivenActive), isMassDrivenActive);
                        break;
                    case Global.InstrumentType.DPIS:
                    case Global.InstrumentType.Infinity:
                        upsPhaseSettings.Lens_14 = new Setting("Lens 14", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens14, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Lens_1 = new Setting("Lens 1", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens1, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Lens_2 = new Setting("Lens 2", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens2, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Lens_3 = new Setting("Lens 3", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens3, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Lens_4 = new Setting("Lens 4", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens4, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Prefilter = new Setting("PF", GetSetting.DPIS(ScanRootNode, DPISLens.UPSPrefilter, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Axial_Bias = new Setting("AB", GetSetting.DPIS(ScanRootNode, DPISLens.UPSAxialBias, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Lens_5 = new Setting("Lens 5", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens5, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Lens_6 = new Setting("Lens 6", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens6, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Lens_E_L = new Setting("Lens E\nLow Mass", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLensE, out isMassDrivenActive, GetSetting.Phase.PosWet,0), isMassDrivenActive);
                        upsPhaseSettings.Lens_E_H = new Setting("Lens E\nHigh Mass", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLensE, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
                        upsPhaseSettings.Flowtube = new Setting("Lens FT", GetSetting.DPIS(ScanRootNode, DPISLens.UPSFlowTube, out isMassDrivenActive, GetSetting.Phase.PosWet), isMassDrivenActive);
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

        #region NegWet
        //Setting check with range check
        public static UPSPhaseSettings GetNegWetUPSPhaseData(this UPSPhaseSettings upsPhaseSettings, XElement ScanRootNode, XElement ConfigRootNode)
        {
            try
            {
                GetNegWetUPSPhaseSettings(ref upsPhaseSettings, ScanRootNode);
                GetUPSPhaseRangesMethod(ref upsPhaseSettings, ConfigRootNode, "UPS_NegWet", Global.InstrumentType.Infinity);
                return upsPhaseSettings;
            }
            catch (Exception ex)
            {
                throw new Exception($"NegWet UPS Phase Data: {ex.Message}");
            }
        }

        private static void GetNegWetUPSPhaseSettings(ref UPSPhaseSettings upsPhaseSettings, XElement ScanRootNode)
        {
            try
            {
                bool isMassDrivenActive;
                upsPhaseSettings.Lens_14 = new Setting("Lens 14", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens14, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Lens_1 = new Setting("Lens 1", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens1, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Lens_2 = new Setting("Lens 2", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens2, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Lens_3 = new Setting("Lens 3", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens3, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Lens_4 = new Setting("Lens 4", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens4, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Prefilter = new Setting("PF", GetSetting.DPIS(ScanRootNode, DPISLens.UPSPrefilter, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Axial_Bias = new Setting("AB", GetSetting.DPIS(ScanRootNode, DPISLens.UPSAxialBias, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Lens_5 = new Setting("Lens 5", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens5, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Lens_6 = new Setting("Lens 6", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens6, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Lens_E_H = new Setting("Lens E", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLensE, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
                upsPhaseSettings.Flowtube = new Setting("Lens FT", GetSetting.DPIS(ScanRootNode, DPISLens.UPSFlowTube, out isMassDrivenActive, GetSetting.Phase.NegWet), isMassDrivenActive);
            }
            catch (Exception ex)
            {
                throw new Exception($"Scan Data Collection failed，Reason：{ex.Message}");
            }
        }
        #endregion

        #region NegDry
        //Setting check with range check
        public static UPSPhaseSettings GetNegDryUPSPhaseData(this UPSPhaseSettings upsPhaseSettings, XElement ScanRootNode, XElement ConfigRootNode)
        {
            try
            {
                GetNegDryUPSPhaseSettings(ref upsPhaseSettings, ScanRootNode);
                GetUPSPhaseRangesMethod(ref upsPhaseSettings, ConfigRootNode, "UPS_NegDry", Global.InstrumentType.Infinity);
                return upsPhaseSettings;
            }
            catch (Exception ex)
            {
                throw new Exception($"NegDry UPS Phase Data: {ex.Message}");
            }
        }

        private static void GetNegDryUPSPhaseSettings(ref UPSPhaseSettings upsPhaseSettings, XElement ScanRootNode)
        {
            try
            {
                bool isMassDrivenActive;
                upsPhaseSettings.Lens_14 = new Setting("Lens 14", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens14, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Lens_1 = new Setting("Lens 1", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens1, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Lens_2 = new Setting("Lens 2", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens2, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Lens_3 = new Setting("Lens 3", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens3, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Lens_4 = new Setting("Lens 4", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens4, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Prefilter = new Setting("PF", GetSetting.DPIS(ScanRootNode, DPISLens.UPSPrefilter, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Axial_Bias = new Setting("AB", GetSetting.DPIS(ScanRootNode, DPISLens.UPSAxialBias, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Lens_5 = new Setting("Lens 5", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens5, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Lens_6 = new Setting("Lens 6", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLens6, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Lens_E_H = new Setting("Lens E", GetSetting.DPIS(ScanRootNode, DPISLens.UPSLensE, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
                upsPhaseSettings.Flowtube = new Setting("Lens FT", GetSetting.DPIS(ScanRootNode, DPISLens.UPSFlowTube, out isMassDrivenActive, GetSetting.Phase.NegDry), isMassDrivenActive);
            }
            catch (Exception ex)
            {
                throw new Exception($"Scan Data Collection failed，Reason：{ex.Message}");
            }
        }
        #endregion

        private static void GetUPSPhaseRangesMethod(ref UPSPhaseSettings upsPhaseSettings, XElement ConfigRootNode, string PhaseName, Global.InstrumentType type)
        {
            try
            {
                switch (type)
                {
                    case Global.InstrumentType.SPIS:
                        upsPhaseSettings.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", PhaseName);

                        upsPhaseSettings.Lens_1.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_1", "Minimum"));
                        upsPhaseSettings.Lens_1.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_1", "Maximum"));

                        upsPhaseSettings.Lens_2.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_2", "Minimum"));
                        upsPhaseSettings.Lens_2.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_2", "Maximum"));

                        upsPhaseSettings.Lens_3.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_3", "Minimum"));
                        upsPhaseSettings.Lens_3.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_3", "Maximum"));

                        upsPhaseSettings.Lens_4.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_4", "Minimum"));
                        upsPhaseSettings.Lens_4.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_4", "Maximum"));

                        upsPhaseSettings.Prefilter.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Prefilter", "Minimum"));
                        upsPhaseSettings.Prefilter.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Prefilter", "Maximum"));

                        upsPhaseSettings.Axial_Bias.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Axial_Bias", "Minimum"));
                        upsPhaseSettings.Axial_Bias.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Axial_Bias", "Maximum"));

                        upsPhaseSettings.Lens_5.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_5", "Minimum"));
                        upsPhaseSettings.Lens_5.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_5", "Maximum"));

                        upsPhaseSettings.Lens_6.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_6", "Minimum"));
                        upsPhaseSettings.Lens_6.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_6", "Maximum"));

                        upsPhaseSettings.Lens_E_L.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_E", "Minimum"));
                        upsPhaseSettings.Lens_E_L.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_E", "Maximum"));

                        upsPhaseSettings.Lens_E_H.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_E", "Minimum"));
                        upsPhaseSettings.Lens_E_H.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Lens_E", "Maximum"));

                        upsPhaseSettings.Flowtube.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Flowtube", "Minimum"));
                        upsPhaseSettings.Flowtube.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "SPIS_UPS_PosWet", "Flowtube", "Maximum"));
                        break;
                    case Global.InstrumentType.DPIS:
                    case Global.InstrumentType.Infinity:
                        upsPhaseSettings.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", PhaseName);
                        upsPhaseSettings.UPS_Extraction_Lens_Gradient_Setting = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Extraction_Lens_Gradient", "Value"));

                        upsPhaseSettings.Lens_14.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_14", "Minimum"));
                        upsPhaseSettings.Lens_14.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_14", "Maximum"));

                        upsPhaseSettings.Lens_1.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_1", "Minimum"));
                        upsPhaseSettings.Lens_1.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_1", "Maximum"));

                        upsPhaseSettings.Lens_2.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_2", "Minimum"));
                        upsPhaseSettings.Lens_2.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_2", "Maximum"));

                        upsPhaseSettings.Lens_3.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_3", "Minimum"));
                        upsPhaseSettings.Lens_3.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_3", "Maximum"));

                        upsPhaseSettings.Lens_4.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_4", "Minimum"));
                        upsPhaseSettings.Lens_4.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_4", "Maximum"));

                        upsPhaseSettings.Prefilter.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Prefilter", "Minimum"));
                        upsPhaseSettings.Prefilter.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Prefilter", "Maximum"));

                        upsPhaseSettings.Axial_Bias.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Axial_Bias", "Minimum"));
                        upsPhaseSettings.Axial_Bias.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Axial_Bias", "Maximum"));

                        upsPhaseSettings.Lens_5.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_5", "Minimum"));
                        upsPhaseSettings.Lens_5.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_5", "Maximum"));

                        upsPhaseSettings.Lens_6.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_6", "Minimum"));
                        upsPhaseSettings.Lens_6.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_6", "Maximum"));

                        if (upsPhaseSettings.Lens_E_L != null)
                        {
                            upsPhaseSettings.Lens_E_L.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_E", "Minimum"));
                            upsPhaseSettings.Lens_E_L.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_E", "Maximum"));
                        }

                        upsPhaseSettings.Lens_E_H.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_E", "Minimum"));
                        upsPhaseSettings.Lens_E_H.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Lens_E", "Maximum"));

                        upsPhaseSettings.Flowtube.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Flowtube", "Minimum"));
                        upsPhaseSettings.Flowtube.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", PhaseName, "Flowtube", "Maximum"));
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
