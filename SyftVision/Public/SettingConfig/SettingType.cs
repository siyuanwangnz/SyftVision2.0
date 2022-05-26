using Public.ChartBuilder;
using Public.ChartBuilder.XY;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SettingConfig
{
    public class SettingType
    {
        public static List<SettingType> ReferList = new List<SettingType>()
        {
            new SettingType("Text","TextLimitSetDialogView", new TextChartFactoryS(new MixXYFactoryS())),
            new SettingType("Value","ValueLimitSetDialogView", new MixChartFactoryS(new MixXYFactoryS())),
            new SettingType("OnOff","OnOffLimitSetDialogView", new TextChartFactoryS(new MixXYFactoryS())),
            new SettingType("Table","TableLimitSetDialogView", new MixChartFactoryS(new MixXYFactoryS())),
            new SettingType("Map","MapLimitSetDialogView", new MixChartFactoryS(new MixXYFactoryS())),
        };
        public SettingType(string name, string limitSetDialog, ChartFactoryS chartFactoryS)
        {
            Name = name;
            LimitSetDialog = limitSetDialog;
            ChartFactoryS = chartFactoryS;
        }
        public string Name { get; }
        public string LimitSetDialog { get; }
        public ChartFactoryS ChartFactoryS { get; }
    }
}
