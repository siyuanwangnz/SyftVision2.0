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
    public class TextLimitSetDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Text Config";
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

            if (SelectedSetting.Text == null)
                Text = SettingText.GetText(SelectedSetting.Content);
            else
            {
                Text = SelectedSetting.Text.Text;
                ReferText = SelectedSetting.Text.ReferText;
            }
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SettingText SettingText = new SettingText();
                    SettingText.Text = Text;
                    SettingText.ReferText = ReferText;

                    DialogParameters param = new DialogParameters();
                    param.Add("SettingText", SettingText);
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
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        private string _referText;
        public string ReferText
        {
            get => _referText;
            set => SetProperty(ref _referText, value);
        }
    }
}