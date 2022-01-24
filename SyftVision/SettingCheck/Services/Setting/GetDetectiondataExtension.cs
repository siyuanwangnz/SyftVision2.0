using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;
using Public.Global;
using SettingCheck.Models;

namespace SettingCheck.Services
{
    static class GetDetectionDataExtension
    {
        #region Get data from scan file 
        //Setting check with range check
        public static DetectionSettings GetDetectionData(this DetectionSettings detectionSettings, XElement ScanRootNode, XElement ConfigRootNode, Global.InstrumentType type)
        {
            try
            {
                GetDetectionSettings(ref detectionSettings, ScanRootNode, type);
                GetDetectionRanges(ref detectionSettings, ConfigRootNode, type);
                return detectionSettings;
            }
            catch (Exception ex)
            {
                throw new Exception($"{type} Detection Data: {ex.Message}");
            }
        }

        private static void GetDetectionSettings(ref DetectionSettings detectionSettings, XElement ScanRootNode, Global.InstrumentType type)
        {
            try
            {
                switch (type)
                {
                    case Global.InstrumentType.SPIS:
                        detectionSettings.Detector_Voltage = new DetectionSetting();
                        detectionSettings.Detector_Voltage.Setting = new Setting("Detector", GetSetting.SPIS(ScanRootNode, SPISLens.Detector), false);

                        detectionSettings.Discriminator = new DetectionSetting();
                        detectionSettings.Discriminator.Setting = new Setting("Discriminator", GetSetting.SPIS(ScanRootNode, SPISLens.Discriminator), false);

                        //Get Settle Time {[xx.xx]}
                        detectionSettings.Settle_Time = new DetectionSetting();
                        detectionSettings.Settle_Time.Setting = new Setting("Settle Time", GetSetting.SPIS(ScanRootNode, SPISLens.SettleTime), false);
                        break;
                    case Global.InstrumentType.DPIS:
                    case Global.InstrumentType.Infinity:
                        detectionSettings.Detector_Voltage = new DetectionSetting();
                        detectionSettings.Detector_Voltage.Setting = new Setting("Detector", GetSetting.DPIS(ScanRootNode, DPISLens.Detector), false);

                        detectionSettings.Discriminator = new DetectionSetting();
                        detectionSettings.Discriminator.Setting = new Setting("Discriminator", GetSetting.DPIS(ScanRootNode, DPISLens.Discriminator), false);

                        //Get Settle Time {[xx.xx]}
                        detectionSettings.Settle_Time = new DetectionSetting();
                        detectionSettings.Settle_Time.Setting = new Setting("Settle Time", GetSetting.DPIS(ScanRootNode, DPISLens.SettleTime), false);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Scan Data Collection Failed，Reason：{ex.Message}");
            }
        }
        #endregion

        private static void GetDetectionRanges(ref DetectionSettings detectionSettings, XElement ConfigRootNode, Global.InstrumentType type)
        {
            try
            {
                switch (type)
                {
                    case Global.InstrumentType.SPIS:
                        detectionSettings.Detector_Voltage.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Detector");
                        detectionSettings.Detector_Voltage.Setting.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Detector", "Detector_Voltage", "Minimum"));
                        detectionSettings.Detector_Voltage.Setting.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Detector", "Detector_Voltage", "Maximum"));

                        detectionSettings.Discriminator.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Discriminator");
                        detectionSettings.Discriminator.Setting.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Discriminator", "Discriminator_Voltage", "Minimum"));
                        detectionSettings.Discriminator.Setting.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Discriminator", "Discriminator_Voltage", "Maximum"));

                        detectionSettings.Settle_Time.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Settle");
                        detectionSettings.Settle_Time.Setting.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Settle", "Settle_Time", "Minimum"));
                        detectionSettings.Settle_Time.Setting.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Settle", "Settle_Time", "Maximum"));
                        break;
                    case Global.InstrumentType.DPIS:
                    case Global.InstrumentType.Infinity:
                        detectionSettings.Detector_Voltage.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Detector");
                        detectionSettings.Detector_Voltage.Setting.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Detector", "Detector_Voltage", "Minimum"));
                        detectionSettings.Detector_Voltage.Setting.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Detector", "Detector_Voltage", "Maximum"));

                        detectionSettings.Discriminator.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Discriminator");
                        detectionSettings.Discriminator.Setting.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Discriminator", "Discriminator_Voltage", "Minimum"));
                        detectionSettings.Discriminator.Setting.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Discriminator", "Discriminator_Voltage", "Maximum"));

                        detectionSettings.Settle_Time.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", "Settle");
                        detectionSettings.Settle_Time.Setting.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Settle", "Settle_Time", "Minimum"));
                        detectionSettings.Settle_Time.Setting.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", "Settle", "Settle_Time", "Maximum"));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Config Data Collection Failed，Reason：{ex.Message}");
            }
        }
    }
}
