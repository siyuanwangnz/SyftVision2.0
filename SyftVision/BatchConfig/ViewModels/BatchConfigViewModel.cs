using BatchConfig.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Public.BatchConfig;
using Public.ChartConfig;
using Public.SFTP;
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
        private ObservableCollection<ChartProp> ChartPropList = new ObservableCollection<ChartProp>();
        public BatchConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _syftServer = new SyftServer();

            ObservableCollection<string> chartFullNameList = new ObservableCollection<string>() {
               "11sdaw|dwawda","22dqwdd|dwwidjwj","33dwdad|dwdada"
            };
            ObservableCollection<string> chartFullNameList1 = new ObservableCollection<string>() {
               "c11sdaw|dwawda","c22dqwdd|dwwidjwj","c33dwdad|dwdada"
            };

            Method method = new Method();
            method.MethodName = "1asdwdwda";
            method.ChartCodeList = chartFullNameList;

            Method method1 = new Method();
            method1.MethodName = "2asdwdwda";
            method1.ChartCodeList = chartFullNameList1;

            MethodsList = new ObservableCollection<Method>() { };
            MethodsList.Add(method);
            MethodsList.Add(method1);


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
                        TreeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Chart);
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
                        _syftServer.Connect();
                        _syftServer.DownloadFile(_syftServer.RemoteChartPath + SelectedTreeNode.Parent + "/" + SelectedTreeNode.Name, _syftServer.LocalChartPath + _syftServer.LocalChartTempFile);
                        _syftServer.Disconnect();
                        SelectedChartProp = new ChartProp(XElement.Load(_syftServer.LocalChartPath + _syftServer.LocalChartTempFile));
                        ChartTittle = SelectedChartProp.Tittle;
                        ChartSubTittle = SelectedChartProp.SubTittle;
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
                    foreach (var chartProp in ChartPropList)
                    {
                        Console.WriteLine(chartProp.Code);
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
                        // Get batches list
                        ObservableCollection<string> batchesNameList = _instrumentServer.GetBatchesList();
                        ObservableCollection<Batch> batchesList = new ObservableCollection<Batch>() { };
                        foreach (var batchName in batchesNameList) batchesList.Add(new Batch(batchName));

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("batchesList", batchesList);
                        _dialogService.ShowDialog("InstruBatchDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                Batch batch = arg.Parameters.GetValue<Batch>("selectedBatch");
                                Console.WriteLine(batch.Name);

                                //// Download file
                                //_syftServer.Connect();
                                //_syftServer.DownloadFile(_syftServer.RemoteChartPath + treeNode.Parent + "/" + treeNode.Name, _syftServer.LocalChartPath + _syftServer.LocalChartTempFile);
                                //_syftServer.Disconnect();

                                //// Set toolbar and component list
                                //ChartProp chartProp = new ChartProp(XElement.Load(_syftServer.LocalChartPath + _syftServer.LocalChartTempFile));
                                //SelectedChartType = chartProp.ChartType;
                                //Tittle = chartProp.Tittle;
                                //SubTittle = chartProp.SubTittle;
                                //SelectedExpectedRange = chartProp.ExpectedRange;
                                //SelectedPhase = chartProp.Phase;
                                //ComponentsList = chartProp.ComponentsList;
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
        private string _ipAddress = "10.0.16.209";
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
        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedChartCode != null)
                    {
                        string selectedChartCode = SelectedChartCode;
                        SelectedMethod.ChartCodeList.Remove(selectedChartCode);
                        foreach (var method in MethodsList)
                        {
                            if (method.ChartCodeList.Contains(selectedChartCode)) return;
                        }
                        if (ChartPropList.Select(a => a.Code).ToList().Contains(selectedChartCode))
                            ChartPropList.Remove(ChartPropList.Single(a => a.Code == selectedChartCode));
                    }
                });
            }
        }
        public DelegateCommand AddCommand
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
        private ObservableCollection<Method> _methodsList;
        public ObservableCollection<Method> MethodsList
        {
            get => _methodsList;
            set => SetProperty(ref _methodsList, value);
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
        #endregion

    }
}
