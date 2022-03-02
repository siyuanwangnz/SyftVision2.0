﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Public.ChartConfig;
using Public.SFTP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        #region ToolBar
        public DelegateCommand OpenCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        // Get tree nodes
                        ObservableCollection<TreeNode> treeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Chart);

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("treeNodes", treeNodes);
                        _dialogService.ShowDialog("OpenDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                // Download file
                                _syftServer.Connect();
                                _syftServer.DownloadFile(_syftServer.RemoteChartPath + treeNode.Parent + "/" + treeNode.Name, _syftServer.LocalChartPath + _syftServer.LocalChartTempFile);
                                _syftServer.Disconnect();

                                // Set toolbar and component list
                                ChartProp chartProp = new ChartProp(XElement.Load(_syftServer.LocalChartPath + _syftServer.LocalChartTempFile));
                                SelectedChartType = chartProp.ChartType;
                                Tittle = chartProp.Tittle;
                                SubTittle = chartProp.SubTittle;
                                SelectedExpectedRange = chartProp.ExpectedRange;
                                SelectedPhase = chartProp.Phase;
                                ComponentsList = chartProp.ComponentsList;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        _syftServer.Disconnect();
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

                    string remoteFolderPath = _syftServer.RemoteChartPath + Tittle + "/";
                    string fileName = SubTittle + ".xml";

                    // Create local file
                    if (!Directory.Exists(_syftServer.LocalChartPath)) Directory.CreateDirectory(_syftServer.LocalChartPath);
                    ChartProp chartProp = new ChartProp(SelectedChartType, Tittle, SubTittle, SelectedExpectedRange, SelectedPhase, ComponentsList);
                    chartProp.XMLGeneration().Save(_syftServer.LocalChartPath + _syftServer.LocalChartTempFile);

                    try
                    {
                        _syftServer.Connect();
                        // Check existing chart config
                        if (_syftServer.Exist(remoteFolderPath + fileName))
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show($"The file already exists, do you want to replace it?", "QUESTION", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (messageBoxResult == MessageBoxResult.No) return;
                        }

                        // Upload file
                        if (!_syftServer.Exist(remoteFolderPath)) _syftServer.CreateDirectory(remoteFolderPath);
                        _syftServer.UploadFile(remoteFolderPath + fileName, _syftServer.LocalChartPath + _syftServer.LocalChartTempFile);
                        _syftServer.Disconnect();
                        MessageBox.Show("Chart has been saved", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        _syftServer.Disconnect();
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
                    ComponentsList = new ObservableCollection<Component> { };
                    ComponentsList.Add(SelectedChartType.Component.Copy());
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
        private ObservableCollection<ChartType> _chartTypeList = ChartType.ReferList;
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

        #region Components List
        private ObservableCollection<Component> _componentsList;
        public ObservableCollection<Component> ComponentsList
        {
            get => _componentsList;
            set => SetProperty(ref _componentsList, value);
        }
        private Component _selectedComponent;
        public Component SelectedComponent
        {
            get => _selectedComponent;
            set => SetProperty(ref _selectedComponent, value);
        }
        public DelegateCommand AddUp
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentsList.Insert(ComponentsList.IndexOf(SelectedComponent), SelectedChartType.Component.Copy());
                });
            }
        }
        public DelegateCommand AddDown
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentsList.Insert(ComponentsList.IndexOf(SelectedComponent) + 1, SelectedChartType.Component.Copy());
                });
            }
        }
        #endregion

    }
}
