using ChartDirector;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Public.Global;
using Public.SFTP;
using SettingCheck.Models;
using SettingCheck.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SettingCheck.ViewModels
{
    public class SettingCheckViewModel : BindableBase
    {
        public SettingCheckViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            //Button Command
            OpenSourceCommand = new DelegateCommand(OpenSource);
            OpenPosWetCommand = new DelegateCommand(OpenPosWet);
            OpenNegWetCommand = new DelegateCommand(OpenNegWet);
            OpenNegDryCommand = new DelegateCommand(OpenNegDry);
            OpenDWSCommand = new DelegateCommand(OpenDWS);
            OpenDWSSpecificCommand = new DelegateCommand(OpenDWSSpecific);
            OpenDetectionCommand = new DelegateCommand(OpenDetection);
            OpenICFCommand = new DelegateCommand(OpenICF);
            ProcessCommand = new DelegateCommand(Process);
            SaveCommand = new DelegateCommand(Save);
            CompareOpenCommand = new DelegateCommand(CompareOpen);
            CompareAddCommand = new DelegateCommand(CompareAdd);
        }

        #region Field
        private string LatestScanFileName = "";

        private int ColorNumber = 0;

        private Dictionary<string, List<Setting>> savedSourceSettingsPSDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedSourceSettingsMVDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedSourceSettingsMESHDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedPosWetPhaseSettingsDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedNegWetPhaseSettingsDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedNegDryPhaseSettingsDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedDWSSettingsDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedDWSSpecificSettingsPFDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedDWSSpecificSettingsABDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedDWSSpecificSettingsLens5Dic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedDetectionSettingsDVDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedDetectionSettingsDSDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, List<Setting>> savedDetectionSettingsSTDic = new Dictionary<string, List<Setting>>();
        private Dictionary<string, SyftXML.Scan> savedICFDic = new Dictionary<string, SyftXML.Scan>();
        #endregion

        #region Property
        public Global.InstrumentType InstrumentType
        {
            get
            {
                if (SPISSelected) return Global.InstrumentType.SPIS;
                else if (DPISSelected) return Global.InstrumentType.DPIS;
                else if (InfinitySelected) return Global.InstrumentType.Infinity;
                else return Global.InstrumentType.Infinity;
            }
        }

        private SourceSettings sourceSettings { get; set; }
        private UPSPhaseSettings posWetPhaseSettings { get; set; }
        private UPSPhaseSettings negWetPhaseSettings { get; set; }
        private UPSPhaseSettings negDryPhaseSettings { get; set; }
        private DWSSettings dwsSettings { get; set; }
        private DWSSpecificSettings dwsSpecificSettings { get; set; }
        private DetectionSettings detectionSettings { get; set; }
        #endregion

        #region Region Navigation
        private readonly IRegionManager _regionManager;

        public DelegateCommand OpenSourceCommand { get; private set; }
        public DelegateCommand OpenPosWetCommand { get; private set; }
        public DelegateCommand OpenNegWetCommand { get; private set; }
        public DelegateCommand OpenNegDryCommand { get; private set; }
        public DelegateCommand OpenDWSCommand { get; private set; }
        public DelegateCommand OpenDWSSpecificCommand { get; private set; }
        public DelegateCommand OpenDetectionCommand { get; private set; }
        public DelegateCommand OpenICFCommand { get; private set; }

        private void OpenSource()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("InstrumentType", InstrumentType);
            param.Add("SourceChartPS", SourceChartPS);
            param.Add("SourceChartMV", SourceChartMV);
            param.Add("SourceChartMESH", SourceChartMESH);
            _regionManager.RequestNavigate("SettingCheckContentRegion", "SourceView", param);
        }
        private void OpenPosWet()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("PosWetChart", PosWetChart);
            _regionManager.RequestNavigate("SettingCheckContentRegion", "PosWetView", param);
        }
        private void OpenNegWet()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("NegWetChart", NegWetChart);
            _regionManager.RequestNavigate("SettingCheckContentRegion", "NegWetView", param);
        }
        private void OpenNegDry()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("NegDryChart", NegDryChart);
            _regionManager.RequestNavigate("SettingCheckContentRegion", "NegDryView", param);
        }
        private void OpenDWS()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("DWSChart", DWSChart);
            _regionManager.RequestNavigate("SettingCheckContentRegion", "DWSView", param);
        }
        private void OpenDWSSpecific()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("DWSSpecificChartPF", DWSSpecificChartPF);
            param.Add("DWSSpecificChartAB", DWSSpecificChartAB);
            param.Add("DWSSpecificChartLens5", DWSSpecificChartLens5);
            _regionManager.RequestNavigate("SettingCheckContentRegion", "DWSSpecificView", param);
        }
        private void OpenDetection()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("DetectionChartDV", DetectionChartDV);
            param.Add("DetectionChartDS", DetectionChartDS);
            param.Add("DetectionChartST", DetectionChartST);
            _regionManager.RequestNavigate("SettingCheckContentRegion", "DetectionView", param);
        }
        private void OpenICF()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("ICFChart", ICFChart);
            _regionManager.RequestNavigate("SettingCheckContentRegion", "SICFView", param);
        }
        #endregion

        #region Binding Property
        private string _ipadress = "10.0.17.";
        public string IPAdress
        {
            get { return _ipadress; }
            set { SetProperty(ref _ipadress, value); }
        }
        //Instrument type
        private bool _spisselected;
        public bool SPISSelected
        {
            get { return _spisselected; }
            set { SetProperty(ref _spisselected, value); }
        }

        private bool _dpisselected;
        public bool DPISSelected
        {
            get { return _dpisselected; }
            set { SetProperty(ref _dpisselected, value); }
        }

        private bool _infinityselected = true;
        public bool InfinitySelected
        {
            get { return _infinityselected; }
            set { SetProperty(ref _infinityselected, value); }
        }
        //Process button
        private double _processbuttonprogress;
        public double ProcessButtonProgress
        {
            get { return _processbuttonprogress; }
            set { _processbuttonprogress = value; RaisePropertyChanged(); /*SetProperty(ref _processbuttonprogress, value);*/ }
        }

        private bool _showprocessbuttonprocess = false;
        public bool ShowProcessButtonProcess
        {
            get => _showprocessbuttonprocess;
            set => SetProperty(ref _showprocessbuttonprocess, value);
        }
        //Backup Button
        private bool _backupIsProcessing = false;
        public bool BackupIsProcessing
        {
            get => _backupIsProcessing;
            set => SetProperty(ref _backupIsProcessing, value);
        }
        private bool _backupIsFinish = false;
        public bool BackupIsFinish
        {
            get => _backupIsFinish;
            set => SetProperty(ref _backupIsFinish, value);
        }
        //Save button
        private double _savebuttonprogress;
        public double SaveButtonProgress
        {
            get { return _savebuttonprogress; }
            set { _savebuttonprogress = value; RaisePropertyChanged(); /*SetProperty(ref _processbuttonprogress, value);*/ }
        }

        private bool _showsavebuttonprocess = false;
        public bool ShowSaveButtonProcess
        {
            get => _showsavebuttonprocess;
            set => SetProperty(ref _showsavebuttonprocess, value);
        }
        //Compare open button
        private double _compareopenbuttonprogress;
        public double CompareOpenButtonProgress
        {
            get { return _compareopenbuttonprogress; }
            set { _compareopenbuttonprogress = value; RaisePropertyChanged(); /*SetProperty(ref _processbuttonprogress, value);*/ }
        }

        private bool _showcompareopenbuttonprocess = false;
        public bool ShowCompareOpenButtonProcess
        {
            get => _showcompareopenbuttonprocess;
            set => SetProperty(ref _showcompareopenbuttonprocess, value);
        }
        //Compare add button
        private double _compareaddbuttonprogress;
        public double CompareAddButtonProgress
        {
            get { return _compareaddbuttonprogress; }
            set { _compareaddbuttonprogress = value; RaisePropertyChanged(); /*SetProperty(ref _processbuttonprogress, value);*/ }
        }

        private bool _showcompareaddbuttonprocess = false;
        public bool ShowCompareAddButtonProcess
        {
            get => _showcompareaddbuttonprocess;
            set => SetProperty(ref _showcompareaddbuttonprocess, value);
        }
        //Instrument show number
        private string _instrumentShowNumebr;
        public string InstrumentShowNumebr
        {
            get => _instrumentShowNumebr;
            set => SetProperty(ref _instrumentShowNumebr, value);
        }
        //Process Time Stemp
        private string _processTimeStemp;
        public string ProcessTimeStemp
        {
            get => _processTimeStemp;
            set => SetProperty(ref _processTimeStemp, value);
        }
        #endregion

        #region Chart
        public BaseChart SourceChartPS { get; set; }
        public BaseChart SourceChartMV { get; set; }
        public BaseChart SourceChartMESH { get; set; }
        public BaseChart PosWetChart { get; set; }
        public BaseChart NegWetChart { get; set; }
        public BaseChart NegDryChart { get; set; }
        public BaseChart DWSChart { get; set; }
        public BaseChart DWSSpecificChartPF { get; set; }
        public BaseChart DWSSpecificChartAB { get; set; }
        public BaseChart DWSSpecificChartLens5 { get; set; }
        public BaseChart DetectionChartDV { get; set; }
        public BaseChart DetectionChartDS { get; set; }
        public BaseChart DetectionChartST { get; set; }
        public BaseChart ICFChart { get; set; }
        #endregion

        #region Process Command
        public DelegateCommand ProcessCommand { get; private set; }

        private void Process()
        {
            Task.Run(() =>
            {
                try
                {
                    ShowProcessButtonProcess = true;

                    XElement LatestScanRootNood = GetRootNode.LatestScan(IPAdress, out LatestScanFileName);
                    XElement ConfigRootNood = GetRootNode.Config();

                    savedSourceSettingsPSDic.Clear();
                    savedSourceSettingsMVDic.Clear();
                    savedSourceSettingsMESHDic.Clear();
                    savedPosWetPhaseSettingsDic.Clear();
                    savedNegWetPhaseSettingsDic.Clear();
                    savedNegDryPhaseSettingsDic.Clear();
                    savedDWSSettingsDic.Clear();
                    savedDWSSpecificSettingsPFDic.Clear();
                    savedDWSSpecificSettingsABDic.Clear();
                    savedDWSSpecificSettingsLens5Dic.Clear();
                    savedDetectionSettingsDVDic.Clear();
                    savedDetectionSettingsDSDic.Clear();
                    savedDetectionSettingsSTDic.Clear();
                    savedICFDic.Clear();

                    SourceChartPS = null;
                    SourceChartMV = null;
                    SourceChartMESH = null;
                    PosWetChart = null;
                    NegWetChart = null;
                    NegDryChart = null;
                    DWSChart = null;
                    DWSSpecificChartPF = null;
                    DWSSpecificChartAB = null;
                    DWSSpecificChartLens5 = null;
                    DetectionChartDV = null;
                    DetectionChartDS = null;
                    DetectionChartST = null;
                    ICFChart = null;

                    sourceSettings = null;
                    posWetPhaseSettings = null;
                    negWetPhaseSettings = null;
                    negDryPhaseSettings = null;
                    dwsSettings = null;
                    dwsSpecificSettings = null;
                    detectionSettings = null;

                    switch (InstrumentType)
                    {
                        case Global.InstrumentType.SPIS:
                            sourceSettings = new SourceSettings();
                            posWetPhaseSettings = new UPSPhaseSettings();
                            dwsSettings = new DWSSettings();
                            dwsSpecificSettings = new DWSSpecificSettings();
                            detectionSettings = new DetectionSettings();

                            sourceSettings.GetSourceData(LatestScanRootNood, ConfigRootNood, InstrumentType);
                            posWetPhaseSettings.GetPosWetUPSPhaseData(LatestScanRootNood, ConfigRootNood, InstrumentType);
                            dwsSettings.GetDWSData(LatestScanRootNood, ConfigRootNood, InstrumentType);
                            dwsSpecificSettings.GetDWSSpecificData(LatestScanRootNood, ConfigRootNood, InstrumentType);
                            detectionSettings.GetDetectionData(LatestScanRootNood, ConfigRootNood, InstrumentType);

                            //outer layer: width = 1400-250-20; height = 900-55-50-20, inner layer: width = x-15, height = y - 30
                            SourceChartPS = ChartGenerator.GetLimitColorChart("Source Presure", sourceSettings.Pressure.Unit, 1114 / 2, 735, sourceSettings.Pressure.SourPhaseList);
                            SourceChartMV = ChartGenerator.GetLimitColorChart("Microwave Power", sourceSettings.MV.Unit, 1114 / 2, 735, sourceSettings.MV.SourPhaseList);
                            PosWetChart = ChartGenerator.GetUPSPhaseLimitColorChart("UPS Pos Wet", posWetPhaseSettings.Unit, 1114, 735, posWetPhaseSettings.UPSPhaseList);

                            DWSChart = ChartGenerator.GetLimitColorChart("DWS", dwsSettings.Unit, 1114, 735, dwsSettings.DWSList);
                            DWSSpecificChartPF = ChartGenerator.GetDWSLensLimitColorChart("DWS Specific Prefilter", dwsSpecificSettings.Prefilter.Unit, dwsSpecificSettings.Prefilter.Linearity_Check, 1114, 735 / 3, dwsSpecificSettings.Prefilter.MassList_Pos);
                            DWSSpecificChartAB = ChartGenerator.GetDWSLensLimitColorChart("DWS Specific Axial Bias", dwsSpecificSettings.Axial_Bias.Unit, dwsSpecificSettings.Axial_Bias.Linearity_Check, 1114, 735 / 3, dwsSpecificSettings.Axial_Bias.MassList_Pos);
                            DWSSpecificChartLens5 = ChartGenerator.GetDWSLensLimitColorChart("DWS Specific Lens 5", dwsSpecificSettings.Lens_5.Unit, dwsSpecificSettings.Lens_5.Linearity_Check, 1114, 735 / 3, dwsSpecificSettings.Lens_5.MassList_Pos);

                            DetectionChartDV = ChartGenerator.GetLimitColorChart("Detector Voltage", detectionSettings.Detector_Voltage.Unit, 1114 / 3, 735, detectionSettings.Detector_Voltage.SettingList);
                            DetectionChartDS = ChartGenerator.GetLimitColorChart("Discriminator", detectionSettings.Discriminator.Unit, 1114 / 3, 735, detectionSettings.Discriminator.SettingList);
                            DetectionChartST = ChartGenerator.GetLimitColorChart("Quad Settle Time", detectionSettings.Settle_Time.Unit, 1114 / 3, 735, detectionSettings.Settle_Time.SettingList);
                            ICFChart = ChartGenerator.GetICFChart(1114, 735, new SyftXML.Scan(LatestScanRootNood));
                            break;
                        default:
                        case Global.InstrumentType.DPIS:
                        case Global.InstrumentType.Infinity:
                            sourceSettings = new SourceSettings();
                            posWetPhaseSettings = new UPSPhaseSettings();
                            negWetPhaseSettings = new UPSPhaseSettings();
                            negDryPhaseSettings = new UPSPhaseSettings();
                            dwsSettings = new DWSSettings();
                            dwsSpecificSettings = new DWSSpecificSettings();
                            detectionSettings = new DetectionSettings();

                            sourceSettings.GetSourceData(LatestScanRootNood, ConfigRootNood, InstrumentType);
                            posWetPhaseSettings.GetPosWetUPSPhaseData(LatestScanRootNood, ConfigRootNood, InstrumentType);
                            negWetPhaseSettings.GetNegWetUPSPhaseData(LatestScanRootNood, ConfigRootNood);
                            negDryPhaseSettings.GetNegDryUPSPhaseData(LatestScanRootNood, ConfigRootNood);
                            dwsSettings.GetDWSData(LatestScanRootNood, ConfigRootNood, InstrumentType);
                            dwsSpecificSettings.GetDWSSpecificData(LatestScanRootNood, ConfigRootNood, InstrumentType);
                            detectionSettings.GetDetectionData(LatestScanRootNood, ConfigRootNood, InstrumentType);

                            //outer layer: width = 1400-250-20; height = 900-55-50-20, inner layer: width = x-15, height = y - 30
                            SourceChartPS = ChartGenerator.GetLimitColorChart("Source Presure", sourceSettings.Pressure.Unit, 1114 / 3, 735, sourceSettings.Pressure.SourPhaseList);
                            SourceChartMV = ChartGenerator.GetLimitColorChart("Microwave Power", sourceSettings.MV.Unit, 1114 / 3, 735, sourceSettings.MV.SourPhaseList);
                            SourceChartMESH = ChartGenerator.GetLimitColorChart("Mesh Voltage", sourceSettings.Mesh.Unit, 1114 / 3, 735, sourceSettings.Mesh.SourPhaseList);
                            PosWetChart = ChartGenerator.GetUPSPhaseLimitColorChart("UPS Pos Wet", posWetPhaseSettings.Unit, posWetPhaseSettings.UPS_Extraction_Lens_Gradient_Check, posWetPhaseSettings.UPS_Einzel_Stack_Check, 1114, 735, posWetPhaseSettings.UPSPhaseList);
                            NegWetChart = ChartGenerator.GetUPSPhaseLimitColorChart("UPS Neg Wet", negWetPhaseSettings.Unit, negWetPhaseSettings.UPS_Extraction_Lens_Gradient_Check, negWetPhaseSettings.UPS_Einzel_Stack_Check, 1114, 735, negWetPhaseSettings.UPSPhaseList);
                            NegDryChart = ChartGenerator.GetUPSPhaseLimitColorChart("UPS Neg Dry", negDryPhaseSettings.Unit, negDryPhaseSettings.UPS_Extraction_Lens_Gradient_Check, negDryPhaseSettings.UPS_Einzel_Stack_Check, 1114, 735, negDryPhaseSettings.UPSPhaseList);

                            DWSChart = ChartGenerator.GetLimitColorChart("DWS", dwsSettings.Unit, 1114, 735, dwsSettings.DWSList);
                            DWSSpecificChartPF = ChartGenerator.GetDWSLensLimitColorChart("DWS Specific Prefilter", dwsSpecificSettings.Prefilter.Unit, dwsSpecificSettings.Prefilter.Linearity_Check, dwsSpecificSettings.Prefilter.Mirror_Check, 1114, 735 / 3, dwsSpecificSettings.Prefilter.MassList_Pos);
                            DWSSpecificChartAB = ChartGenerator.GetDWSLensLimitColorChart("DWS Specific Axial Bias", dwsSpecificSettings.Axial_Bias.Unit, dwsSpecificSettings.Axial_Bias.Linearity_Check, dwsSpecificSettings.Axial_Bias.Mirror_Check, 1114, 735 / 3, dwsSpecificSettings.Axial_Bias.MassList_Pos);
                            DWSSpecificChartLens5 = ChartGenerator.GetDWSLensLimitColorChart("DWS Specific Lens 5", dwsSpecificSettings.Lens_5.Unit, dwsSpecificSettings.Lens_5.Linearity_Check, dwsSpecificSettings.Lens_5.Mirror_Check, 1114, 735 / 3, dwsSpecificSettings.Lens_5.MassList_Pos);

                            DetectionChartDV = ChartGenerator.GetLimitColorChart("Detector Voltage", detectionSettings.Detector_Voltage.Unit, 1114 / 3, 735, detectionSettings.Detector_Voltage.SettingList);
                            DetectionChartDS = ChartGenerator.GetLimitColorChart("Discriminator", detectionSettings.Discriminator.Unit, 1114 / 3, 735, detectionSettings.Discriminator.SettingList);
                            DetectionChartST = ChartGenerator.GetLimitColorChart("Quad Settle Time", detectionSettings.Settle_Time.Unit, 1114 / 3, 735, detectionSettings.Settle_Time.SettingList);
                            ICFChart = ChartGenerator.GetICFChart(1114, 735, new SyftXML.Scan(LatestScanRootNood));
                            break;
                    }
                    //Get instrument number
                    InstrumentShowNumebr = Regex.Match(GetSetting.Content(LatestScanRootNood, "instrument.name"), @"(\d{4})").Groups[1].Value;
                    ProcessTimeStemp = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                    ShowProcessButtonProcess = false;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR");
                    ProcessButtonProgress = 0;
                    ShowProcessButtonProcess = false;
                }
            });
        }
        #endregion

        #region Backup Command
        public DelegateCommand BackupCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
                    folderDlg.IsFolderPicker = true;
                    folderDlg.Title = "Select a Target Folder to Backup ETC Folder";
                    if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        Task.Run(() =>
                        {
                            SFTPServices sftp = new SFTPServices(IPAdress, "22", "root", Global.Password);
                            try
                            {
                                //Delete temp/Setting_Check/backup
                                if (Directory.Exists("./temp/Setting_Check/backup"))
                                {
                                    new DirectoryInfo("./temp/Setting_Check/backup").Delete(true);
                                }
                                BackupIsFinish = false;
                                BackupIsProcessing = true;
                                sftp.Connect();
                                sftp.DownloadDirectory($"/usr/local/syft/etc", "./temp/Setting_Check/backup/etc");
                                sftp.Disconnect();
                                ZipFile.CreateFromDirectory("./temp/Setting_Check/backup", $"{folderDlg.FileName}/etc_{InstrumentShowNumebr}_{DateTime.Now:yMMdd HHmmss}.zip");
                                BackupIsProcessing = false;
                                BackupIsFinish = true;
                                Thread.Sleep(5000);
                                BackupIsFinish = false;
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show($"{ex.Message}", "ERROR");
                                BackupIsProcessing = false;
                                BackupIsFinish = false;
                            }
                            finally
                            {
                                sftp.Disconnect();
                            }
                        });
                    }
                });
            }
        }
        #endregion

        #region Save Command
        public DelegateCommand SaveCommand { get; private set; }

        private void Save()
        {
            CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
            folderDlg.IsFolderPicker = true;
            folderDlg.Title = "Select a Target Folder to Save";
            if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Task.Run(() =>
                {
                    try
                    {
                        ShowSaveButtonProcess = true;
                        //Get file name without suffix
                        string FileNamenoSuffix = $"{InstrumentShowNumebr}_{Global.UserName} Setting Check Report_{DateTime.Now:yMMdd HHmmss}";
                        //Get file full path without suffix
                        string FilePath = $"{folderDlg.FileName}/{FileNamenoSuffix}";
                        //Save the chart
                        string SourceChartPSPath = "";
                        string SourceChartMVPath = "";
                        string SourceChartMESHPath = "";
                        string PosWetChartPath = "";
                        string NegWetChartPath = "";
                        string NegDryChartPath = "";
                        string DWSChartPath = "";
                        string DWSSpecificChartPFPath = "";
                        string DWSSpecificChartABPath = "";
                        string DWSSpecificChartLens5Path = "";
                        string DetectionChartDVPath = "";
                        string DetectionChartDSPath = "";
                        string DetectionChartSTPath = "";
                        string ICFChartPath = "";

                        switch (InstrumentType)
                        {
                            case Global.InstrumentType.SPIS:
                                SourceChartPSPath = SourceChartPS.SavePNGChart();
                                SourceChartMVPath = SourceChartMV.SavePNGChart();
                                PosWetChartPath = PosWetChart.SavePNGChart();
                                DWSChartPath = DWSChart.SavePNGChart();
                                DWSSpecificChartPFPath = DWSSpecificChartPF.SavePNGChart();
                                DWSSpecificChartABPath = DWSSpecificChartAB.SavePNGChart();
                                DWSSpecificChartLens5Path = DWSSpecificChartLens5.SavePNGChart();
                                DetectionChartDVPath = DetectionChartDV.SavePNGChart();
                                DetectionChartDSPath = DetectionChartDS.SavePNGChart();
                                DetectionChartSTPath = DetectionChartST.SavePNGChart();
                                ICFChartPath = ICFChart.SavePNGChart();
                                break;
                            case Global.InstrumentType.DPIS:
                            case Global.InstrumentType.Infinity:
                                SourceChartPSPath = SourceChartPS.SavePNGChart();
                                SourceChartMVPath = SourceChartMV.SavePNGChart();
                                SourceChartMESHPath = SourceChartMESH.SavePNGChart();
                                PosWetChartPath = PosWetChart.SavePNGChart();
                                NegWetChartPath = NegWetChart.SavePNGChart();
                                NegDryChartPath = NegDryChart.SavePNGChart();
                                DWSChartPath = DWSChart.SavePNGChart();
                                DWSSpecificChartPFPath = DWSSpecificChartPF.SavePNGChart();
                                DWSSpecificChartABPath = DWSSpecificChartAB.SavePNGChart();
                                DWSSpecificChartLens5Path = DWSSpecificChartLens5.SavePNGChart();
                                DetectionChartDVPath = DetectionChartDV.SavePNGChart();
                                DetectionChartDSPath = DetectionChartDS.SavePNGChart();
                                DetectionChartSTPath = DetectionChartST.SavePNGChart();
                                ICFChartPath = ICFChart.SavePNGChart();
                                break;
                            default:
                                break;
                        }


                        if (savedPosWetPhaseSettingsDic.Count == 0 && savedICFDic.Count == 0)
                        {
                            //Copy scan
                            File.Copy($"./temp/Setting_Check/{LatestScanFileName}", $"{FilePath}.xml", true);
                            //Create report
                            PdfDocument pdf = new PdfDocument(new PdfWriter($"{FilePath}.pdf"));
                            Document doc = new Document(pdf, PageSize.A4);
                            doc.SetMargins(10, 10, 10, 10);
                            //Create header 
                            PDFGenerator.GenerateHeadPDF(doc, "P3 Setting Check");
                            //Create infor 
                            PDFGenerator.GenerateInfoPDF(doc, GetRootNode.SavedScan($"{FilePath}.xml"));
                            //Create table 
                            PDFGenerator.GenerateTablePDF(doc,
                                sourceSettings, posWetPhaseSettings, negWetPhaseSettings, negDryPhaseSettings,
                                dwsSettings, dwsSpecificSettings, detectionSettings);
                            //Create chart 
                            PDFGenerator.GenerateChartPDF(doc, "Setting Check Chart", false, SourceChartPSPath, SourceChartMVPath, SourceChartMESHPath,
                                PosWetChartPath, NegWetChartPath, NegDryChartPath, DWSChartPath,
                                DWSSpecificChartPFPath, DWSSpecificChartABPath, DWSSpecificChartLens5Path,
                                DetectionChartDVPath, DetectionChartDSPath, DetectionChartSTPath, ICFChartPath);

                            doc.Close();
                        }
                        else
                        {
                            //Create report
                            PdfDocument pdf = new PdfDocument(new PdfWriter($"{FilePath}.pdf"));
                            Document doc = new Document(pdf, PageSize.A4);
                            doc.SetMargins(30, 10, 10, 10);
                            //Create chart 
                            PDFGenerator.GenerateChartPDF(doc, "Instrument Settings Chart", true, SourceChartPSPath, SourceChartMVPath, SourceChartMESHPath,
                                PosWetChartPath, NegWetChartPath, NegDryChartPath, DWSChartPath,
                                DWSSpecificChartPFPath, DWSSpecificChartABPath, DWSSpecificChartLens5Path,
                                DetectionChartDVPath, DetectionChartDSPath, DetectionChartSTPath, ICFChartPath);

                            doc.Close();
                        }

                        //Delete temp/Setting_Check/chart_temp
                        if (Directory.Exists("./temp/Setting_Check/chart_temp"))
                        {
                            new DirectoryInfo("./temp/Setting_Check/chart_temp").Delete(true);
                        }
                        //Delete temp/Setting_Check/latest scan
                        if (File.Exists($"./temp/Setting_Check/{LatestScanFileName}"))
                        {
                            File.Delete($"./temp/Setting_Check/{LatestScanFileName}");
                        }

                        ShowSaveButtonProcess = false;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR");
                        SaveButtonProgress = 0;
                        ShowSaveButtonProcess = false;
                    }
                });
            }
        }
        #endregion

        #region Compare Open Command
        public DelegateCommand CompareOpenCommand { get; private set; }

        private void CompareOpen()
        {
            CommonOpenFileDialog fileDlg = new CommonOpenFileDialog();
            fileDlg.Filters.Add(new CommonFileDialogFilter("XML", "*.xml"));
            fileDlg.Multiselect = true;
            fileDlg.Title = "Select Saved Setting Check xml files or xml scan files";
            if (fileDlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                XElement ScanRootNood;

                savedSourceSettingsPSDic.Clear();
                savedSourceSettingsMVDic.Clear();
                savedSourceSettingsMESHDic.Clear();
                savedPosWetPhaseSettingsDic.Clear();
                savedNegWetPhaseSettingsDic.Clear();
                savedNegDryPhaseSettingsDic.Clear();
                savedDWSSettingsDic.Clear();
                savedDWSSpecificSettingsPFDic.Clear();
                savedDWSSpecificSettingsABDic.Clear();
                savedDWSSpecificSettingsLens5Dic.Clear();
                savedDetectionSettingsDVDic.Clear();
                savedDetectionSettingsDSDic.Clear();
                savedDetectionSettingsSTDic.Clear();
                savedICFDic.Clear();

                SourceChartPS = null;
                SourceChartMV = null;
                SourceChartMESH = null;
                PosWetChart = null;
                NegWetChart = null;
                NegDryChart = null;
                DWSChart = null;
                DWSSpecificChartPF = null;
                DWSSpecificChartAB = null;
                DWSSpecificChartLens5 = null;
                DetectionChartDV = null;
                DetectionChartDS = null;
                DetectionChartST = null;
                ICFChart = null;

                ColorNumber = 0;

                Task.Run(() =>
                {
                    try
                    {
                        ShowCompareOpenButtonProcess = true;
                        XElement ConfigRootNood = GetRootNode.Config();
                        switch (InstrumentType)
                        {
                            case Global.InstrumentType.SPIS:
                                foreach (var item in fileDlg.FileNames)
                                {
                                    ScanRootNood = GetRootNode.SavedScan($"{item}");

                                    SourceSettings sourceSettings = new SourceSettings();
                                    UPSPhaseSettings posWetPhaseSettings = new UPSPhaseSettings();
                                    DWSSettings dwsSettings = new DWSSettings();
                                    DWSSpecificSettings dwsSpecificSettings = new DWSSpecificSettings();
                                    DetectionSettings detectionSettings = new DetectionSettings();

                                    sourceSettings.GetSourceData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    posWetPhaseSettings.GetPosWetUPSPhaseData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    dwsSettings.GetDWSData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    dwsSpecificSettings.GetDWSSpecificData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    detectionSettings.GetDetectionData(ScanRootNood, ConfigRootNood, InstrumentType);

                                    string folderName = item.Remove(0, item.LastIndexOf('\\') + 1);
                                    string fileNamenoSuffix = folderName.Substring(0, folderName.LastIndexOf('.'));
                                    int color = -1;
                                    try
                                    {
                                        color = Global.ColorPool[ColorNumber++];
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    savedSourceSettingsPSDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.Pressure.SourPhaseList);
                                    savedSourceSettingsMVDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.MV.SourPhaseList);
                                    savedPosWetPhaseSettingsDic.Add($"{fileNamenoSuffix}({color})", posWetPhaseSettings.UPSPhaseList);
                                    savedDWSSettingsDic.Add($"{fileNamenoSuffix}({color})", dwsSettings.DWSList);
                                    savedDWSSpecificSettingsPFDic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Prefilter.MassList_Pos);
                                    savedDWSSpecificSettingsABDic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Axial_Bias.MassList_Pos);
                                    savedDWSSpecificSettingsLens5Dic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Lens_5.MassList_Pos);
                                    savedDetectionSettingsDVDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Detector_Voltage.SettingList);
                                    savedDetectionSettingsDSDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Discriminator.SettingList);
                                    savedDetectionSettingsSTDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Settle_Time.SettingList);
                                    savedICFDic.Add($"{fileNamenoSuffix}({color})", new SyftXML.Scan(ScanRootNood));
                                }

                                //outer layer: width = 1400-250-20; height = 900-55-50-20, inner layer: width = x-15, height = y - 30
                                SourceChartPS = ChartGenerator.GetCompareChart("Source Presure", "Torr", 1114 / 2, 735, true, savedSourceSettingsPSDic);
                                SourceChartMV = ChartGenerator.GetCompareChart("MV", "W", 1114 / 2, 735, true, savedSourceSettingsMVDic);
                                PosWetChart = ChartGenerator.GetCompareChart("UPS Pos Wet", "V", 1114, 735, true, savedPosWetPhaseSettingsDic);
                                DWSChart = ChartGenerator.GetCompareChart("DWS", "V", 1114, 735, true, savedDWSSettingsDic);
                                DWSSpecificChartPF = ChartGenerator.GetCompareChart("DWS Specific Prefilter", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsPFDic);
                                DWSSpecificChartAB = ChartGenerator.GetCompareChart("DWS Specific Axial Bias", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsABDic);
                                DWSSpecificChartLens5 = ChartGenerator.GetCompareChart("DWS Specific Lens 5", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsLens5Dic);
                                DetectionChartDV = ChartGenerator.GetCompareChart("Detector Voltage", "V", 1114 / 3, 735, true, savedDetectionSettingsDVDic);
                                DetectionChartDS = ChartGenerator.GetCompareChart("Discriminator", "V", 1114 / 3, 735, false, savedDetectionSettingsDSDic);
                                DetectionChartST = ChartGenerator.GetCompareChart("Settle Time", "ms", 1114 / 3, 735, true, savedDetectionSettingsSTDic);
                                ICFChart = ChartGenerator.GetICFCompareChart(1114, 735, savedICFDic, true);
                                break;
                            case Global.InstrumentType.DPIS:
                            case Global.InstrumentType.Infinity:
                                foreach (var item in fileDlg.FileNames)
                                {
                                    ScanRootNood = GetRootNode.SavedScan($"{item}");

                                    SourceSettings sourceSettings = new SourceSettings();
                                    UPSPhaseSettings posWetPhaseSettings = new UPSPhaseSettings();
                                    UPSPhaseSettings negWetPhaseSettings = new UPSPhaseSettings();
                                    UPSPhaseSettings negDryPhaseSettings = new UPSPhaseSettings();
                                    DWSSettings dwsSettings = new DWSSettings();
                                    DWSSpecificSettings dwsSpecificSettings = new DWSSpecificSettings();
                                    DetectionSettings detectionSettings = new DetectionSettings();

                                    sourceSettings.GetSourceData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    posWetPhaseSettings.GetPosWetUPSPhaseData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    negWetPhaseSettings.GetNegWetUPSPhaseData(ScanRootNood, ConfigRootNood);
                                    negDryPhaseSettings.GetNegDryUPSPhaseData(ScanRootNood, ConfigRootNood);
                                    dwsSettings.GetDWSData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    dwsSpecificSettings.GetDWSSpecificData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    detectionSettings.GetDetectionData(ScanRootNood, ConfigRootNood, InstrumentType);

                                    string folderName = item.Remove(0, item.LastIndexOf('\\') + 1);
                                    string fileNamenoSuffix = folderName.Substring(0, folderName.LastIndexOf('.'));
                                    int color = -1;
                                    try
                                    {
                                        color = Global.ColorPool[ColorNumber++];
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    savedSourceSettingsPSDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.Pressure.SourPhaseList);
                                    savedSourceSettingsMVDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.MV.SourPhaseList);
                                    savedSourceSettingsMESHDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.Mesh.SourPhaseList);
                                    savedPosWetPhaseSettingsDic.Add($"{fileNamenoSuffix}({color})", posWetPhaseSettings.UPSPhaseList);
                                    savedNegWetPhaseSettingsDic.Add($"{fileNamenoSuffix}({color})", negWetPhaseSettings.UPSPhaseList);
                                    savedNegDryPhaseSettingsDic.Add($"{fileNamenoSuffix}({color})", negDryPhaseSettings.UPSPhaseList);
                                    savedDWSSettingsDic.Add($"{fileNamenoSuffix}({color})", dwsSettings.DWSList);
                                    savedDWSSpecificSettingsPFDic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Prefilter.MassList_Pos);
                                    savedDWSSpecificSettingsABDic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Axial_Bias.MassList_Pos);
                                    savedDWSSpecificSettingsLens5Dic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Lens_5.MassList_Pos);
                                    savedDetectionSettingsDVDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Detector_Voltage.SettingList);
                                    savedDetectionSettingsDSDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Discriminator.SettingList);
                                    savedDetectionSettingsSTDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Settle_Time.SettingList);
                                    savedICFDic.Add($"{fileNamenoSuffix}({color})", new SyftXML.Scan(ScanRootNood));
                                }

                                //outer layer: width = 1400-250-20; height = 900-55-50-20, inner layer: width = x-15, height = y - 30
                                SourceChartPS = ChartGenerator.GetCompareChart("Source Presure", "Torr", 1114 / 3, 735, true, savedSourceSettingsPSDic);
                                SourceChartMV = ChartGenerator.GetCompareChart("MV", "W", 1114 / 3, 735, true, savedSourceSettingsMVDic);
                                SourceChartMESH = ChartGenerator.GetCompareChart("Mesh Voltage", "V", 1114 / 3, 735, true, savedSourceSettingsMESHDic);
                                PosWetChart = ChartGenerator.GetCompareChart("UPS Pos Wet", "V", 1114, 735, true, savedPosWetPhaseSettingsDic);
                                NegWetChart = ChartGenerator.GetCompareChart("UPS Neg Wet", "V", 1114, 735, true, savedNegWetPhaseSettingsDic);
                                NegDryChart = ChartGenerator.GetCompareChart("UPS Neg Dry", "V", 1114, 735, true, savedNegDryPhaseSettingsDic);
                                DWSChart = ChartGenerator.GetCompareChart("DWS", "V", 1114, 735, true, savedDWSSettingsDic);
                                DWSSpecificChartPF = ChartGenerator.GetCompareChart("DWS Specific Prefilter", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsPFDic);
                                DWSSpecificChartAB = ChartGenerator.GetCompareChart("DWS Specific Axial Bias", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsABDic);
                                DWSSpecificChartLens5 = ChartGenerator.GetCompareChart("DWS Specific Lens 5", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsLens5Dic);
                                DetectionChartDV = ChartGenerator.GetCompareChart("Detector Voltage", "V", 1114 / 3, 735, true, savedDetectionSettingsDVDic);
                                DetectionChartDS = ChartGenerator.GetCompareChart("Discriminator", "V", 1114 / 3, 735, false, savedDetectionSettingsDSDic);
                                DetectionChartST = ChartGenerator.GetCompareChart("Settle Time", "ms", 1114 / 3, 735, true, savedDetectionSettingsSTDic);
                                ICFChart = ChartGenerator.GetICFCompareChart(1114, 735, savedICFDic);
                                break;
                            default:
                                break;
                        }
                        ShowCompareOpenButtonProcess = false;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR");
                        SaveButtonProgress = 0;
                        ShowCompareOpenButtonProcess = false;
                    }
                });
            }
        }
        #endregion

        #region Compare Add Command
        public DelegateCommand CompareAddCommand { get; private set; }

        private void CompareAdd()
        {
            XElement ScanRootNood;

            if (savedPosWetPhaseSettingsDic.Count == 0) return;
            if (savedICFDic.Count == 0) return;

            SourceChartPS = null;
            SourceChartMV = null;
            SourceChartMESH = null;
            PosWetChart = null;
            NegWetChart = null;
            NegDryChart = null;
            DWSChart = null;
            DWSSpecificChartPF = null;
            DWSSpecificChartAB = null;
            DWSSpecificChartLens5 = null;
            DetectionChartDV = null;
            DetectionChartDS = null;
            DetectionChartST = null;
            ICFChart = null;

            CommonOpenFileDialog fileDlg = new CommonOpenFileDialog();
            fileDlg.Filters.Add(new CommonFileDialogFilter("XML", "*.xml"));
            fileDlg.Multiselect = true;
            fileDlg.Title = "Select Saved Setting Check xml files or xml scan files";
            if (fileDlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Task.Run(() =>
                {
                    try
                    {
                        ShowCompareAddButtonProcess = true;
                        XElement ConfigRootNood = GetRootNode.Config();
                        switch (InstrumentType)
                        {
                            case Global.InstrumentType.SPIS:
                                foreach (var item in fileDlg.FileNames)
                                {
                                    ScanRootNood = GetRootNode.SavedScan($"{item}");

                                    SourceSettings sourceSettings = new SourceSettings();
                                    UPSPhaseSettings posWetPhaseSettings = new UPSPhaseSettings();
                                    DWSSettings dwsSettings = new DWSSettings();
                                    DWSSpecificSettings dwsSpecificSettings = new DWSSpecificSettings();
                                    DetectionSettings detectionSettings = new DetectionSettings();

                                    sourceSettings.GetSourceData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    posWetPhaseSettings.GetPosWetUPSPhaseData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    dwsSettings.GetDWSData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    dwsSpecificSettings.GetDWSSpecificData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    detectionSettings.GetDetectionData(ScanRootNood, ConfigRootNood, InstrumentType);

                                    string folderName = item.Remove(0, item.LastIndexOf('\\') + 1);
                                    string fileNamenoSuffix = folderName.Substring(0, folderName.LastIndexOf('.'));
                                    int color = -1;
                                    try
                                    {
                                        color = Global.ColorPool[ColorNumber++];
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    savedSourceSettingsPSDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.Pressure.SourPhaseList);
                                    savedSourceSettingsMVDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.MV.SourPhaseList);
                                    savedPosWetPhaseSettingsDic.Add($"{fileNamenoSuffix}({color})", posWetPhaseSettings.UPSPhaseList);
                                    savedDWSSettingsDic.Add($"{fileNamenoSuffix}({color})", dwsSettings.DWSList);
                                    savedDWSSpecificSettingsPFDic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Prefilter.MassList_Pos);
                                    savedDWSSpecificSettingsABDic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Axial_Bias.MassList_Pos);
                                    savedDWSSpecificSettingsLens5Dic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Lens_5.MassList_Pos);
                                    savedDetectionSettingsDVDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Detector_Voltage.SettingList);
                                    savedDetectionSettingsDSDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Discriminator.SettingList);
                                    savedDetectionSettingsSTDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Settle_Time.SettingList);
                                    savedICFDic.Add($"{fileNamenoSuffix}({color})", new SyftXML.Scan(ScanRootNood));
                                }

                                //outer layer: width = 1400-250-20; height = 900-55-50-20, inner layer: width = x-15, height = y - 30
                                SourceChartPS = ChartGenerator.GetCompareChart("Source Presure", "Torr", 1114 / 2, 735, true, savedSourceSettingsPSDic);
                                SourceChartMV = ChartGenerator.GetCompareChart("MV", "W", 1114 / 2, 735, true, savedSourceSettingsMVDic);
                                PosWetChart = ChartGenerator.GetCompareChart("UPS Pos Wet", "V", 1114, 735, true, savedPosWetPhaseSettingsDic);

                                DWSChart = ChartGenerator.GetCompareChart("DWS", "V", 1114, 735, true, savedDWSSettingsDic);
                                DWSSpecificChartPF = ChartGenerator.GetCompareChart("DWS Specific Prefilter", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsPFDic);
                                DWSSpecificChartAB = ChartGenerator.GetCompareChart("DWS Specific Axial Bias", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsABDic);
                                DWSSpecificChartLens5 = ChartGenerator.GetCompareChart("DWS Specific Lens 5", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsLens5Dic);

                                DetectionChartDV = ChartGenerator.GetCompareChart("Detector Voltage", "V", 1114 / 3, 735, true, savedDetectionSettingsDVDic);
                                DetectionChartDS = ChartGenerator.GetCompareChart("Discriminator", "V", 1114 / 3, 735, false, savedDetectionSettingsDSDic);
                                DetectionChartST = ChartGenerator.GetCompareChart("Settle Time", "ms", 1114 / 3, 735, true, savedDetectionSettingsSTDic);

                                ICFChart = ChartGenerator.GetICFCompareChart(1114, 735, savedICFDic, true);
                                break;
                            case Global.InstrumentType.DPIS:
                            case Global.InstrumentType.Infinity:
                                foreach (var item in fileDlg.FileNames)
                                {
                                    ScanRootNood = GetRootNode.SavedScan($"{item}");

                                    SourceSettings sourceSettings = new SourceSettings();
                                    UPSPhaseSettings posWetPhaseSettings = new UPSPhaseSettings();
                                    UPSPhaseSettings negWetPhaseSettings = new UPSPhaseSettings();
                                    UPSPhaseSettings negDryPhaseSettings = new UPSPhaseSettings();
                                    DWSSettings dwsSettings = new DWSSettings();
                                    DWSSpecificSettings dwsSpecificSettings = new DWSSpecificSettings();
                                    DetectionSettings detectionSettings = new DetectionSettings();

                                    sourceSettings.GetSourceData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    posWetPhaseSettings.GetPosWetUPSPhaseData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    negWetPhaseSettings.GetNegWetUPSPhaseData(ScanRootNood, ConfigRootNood);
                                    negDryPhaseSettings.GetNegDryUPSPhaseData(ScanRootNood, ConfigRootNood);
                                    dwsSettings.GetDWSData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    dwsSpecificSettings.GetDWSSpecificData(ScanRootNood, ConfigRootNood, InstrumentType);
                                    detectionSettings.GetDetectionData(ScanRootNood, ConfigRootNood, InstrumentType);

                                    string folderName = item.Remove(0, item.LastIndexOf('\\') + 1);
                                    string fileNamenoSuffix = folderName.Substring(0, folderName.LastIndexOf('.'));
                                    int color = -1;
                                    try
                                    {
                                        color = Global.ColorPool[ColorNumber++];
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    savedSourceSettingsPSDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.Pressure.SourPhaseList);
                                    savedSourceSettingsMVDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.MV.SourPhaseList);
                                    savedSourceSettingsMESHDic.Add($"{fileNamenoSuffix}({color})", sourceSettings.Mesh.SourPhaseList);
                                    savedPosWetPhaseSettingsDic.Add($"{fileNamenoSuffix}({color})", posWetPhaseSettings.UPSPhaseList);
                                    savedNegWetPhaseSettingsDic.Add($"{fileNamenoSuffix}({color})", negWetPhaseSettings.UPSPhaseList);
                                    savedNegDryPhaseSettingsDic.Add($"{fileNamenoSuffix}({color})", negDryPhaseSettings.UPSPhaseList);
                                    savedDWSSettingsDic.Add($"{fileNamenoSuffix}({color})", dwsSettings.DWSList);
                                    savedDWSSpecificSettingsPFDic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Prefilter.MassList_Pos);
                                    savedDWSSpecificSettingsABDic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Axial_Bias.MassList_Pos);
                                    savedDWSSpecificSettingsLens5Dic.Add($"{fileNamenoSuffix}({color})", dwsSpecificSettings.Lens_5.MassList_Pos);
                                    savedDetectionSettingsDVDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Detector_Voltage.SettingList);
                                    savedDetectionSettingsDSDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Discriminator.SettingList);
                                    savedDetectionSettingsSTDic.Add($"{fileNamenoSuffix}({color})", detectionSettings.Settle_Time.SettingList);
                                    savedICFDic.Add($"{fileNamenoSuffix}({color})", new SyftXML.Scan(ScanRootNood));
                                }

                                //outer layer: width = 1400-250-20; height = 900-55-50-20, inner layer: width = x-15, height = y - 30
                                SourceChartPS = ChartGenerator.GetCompareChart("Source Presure", "Torr", 1114 / 3, 735, true, savedSourceSettingsPSDic);
                                SourceChartMV = ChartGenerator.GetCompareChart("MV", "W", 1114 / 3, 735, true, savedSourceSettingsMVDic);
                                SourceChartMESH = ChartGenerator.GetCompareChart("Mesh Voltage", "V", 1114 / 3, 735, true, savedSourceSettingsMESHDic);
                                PosWetChart = ChartGenerator.GetCompareChart("UPS Pos Wet", "V", 1114, 735, true, savedPosWetPhaseSettingsDic);
                                NegWetChart = ChartGenerator.GetCompareChart("UPS Neg Wet", "V", 1114, 735, true, savedNegWetPhaseSettingsDic);
                                NegDryChart = ChartGenerator.GetCompareChart("UPS Neg Dry", "V", 1114, 735, true, savedNegDryPhaseSettingsDic);

                                DWSChart = ChartGenerator.GetCompareChart("DWS", "V", 1114, 735, true, savedDWSSettingsDic);
                                DWSSpecificChartPF = ChartGenerator.GetCompareChart("DWS Specific Prefilter", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsPFDic);
                                DWSSpecificChartAB = ChartGenerator.GetCompareChart("DWS Specific Axial Bias", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsABDic);
                                DWSSpecificChartLens5 = ChartGenerator.GetCompareChart("DWS Specific Lens 5", "V", 1114, 735 / 3, false, savedDWSSpecificSettingsLens5Dic);

                                DetectionChartDV = ChartGenerator.GetCompareChart("Detector Voltage", "V", 1114 / 3, 735, true, savedDetectionSettingsDVDic);
                                DetectionChartDS = ChartGenerator.GetCompareChart("Discriminator", "V", 1114 / 3, 735, false, savedDetectionSettingsDSDic);
                                DetectionChartST = ChartGenerator.GetCompareChart("Settle Time", "ms", 1114 / 3, 735, true, savedDetectionSettingsSTDic);

                                ICFChart = ChartGenerator.GetICFCompareChart(1114, 735, savedICFDic);
                                break;
                            default:
                                break;
                        }
                        ShowCompareAddButtonProcess = false;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR");
                        SaveButtonProgress = 0;
                        ShowCompareAddButtonProcess = false;
                    }
                });
            }
        }
        #endregion
    }
}
