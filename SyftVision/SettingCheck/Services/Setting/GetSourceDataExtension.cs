using System;
using System.Windows;
using System.Xml.Linq;
using Public.Global;
using SettingCheck.Models;

namespace SettingCheck.Services
{
    static class GetSourceDataExtension
    {
        #region Get data from scan file
        //Setting check with range check
        public static SourceSettings GetSourceData(this SourceSettings sourceSettings, XElement ScanRootNode, XElement ConfigRootNode, Global.InstrumentType type)
        {
            try
            {
                GetSourceSettings(ref sourceSettings, ScanRootNode, type);
                GetSourceRanges(ref sourceSettings, ConfigRootNode, type);
                return sourceSettings;
            }
            catch (Exception ex)
            {
                throw new Exception($"Source Data: {ex.Message}");
            }
        }

        private static void GetSourceSettings(ref SourceSettings sourceSettings, XElement ScanRootNode, Global.InstrumentType type)
        {
            try
            {
                switch (type)
                {
                    case Global.InstrumentType.SPIS:
                        sourceSettings.Pressure = new SourcePhaseSettings();
                        sourceSettings.Pressure.PosWet = new Setting("PosWet", GetSetting.SPIS(ScanRootNode, SPISLens.SourcePressure), false);

                        sourceSettings.MV = new SourcePhaseSettings();
                        sourceSettings.MV.PosWet = new Setting("PosWet", GetSetting.SPIS(ScanRootNode, SPISLens.MicrowavePower), false);
                        break;
                    case Global.InstrumentType.DPIS:
                    case Global.InstrumentType.Infinity:
                        sourceSettings.Pressure = new SourcePhaseSettings();
                        sourceSettings.Pressure.PosWet = new Setting("PosWet", GetSetting.DPIS(ScanRootNode, DPISLens.SourcePressure_PosWet), false);
                        sourceSettings.Pressure.NetWet = new Setting("NegWet", GetSetting.DPIS(ScanRootNode, DPISLens.SourcePressure_NegWet), false);
                        sourceSettings.Pressure.NetDry = new Setting("NegDry", GetSetting.DPIS(ScanRootNode, DPISLens.SourcePressure_NegDry), false);

                        sourceSettings.MV = new SourcePhaseSettings();
                        sourceSettings.MV.PosWet = new Setting("PosWet", GetSetting.DPIS(ScanRootNode, DPISLens.MicrowavePower_PosWet), false);
                        sourceSettings.MV.NetWet = new Setting("NegWet", GetSetting.DPIS(ScanRootNode, DPISLens.MicrowavePower_NegWet), false);
                        sourceSettings.MV.NetDry = new Setting("NegDry", GetSetting.DPIS(ScanRootNode, DPISLens.MicrowavePower_NegDry), false);

                        sourceSettings.Mesh = new SourcePhaseSettings();
                        sourceSettings.Mesh.PosWet = new Setting("PosWet", 0, false);
                        sourceSettings.Mesh.NetWet = new Setting("NegWet", GetSetting.DPIS(ScanRootNode, DPISLens.Mesh_NegWet), false);
                        sourceSettings.Mesh.NetDry = new Setting("NegDry", GetSetting.DPIS(ScanRootNode, DPISLens.Mesh_NegDry), false);
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

        private static void GetSourceRanges(ref SourceSettings sourceSettings, XElement ConfigRootNode, Global.InstrumentType type)
        {
            try
            {
                switch (type)
                {
                    case Global.InstrumentType.SPIS:
                        {
                            sourceSettings.Pressure.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Source_pressure");

                            sourceSettings.Pressure.PosWet.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Source_pressure", "SPIS_PosWet", "Minimum"));
                            sourceSettings.Pressure.PosWet.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Source_pressure", "SPIS_PosWet", "Maximum"));
                        }
                        {
                            sourceSettings.MV.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "MW_Power");

                            sourceSettings.MV.PosWet.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "MW_Power", "SPIS_PosWet", "Minimum"));
                            sourceSettings.MV.PosWet.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "MW_Power", "SPIS_PosWet", "Maximum"));
                        }
                        break;
                    case Global.InstrumentType.DPIS:
                    case Global.InstrumentType.Infinity:
                        {
                            sourceSettings.Pressure.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Source_pressure");

                            sourceSettings.Pressure.PosWet.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Source_pressure", "PosWet", "Minimum"));
                            sourceSettings.Pressure.PosWet.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Source_pressure", "PosWet", "Maximum"));

                            sourceSettings.Pressure.NetWet.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Source_pressure", "NegWet", "Minimum"));
                            sourceSettings.Pressure.NetWet.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Source_pressure", "NegWet", "Maximum"));

                            sourceSettings.Pressure.NetDry.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Source_pressure", "NegDry", "Minimum"));
                            sourceSettings.Pressure.NetDry.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Source_pressure", "NegDry", "Maximum"));
                        }
                        {
                            sourceSettings.MV.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "MW_Power");

                            sourceSettings.MV.PosWet.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "MW_Power", "PosWet", "Minimum"));
                            sourceSettings.MV.PosWet.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "MW_Power", "PosWet", "Maximum"));

                            sourceSettings.MV.NetWet.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "MW_Power", "NegWet", "Minimum"));
                            sourceSettings.MV.NetWet.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "MW_Power", "NegWet", "Maximum"));

                            sourceSettings.MV.NetDry.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "MW_Power", "NegDry", "Minimum"));
                            sourceSettings.MV.NetDry.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "MW_Power", "NegDry", "Maximum"));
                        }
                        {
                            sourceSettings.Mesh.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Mesh_Voltage");

                            sourceSettings.Mesh.PosWet.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Mesh_Voltage", "PosWet", "Minimum"));
                            sourceSettings.Mesh.PosWet.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Mesh_Voltage", "PosWet", "Maximum"));

                            sourceSettings.Mesh.NetWet.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Mesh_Voltage", "NegWet", "Minimum"));
                            sourceSettings.Mesh.NetWet.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Mesh_Voltage", "NegWet", "Maximum"));

                            sourceSettings.Mesh.NetDry.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Mesh_Voltage", "NegDry", "Minimum"));
                            sourceSettings.Mesh.NetDry.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Mesh_Voltage", "NegDry", "Maximum"));
                        }
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
