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
            Setting SelectedSetting = parameters.GetValue<Setting>("SelectedSetting");

            SettingName = SelectedSetting.Name;

            if (SelectedSetting.TableSetList == null)
                TableSetList = SettingTable.GetTableSetList(SelectedSetting.Content);
            else
                TableSetList = SelectedSetting.TableSetList;
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    // Re-order list
                    List<SettingTable> list = TableSetList.ToList();
                    list.Sort((a, b) => a.Key.CompareTo(b.Key));

                    DialogParameters param = new DialogParameters();
                    param.Add("TableSetList", list.Count == 0 ? null : new ObservableCollection<SettingTable>(list));
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
                });
            }
        }
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    TableSetList.Insert(TableSetList.IndexOf(SelectedTableSet) + 1, new SettingTable());
                });
            }
        }
        private ObservableCollection<SettingTable> _tableSetList;
        public ObservableCollection<SettingTable> TableSetList
        {
            get => _tableSetList;
            set => SetProperty(ref _tableSetList, value);
        }
        private SettingTable _selectedTableSet;
        public SettingTable SelectedTableSet
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