using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Public.ChartConfig;
using Public.SettingConfig;
using Public.SFTP;
using Public.TreeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace SettingConfig.ViewModels
{
    public class SettingConfigViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly SyftServer _syftServer;
        private InstrumentServer _instrumentServer;
        public SettingConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _syftServer = new SyftServer();
        }

        #region Toolbar
        public DelegateCommand OpenCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        // Get tree nodes
                        List<TreeNode> treeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Setting);

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("treeNodes", treeNodes);
                        _dialogService.ShowDialog("SyftSettingDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                SettingProp settingProp = _syftServer.DownloadSetting(treeNode, false);

                                Tittle = settingProp.Tittle;
                                SubTittle = settingProp.SubTittle;
                                FilterOffList = new ObservableCollection<FilterOff>(settingProp.FilterOffList);
                                SettingList = new ObservableCollection<Setting>(settingProp.SettingList);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (Tittle == null || Tittle == "" || SubTittle == null || SubTittle == "")
                    {
                        MessageBox.Show($"Tittle, Sub-Tittle can not be empty", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    SettingProp settingProp = new SettingProp(Tittle, SubTittle, FilterOffList.ToList(), SettingList.ToList());

                    try
                    {
                        _syftServer.UploadSetting(settingProp);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        private string _ipAddress = "10.0.17.";
        public string IPAddress
        {
            get => _ipAddress;
            set => SetProperty(ref _ipAddress, value);
        }
        public DelegateCommand NewCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    _instrumentServer = new InstrumentServer(IPAddress, new Public.Global.Options());

                    try
                    {
                        // Get tree nodes
                        List<TreeNode> treeNodes = _instrumentServer.GetScanFileTreeNodes();

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("treeNodes", treeNodes);
                        _dialogService.ShowDialog("InstruScanDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                SettingList = new ObservableCollection<Setting>(_instrumentServer.GetSettingListFromScanFile(treeNode, FilterOffList.ToList()));
                            }
                        });

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        private string _tittle;
        public string Tittle
        {
            get => _tittle;
            set => SetProperty(ref _tittle, value);
        }
        private string _subTittle;
        public string SubTittle
        {
            get => _subTittle;
            set => SetProperty(ref _subTittle, value);
        }
        #endregion

        private ObservableCollection<FilterOff> _filterOffList = new ObservableCollection<FilterOff>() { new FilterOff() };
        public ObservableCollection<FilterOff> FilterOffList
        {
            get => _filterOffList;
            set => SetProperty(ref _filterOffList, value);
        }
        private FilterOff _selectedFilterOff;
        public FilterOff SelectedFilterOff
        {
            get => _selectedFilterOff;
            set => SetProperty(ref _selectedFilterOff, value);
        }
        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    FilterOffList.Remove(SelectedFilterOff);
                    if (FilterOffList.Count == 0) FilterOffList.Add(new FilterOff());
                });
            }
        }
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    FilterOffList.Insert(FilterOffList.IndexOf(SelectedFilterOff) + 1, new FilterOff());
                });
            }
        }

        private ObservableCollection<Setting> _settingList;
        public ObservableCollection<Setting> SettingList
        {
            get => _settingList;
            set => SetProperty(ref _settingList, value);
        }
        private Setting _selectedSetting;
        public Setting SelectedSetting
        {
            get => _selectedSetting;
            set => SetProperty(ref _selectedSetting, value);
        }
        public DelegateCommand SelectedCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    // Navigate to dialog
                    DialogParameters param = new DialogParameters();
                    param.Add("SelectedSetting", SelectedSetting);
                    _dialogService.ShowDialog(SelectedSetting.Type.LimitSetDialog, param, arg =>
                    {
                        if (arg.Result == ButtonResult.OK)
                        {
                            switch (SelectedSetting.Type.Name)
                            {
                                case "Map":
                                    SelectedSetting.MapSetList = arg.Parameters.GetValue<List<SettingMap>>("MapSetList");
                                    break;
                                case "Table":
                                    SelectedSetting.TableSetList = arg.Parameters.GetValue<List<SettingTable>>("TableSetList");
                                    break;
                                case "OnOff":
                                    SelectedSetting.OnOff = arg.Parameters.GetValue<SettingOnOff>("SettingOnOff");
                                    break;
                                case "Value":
                                    SelectedSetting.Value = arg.Parameters.GetValue<SettingValue>("SettingValue");
                                    break;
                                case "Text":
                                    SelectedSetting.Text = arg.Parameters.GetValue<SettingText>("SettingText");
                                    break;
                            }
                            SelectedSetting.UpdateValid();
                        }
                    });
                });
            }
        }
    }
}
