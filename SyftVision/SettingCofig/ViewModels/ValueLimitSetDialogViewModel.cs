using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Public.SettingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingConfig.ViewModels
{
    public class ValueLimitSetDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Value Config";
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            SettingName = "NeoDetectionZone.ZoneSetting.detectorDiscriminatorVolts";
            Value = 123;
            UpperLimit = 12323;
            UnderLimit = -231;
        }
        private string _settingName;
        public string SettingName
        {
            get => _settingName;
            set => SetProperty(ref _settingName, value);
        }
        private double _value;
        public double Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        private double _upperLimit;
        public double UpperLimit
        {
            get => _upperLimit;
            set => SetProperty(ref _upperLimit, value);
        }
        private double _underLimit;
        public double UnderLimit
        {
            get => _underLimit;
            set => SetProperty(ref _underLimit, value);
        }
    }
}