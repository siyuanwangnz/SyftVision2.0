using BatchAnalysis.Models;
using ChartDirector;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Public.BatchConfig;
using Public.SFTP;
using Public.TreeList;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace BatchAnalysis.ViewModels
{
    public class BatchAnalysisViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly SyftServer _syftServer;
        private BatchProp SelectedBatchProp;
        public BatchAnalysisViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _syftServer = new SyftServer();

            #region chart list test
            // The data for the bar chart
            double[] data = { 85, 156, 179.5, 211, 123 };

            // The labels for the bar chart
            string[] labels = { "Mon", "Tue", "Wed", "Thu", "Fri" };

            // Create a XYChart object of size 250 x 250 pixels
            XYChart c = new XYChart(250, 250);

            // Set the plotarea at (30, 20) and of size 200 x 200 pixels
            c.setPlotArea(30, 20, 200, 200);

            BarLayer layer = c.addBarLayer2(Chart.Side);

            c.xAxis().setLabels(labels);

            layer.addDataSet(data, 0x5588bb, "Test");

            layer.setHTMLImageMap("", "", "title='{value} at {xLabel} (Scan: {dataSetName})'");

            // The data for the bar chart
            data = new double[] { 100, 200, 150, 250, 300 };

            // The labels for the bar chart
            labels = new string[] { "Mon1", "Tue1", "Wed1", "Thu1", "Fri1" };

            // Create a XYChart object of size 250 x 250 pixels
            XYChart c1 = new XYChart(250, 250);

            // Set the plotarea at (30, 20) and of size 200 x 200 pixels
            c1.setPlotArea(30, 20, 200, 200);

            // Add a bar chart layer using the given data
            c1.addBarLayer(data);

            // Set the labels on the x axis.
            c1.xAxis().setLabels(labels);

            ResultChart chart = new ResultChart(c);
            ResultChart chart1 = new ResultChart(c1);

            ChartList = new ObservableCollection<ResultChart>() { chart, chart1 };
            #endregion

        }

        #region Toolbar
        public DelegateCommand SelectCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        // Get tree nodes
                        ObservableCollection<TreeNode> treeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Batch);

                        // Navigate to dialog
                        DialogParameters param = new DialogParameters();
                        param.Add("treeNodes", treeNodes);
                        _dialogService.ShowDialog("SyftBatchDialogView", param, arg =>
                        {
                            if (arg.Result == ButtonResult.OK)
                            {
                                TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                SelectedBatchProp = _syftServer.DownloadBatch(treeNode);

                                Tittle = SelectedBatchProp.Tittle;
                                SubTittle = SelectedBatchProp.SubTittle;
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
        private DateTime _startDate = DateTime.Now.AddDays(-1);
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }
        private DateTime _startTime = new DateTime(2021, 07, 26, 0, 0, 0);
        public DateTime StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }
        private string _ipAddress = "10.0.16.209";
        public string IPAddress
        {
            get => _ipAddress;
            set => SetProperty(ref _ipAddress, value);
        }
        public DelegateCommand ProcessCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
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
        // Match pop box
        private ObservableCollection<int> _matchLevel = new ObservableCollection<int>() { 0, 1, 2 };
        public ObservableCollection<int> MatchLevel
        {
            get => _matchLevel;
            set => SetProperty(ref _matchLevel, value);
        }
        private int _selectedMatchLevel = 2;
        public int SelectedMatchLevel
        {
            get => _selectedMatchLevel;
            set => SetProperty(ref _selectedMatchLevel, value);
        }
        public DelegateCommand MatchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                });
            }
        }
        #endregion

        private ObservableCollection<ResultChart> _chartList;
        public ObservableCollection<ResultChart> ChartList
        {
            get => _chartList;
            set => SetProperty(ref _chartList, value);
        }

        #region Attached Property
        public static BaseChart GetAttachedChart(DependencyObject obj)
        {
            return (BaseChart)obj.GetValue(AttachedChartProperty);
        }
        public static void SetAttachedChart(DependencyObject obj, BaseChart value)
        {
            obj.SetValue(AttachedChartProperty, value);
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachedChartProperty =
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(BatchAnalysisViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) =>
            {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion

    }
}
