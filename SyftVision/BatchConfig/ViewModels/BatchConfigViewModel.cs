using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Public.BatchConfig;
using Public.ChartConfig;
using Public.SFTP;
using Public.TreeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace BatchConfig.ViewModels
{
    public class BatchConfigViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly SyftServer _syftServer;
        private InstrumentServer _instrumentServer;
        private ChartProp SelectedChartProp;
        public BatchConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _syftServer = new SyftServer();
        }
        #region Chart Config
        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        TreeNodes = new ObservableCollection<TreeNode>(_syftServer.GetTreeNodes(SyftServer.Type.Chart));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                });
            }
        }
        public DelegateCommand SelectedCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedTreeNode.Parent == null) return;
                    try
                    {
                        SelectedChartProp = _syftServer.DownloadChart(SelectedTreeNode);

                        ChartTittle = SelectedChartProp.Tittle;
                        ChartSubTittle = SelectedChartProp.SubTittle;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        private string _chartTittle;
        public string ChartTittle
        {
            get => _chartTittle;
            set => SetProperty(ref _chartTittle, value);
        }
        private string _chartSubTittle;
        public string ChartSubTittle
        {
            get => _chartSubTittle;
            set => SetProperty(ref _chartSubTittle, value);
        }
        private ObservableCollection<TreeNode> _treeNodes;
        public ObservableCollection<TreeNode> TreeNodes
        {
            get => _treeNodes;
            set => SetProperty(ref _treeNodes, value);
        }
        private TreeNode _selectedTreeNode;
        public TreeNode SelectedTreeNode
        {
            get => _selectedTreeNode;
            set => SetProperty(ref _selectedTreeNode, value);
        }
        #endregion

        #region Batch Config
        // Toolbar
        public DelegateCommand OpenCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        // Get tree nodes
                        List<TreeNode> treeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Batch);

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("treeNodes", treeNodes);
                        _dialogService.ShowDialog("SyftBatchDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                BatchProp batchProp = _syftServer.DownloadBatch(treeNode);

                                BatchTittle = batchProp.Tittle;
                                BatchSubTittle = batchProp.SubTittle;
                                MethodList = new ObservableCollection<Method>(batchProp.MethodList);
                                ChartPropList = new ObservableCollection<ChartProp>(batchProp.ChartPropList);
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
                    if (BatchTittle == null || BatchTittle == "" || BatchSubTittle == null || BatchSubTittle == "")
                    {
                        MessageBox.Show($"Tittle, Sub-Tittle can not be empty", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    BatchProp batchProp = new BatchProp(BatchTittle, BatchSubTittle, MethodList.ToList(), ChartPropList.ToList());

                    try
                    {
                        _syftServer.UploadBatch(batchProp);
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
                    _instrumentServer = new InstrumentServer(IPAddress, new Public.Global.Options());

                    try
                    {
                        // Get batch file list
                        List<string> batchFileList = _instrumentServer.GetBatchFileList();

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("batchFileList", batchFileList);
                        _dialogService.ShowDialog("InstruBatchDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                string selectedBatchFile = arg.Parameters.GetValue<string>("selectedBatchFile");
                                MethodList = new ObservableCollection<Method>(_instrumentServer.GetMethodListFromBatchFile(selectedBatchFile));
                                ChartPropList.Clear();
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
        private string _ipAddress = "10.0.17.";
        public string IPAddress
        {
            get => _ipAddress;
            set => SetProperty(ref _ipAddress, value);
        }
        private string _batchTittle;
        public string BatchTittle
        {
            get => _batchTittle;
            set => SetProperty(ref _batchTittle, value);
        }
        private string _batchSubTittle;
        public string BatchSubTittle
        {
            get => _batchSubTittle;
            set => SetProperty(ref _batchSubTittle, value);
        }
        // Methods list
        public DelegateCommand MethodDeleteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    List<string> chartCodeList = SelectedMethod.ChartCodeList.ToList();

                    MethodList.Remove(SelectedMethod);

                    if (MethodList.Count == 0)
                    {
                        MethodList.Add(new Method());
                        ChartPropList.Clear();
                        return;
                    }

                    // Get non-repetitive chart code list
                    List<string> chartCodeFullList = new List<string>();
                    foreach (var method in MethodList) chartCodeFullList.AddRange(method.ChartCodeList);
                    chartCodeFullList = new List<string>(new HashSet<string>(chartCodeFullList));

                    foreach (var chartCode in chartCodeList)
                    {
                        if (chartCodeFullList.Contains(chartCode)) continue;
                        if (ChartPropList.Select(a => a.Code).ToList().Contains(chartCode))
                            ChartPropList.Remove(ChartPropList.Single(a => a.Code == chartCode));
                    }

                });
            }
        }
        public DelegateCommand MethodAddUpCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MethodList.Insert(MethodList.IndexOf(SelectedMethod), new Method());
                });
            }
        }
        public DelegateCommand MethodAddDownCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MethodList.Insert(MethodList.IndexOf(SelectedMethod) + 1, new Method());
                });
            }
        }
        public DelegateCommand ChartDeleteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedChartCode != null)
                    {
                        string selectedChartCode = SelectedChartCode;

                        SelectedMethod.ChartCodeList.Remove(selectedChartCode);

                        // Get non-repetitive chart code list
                        List<string> chartCodeFullList = new List<string>();
                        foreach (var method in MethodList) chartCodeFullList.AddRange(method.ChartCodeList);
                        chartCodeFullList = new List<string>(new HashSet<string>(chartCodeFullList));

                        if (!chartCodeFullList.Contains(selectedChartCode) && ChartPropList.Select(a => a.Code).ToList().Contains(selectedChartCode))
                            ChartPropList.Remove(ChartPropList.Single(a => a.Code == selectedChartCode));
                    }
                });
            }
        }
        public DelegateCommand ChartAddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedChartProp != null && !SelectedMethod.ChartCodeList.Contains(SelectedChartProp.Code))
                    {
                        SelectedMethod.ChartCodeList.Add(SelectedChartProp.Code);
                        if (!ChartPropList.Contains(SelectedChartProp)) ChartPropList.Add(SelectedChartProp);
                    }
                });
            }
        }
        private ObservableCollection<Method> _methodList = new ObservableCollection<Method>() { new Method() };
        public ObservableCollection<Method> MethodList
        {
            get => _methodList;
            set => SetProperty(ref _methodList, value);
        }
        private Method _selectedMethod;
        public Method SelectedMethod
        {
            get => _selectedMethod;
            set => SetProperty(ref _selectedMethod, value);
        }
        private string _selectedChartCode;
        public string SelectedChartCode
        {
            get => _selectedChartCode;
            set => SetProperty(ref _selectedChartCode, value);
        }
        private ObservableCollection<ChartProp> _chartPropList = new ObservableCollection<ChartProp>() { };
        public ObservableCollection<ChartProp> ChartPropList
        {
            get => _chartPropList;
            set => SetProperty(ref _chartPropList, value);
        }
        #endregion

    }
}
