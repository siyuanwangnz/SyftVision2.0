using BatchAnalysis.Models;
using BatchAnalysis.Services;
using ChartDirector;
using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

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
            //// The data for the bar chart
            //double[] data = { 85, 156, 179.5, 211, 123 };
            //double[] data1 = { 35, 126, 79.5, 111, 213 };
            //double[] datab = { 15, 46, 29.5, 51, 93 };
            //double[] x = { 10, 20, 30, 40, 50 };
            //double[] x1 = { 5, 15, 25, 35, 45 };
            //double[] y1 = { 12000000, 11000000, 9000000, 7000000, 13000000, 12000000, 11000000, 9000000, 7000000, 13000000, 12000000, 11000000, 9000000, 7000000, 13000000 };
            //double[] y2 = { 10000000, 12000000, 7000000, 13000000, 11000000, 10000000, 12000000, 7000000, 13000000, 11000000, 10000000, 12000000, 7000000, 13000000, 11000000 };
            //// The labels for the bar chart
            //List<string> l = new List<string>();
            //for (int i = 0; i < 60; i++)
            //{
            //    l.Add($"{i}qweasdz-123456");
            //}
            //string[] labels = l.ToArray();

            //XYChart c = new XYChart(1116, 520);

            //// Set chart position, size and style
            //c.setPlotArea(80, 30, c.getWidth() - 110, c.getHeight() - 100, -1, -1, -1, 0x40dddddd, 0x40dddddd);

            //// Add a title
            //c.addTitle(Chart.TopCenter, "SKDNKSND-SDJJSDSN(sdsd#sdjsahd)", "Arial Bold", 16);

            //// Set the x and y axis stems and the label font
            //c.xAxis().setColors(Chart.Transparent);
            //c.yAxis().setColors(Chart.Transparent);
            //c.xAxis().setLabelStyle("Arial", 10);
            //c.yAxis().setLabelStyle("Arial", 10);

            //// Add axis number format
            //c.setNumberFormat(',');

            //// Add layers

            //LineLayer layer = c.addLineLayer2();
            //layer.setLineWidth(1);
            //layer.setFastLineMode();
            //layer.addDataSet(y1, 0xee9944, $"ewerwerwerewrewr");
            //layer.setHTMLImageMap("", "", "title='{value} cps ({dataSetName}) at {xLabel}'");

            //c.xAxis().setLabels(labels);
            //c.xAxis().setLabelStyle("Arial", 8, Chart.TextColor, 30);

            //LineLayer layer1 = c.addLineLayer2();
            //layer1.setLineWidth(1);
            //layer1.setFastLineMode();
            //layer1.addDataSet(y2, 0x449944, $"1231313werewrewr");
            //layer.setHTMLImageMap("", "", "title='{value} cps ({dataSetName}) at {xLabel}'");

            //// Set step of labels
            //int count = (int)Math.Round((double)labels.Count()/ 40);
            //c.xAxis().setLabelStep(count);

            //SyftChart chart = new SyftChart(c);

            //SyftChartList = new ObservableCollection<SyftChart>() { chart };
            #endregion

        }

        #region Toolbar
        public DelegateCommand DownloadCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

                    //Open folder path selection dialog
                    CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
                    folderDlg.IsFolderPicker = true;
                    folderDlg.Title = "Select a Target Folder to Download Batch Config Files";
                    if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        try
                        {
                            _syftServer.DownloadBatchConfigFolder(folderDlg.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        }
        private bool _localBatchSelectIsChecked;
        public bool LocalBatchSelectIsChecked
        {
            get => _localBatchSelectIsChecked;
            set => SetProperty(ref _localBatchSelectIsChecked, value);
        }
        public DelegateCommand SelectCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

                    try
                    {
                        if (LocalBatchSelectIsChecked) // Local Selection
                        {
                            //Open file path selection dialog
                            CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
                            folderDlg.Title = "Select a Local Batch Config File";
                            if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                SelectedBatchProp = new BatchProp(XElement.Load(folderDlg.FileName));

                                Tittle = SelectedBatchProp.Tittle;
                                SubTittle = SelectedBatchProp.SubTittle;
                            }
                        }
                        else // Remote Selection
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
        private string _ipAddress = "10.0.17.";
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

                    if (SyftDataHub == null || SyftDataHub.ScanCount == 0)
                    {
                        MessageBox.Show($"Please select a matched batch", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Task = Task.Run(() =>
                    {
                        try
                        {
                            Progress = 0;

                            if (LocalMatchIsChecked) Progress = 30.0;
                            else
                            {
                                // Download selected batch
                                _instrumentServer.ClearLocalScanPath();
                                foreach (var batch in SyftDataHub.SelectedBatchList)
                                    _instrumentServer.DownloadScanFileList(batch.ScanList, GetProgressAction(30.0, SyftDataHub.ScanCount));
                            }

                            // Get scan status list
                            SyftScanList = new ObservableCollection<SyftScan>(SyftDataHub.GetSyftScanList(GetProgressAction(10.0, SyftDataHub.ScanCount)));

                            // Get info list
                            SyftInfoList = new ObservableCollection<SyftInfo>(SyftDataHub.GetSyftInfoList());

                            // Get syft chart list
                            SyftChartList = new ObservableCollection<SyftChart>(SyftDataHub.GetSyftChartList(GetProgressAction(60.0, SyftDataHub.BatchProp.ChartPropList.Count)));

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });

                });
            }
        }
        private bool _scanFilesCollectionIsChecked;
        public bool ScanFilesCollectionIsChecked
        {
            get => _scanFilesCollectionIsChecked;
            set => SetProperty(ref _scanFilesCollectionIsChecked, value);
        }
        private string _comments = "";
        public string Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

                    if (SyftDataHub == null || !SyftDataHub.IsAvailable) return;

                    //Open folder path selection dialog
                    CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
                    folderDlg.IsFolderPicker = true;
                    folderDlg.Title = "Select a Target Folder to Save";
                    if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        try
                        {
                            if (ScanFilesCollectionIsChecked) InstrumentServer.CopyScanFile(folderDlg.FileName);

                            new SyftPDF(SyftDataHub, Comments, folderDlg.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


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
        private bool _localMatchIsChecked;
        public bool LocalMatchIsChecked
        {
            get => _localMatchIsChecked;
            set => SetProperty(ref _localMatchIsChecked, value);
        }
        public DelegateCommand MatchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

                    if (SelectedBatchProp == null)
                    {
                        MessageBox.Show($"Please select a Batch Config", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    try
                    {
                        if (LocalMatchIsChecked) // Local Match
                        {
                            //Open folder path selection dialog
                            CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
                            folderDlg.IsFolderPicker = true;
                            folderDlg.Title = "Select a Target Folder to Match";
                            if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                ScanFileList = new List<ScanFile>();

                                DirectoryInfo folder = new DirectoryInfo(folderDlg.FileName);
                                foreach (var file in folder.GetFiles("*.xml", SearchOption.AllDirectories))
                                {
                                    ScanFile scanFile = new ScanFile(file.Name);
                                    scanFile.FullLocalFolder = file.Directory.FullName;
                                    ScanFileList.Add(scanFile);
                                }

                                ScanFileList.Sort((a, b) => a.Date_Time.CompareTo(b.Date_Time) == 0 ? a.ID.CompareTo(b.ID) : a.Date_Time.CompareTo(b.Date_Time));
                            }
                            else
                                return;
                        }
                        else // Remote Match
                        {
                            _instrumentServer = new InstrumentServer(IPAddress, new Public.Global.Options());

                            ScanFileList = _instrumentServer.GetScanFileList(StartDate, StartTime);
                        }

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
        private ObservableCollection<SyftInfo> _syftInfoList;
        public ObservableCollection<SyftInfo> SyftInfoList
        {
            get => _syftInfoList;
            set => SetProperty(ref _syftInfoList, value);
        }
        private ObservableCollection<SyftScan> _syftScanList;
        public ObservableCollection<SyftScan> SyftScanList
        {
            get => _syftScanList;
            set => SetProperty(ref _syftScanList, value);
        }
        private ObservableCollection<SyftChart> _syftChartList;
        public ObservableCollection<SyftChart> SyftChartList
        {
            get => _syftChartList;
            set => SetProperty(ref _syftChartList, value);
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
