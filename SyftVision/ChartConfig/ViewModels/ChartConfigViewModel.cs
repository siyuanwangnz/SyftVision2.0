using ChartConfig.Models;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
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
        private readonly SFTPServices _sftpServices;
        private readonly string _localPath = "./Temp/ChartConfig/";
        private readonly string _localTempFile = "ChartTemp.xml";
        private readonly string _remotePath = "/home/sftp/files/syft-vision2/ChartConfig/";
        public ChartConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _sftpServices = new SFTPServices("tools.syft.com", "22", "sftp", "MuhPEzxNchfr8nyZ");
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
                        _sftpServices.Connect();
                        List<string> folders = _sftpServices.GetDirectoryList(_remotePath);
                        ObservableCollection<TreeNode> treeNodes = new ObservableCollection<TreeNode>();
                        foreach (var folder in folders)
                        {
                            if (folder == "." || folder == "..") continue;
                            // Set name
                            TreeNode treeNode = new TreeNode();
                            treeNode.Name = folder;
                            // Set child nodes
                            List<string> files = _sftpServices.GetFileList(_remotePath + folder, "xml");
                            List<TreeNode> treeChildNodes = new List<TreeNode>();
                            foreach (string file in files)
                            {
                                TreeNode treeChildNode = new TreeNode();
                                treeChildNode.Name = file;
                                treeChildNode.Parent = folder;
                                treeChildNodes.Add(treeChildNode);
                            }
                            treeNode.ChildNodes = treeChildNodes;

                            treeNodes.Add(treeNode);
                        }
                        _sftpServices.Disconnect();

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("treeNodes", treeNodes);
                        _dialogService.ShowDialog("OpenDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                // Download file
                                _sftpServices.Connect();
                                _sftpServices.DownloadFile(_remotePath + treeNode.Parent + "/" + treeNode.Name, _localPath + _localTempFile);
                                _sftpServices.Disconnect();

                                // Set toolbar and component list
                                ChartProp chartProp = new ChartProp(XElement.Load(_localPath + _localTempFile));
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
                        MessageBox.Show($"{ex.Message}", "ERROR");
                    }
                    finally
                    {
                        _sftpServices.Disconnect();
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
                        MessageBox.Show($"Chart Type, Tittle, Sub-Tittle can not be empty", "ERROR");
                        return;
                    }

                    string remoteFolderPath = _remotePath + Tittle + "/";
                    string fileName = SubTittle + ".xml";

                    // Create local file
                    if (!Directory.Exists(_localPath)) Directory.CreateDirectory(_localPath);
                    ChartProp chartProp = new ChartProp(SelectedChartType, Tittle, SubTittle, SelectedExpectedRange, SelectedPhase, ComponentsList);
                    chartProp.XMLGeneration().Save(_localPath + _localTempFile);

                    try
                    {
                        // Upload file
                        _sftpServices.Connect();
                        if (!_sftpServices.Exist(remoteFolderPath)) _sftpServices.CreateDirectory(remoteFolderPath);
                        _sftpServices.UploadFile(remoteFolderPath + fileName, _localPath + _localTempFile);
                        _sftpServices.Disconnect();
                        MessageBox.Show("Chart has been saved", "INFO");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR");
                    }
                    finally
                    {
                        _sftpServices.Disconnect();
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
                    if (SelectedChartType != null)
                    {
                        ComponentsList = new ObservableCollection<Component> { };
                        ComponentsList.Add(SelectedChartType.Component.Copy());
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
