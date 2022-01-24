using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;
using Public.Global;
using SettingCheck.Models;

namespace SettingCheck.Services
{
    static class GetDWSSpecificDataExtension
    {
        #region Get data from scan file 
        //Setting check with range check
        public static DWSSpecificSettings GetDWSSpecificData(this DWSSpecificSettings dwsSpecificSettings, XElement ScanRootNode, XElement ConfigRootNode, Global.InstrumentType type)
        {
            try
            {
                GetDWSSpecificSettings(ref dwsSpecificSettings, ScanRootNode, type);
                GetDWSSpecificMassRanges(dwsSpecificSettings.Prefilter, ConfigRootNode, "DWS_Prefilter", type);
                GetDWSSpecificMassRanges(dwsSpecificSettings.Axial_Bias, ConfigRootNode, "DWS_Axial_Bias", type);
                GetDWSSpecificMassRanges(dwsSpecificSettings.Lens_5, ConfigRootNode, "DWS_Lens_5", type);
                return dwsSpecificSettings;
            }
            catch (Exception ex)
            {
                throw new Exception($"DWS Specific Data: {ex.Message}");
            }
        }

        private static void GetDWSSpecificSettings(ref DWSSpecificSettings dwsSpecificSettings, XElement ScanRootNode, Global.InstrumentType type)
        {
            try
            {
                Dictionary<double, double> massTableDic;
                switch (type)
                {
                    case Global.InstrumentType.SPIS:
                        GetSetting.SPIS(ScanRootNode, SPISLens.DWSPrefilter, out massTableDic);
                        dwsSpecificSettings.Prefilter = GetDWSSpecificMassSettings(massTableDic);
                        GetSetting.SPIS(ScanRootNode, SPISLens.DWSAxialBias, out massTableDic);
                        dwsSpecificSettings.Axial_Bias = GetDWSSpecificMassSettings(massTableDic);
                        GetSetting.SPIS(ScanRootNode, SPISLens.DWSLens5, out massTableDic);
                        dwsSpecificSettings.Lens_5 = GetDWSSpecificMassSettings(massTableDic);
                        break;
                    case Global.InstrumentType.DPIS:
                    case Global.InstrumentType.Infinity:
                        GetSetting.DPIS(ScanRootNode, DPISLens.DWSPrefilter, out massTableDic);
                        dwsSpecificSettings.Prefilter = GetDWSSpecificMassSettings(massTableDic);
                        GetSetting.DPIS(ScanRootNode, DPISLens.DWSAxialBias, out massTableDic);
                        dwsSpecificSettings.Axial_Bias = GetDWSSpecificMassSettings(massTableDic);
                        GetSetting.DPIS(ScanRootNode, DPISLens.DWSLens5, out massTableDic);
                        dwsSpecificSettings.Lens_5 = GetDWSSpecificMassSettings(massTableDic);
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

        private static DWSSpecificMassSettings GetDWSSpecificMassSettings(Dictionary<double, double> dictionary)
        {
            DWSSpecificMassSettings dwsSpecificMassSettings = new DWSSpecificMassSettings();
            dwsSpecificMassSettings.MassList_Pos = new List<Setting>();
            dwsSpecificMassSettings.MassList_Neg = new List<Setting>();
            if (dictionary.Count == 0 && dictionary.ContainsKey(-1))
            {
                dwsSpecificMassSettings.MassList_Pos.Add(new Setting("Fixed Setting", dictionary[-1], false));
            }
            else
            {
                foreach (var item in dictionary)
                {
                    if (item.Key < 0)
                    {
                        dwsSpecificMassSettings.MassList_Neg.Add(new Setting(item.Key.ToString(), item.Value, true));
                    }
                    if (item.Key > 0)
                    {
                        dwsSpecificMassSettings.MassList_Pos.Add(new Setting(item.Key.ToString(), item.Value, true));
                    }
                }
            }
            return dwsSpecificMassSettings;
        }

        private static void GetDWSSpecificMassRanges(DWSSpecificMassSettings dwsSpecificMassSettings, XElement ConfigRootNode, string LensName, Global.InstrumentType type)
        {
            try
            {
                dwsSpecificMassSettings.Unit = GetConfigData.Unit(ConfigRootNode, "ThreePhase", LensName);

                foreach (var item in dwsSpecificMassSettings.MassList_Pos)
                {
                    double mass = double.Parse(item.Name);
                    switch (type)
                    {
                        case Global.InstrumentType.SPIS:
                            if (mass <= 19)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "DPIS_Mass_Lower_19", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "DPIS_Mass_Lower_19", "Maximum"));
                            }
                            else
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "DPIS_Mass_Upper_236", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "DPIS_Mass_Upper_236", "Maximum"));
                            }
                            break;
                        case Global.InstrumentType.DPIS:
                            if (mass <= 19)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "DPIS_Mass_Lower_19", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "DPIS_Mass_Lower_19", "Maximum"));
                            }
                            else
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "DPIS_Mass_Upper_236", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "DPIS_Mass_Upper_236", "Maximum"));
                            }
                            break;
                        case Global.InstrumentType.Infinity:
                            if (mass <= 19)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_Lower_19", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_Lower_19", "Maximum"));
                            }
                            else if (mass == 28)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_28", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_28", "Maximum"));
                            }
                            else if (mass == 57)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_57", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_57", "Maximum"));
                            }
                            else if (mass == 78)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_78", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_78", "Maximum"));
                            }
                            else if (mass == 106)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_106", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_106", "Maximum"));
                            }
                            else if (mass == 150)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_150", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_150", "Maximum"));
                            }
                            else if (mass == 186)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_186", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_186", "Maximum"));
                            }
                            else if (mass >= 236)
                            {
                                item.MinimumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_Upper_236", "Minimum"));
                                item.MaximumValue = double.Parse(GetConfigData.Range(ConfigRootNode, "ThreePhase", LensName, "Mass_Upper_236", "Maximum"));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Config Data Collection failed，Reason：{ex.Message}");
            }
        }
    }
}
