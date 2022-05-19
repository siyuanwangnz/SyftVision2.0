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
            new SettingType("Text","TextLimitSetDialogView", new BarChartFactory(new SensitivityXYFactory())),
            new SettingType("Value","ValueLimitSetDialogView", new BarChartFactory(new SensitivityXYFactory())),
            new SettingType("OnOff","OnOffLimitSetDialogView", new BarChartFactory(new SensitivityXYFactory())),
            new SettingType("Table","TableLimitSetDialogView", new BarChartFactory(new SensitivityXYFactory())),
            new SettingType("Map","MapLimitSetDialogView", new BarChartFactory(new SensitivityXYFactory())),
        };
        public SettingType(string name, string limitSetDialog, ChartFactory chartFactory)
        {
            Name = name;
            LimitSetDialog = limitSetDialog;
            ChartFactory = chartFactory;
        }
        public string Name { get; }
        public string LimitSetDialog { get; }
        public ChartFactory ChartFactory { get; }
    }
}
