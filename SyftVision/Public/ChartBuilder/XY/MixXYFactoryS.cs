﻿using Public.Global;
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
                    for (int i = 0; i < setting.MapSetList.Count; i++)
                    {
                        yLayerList.Add(new XYItemS.YLayer(new XYLegend(setting.ScanList[i].Name, MyColor.Pool[i]),
                            setting.MapSetList.Select(a => a.Value.ValueList[i]).ToList()));
                    }
                    return new XYItemS(labelList, yLayerList, upperLimitList, underLimitList);
                case "Table":
                    labelList = setting.TableSetList.Select(a => a.Key.ToString()).ToList();
                    upperLimitList = setting.TableSetList.Select(a => a.Value.UpperLimit).ToList();
                    underLimitList = setting.TableSetList.Select(a => a.Value.UnderLimit).ToList();
                    yLayerList = new List<XYItemS.YLayer>();
                    for (int i = 0; i < setting.TableSetList.Count; i++)
                    {
                        yLayerList.Add(new XYItemS.YLayer(new XYLegend(setting.ScanList[i].Name, MyColor.Pool[i]),
                            setting.TableSetList.Select(a => a.Value.ValueList[i]).ToList()));
                    }
                    return new XYItemS(labelList, yLayerList, upperLimitList, underLimitList);
                case "Value":

                    break;
            }
        }
    }
}
