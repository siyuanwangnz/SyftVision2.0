using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingCheck.Models
{
    public class DPISLens
    {
        //Source
        public const string SourcePressure_PosWet = "NeoVacuumZoneDpis2.ZoneSetting.sourcePressurePositiveIonsWet";
        public const string SourcePressure_NegWet = "NeoVacuumZoneDpis2.ZoneSetting.sourcePressureNegativeIonsWet";
        public const string SourcePressure_NegDry = "NeoVacuumZoneDpis2.ZoneSetting.sourcePressureNegativeIonsDry";
        public const string MicrowavePower_PosWet = "NeoSourceZone.ZoneSetting.microwavePowerWetPositive";
        public const string MicrowavePower_NegWet = "NeoSourceZone.ZoneSetting.microwavePowerWetNegative";
        public const string MicrowavePower_NegDry = "NeoSourceZone.ZoneSetting.microwavePowerDryNegative";
        public const string Mesh_NegWet = "NeoSourceZone.ZoneSetting.meshVoltageWetNegative";
        public const string Mesh_NegDry = "NeoSourceZone.ZoneSetting.meshVoltageDryNegative";
        //UPS
        public const string UPSLens14 = "NeoSelectionZone.ZoneSetting.lens14";
        public const string UPSLens1 = "NeoSelectionZone.ZoneSetting.lens1";
        public const string UPSLens2 = "NeoSelectionZone.ZoneSetting.lens2";
        public const string UPSLens3 = "NeoSelectionZone.ZoneSetting.lens3";
        public const string UPSLens4 = "NeoSelectionZone.ZoneSetting.lens4";
        public const string UPSPrefilter = "NeoSelectionZone.ZoneSetting.quadrupolePrefilter";
        public const string UPSAxialBias = "NeoSelectionZone.ZoneSetting.quadrupoleAxialBias";
        public const string UPSLens5 = "NeoSelectionZone.ZoneSetting.lens5";
        public const string UPSLens6 = "NeoSelectionZone.ZoneSetting.lens6";
        public const string UPSLensE = "NeoSelectionZone.ZoneSetting.lensE";
        public const string UPSFlowTube = "NeoSelectionZone.ZoneSetting.lensFt";
        //DWS
        public const string DWSLensE = "NeoDetectionZone.ZoneSetting.lensE";
        public const string DWSLens1 = "NeoDetectionZone.ZoneSetting.lens1";
        public const string DWSIGBias = "NeoDetectionZone.ZoneSetting.igBias";
        public const string DWSIGRF = "NeoDetectionZone.ZoneSetting.igRf";
        public const string DWSLens2 = "NeoDetectionZone.ZoneSetting.lens2";
        public const string DWSLens3 = "NeoDetectionZone.ZoneSetting.lens3";
        public const string DWSLens4 = "NeoDetectionZone.ZoneSetting.lens4";
        //DWS Specific
        public const string DWSPrefilter = "NeoDetectionZone.ZoneSetting.quadrupolePrefilter";
        public const string DWSAxialBias = "NeoDetectionZone.ZoneSetting.quadrupoleAxialBias";
        public const string DWSLens5 = "NeoDetectionZone.ZoneSetting.lens5";
        //Detection
        public const string Detector = "NeoDetectionZone.ZoneSetting.detectorVolts";
        public const string Discriminator = "NeoDetectionZone.ZoneSetting.detectorDiscriminatorVolts";
        public const string SettleTime = "scanner.dws.quad.settle_time";

        //Fix Volt Setting
        public const string FixVolt = "FixedVolts";
        //Mass Table
        public const string MassTable = "MassTable";
        //Mass Driven
        public const string MassDriven = "MassDriven";
    }
}
