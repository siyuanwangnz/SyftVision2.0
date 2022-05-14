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
            SettingName = "NeoDetectionZone.ZoneSetting.attenuationFactorMap";
            MapSetList = new ObservableCollection<SettingTable<string>>() { new SettingTable<string>() { Key="asd",SyftValue = new SettingValue(){ Value=23, UnderLimit=-23, UpperLimit=23} },
                new SettingTable<string>() { Key="asd",SyftValue = new SettingValue(){ Value=23, UnderLimit=-23, UpperLimit=23} }};
        }
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MapSetList.Insert(MapSetList.IndexOf(SelectedMapSet) + 1, new SettingTable<string>());
                });
            }
        }
        private ObservableCollection<SettingTable<string>> _mapSetList;
        public ObservableCollection<SettingTable<string>> MapSetList
        {
            get => _mapSetList;
            set => SetProperty(ref _mapSetList, value);
        }
        private SettingTable<string> _selectedMapSet;
        public SettingTable<string> SelectedMapSet
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

