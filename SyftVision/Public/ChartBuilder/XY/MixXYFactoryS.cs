using Public.Global;
using Public.SettingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public class MixXYFactoryS : XYFactoryS
    {
        public override XYItemS GetXYItemS(Setting setting)
        {
            List<string> labelList;
            List<double> upperLimitList;
            List<double> underLimitList;
            List<XYItemS.YLayer> yLayerList;

            switch (setting.Type.Name)
            {
                case "Map":
                    labelList = setting.MapSetList.Select(a => a.Key).ToList();
                    upperLimitList = setting.MapSetList.Select(a => a.Value.UpperLimit).ToList();
                    underLimitList = setting.MapSetList.Select(a => a.Value.UnderLimit).ToList();
                    yLayerList = new List<XYItemS.YLayer>();
                    for (int i = 0; i < setting.ScanList.Count; i++)
                    {
                        yLayerList.Add(new XYItemS.YLayer(new XYLegend(setting.ScanList[i].File, MyColor.Pool[i]),
                            setting.MapSetList.Select(a => a.Value.ValueList[i]).ToList()));
                    }
                    return new XYItemS(labelList, yLayerList, upperLimitList, underLimitList);
                case "Table":
                    labelList = setting.TableSetList.Select(a => a.Key < 0 ? "\\" + a.Key.ToString() : a.Key.ToString()).ToList();
                    upperLimitList = setting.TableSetList.Select(a => a.Value.UpperLimit).ToList();
                    underLimitList = setting.TableSetList.Select(a => a.Value.UnderLimit).ToList();
                    yLayerList = new List<XYItemS.YLayer>();
                    for (int i = 0; i < setting.ScanList.Count; i++)
                    {
                        yLayerList.Add(new XYItemS.YLayer(new XYLegend(setting.ScanList[i].File, MyColor.Pool[i]),
                            setting.TableSetList.Select(a => a.Value.ValueList[i]).ToList()));
                    }
                    return new XYItemS(labelList, yLayerList, upperLimitList, underLimitList);
                case "Value":
                    labelList = setting.ScanList.Select(a => a.Date_Time).ToList();
                    upperLimitList = new List<double>();
                    underLimitList = new List<double>();
                    foreach (var item in setting.ScanList)
                    {
                        upperLimitList.Add(setting.Value.UpperLimit);
                        underLimitList.Add(setting.Value.UnderLimit);
                    }
                    return new XYItemS(labelList, new List<XYItemS.YLayer>() { new XYItemS.YLayer(new XYLegend("N/A", MyColor.Pool.First()), setting.Value.ValueList) }, upperLimitList, underLimitList);
                case "OnOff":
                    labelList = new List<string>() { "Expect" };
                    labelList.AddRange(setting.ScanList.Select(a => a.Date_Time).ToList());
                    yLayerList = new List<XYItemS.YLayer>() { new XYItemS.YLayer(new XYLegend(setting.OnOff.ReferOnOff.ToString(), MyColor.Pool[0]), new List<double>() { 10 }) };
                    for (int i = 0; i < setting.ScanList.Count; i++)
                    {
                        yLayerList.Add(new XYItemS.YLayer(new XYLegend(setting.OnOff.OnOffList[i].ToString(), MyColor.Pool[i + 1]), new List<double>() { 10 }));
                    }
                    return new XYItemS(labelList, yLayerList);
                case "Text":
                    labelList = new List<string>() { "Expect" };
                    labelList.AddRange(setting.ScanList.Select(a => a.Date_Time).ToList());
                    yLayerList = new List<XYItemS.YLayer>() { new XYItemS.YLayer(new XYLegend(setting.Text.ReferText, MyColor.Pool[0]), new List<double>() { 10 }) };
                    for (int i = 0; i < setting.ScanList.Count; i++)
                    {
                        yLayerList.Add(new XYItemS.YLayer(new XYLegend(setting.Text.TextList[i], MyColor.Pool[i + 1]), new List<double>() { 10 }));
                    }
                    return new XYItemS(labelList, yLayerList);
            }
            return null;
        }
    }
}
