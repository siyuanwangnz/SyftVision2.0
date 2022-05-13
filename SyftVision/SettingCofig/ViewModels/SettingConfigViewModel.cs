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
        public SettingConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _syftServer = new SyftServer();
            #region Test
            FilterOffList = new ObservableCollection<FilterOff>() { new FilterOff(), new FilterOff() };
            SettingList = new ObservableCollection<Setting>() {
                new Setting("NeoSelectionZone.ZoneSetting.lens14MassTable",
                "-400.0,-126.0;-46.0,-126.0;-32.0,-95.4;-17.0,-95.4;-16.0,-126.0;-0.1,-126.0;0.1,42.5;19.0,42.5;30.0,42.5;32.0,42.5;400.0,42.5;"),
                new Setting("NeoDetectionZone.ZoneSetting.attenuationFactorMap",
                "(H3O+,19+,6.578)(NO+,30+,10.22)(O2+,32+,10.19)(H3O+,37+,10.6)(H3O+,55+,18.10)(OH-,17-,2.756)(O2-,32-,11.90)(O-,16-,2.220)(NO2-,46-,11.40)" ),
                new Setting("NeoDetectionZone.ZoneSetting.detectorDiscriminatorVolts",
                "-2.5") };
            #endregion
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
                        ObservableCollection<TreeNode> treeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Setting);

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("treeNodes", treeNodes);
                        _dialogService.ShowDialog("SyftSettingDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                //ChartProp chartProp = _syftServer.DownloadChart(treeNode);


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

                    try
                    {
                        foreach (var item in SettingList)
                        {
                            Console.WriteLine($"Selected Item : {item.Type.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        public DelegateCommand NewCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

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

        private ObservableCollection<FilterOff> _filterOffList;
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

    }
}
