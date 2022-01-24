using SettingCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace SettingCheck.Services
{
    class GetSetting
    {
        public enum Phase
        {
            PosWet,
            NegWet,
            NegDry
        }

        public static double SPIS(XElement RootNode, string lens, out Dictionary<double, double> massTableDic, double specialMass = -1)
        {
            massTableDic = new Dictionary<double, double>() { { -1, double.Parse(Content(RootNode, $"{lens}{SPISLens.FixVolt}")) } };
            switch (lens)
            {
                case SPISLens.SourcePressure:
                case SPISLens.MicrowavePower:
                case SPISLens.Detector:
                case SPISLens.Discriminator:
                    return double.Parse(Content(RootNode, lens));
                case SPISLens.SettleTime:
                    return double.Parse(Regex.Match(Content(RootNode, lens), @"^\{\[(\d*\.\d*)\]\}$").Groups[1].Value);
                default:
                    if (Content(RootNode, $"{lens}{SPISLens.MassDriven}") == "false")
                    {
                        return double.Parse(Content(RootNode, $"{lens}{SPISLens.FixVolt}"));
                    }
                    else
                    {
                        massTableDic = MassTableDic(MassTable(RootNode, $"{lens}{SPISLens.MassTable}"));
                        if (massTableDic.Count != 0)
                        {
                            if (specialMass == -1) return massTableDic.Last().Value;
                            foreach (var mass in massTableDic)
                            {
                                if (mass.Key >= specialMass) return mass.Value;
                            }
                        }
                        throw new Exception($"SPIS Mass Table Collection Fail");
                    }
            }

        }

        public static double SPIS(XElement RootNode, string lens, double specialMass = -1)
        {
            switch (lens)
            {
                case SPISLens.SourcePressure:
                case SPISLens.MicrowavePower:
                case SPISLens.Detector:
                case SPISLens.Discriminator:
                    return double.Parse(Content(RootNode, lens));
                case SPISLens.SettleTime:
                    return double.Parse(Regex.Match(Content(RootNode, lens), @"^\{\[(\d*\.\d*)\]\}$").Groups[1].Value);
                default:
                    if (Content(RootNode, $"{lens}{SPISLens.MassDriven}") == "false")
                    {
                        return double.Parse(Content(RootNode, $"{lens}{SPISLens.FixVolt}"));
                    }
                    else
                    {
                        var massTableDic = MassTableDic(MassTable(RootNode, $"{lens}{SPISLens.MassTable}"));
                        if (massTableDic.Count != 0)
                        {
                            if (specialMass == -1) return massTableDic.Last().Value;
                            foreach (var mass in massTableDic)
                            {
                                if (mass.Key >= specialMass) return mass.Value;
                            }
                        }
                        throw new Exception($"SPIS Mass Table Collection Fail");
                    }
            }

        }

        public static double SPIS(XElement RootNode, string lens, out bool isMassDrivenActive, double specialMass = -1)
        {
            isMassDrivenActive = false;
            switch (lens)
            {
                case SPISLens.SourcePressure:
                case SPISLens.MicrowavePower:
                case SPISLens.Detector:
                case SPISLens.Discriminator:
                    return double.Parse(Content(RootNode, lens));
                case SPISLens.SettleTime:
                    return double.Parse(Regex.Match(Content(RootNode, lens), @"^\{\[(\d*\.\d*)\]\}$").Groups[1].Value);
                default:
                    if (Content(RootNode, $"{lens}{SPISLens.MassDriven}") == "false")
                    {
                        return double.Parse(Content(RootNode, $"{lens}{SPISLens.FixVolt}"));
                    }
                    else
                    {
                        isMassDrivenActive = true;
                        var massTableDic = MassTableDic(MassTable(RootNode, $"{lens}{SPISLens.MassTable}"));
                        if (massTableDic.Count != 0)
                        {
                            if (specialMass == -1) return massTableDic.Last().Value;
                            foreach (var mass in massTableDic)
                            {
                                if (mass.Key >= specialMass) return mass.Value;
                            }
                        }
                        throw new Exception($"SPIS Mass Table Collection Fail");
                    }
            }

        }

        public static double DPIS(XElement RootNode, string lens, out Dictionary<double, double> massTableDic, Phase phase = Phase.PosWet, double specialMass = -1)
        {
            double temp;
            switch (phase)
            {
                default:
                case Phase.PosWet:
                    temp = double.Parse(Content(RootNode, $"{lens}{DPISLens.FixVolt}"));
                    break;
                case Phase.NegWet:
                case Phase.NegDry:
                    temp = 0 - double.Parse(Content(RootNode, $"{lens}{DPISLens.FixVolt}"));
                    break;
            }
            massTableDic = new Dictionary<double, double>() { { -1, temp } };
            switch (lens)
            {
                case DPISLens.SourcePressure_PosWet:
                case DPISLens.SourcePressure_NegWet:
                case DPISLens.SourcePressure_NegDry:
                case DPISLens.MicrowavePower_PosWet:
                case DPISLens.MicrowavePower_NegWet:
                case DPISLens.MicrowavePower_NegDry:
                case DPISLens.Mesh_NegWet:
                case DPISLens.Mesh_NegDry:
                case DPISLens.Detector:
                case DPISLens.Discriminator:
                    return double.Parse(Content(RootNode, lens));
                case DPISLens.SettleTime:
                    return double.Parse(Regex.Match(Content(RootNode, lens), @"^\{\[(\d*\.\d*)\]\}$").Groups[1].Value);
                default:
                    if (Content(RootNode, $"{lens}{DPISLens.MassDriven}") == "false")
                    {
                        switch (phase)
                        {
                            default:
                            case Phase.PosWet:
                                return double.Parse(Content(RootNode, $"{lens}{DPISLens.FixVolt}"));
                            case Phase.NegWet:
                            case Phase.NegDry:
                                return 0 - double.Parse(Content(RootNode, $"{lens}{DPISLens.FixVolt}"));
                        }
                    }
                    else
                    {
                        massTableDic = MassTableDic(MassTable_Bug(RootNode, $"{lens}{DPISLens.MassTable}"));
                        if (massTableDic.Count != 0)
                        {
                            if (specialMass == -1)
                            {
                                switch (phase)
                                {
                                    case Phase.PosWet:
                                        return massTableDic.Last().Value;
                                    case Phase.NegWet:
                                        foreach (var mass in massTableDic)
                                            if (mass.Key >= -32 && mass.Key <= -17) return mass.Value;
                                        throw new Exception($"{lens} Neg Wet Setting Collection Fail");
                                    case Phase.NegDry:
                                        foreach (var mass in massTableDic)
                                            if (mass.Key <= -46 || (mass.Key >= -16 && mass.Key < 0)) return mass.Value;
                                        throw new Exception($"{lens} Neg Dry Setting Collection Fail");
                                    default:
                                        break;
                                }
                            }
                            foreach (var mass in massTableDic)
                                if (mass.Key >= specialMass) return mass.Value;
                        }
                        throw new Exception($"DPIS Mass Table Collection Fail");
                    }
            }

        }

        public static double DPIS(XElement RootNode, string lens, Phase phase = Phase.PosWet, double specialMass = -1)
        {
            switch (lens)
            {
                case DPISLens.SourcePressure_PosWet:
                case DPISLens.SourcePressure_NegWet:
                case DPISLens.SourcePressure_NegDry:
                case DPISLens.MicrowavePower_PosWet:
                case DPISLens.MicrowavePower_NegWet:
                case DPISLens.MicrowavePower_NegDry:
                case DPISLens.Mesh_NegWet:
                case DPISLens.Mesh_NegDry:
                case DPISLens.Detector:
                case DPISLens.Discriminator:
                    return double.Parse(Content(RootNode, lens));
                case DPISLens.SettleTime:
                    return double.Parse(Regex.Match(Content(RootNode, lens), @"^\{\[(\d*\.\d*)\]\}$").Groups[1].Value);
                default:
                    if (Content(RootNode, $"{lens}{DPISLens.MassDriven}") == "false")
                    {
                        switch (phase)
                        {
                            default:
                            case Phase.PosWet:
                                return double.Parse(Content(RootNode, $"{lens}{DPISLens.FixVolt}"));
                            case Phase.NegWet:
                            case Phase.NegDry:
                                return 0 - double.Parse(Content(RootNode, $"{lens}{DPISLens.FixVolt}"));
                        }
                    }
                    else
                    {
                        var massTableDic = MassTableDic(MassTable_Bug(RootNode, $"{lens}{DPISLens.MassTable}"));
                        if (massTableDic.Count != 0)
                        {
                            if (specialMass == -1)
                            {
                                switch (phase)
                                {
                                    case Phase.PosWet:
                                        return massTableDic.Last().Value;
                                    case Phase.NegWet:
                                        foreach (var mass in massTableDic)
                                            if (mass.Key >= -32 && mass.Key <= -17) return mass.Value;
                                        throw new Exception($"{lens} Neg Wet Setting Collection Fail");
                                    case Phase.NegDry:
                                        foreach (var mass in massTableDic)
                                            if (mass.Key <= -46 || (mass.Key >= -16 && mass.Key < 0)) return mass.Value;
                                        throw new Exception($"{lens} Neg Dry Setting Collection Fail");
                                    default:
                                        break;
                                }
                            }
                            foreach (var mass in massTableDic)
                                if (mass.Key >= specialMass) return mass.Value;
                        }
                        throw new Exception($"DPIS Mass Table Collection Fail");
                    }
            }

        }

        public static double DPIS(XElement RootNode, string lens, out bool isMassDrivenActive, Phase phase = Phase.PosWet, double specialMass = -1)
        {
            isMassDrivenActive = false;
            switch (lens)
            {
                case DPISLens.SourcePressure_PosWet:
                case DPISLens.SourcePressure_NegWet:
                case DPISLens.SourcePressure_NegDry:
                case DPISLens.MicrowavePower_PosWet:
                case DPISLens.MicrowavePower_NegWet:
                case DPISLens.MicrowavePower_NegDry:
                case DPISLens.Mesh_NegWet:
                case DPISLens.Mesh_NegDry:
                case DPISLens.Detector:
                case DPISLens.Discriminator:
                    return double.Parse(Content(RootNode, lens));
                case DPISLens.SettleTime:
                    return double.Parse(Regex.Match(Content(RootNode, lens), @"^\{\[(\d*\.\d*)\]\}$").Groups[1].Value);
                default:
                    if (Content(RootNode, $"{lens}{DPISLens.MassDriven}") == "false")
                    {
                        switch (phase)
                        {
                            default:
                            case Phase.PosWet:
                                return double.Parse(Content(RootNode, $"{lens}{DPISLens.FixVolt}"));
                            case Phase.NegWet:
                            case Phase.NegDry:
                                return 0 - double.Parse(Content(RootNode, $"{lens}{DPISLens.FixVolt}"));
                        }
                    }
                    else
                    {
                        isMassDrivenActive = true;
                        var massTableDic = MassTableDic(MassTable_Bug(RootNode, $"{lens}{DPISLens.MassTable}"));
                        if (massTableDic.Count != 0)
                        {
                            if (specialMass == -1)
                            {
                                switch (phase)
                                {
                                    case Phase.PosWet:
                                        return massTableDic.Last().Value;
                                    case Phase.NegWet:
                                        foreach (var mass in massTableDic)
                                            if (mass.Key >= -32 && mass.Key <= -17) return mass.Value;
                                        throw new Exception($"{lens} Neg Wet Setting Collection Fail");
                                    case Phase.NegDry:
                                        foreach (var mass in massTableDic)
                                            if (mass.Key <= -46 || (mass.Key >= -16 && mass.Key < 0)) return mass.Value;
                                        throw new Exception($"{lens} Neg Dry Setting Collection Fail");
                                    default:
                                        break;
                                }
                            }
                            foreach (var mass in massTableDic)
                                if (mass.Key >= specialMass) return mass.Value;
                        }
                        throw new Exception($"DPIS Mass Table Collection Fail");
                    }
            }

        }

        //Node Content by attribute
        public static string Content(XElement RootNode, string AttributeName)
        {
            if (RootNode == null) return "";
            IEnumerable<XElement> targetNodes =
                from target in RootNode.Descendants("setting")
                where target.Attribute("name").Value.Equals(AttributeName)
                select target;
            foreach (var node in targetNodes)
            {
                return node.Value;
            }
            return "";
        }
        //Mass table
        //add ";" at end
        private static string MassTable(XElement RootNode, string AttributeName)
        {
            if (RootNode == null) return "";
            IEnumerable<XElement> targetNodes =
                from target in RootNode.Descendants("setting")
                where target.Attribute("name").Value.Equals(AttributeName)
                select target;
            foreach (var node in targetNodes)
            {
                return $"{node.Value};";
            }
            return "";
        }
        //Mass table by attribute
        //There is a bug of mass table format
        //Bug example: <setting name="NeoSelectionZone.ZoneSetting.lens14MassTable" units="44.0;">-200.0,-70.0;-46.0,-70.0;-32.0,-69.0;-17.0,-69.0;-16.0,-70.0;-0.1,-70.0;0.0,44.0;200.0</setting> 
        //For mass table, attribute "units" value should be at end of node content
        private static string MassTable_Bug(XElement RootNode, string AttributeName)
        {
            if (RootNode == null) return "";
            IEnumerable<XElement> targetNodes =
                from target in RootNode.Descendants("setting")
                where target.Attribute("name").Value.Equals(AttributeName)
                select target;
            foreach (var node in targetNodes)
            {
                if (node.Attribute("units") != null)
                {
                    if (node.Attribute("units").Value != "") return $"{node.Value},{node.Attribute("units").Value}";
                }
                else
                    return node.Value;
            }
            return "";
        }
        //Mass table dictionary from mass table
        private static Dictionary<double, double> MassTableDic(string MassTable)
        {
            Dictionary<double, double> dictionary = new Dictionary<double, double>();
            if (MassTable == null) return dictionary;
            while (MassTable.Contains(";"))
            {
                //Get xxx,xxx;
                string temp = MassTable.Substring(0, MassTable.IndexOf(";") + 1);
                //Add xxx,xxx; to dictionary
                dictionary.Add(double.Parse(temp.Substring(0, temp.IndexOf(","))), double.Parse(temp.Substring(temp.IndexOf(",") + 1, temp.IndexOf(";") - temp.IndexOf(",") - 1)));
                //Trim off front xxx,xxx;
                MassTable = MassTable.Remove(0, MassTable.IndexOf(";") + 1);
            }
            return dictionary;
        }

    }
}
