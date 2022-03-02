using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Public.BatchConfig;
using Public.ChartConfig;
using Public.SFTP;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Linq;

namespace BatchConfig.ViewModels
{
    public class BatchConfigViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly SyftServer _syftServer;
        private ChartProp SelectedChartProp;
        public BatchConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _syftServer = new SyftServer();

            ObservableCollection<ChartProp> chartProps = new ObservableCollection<ChartProp>() {
                new ChartProp(ChartType.ReferList[0],"1wqe","1asda","Upper","All",new ObservableCollection<Component>(){ }),
                new ChartProp(ChartType.ReferList[1],"2wqe","2asda","Upper","All",new ObservableCollection<Component>(){ }),
                new ChartProp(ChartType.ReferList[0],"1wqe","1asda","Upper","All",new ObservableCollection<Component>(){ })

            };
            ObservableCollection<ChartProp> chartProps1 = new ObservableCollection<ChartProp>() {
                new ChartProp(ChartType.ReferList[0],"11wqe","11asda","Upper","All",new ObservableCollection<Component>(){ }),
                new ChartProp(ChartType.ReferList[1],"22wqe","22asda","Upper","All",new ObservableCollection<Component>(){ })
            };

            Method method = new Method();
            method.MethodName = "1asdwdwda";
            method.ChartsList = chartProps;

            Method method1 = new Method();
            method1.MethodName = "2asdwdwda";
            method1.ChartsList = chartProps1;

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
        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedChart != null) SelectedMethod.ChartsList.Remove(SelectedChart);
                });
            }
        }
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedChartProp != null) SelectedMethod.ChartsList.Add(SelectedChartProp);

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
        //private ObservableCollection<ChartProp> _chartsList;
        //public ObservableCollection<ChartProp> ChartsList
        //{
        //    get => _chartsList;
        //    set => SetProperty(ref _chartsList, value);
        //}
        private ChartProp _selectedChart;
        public ChartProp SelectedChart
        {
            get => _selectedChart;
            set => SetProperty(ref _selectedChart, value);
        }
        #endregion

    }
}
