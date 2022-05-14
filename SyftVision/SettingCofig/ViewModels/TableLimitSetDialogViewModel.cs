﻿using Prism.Commands;
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
    public class TableLimitSetDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Table Config";
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
            SettingName = "NeoSelectionZone.ZoneSetting.lens14MassTable";
            TableSetList = new ObservableCollection<SettingTable<double>>() { new SettingTable<double>() { Key = 2313, SyftValue = new SettingValue() { Value = 123, UnderLimit = 1231, UpperLimit = 43432 } },
            new SettingTable<double>() { Key = 2313, SyftValue = new SettingValue() { Value = 123, UnderLimit = 1231, UpperLimit = 43432 } }};
        }
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    TableSetList.Insert(TableSetList.IndexOf(SelectedTableSet) + 1, new SettingTable<double>());
                });
            }
        }
        private ObservableCollection<SettingTable<double>> _tableSetList;
        public ObservableCollection<SettingTable<double>> TableSetList
        {
            get => _tableSetList;
            set => SetProperty(ref _tableSetList, value);
        }
        private SettingTable<double> _selectedTableSet;
        public SettingTable<double> SelectedTableSet
        {
            get => _selectedTableSet;
            set => SetProperty(ref _selectedTableSet, value);
        }
        private string _settingName;
        public string SettingName
        {
            get => _settingName;
            set => SetProperty(ref _settingName, value);
        }
    }
}