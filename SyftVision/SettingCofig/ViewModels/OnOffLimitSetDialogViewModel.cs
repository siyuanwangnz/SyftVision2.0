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
    public class OnOffLimitSetDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "OnOff Config";
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
            Setting SelectedSetting = parameters.GetValue<Setting>("SelectedSetting");

            SettingName = SelectedSetting.Name;

            if (SelectedSetting.OnOff == null)
                OnOff = SettingOnOff.GetOnOff(SelectedSetting.ContentList[0]);
            else
            {
                OnOff = SelectedSetting.OnOff.OnOffList[0];
                ReferOnOff = SelectedSetting.OnOff.ReferOnOff;
            }
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SettingOnOff SettingOnOff = new SettingOnOff();
                    SettingOnOff.OnOffList[0] = OnOff;
                    SettingOnOff.ReferOnOff = ReferOnOff;

                    DialogParameters param = new DialogParameters();
                    param.Add("SettingOnOff", SettingOnOff);
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
                });
            }
        }
        private string _settingName;
        public string SettingName
        {
            get => _settingName;
            set => SetProperty(ref _settingName, value);
        }
        private bool _onOff;
        public bool OnOff
        {
            get => _onOff;
            set => SetProperty(ref _onOff, value);
        }
        private bool _referOnOff;
        public bool ReferOnOff
        {
            get => _referOnOff;
            set => SetProperty(ref _referOnOff, value);
        }
        public DelegateCommand CloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
                });
            }
        }
    }
}