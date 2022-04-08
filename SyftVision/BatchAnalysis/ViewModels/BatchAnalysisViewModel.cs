﻿using BatchAnalysis.Models;
using BatchAnalysis.Services;
using ChartDirector;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Public.BatchConfig;
using Public.Instrument;
using Public.SFTP;
using Public.TreeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BatchAnalysis.ViewModels
{
    public class BatchAnalysisViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly SyftServer _syftServer;
        private InstrumentServer _instrumentServer;
        private SyftDataHub SyftDataHub;
        private BatchProp SelectedBatchProp;
        private List<ScanFile> ScanFileList;
        private Task Task;
        private bool TaskIsRunning()
        {
            if (Task != null && !Task.IsCompleted)
            {
                MessageBox.Show($"In the process", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            return false;
        }
        private Action GetProgressAction(double range, int scanCount)
        {
            double step = range / scanCount;
            return new Action(() => Progress += step);
        }
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
            XYChart c = new XYChart(1100, 550, 0xccccff);

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
            XYChart c1 = new XYChart(1100, 550, 0xccccff);

            // Set the plotarea at (30, 20) and of size 200 x 200 pixels
            c1.setPlotArea(30, 20, 200, 200);

            // Add a bar chart layer using the given data
            c1.addBarLayer(data);

            // Set the labels on the x axis.
            c1.xAxis().setLabels(labels);

            SyftChart chart = new SyftChart(c);
            SyftChart chart1 = new SyftChart(c1);

            ChartList = new ObservableCollection<SyftChart>() { chart, chart1 };
            #endregion

        }

        #region Toolbar
        public DelegateCommand SelectCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

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
        private string _ipAddress = "10.0.17.199";
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
                    if (TaskIsRunning()) return;

                    if (SyftDataHub == null || SyftDataHub.scanCount == 0)
                    {
                        MessageBox.Show($"Please select a matched batch", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Task = Task.Run(() =>
                    {
                        try
                        {
                            Progress = 0;

                            // Download selected batch
                            _instrumentServer.ClearLocalScanPath();
                            foreach (var batch in SyftDataHub.SelectedBatchList)
                                _instrumentServer.DownloadScanFileList(batch.ScanList, GetProgressAction(30.0, SyftDataHub.scanCount));

                            // Get scan status list
                            ScanStatusList = new ObservableCollection<ScanStatus>(SyftDataHub.GetScanStatusList(GetProgressAction(10.0, SyftDataHub.scanCount)));

                            //Get info list
                            InfoList = new ObservableCollection<Info>(SyftDataHub.GetInfoList());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });

                });
            }
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

                    foreach (var item in SyftDataHub.MatchedBatchList)
                    {
                        Console.WriteLine(item.IsChecked);
                    }
                });
            }
        }
        // Match pop box
        private ObservableCollection<string> _matchLevelList = new ObservableCollection<string>() { "Low", "High" };
        public ObservableCollection<string> MatchLevelList
        {
            get => _matchLevelList;
            set => SetProperty(ref _matchLevelList, value);
        }
        private string _selectedMatchLevel = "High";
        public string SelectedMatchLevel
        {
            get => _selectedMatchLevel;
            set => SetProperty(ref _selectedMatchLevel, value);
        }
        private ObservableCollection<MatchedBatch> _matchedBatchList;
        public ObservableCollection<MatchedBatch> MatchedBatchList
        {
            get => _matchedBatchList;
            set => SetProperty(ref _matchedBatchList, value);
        }
        public DelegateCommand MatchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

                    _instrumentServer = new InstrumentServer(IPAddress, new Public.Global.Options());

                    try
                    {
                        if (SelectedBatchProp == null)
                        {
                            MessageBox.Show($"Please select a Batch Config", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        ScanFileList = _instrumentServer.GetScanFileList(StartDate, StartTime);

                        SyftDataHub = new SyftDataHub(SelectedBatchProp, new SyftMatch(ScanFileList, SelectedMatchLevel));

                        MatchedBatchList = new ObservableCollection<MatchedBatch>(SyftDataHub.MatchedBatchList);

                        if (MatchedBatchList.Count == 0) MessageBox.Show($"No matched batches", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        public DelegateCommand TroubleshootCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

                    if (ScanFileList == null || SelectedBatchProp == null) return;

                    // Navigate to dialog
                    DialogParameters param = new DialogParameters();
                    param.Add("sourceScanList", ScanFileList.Select(a => a.Name).ToList());
                    param.Add("referScanList", SelectedBatchProp.MethodList.Select(a => a.Name).ToList());
                    _dialogService.ShowDialog("TroubleshootDialogView", param, arg =>
                    {
                    });
                });
            }
        }
        #endregion
        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                SetProperty(ref _progress, value);
                //Send event message
                _eventAggregator.GetEvent<Public.Event.MessageEvent>().Publish(_progress);
            }
        }
        private ObservableCollection<Info> _infoList;
        public ObservableCollection<Info> InfoList
        {
            get => _infoList;
            set => SetProperty(ref _infoList, value);
        }
        private ObservableCollection<ScanStatus> _scanStatusList;
        public ObservableCollection<ScanStatus> ScanStatusList
        {
            get => _scanStatusList;
            set => SetProperty(ref _scanStatusList, value);
        }
        private ObservableCollection<SyftChart> _chartList;
        public ObservableCollection<SyftChart> ChartList
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
