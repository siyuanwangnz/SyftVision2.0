using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Public.ChartConfig;
using Public.SFTP;
using Public.TreeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace ChartConfig.ViewModels
{
    public class ChartConfigViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly SyftServer _syftServer;
        public ChartConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
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
                        List<TreeNode> treeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Chart);

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("treeNodes", treeNodes);
                        _dialogService.ShowDialog("SyftChartDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                ChartProp chartProp = _syftServer.DownloadChart(treeNode);

                                SelectedChartType = chartProp.ChartType;
                                Tittle = chartProp.Tittle;
                                SubTittle = chartProp.SubTittle;
                                SelectedExpectedRange = chartProp.ExpectedRange;
                                SelectedPhase = chartProp.Phase;
                                ComponentList = new ObservableCollection<Component>(chartProp.ComponentList);
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
                    if (SelectedChartType == null || Tittle == null || Tittle == "" || SubTittle == null || SubTittle == "")
                    {
                        MessageBox.Show($"Chart Type, Tittle, Sub-Tittle can not be empty", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    ChartProp chartProp = new ChartProp(SelectedChartType, Tittle, SubTittle, SelectedExpectedRange, SelectedPhase, ComponentList.ToList());

                    try
                    {
                        _syftServer.UploadChart(chartProp);
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
                    if (SelectedChartType == null)
                    {
                        MessageBox.Show($"Chart Type can not be empty", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    ComponentList = new ObservableCollection<Component> { };
                    ComponentList.Add(SelectedChartType.Component.Copy());
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
        private ObservableCollection<ChartType> _chartTypeList = new ObservableCollection<ChartType>(ChartType.ReferList);
        public ObservableCollection<ChartType> ChartTypeList
        {
            get => _chartTypeList;
            set => SetProperty(ref _chartTypeList, value);
        }
        private ChartType _selectedChartType;
        public ChartType SelectedChartType
        {
            get => _selectedChartType;
            set => SetProperty(ref _selectedChartType, value);
        }
        private ObservableCollection<string> _expectedRangeList = new ObservableCollection<string>() { "Upper Limit", "Under Limit" };
        public ObservableCollection<string> ExpectedRangeList
        {
            get => _expectedRangeList;
            set => SetProperty(ref _expectedRangeList, value);
        }
        private string _selectedExpectedRange = "Upper Limit";
        public string SelectedExpectedRange
        {
            get => _selectedExpectedRange;
            set => SetProperty(ref _selectedExpectedRange, value);
        }
        private ObservableCollection<string> _phaseList = new ObservableCollection<string>() { "All", "Preparation", "Background", "Sample" };
        public ObservableCollection<string> PhaseList
        {
            get => _phaseList;
            set => SetProperty(ref _phaseList, value);
        }

        private string _selectedPhase = "All";
        public string SelectedPhase
        {
            get => _selectedPhase;
            set => SetProperty(ref _selectedPhase, value);
        }
        #endregion

        #region Component list
        private ObservableCollection<Component> _componentList = new ObservableCollection<Component>();
        public ObservableCollection<Component> ComponentList
        {
            get => _componentList;
            set => SetProperty(ref _componentList, value);
        }
        private Component _selectedComponent;
        public Component SelectedComponent
        {
            get => _selectedComponent;
            set => SetProperty(ref _selectedComponent, value);
        }
        public DelegateCommand AddUpCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentList.Insert(ComponentList.IndexOf(SelectedComponent), SelectedChartType.Component.Copy());
                });
            }
        }
        public DelegateCommand AddDownCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentList.Insert(ComponentList.IndexOf(SelectedComponent) + 1, SelectedChartType.Component.Copy());
                });
            }
        }
        #endregion

    }
}
