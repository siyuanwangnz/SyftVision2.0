using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Public.SettingConfig;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingConfig.ViewModels
{
    public class MapLimitSetDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Map Config";
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

            MapSetList = SettingMap.GetMapSetList(SelectedSetting.Content);

        }
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MapSetList.Insert(MapSetList.IndexOf(SelectedMapSet) + 1, new SettingMap());
                });
            }
        }
        private ObservableCollection<SettingMap> _mapSetList;
        public ObservableCollection<SettingMap> MapSetList
        {
            get => _mapSetList;
            set => SetProperty(ref _mapSetList, value);
        }
        private SettingMap _selectedMapSet;
        public SettingMap SelectedMapSet
        {
            get => _selectedMapSet;
            set => SetProperty(ref _selectedMapSet, value);
        }
        private string _settingName;
        public string SettingName
        {
            get => _settingName;
            set => SetProperty(ref _settingName, value);
        }
    }
}

