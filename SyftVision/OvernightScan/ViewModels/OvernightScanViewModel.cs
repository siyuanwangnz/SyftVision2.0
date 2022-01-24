using ChartDirector;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using OvernightScan.Models;
using OvernightScan.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Public.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace OvernightScan.ViewModels
{
    public class OvernightScanViewModel : BindableBase
    {
        public OvernightScanViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

        }
        #region Field
        private readonly IEventAggregator _eventAggregator;

        public Dictionary<string, List<ScanStatus>> batchScansStatus;
        #endregion

        #region Property
        public int NumberofBatches { get; private set; } = 1;

        public Global.BatchType InstrumentBatchType { get; private set; } = Global.BatchType.Infinity_SensAndImpu;

        public SyftXML.Instrument instrument { get; private set; }
        #endregion

        #region Region Navigation
        private readonly IRegionManager _regionManager;
        //Batch Status
        public DelegateCommand Open_BatchStatus_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("batchScansStatus", batchScansStatus);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "BatchStatusView", param);
                });
            }
        }
        //Cold Start
        public DelegateCommand Open_ColdStart_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("SensDevChart", ColdStartSensDevChart);
                    param.Add("Mark3And4DevChart", ColdStartMark3And4DevChart);
                    param.Add("BothEndsDevChart", ColdStartBothEndsDevChart);
                    param.Add("SensitivetiesChart", ColdStartSensitivetiesChart);
                    param.Add("ReagentIonsChart", ColdStartReagentIonsChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "ColdStartView", param);
                });
            }
        }
        //DPIS
        public DelegateCommand Open_SourceSwitchStability_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("ReagentRSDChart", DIPSourceSwitchStabilityReagentRSDChart);
                    param.Add("PosWetReagentChart", DIPSourceSwitchStabilityPosWetReagentChart);
                    param.Add("NegWetReagentChart", DIPSourceSwitchStabilityNegWetReagentChart);
                    param.Add("NegDryReagentChart", DIPSourceSwitchStabilityNegDryReagentChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "SourceSwitchStabilityView", param);
                });
            }
        }
        public DelegateCommand Open_UPSAndDWSCurrent_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("PosWetUPSCurrentChart", DPISLongTermStabilityPosWetUPSCurrentChart);
                    param.Add("PosWetDWSCurrentChart", DPISLongTermStabilityPosWetDWSCurrentChart);
                    param.Add("NegWetUPSCurrentChart", DPISLongTermStabilityNegWetUPSCurrentChart);
                    param.Add("NegWetDWSCurrentChart", DPISLongTermStabilityNegWetDWSCurrentChart);
                    param.Add("NegDryUPSCurrentChart", DPISLongTermStabilityNegDryUPSCurrentChart);
                    param.Add("NegDryDWSCurrentChart", DPISLongTermStabilityNegDryDWSCurrentChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "UPSAndDWSCurrentView", param);
                });
            }
        }
        //DPIS Common
        public DelegateCommand Open_LongTermStability_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("ReagentRSDChartList", DPISLongTermStabilityReagentRSDChartList);
                    param.Add("PosWetReagentChartList", DPISLongTermStabilityPosWetReagentChartList);
                    param.Add("PosWetProductChartList", DPISLongTermStabilityPosWetProductChartList);
                    param.Add("NegWetReagentChartList", DPISLongTermStabilityNegWetReagentChartList);
                    param.Add("NegWetProductChartList", DPISLongTermStabilityNegWetProductChartList);
                    param.Add("NegDryReagentChartList", DPISLongTermStabilityNegDryReagentChartList);
                    param.Add("NegDryProductChartList", DPISLongTermStabilityNegDryProductChartList);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "LongTermStabilityView", param);
                });
            }
        }
        public DelegateCommand Open_ShortTermStability_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("ReagentRSDChart", DPISShortTermStabilityReagentRSDChart);
                    param.Add("PosWetReagentChart", DPISShortTermStabilityPosWetReagentChart);
                    param.Add("NegWetReagentChart", DPISShortTermStabilityNegWetReagentChart);
                    param.Add("NegDryReagentChart", DPISShortTermStabilityNegDryReagentChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "ShortTermStabilityView", param);
                });
            }
        }
        public DelegateCommand Open_SourceSwitchAndSettleTime_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("SourceSwitchAndSettleTimeChart", SourceSwitchAndSettleTimeChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "SourceSwitchAndSettleTimeView", param);
                });
            }
        }
        //Infinity
        public DelegateCommand Open_LODs_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("LODsChart", LODsChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "LODsView", param);
                });
            }
        }
        public DelegateCommand Open_Background_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("Compounds75Chart", BackgroundCompounds75Chart);
                    param.Add("Compounds51Chart", BackgroundCompounds51Chart);
                    param.Add("Compounds52Chart", BackgroundCompounds52Chart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "BackgroundView", param);
                });
            }
        }
        public DelegateCommand Open_Stability_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("ConcentrationsRSDChartList", StabilityConcentrationsRSDChartList);
                    param.Add("ConcentrationsChart", StabilityConcentrationsChart);
                    param.Add("ReagentIonsChart", StabilityReagentIonsChart);
                    param.Add("ProductIonsChart", StabilityProductIonsChart);
                    param.Add("ConcentrationswEOVChart", StabilityConcentrationswEOVChart);
                    param.Add("ReactionTimeEOVChart", StabilityReactionTimeEOVChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "StabilityView", param);
                });
            }
        }
        //SPIS
        public DelegateCommand Open_EffectofValidation_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("ConcentrationsRSDChart", EOVConcentrationsRSDChart);
                    param.Add("ConcentrationsChart", EOVConcentrationsChart);
                    param.Add("ReagentIonsChart", EOVReagentIonsChart);
                    param.Add("ProductIonsChart", EOVProductIonsChart);
                    param.Add("ReactionTimeEOVChart", EOVReactionTimeEOVChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "EffectofValidationView", param);
                });
            }
        }
        public DelegateCommand Open_OnOff_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("ConcentrationsRSDChart", SPISOnOffConcentrationsRSDChart);
                    param.Add("ConcentrationsChart", SPISOnOffConcentrationsChart);
                    param.Add("ReagentIonsChart", SPISOnOffReagentIonsChart);
                    param.Add("ProductIonsChart", SPISOnOffProductIonsChart);
                    param.Add("QuadStabilityChart", SPISOnOffQuadStabilityChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "OnOffView", param);
                });
            }
        }
        public DelegateCommand Open_Overnight_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("InstrumentBatchType", InstrumentBatchType);

                    param.Add("ConcentrationsRSDChartOverall", SPISOvernightConcentrationsRSDChartOverall);

                    param.Add("ConcentrationsRSDChart", SPISOvernightConcentrationsRSDChart);
                    param.Add("ConcentrationsChart", SPISOvernightConcentrationsChart);
                    param.Add("ReagentIonsChart", SPISOvernightReagentIonsChart);
                    param.Add("ProductIonsChart", SPISOvernightProductIonsChart);
                    param.Add("QuadStabilityChart", SPISOvernightQuadStabilityChart);

                    param.Add("ConcentrationsRSDChart2", SPISOvernightConcentrationsRSDChart2);
                    param.Add("ConcentrationsChart2", SPISOvernightConcentrationsChart2);
                    param.Add("ReagentIonsChart2", SPISOvernightReagentIonsChart2);
                    param.Add("ProductIonsChart2", SPISOvernightProductIonsChart2);
                    param.Add("QuadStabilityChart2", SPISOvernightQuadStabilityChart2);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "OvernightView", param);
                });
            }
        }
        //ThreePhase
        public DelegateCommand Open_ICF_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("ICFChart", ICFChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "ICFView", param);
                });
            }
        }
        public DelegateCommand Open_InjectionScan_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("PosWetChart", InjectionScanPosWetChart);
                    param.Add("NegWetChart", InjectionScanNegWetChart);
                    param.Add("NegDryChart", InjectionScanNegDryChart);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "InjectionScanView", param);
                });
            }
        }
        public DelegateCommand Open_SensitiveAndImpurity_Command
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("NumberofBatches", NumberofBatches);
                    param.Add("SensitiveChartList", SensitiveChartList);
                    param.Add("ImpurityChartList", ImpurityChartList);
                    _regionManager.RequestNavigate("OvernightScanContentRegion", "SensitiveAndImpurityView", param);
                });
            }
        }
        #endregion

        #region Chart
        //Cold Start
        public BaseChart ColdStartSensDevChart { get; set; }
        public BaseChart ColdStartMark3And4DevChart { get; set; }
        public BaseChart ColdStartBothEndsDevChart { get; set; }
        public BaseChart ColdStartSensitivetiesChart { get; set; }
        public BaseChart ColdStartReagentIonsChart { get; set; }
        //SPIS
        public BaseChart SPISOvernightConcentrationsRSDChartOverall { get; set; }
        public BaseChart SPISOvernightConcentrationsRSDChart { get; set; }
        public BaseChart SPISOvernightConcentrationsChart { get; set; }
        public BaseChart SPISOvernightReagentIonsChart { get; set; }
        public BaseChart SPISOvernightProductIonsChart { get; set; }
        public BaseChart SPISOvernightQuadStabilityChart { get; set; }
        public BaseChart SPISOvernightConcentrationsRSDChart2 { get; set; }
        public BaseChart SPISOvernightConcentrationsChart2 { get; set; }
        public BaseChart SPISOvernightReagentIonsChart2 { get; set; }
        public BaseChart SPISOvernightProductIonsChart2 { get; set; }
        public BaseChart SPISOvernightQuadStabilityChart2 { get; set; }

        public BaseChart SPISOnOffConcentrationsRSDChart { get; set; }
        public BaseChart SPISOnOffConcentrationsChart { get; set; }
        public BaseChart SPISOnOffReagentIonsChart { get; set; }
        public BaseChart SPISOnOffProductIonsChart { get; set; }
        public BaseChart SPISOnOffQuadStabilityChart { get; set; }
        //EOV
        public BaseChart EOVConcentrationsRSDChart { get; set; }
        public BaseChart EOVConcentrationsChart { get; set; }
        public BaseChart EOVReagentIonsChart { get; set; }
        public BaseChart EOVProductIonsChart { get; set; }
        public BaseChart EOVReactionTimeEOVChart { get; set; }
        //DPIS
        public BaseChart DIPSourceSwitchStabilityReagentRSDChart { get; set; }
        public BaseChart DIPSourceSwitchStabilityPosWetReagentChart { get; set; }
        public BaseChart DIPSourceSwitchStabilityNegWetReagentChart { get; set; }
        public BaseChart DIPSourceSwitchStabilityNegDryReagentChart { get; set; }

        public BaseChart DPISLongTermStabilityPosWetUPSCurrentChart { get; set; }
        public BaseChart DPISLongTermStabilityPosWetDWSCurrentChart { get; set; }
        public BaseChart DPISLongTermStabilityNegWetUPSCurrentChart { get; set; }
        public BaseChart DPISLongTermStabilityNegWetDWSCurrentChart { get; set; }
        public BaseChart DPISLongTermStabilityNegDryUPSCurrentChart { get; set; }
        public BaseChart DPISLongTermStabilityNegDryDWSCurrentChart { get; set; }
        //DPIS Common
        public List<XYChart> DPISLongTermStabilityReagentRSDChartList { get; set; }
        public List<XYChart> DPISLongTermStabilityPosWetReagentChartList { get; set; }
        public List<XYChart> DPISLongTermStabilityPosWetProductChartList { get; set; }
        public List<XYChart> DPISLongTermStabilityNegWetReagentChartList { get; set; }
        public List<XYChart> DPISLongTermStabilityNegWetProductChartList { get; set; }
        public List<XYChart> DPISLongTermStabilityNegDryReagentChartList { get; set; }
        public List<XYChart> DPISLongTermStabilityNegDryProductChartList { get; set; }

        public BaseChart DPISShortTermStabilityReagentRSDChart { get; set; }
        public BaseChart DPISShortTermStabilityPosWetReagentChart { get; set; }
        public BaseChart DPISShortTermStabilityNegWetReagentChart { get; set; }
        public BaseChart DPISShortTermStabilityNegDryReagentChart { get; set; }

        public BaseChart SourceSwitchAndSettleTimeChart { get; set; }
        //Infinity
        public BaseChart LODsChart { get; set; }

        public BaseChart BackgroundCompounds75Chart { get; set; }
        public BaseChart BackgroundCompounds51Chart { get; set; }
        public BaseChart BackgroundCompounds52Chart { get; set; }

        public List<XYChart> StabilityConcentrationsRSDChartList { get; set; }
        public BaseChart StabilityConcentrationsChart { get; set; }
        public BaseChart StabilityReagentIonsChart { get; set; }
        public BaseChart StabilityProductIonsChart { get; set; }
        public BaseChart StabilityConcentrationswEOVChart { get; set; }
        public BaseChart StabilityReactionTimeEOVChart { get; set; }
        //Three Phase
        public BaseChart ICFChart { get; set; }

        public BaseChart InjectionScanPosWetChart { get; set; }
        public BaseChart InjectionScanNegWetChart { get; set; }
        public BaseChart InjectionScanNegDryChart { get; set; }

        public List<XYChart> SensitiveChartList { get; set; }
        public List<XYChart> ImpurityChartList { get; set; }
        #endregion

        #region Binding Property
        //Combox - Batch type
        //SPIS
        private bool _spisOvernightSelected;
        public bool SPISOvernightSelected
        {
            get { return _spisOvernightSelected; }
            set
            {
                SetProperty(ref _spisOvernightSelected, value);
                if (NumberofBatches > 2) TwoSelected = true;
                InstrumentBatchType = Global.BatchType.SPIS_Overnight;
            }
        }
        private bool _spisOnOffSelected;
        public bool SPISOnOffSelected
        {
            get { return _spisOnOffSelected; }
            set
            {
                SetProperty(ref _spisOnOffSelected, value);
                OneSelected = true;
                InstrumentBatchType = Global.BatchType.SPIS_OnOff;
            }
        }
        private bool _spisEOVSelected;
        public bool SPISEOVSelected
        {
            get { return _spisEOVSelected; }
            set
            {
                SetProperty(ref _spisEOVSelected, value);
                OneSelected = true;
                InstrumentBatchType = Global.BatchType.SPIS_EOV;
            }
        }
        private bool _spisWeekendSelected;
        public bool SPISWeekendSelected
        {
            get { return _spisWeekendSelected; }
            set
            {
                SetProperty(ref _spisWeekendSelected, value);
                OneSelected = true;
                InstrumentBatchType = Global.BatchType.SPIS_Weekend;
            }
        }
        //DPIS
        private bool _dpisCoarseSelected;
        public bool DPISCoarseSelected
        {
            get { return _dpisCoarseSelected; }
            set
            {
                SetProperty(ref _dpisCoarseSelected, value);
                InstrumentBatchType = Global.BatchType.DPIS_Coarse;
            }
        }
        private bool _dpisFineSelected;
        public bool DPISFineSelected
        {
            get { return _dpisFineSelected; }
            set
            {
                SetProperty(ref _dpisFineSelected, value);
                InstrumentBatchType = Global.BatchType.DPIS_Fine;
            }
        }
        private bool _dpisEOVSelected;
        public bool DPISEOVSelected
        {
            get { return _dpisEOVSelected; }
            set
            {
                SetProperty(ref _dpisEOVSelected, value);
                OneSelected = true;
                InstrumentBatchType = Global.BatchType.DPIS_EOV;
            }
        }
        private bool _dpisColdStartSelected;
        public bool DPISColdStartSelected
        {
            get { return _dpisColdStartSelected; }
            set
            {
                SetProperty(ref _dpisColdStartSelected, value);
                OneSelected = true;
                InstrumentBatchType = Global.BatchType.DPIS_ColdStart;
            }
        }
        private bool _dpisSensAndImpuSelected;
        public bool DPISSensAndImpuSelected
        {
            get { return _dpisSensAndImpuSelected; }
            set
            {
                SetProperty(ref _dpisSensAndImpuSelected, value);
                OneSelected = true;
                InstrumentBatchType = Global.BatchType.DPIS_SensAndImpu;
            }
        }
        //Infinity
        private bool _infinitySelected;
        public bool InfinitySelected
        {
            get { return _infinitySelected; }
            set
            {
                SetProperty(ref _infinitySelected, value);
                InstrumentBatchType = Global.BatchType.Infinity;
            }
        }
        private bool _infinityColdStartSelected;
        public bool InfinityColdStartSelected
        {
            get { return _infinityColdStartSelected; }
            set
            {
                SetProperty(ref _infinityColdStartSelected, value);
                OneSelected = true;
                InstrumentBatchType = Global.BatchType.Infinity_ColdStart;
            }
        }
        private bool _infinityEOBSelected;
        public bool InfinityEOBSelected
        {
            get { return _infinityEOBSelected; }
            set
            {
                SetProperty(ref _infinityEOBSelected, value);
                OneSelected = true;
                InstrumentBatchType = Global.BatchType.Infinity_EOB;
            }
        }
        private bool _infinitySensAndImpuSelected = true;
        public bool InfinitySensAndImpuSelected
        {
            get { return _infinitySensAndImpuSelected; }
            set
            {
                SetProperty(ref _infinitySensAndImpuSelected, value);
                OneSelected = true;
                InstrumentBatchType = Global.BatchType.Infinity_SensAndImpu;
            }
        }
        //Combox - Number of batch 
        private bool _oneSelected = true;
        public bool OneSelected
        {
            get { return _oneSelected; }
            set
            {
                SetProperty(ref _oneSelected, value);
                NumberofBatches = 1;
            }
        }

        private bool _twoSelected;
        public bool TwoSelected
        {
            get { return _twoSelected; }
            set
            {
                SetProperty(ref _twoSelected, value);
                NumberofBatches = 2;
            }
        }

        private bool _threeSelected;
        public bool ThreeSelected
        {
            get { return _threeSelected; }
            set
            {
                SetProperty(ref _threeSelected, value);
                NumberofBatches = 3;
            }
        }

        private bool _fourSelected;
        public bool FourSelected
        {
            get { return _fourSelected; }
            set
            {
                SetProperty(ref _fourSelected, value);
                NumberofBatches = 4;
            }
        }

        private bool _fiveSelected;
        public bool FiveSelected
        {
            get { return _fiveSelected; }
            set
            {
                SetProperty(ref _fiveSelected, value);
                NumberofBatches = 5;
            }
        }
        //DatePicker - Start date
        private DateTime _startDate = DateTime.Now.AddDays(-1);
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }
        //TimePicker - Start time
        private DateTime _startTime = new DateTime(2021, 07, 26, 0, 0, 0);
        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }
        //Textbox - IP address
        private string _ipAddress = "10.0.17.";
        public string IPAddress
        {
            get { return _ipAddress; }
            set { SetProperty(ref _ipAddress, value); }
        }
        //Button - Process button
        private double _processButtonProgress;
        public double ProcessButtonProgress
        {
            get { return _processButtonProgress; }
            set
            {
                SetProperty(ref _processButtonProgress, value);
                //Send event message
                _eventAggregator.GetEvent<Public.Event.MessageEvent>().Publish(_processButtonProgress);
            }
        }

        private bool _showProcessButtonProcess = false;
        public bool ShowProcessButtonProcess
        {
            get => _showProcessButtonProcess;
            set => SetProperty(ref _showProcessButtonProcess, value);
        }
        //Button - Save button
        private bool _showSaveButtonProcess = false;
        public bool ShowSaveButtonProcess
        {
            get => _showSaveButtonProcess;
            set => SetProperty(ref _showSaveButtonProcess, value);
        }

        private string _comments = "";
        public string Comments
        {
            get { return _comments; }
            set { SetProperty(ref _comments, value); }
        }

        private bool _saveScanFilesEnable = false;
        public bool SaveScanFilesEnable
        {
            get => _saveScanFilesEnable;
            set => SetProperty(ref _saveScanFilesEnable, value);
        }
        //LBI - Batched Select Position
        private bool _tailSelected = true;
        public bool TailSelected
        {
            get => _tailSelected;
            set => SetProperty(ref _tailSelected, value);
        }
        //Slider - Match Intensity
        private int _sliderValue = 2;
        public int SliderValue
        {
            get => _sliderValue;
            set => SetProperty(ref _sliderValue, value);
        }
        //Text - Instrument number
        private string _instrumentShowNumebr;
        public string InstrumentShowNumebr
        {
            get { return _instrumentShowNumebr; }
            set { SetProperty(ref _instrumentShowNumebr, value); }
        }
        //Text - Process Time Stemp
        private string _processTimeStemp;
        public string ProcessTimeStemp
        {
            get => _processTimeStemp;
            set => SetProperty(ref _processTimeStemp, value);
        }
        #endregion

        #region Process Command
        public DelegateCommand ProcessCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            ShowProcessButtonProcess = true;
                            ProcessButtonProgress = 0;
                            //Get Scans
                            Action action1 = new Action(() => ProcessButtonProgress += 0.5);
                            Action action2 = new Action(() =>
                            {
                                switch (NumberofBatches)
                                {
                                    case 1:
                                        ProcessButtonProgress += 0.03;
                                        goto case 2;
                                    case 2:
                                        ProcessButtonProgress += 0.03;
                                        goto case 3;
                                    case 3:
                                        ProcessButtonProgress += 0.03;
                                        goto case 4;
                                    case 4:
                                        ProcessButtonProgress += 0.03;
                                        goto case 5;
                                    default:
                                    case 5:
                                        ProcessButtonProgress += 0.03;
                                        break;
                                }
                            });

                            Dictionary<string, List<TargetScanInfo>> TargetScanFileInfoListDic = GetScans.BatchScans(InstrumentBatchType, NumberofBatches, SliderValue, TailSelected, StartDate, StartTime, IPAddress, out batchScansStatus, action1, action2);
                            //Get instrument info
                            instrument = new SyftXML.Scan(Directory.GetFiles("./temp/Batch_Analysis", "*", SearchOption.AllDirectories).First()).GetInstrumentInfo();
                            InstrumentShowNumebr = instrument.Number;
                            //Reset Chart
                            #region Reset Chart
                            ColdStartSensDevChart = null;
                            ColdStartMark3And4DevChart = null;
                            ColdStartBothEndsDevChart = null;
                            ColdStartSensitivetiesChart = null;
                            ColdStartReagentIonsChart = null;

                            SPISOvernightConcentrationsRSDChartOverall = null;
                            SPISOvernightConcentrationsRSDChart = null;
                            SPISOvernightConcentrationsChart = null;
                            SPISOvernightReagentIonsChart = null;
                            SPISOvernightProductIonsChart = null;
                            SPISOvernightQuadStabilityChart = null;
                            SPISOvernightConcentrationsRSDChart2 = null;
                            SPISOvernightConcentrationsChart2 = null;
                            SPISOvernightReagentIonsChart2 = null;
                            SPISOvernightProductIonsChart2 = null;
                            SPISOvernightQuadStabilityChart2 = null;

                            SPISOnOffConcentrationsRSDChart = null;
                            SPISOnOffConcentrationsChart = null;
                            SPISOnOffReagentIonsChart = null;
                            SPISOnOffProductIonsChart = null;
                            SPISOnOffQuadStabilityChart = null;

                            EOVConcentrationsRSDChart = null;
                            EOVConcentrationsChart = null;
                            EOVReagentIonsChart = null;
                            EOVProductIonsChart = null;
                            EOVReactionTimeEOVChart = null;

                            DIPSourceSwitchStabilityReagentRSDChart = null;
                            DIPSourceSwitchStabilityPosWetReagentChart = null;
                            DIPSourceSwitchStabilityNegWetReagentChart = null;
                            DIPSourceSwitchStabilityNegDryReagentChart = null;

                            DPISLongTermStabilityPosWetUPSCurrentChart = null;
                            DPISLongTermStabilityPosWetDWSCurrentChart = null;
                            DPISLongTermStabilityNegWetUPSCurrentChart = null;
                            DPISLongTermStabilityNegWetDWSCurrentChart = null;
                            DPISLongTermStabilityNegDryUPSCurrentChart = null;
                            DPISLongTermStabilityNegDryDWSCurrentChart = null;

                            DPISLongTermStabilityReagentRSDChartList = null;
                            DPISLongTermStabilityPosWetReagentChartList = null;
                            DPISLongTermStabilityPosWetProductChartList = null;
                            DPISLongTermStabilityNegWetReagentChartList = null;
                            DPISLongTermStabilityNegWetProductChartList = null;
                            DPISLongTermStabilityNegDryReagentChartList = null;
                            DPISLongTermStabilityNegDryProductChartList = null;

                            DPISShortTermStabilityReagentRSDChart = null;
                            DPISShortTermStabilityPosWetReagentChart = null;
                            DPISShortTermStabilityNegWetReagentChart = null;
                            DPISShortTermStabilityNegDryReagentChart = null;

                            SourceSwitchAndSettleTimeChart = null;

                            LODsChart = null;

                            BackgroundCompounds75Chart = null;
                            BackgroundCompounds51Chart = null;
                            BackgroundCompounds52Chart = null;

                            StabilityConcentrationsRSDChartList = null;
                            StabilityConcentrationsChart = null;
                            StabilityReagentIonsChart = null;
                            StabilityProductIonsChart = null;
                            StabilityConcentrationswEOVChart = null;
                            StabilityReactionTimeEOVChart = null;

                            ICFChart = null;

                            InjectionScanPosWetChart = null;
                            InjectionScanNegWetChart = null;
                            InjectionScanNegDryChart = null;

                            SensitiveChartList = null;
                            ImpurityChartList = null;
                            #endregion
                            //Get Chart
                            switch (InstrumentBatchType)
                            {
                                case Global.BatchType.Infinity_ColdStart:
                                case Global.BatchType.DPIS_ColdStart:
                                    ProcessButtonProgress = 85;
                                    ColdStartSensDevChart = new DevBarChartInfo(InstrumentBatchType, "Cold Start", "Sensitiveties Deviation", TargetScanFileInfoListDic, DevBarChartInfo.Type.Sens).XYChartList[0]; ProcessButtonProgress += 3;
                                    ColdStartMark3And4DevChart = new DevBarChartInfo(InstrumentBatchType, "Cold Start", "3hr And 4hr Mark Deviation", TargetScanFileInfoListDic, DevBarChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    ColdStartBothEndsDevChart = new DevBarChartInfo(InstrumentBatchType, "Cold Start", "Both Ends Mark Deviation", TargetScanFileInfoListDic, DevBarChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    ColdStartSensitivetiesChart = new SensitiveBarChartInfo(InstrumentBatchType, "Cold Start", "Sensitiveties", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 3;
                                    ColdStartReagentIonsChart = new TraceLineChartInfo(InstrumentBatchType, "Cold Start", "Reagent Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    break;
                                case Global.BatchType.DPIS_SensAndImpu:
                                case Global.BatchType.Infinity_SensAndImpu:
                                    ProcessButtonProgress = 80;
                                    SensitiveChartList = new SensitiveBarChartInfo(InstrumentBatchType, "Sensitiveties and Impurities", "Sensitiveties", TargetScanFileInfoListDic).XYChartList; ProcessButtonProgress += 10;
                                    ImpurityChartList = new ImpurityBarChartInfo(InstrumentBatchType, "Sensitiveties and Impurities", "Impurities", TargetScanFileInfoListDic).XYChartList; ProcessButtonProgress += 10;
                                    break;
                                case Global.BatchType.SPIS_Overnight:
                                    if (NumberofBatches == 1)
                                    {
                                        ProcessButtonProgress = 85;
                                        SPISOvernightConcentrationsRSDChart = new RSDBarChartInfo(InstrumentBatchType, "Overnight", "Concentrations RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                        SPISOvernightConcentrationsChart = new TraceLineChartInfo(InstrumentBatchType, "Overnight", "Concentrations", TargetScanFileInfoListDic, TraceLineChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                        SPISOvernightReagentIonsChart = new TraceLineChartInfo(InstrumentBatchType, "Overnight", "Reagent Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                        SPISOvernightProductIonsChart = new TraceLineChartInfo(InstrumentBatchType, "Overnight", "Product Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                        SPISOvernightQuadStabilityChart = new MassLineChartInfo(InstrumentBatchType, "Overnight", "Quad Stability", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 3;
                                    }
                                    else
                                    {
                                        ProcessButtonProgress = 82;
                                        var rsdChartList = new RSDBarChartInfo(InstrumentBatchType, "Overnight", "Concentrations RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.Conc, false).XYChartList; ProcessButtonProgress += 3;
                                        var concChartList = new TraceLineChartInfo(InstrumentBatchType, "Overnight", "Concentrations", TargetScanFileInfoListDic, TraceLineChartInfo.Type.Conc, false).XYChartList; ProcessButtonProgress += 3;
                                        var reagentChartList = new TraceLineChartInfo(InstrumentBatchType, "Overnight", "Reagent Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS, false).XYChartList; ProcessButtonProgress += 3;
                                        var productChartList = new TraceLineChartInfo(InstrumentBatchType, "Overnight", "Product Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS, false).XYChartList; ProcessButtonProgress += 3;
                                        var quadChartList = new MassLineChartInfo(InstrumentBatchType, "Overnight", "Quad Stability", TargetScanFileInfoListDic, false).XYChartList; ProcessButtonProgress += 3;

                                        SPISOvernightConcentrationsRSDChartOverall = rsdChartList?[0];

                                        SPISOvernightConcentrationsRSDChart = rsdChartList?[1];
                                        SPISOvernightConcentrationsChart = concChartList?[0];
                                        SPISOvernightReagentIonsChart = reagentChartList?[0];
                                        SPISOvernightProductIonsChart = productChartList?[0];
                                        SPISOvernightQuadStabilityChart = quadChartList?[0];

                                        SPISOvernightConcentrationsRSDChart2 = rsdChartList?[2];
                                        SPISOvernightConcentrationsChart2 = concChartList?[1];
                                        SPISOvernightReagentIonsChart2 = reagentChartList?[1];
                                        SPISOvernightProductIonsChart2 = productChartList?[1];
                                        SPISOvernightQuadStabilityChart2 = quadChartList?[1]; ProcessButtonProgress += 3;
                                    }

                                    break;
                                case Global.BatchType.SPIS_OnOff:
                                    ProcessButtonProgress = 85;
                                    SPISOnOffConcentrationsRSDChart = new RSDBarChartInfo(InstrumentBatchType, "On/Off Cycle", "Concentrations RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOnOffConcentrationsChart = new TraceLineChartInfo(InstrumentBatchType, "On/Off Cycle", "Concentrations", TargetScanFileInfoListDic, TraceLineChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOnOffReagentIonsChart = new TraceLineChartInfo(InstrumentBatchType, "On/Off Cycle", "Reagent Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOnOffProductIonsChart = new TraceLineChartInfo(InstrumentBatchType, "On/Off Cycle", "Product Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOnOffQuadStabilityChart = new MassLineChartInfo(InstrumentBatchType, "On/Off Cycle", "Quad Stability", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 3;
                                    break;
                                case Global.BatchType.SPIS_EOV:
                                case Global.BatchType.DPIS_EOV:
                                    ProcessButtonProgress = 85;
                                    EOVConcentrationsRSDChart = new RSDBarChartInfo(InstrumentBatchType, "Effect of Validation", "Concentrations RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    EOVConcentrationsChart = new TraceLineChartInfo(InstrumentBatchType, "Effect of Validation", "Concentrations", TargetScanFileInfoListDic, TraceLineChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    EOVReagentIonsChart = new TraceLineChartInfo(InstrumentBatchType, "Effect of Validation", "Reagent Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    EOVProductIonsChart = new TraceLineChartInfo(InstrumentBatchType, "Effect of Validation", "Product Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    EOVReactionTimeEOVChart = new ReactionTimeLineChartInfo(InstrumentBatchType, "Effect of Validation", "Concentrations", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 3;
                                    break;
                                case Global.BatchType.SPIS_Weekend:
                                    ProcessButtonProgress = 52;
                                    SPISOvernightConcentrationsRSDChartOverall = new RSDBarChartInfo(InstrumentBatchType, "OvernightOverall", "Concentrations RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;

                                    SPISOvernightConcentrationsRSDChart = new RSDBarChartInfo(InstrumentBatchType, "Overnight 1", "Concentrations RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOvernightConcentrationsChart = new TraceLineChartInfo(InstrumentBatchType, "Overnight 1", "Concentrations", TargetScanFileInfoListDic, TraceLineChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOvernightReagentIonsChart = new TraceLineChartInfo(InstrumentBatchType, "Overnight 1", "Reagent Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOvernightProductIonsChart = new TraceLineChartInfo(InstrumentBatchType, "Overnight 1", "Product Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOvernightQuadStabilityChart = new MassLineChartInfo(InstrumentBatchType, "Overnight 1", "Quad Stability", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 3;

                                    SPISOvernightConcentrationsRSDChart2 = new RSDBarChartInfo(InstrumentBatchType, "Overnight 2", "Concentrations RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOvernightConcentrationsChart2 = new TraceLineChartInfo(InstrumentBatchType, "Overnight 2", "Concentrations", TargetScanFileInfoListDic, TraceLineChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOvernightReagentIonsChart2 = new TraceLineChartInfo(InstrumentBatchType, "Overnight 2", "Reagent Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOvernightProductIonsChart2 = new TraceLineChartInfo(InstrumentBatchType, "Overnight 2", "Product Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    SPISOvernightQuadStabilityChart2 = new MassLineChartInfo(InstrumentBatchType, "Overnight 2", "Quad Stability", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 3;

                                    EOVConcentrationsRSDChart = new RSDBarChartInfo(InstrumentBatchType, "Effect of Validation", "Concentrations RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    EOVConcentrationsChart = new TraceLineChartInfo(InstrumentBatchType, "Effect of Validation", "Concentrations", TargetScanFileInfoListDic, TraceLineChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    EOVReagentIonsChart = new TraceLineChartInfo(InstrumentBatchType, "Effect of Validation", "Reagent Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    EOVProductIonsChart = new TraceLineChartInfo(InstrumentBatchType, "Effect of Validation", "Product Ions", TargetScanFileInfoListDic, TraceLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    EOVReactionTimeEOVChart = new ReactionTimeLineChartInfo(InstrumentBatchType, "Effect of Validation", "Concentrations", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 3;
                                    break;
                                case Global.BatchType.DPIS_Coarse:
                                    ProcessButtonProgress = 79;
                                    DPISLongTermStabilityReagentRSDChartList = new RSDBarChartInfo(InstrumentBatchType, "Long Term Stability", "Reagent Ions RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.CPS, false).XYChartList; ProcessButtonProgress += 2;
                                    DPISLongTermStabilityPosWetReagentChartList = new OverlapLineChartInfo(InstrumentBatchType, "Long Term Stability", "PosWet Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS, false).XYChartList; ProcessButtonProgress += 2;
                                    DPISLongTermStabilityPosWetProductChartList = new OverlapLineChartInfo(InstrumentBatchType, "Long Term Stability", "PosWet Product Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS, false).XYChartList; ProcessButtonProgress += 2;
                                    DPISLongTermStabilityNegWetReagentChartList = new OverlapLineChartInfo(InstrumentBatchType, "Long Term Stability", "NegWet Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS, false).XYChartList; ProcessButtonProgress += 2;
                                    DPISLongTermStabilityNegWetProductChartList = new OverlapLineChartInfo(InstrumentBatchType, "Long Term Stability", "NegWet Product Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS, false).XYChartList; ProcessButtonProgress += 2;
                                    DPISLongTermStabilityNegDryReagentChartList = new OverlapLineChartInfo(InstrumentBatchType, "Long Term Stability", "NegDry Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS, false).XYChartList; ProcessButtonProgress += 2;
                                    DPISLongTermStabilityNegDryProductChartList = new OverlapLineChartInfo(InstrumentBatchType, "Long Term Stability", "NegDry Product Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS, false).XYChartList; ProcessButtonProgress += 2;

                                    DPISShortTermStabilityReagentRSDChart = new RSDBarChartInfo(InstrumentBatchType, "Short Term Stability", "Reagent Ions RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DPISShortTermStabilityPosWetReagentChart = new OverlapLineChartInfo(InstrumentBatchType, "Short Term Stability", "PosWet Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DPISShortTermStabilityNegWetReagentChart = new OverlapLineChartInfo(InstrumentBatchType, "Short Term Stability", "NegWet Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DPISShortTermStabilityNegDryReagentChart = new OverlapLineChartInfo(InstrumentBatchType, "Short Term Stability", "NegDry Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 1;

                                    SourceSwitchAndSettleTimeChart = new OverlapLineChartInfo(InstrumentBatchType, "Source Switch and Settle Time", "Source Switch and Settle Time", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 1;

                                    InjectionScanPosWetChart = new InjectionLineChartInfo(InstrumentBatchType, "Injection Scan", "PosWet", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 1;
                                    InjectionScanNegWetChart = new InjectionLineChartInfo(InstrumentBatchType, "Injection Scan", "NegWet", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 1;
                                    InjectionScanNegDryChart = new InjectionLineChartInfo(InstrumentBatchType, "Injection Scan", "NegDry", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress = 100;
                                    break;
                                case Global.BatchType.DPIS_Fine:
                                    ProcessButtonProgress = 64;
                                    SensitiveChartList = new SensitiveBarChartInfo(InstrumentBatchType, "Sensitiveties and Impurities", "Sensitiveties", TargetScanFileInfoListDic, false, false).XYChartList; ProcessButtonProgress += 2;
                                    ImpurityChartList = new ImpurityBarChartInfo(InstrumentBatchType, "Sensitiveties and Impurities", "Impurities", TargetScanFileInfoListDic, false, false).XYChartList; ProcessButtonProgress += 2;

                                    DPISLongTermStabilityPosWetUPSCurrentChart = new CurrentLineChartInfo(InstrumentBatchType, "Long Term Stability", "PosWet Reagent Ions", TargetScanFileInfoListDic, CurrentLineChartInfo.Type.UPS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DPISLongTermStabilityPosWetDWSCurrentChart = new CurrentLineChartInfo(InstrumentBatchType, "Long Term Stability", "PosWet Reagent Ions", TargetScanFileInfoListDic, CurrentLineChartInfo.Type.DWS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DPISLongTermStabilityNegWetUPSCurrentChart = new CurrentLineChartInfo(InstrumentBatchType, "Long Term Stability", "NegWet Reagent Ions", TargetScanFileInfoListDic, CurrentLineChartInfo.Type.UPS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DPISLongTermStabilityNegWetDWSCurrentChart = new CurrentLineChartInfo(InstrumentBatchType, "Long Term Stability", "NegWet Reagent Ions", TargetScanFileInfoListDic, CurrentLineChartInfo.Type.DWS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DPISLongTermStabilityNegDryUPSCurrentChart = new CurrentLineChartInfo(InstrumentBatchType, "Long Term Stability", "NegDry Reagent Ions", TargetScanFileInfoListDic, CurrentLineChartInfo.Type.UPS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DPISLongTermStabilityNegDryDWSCurrentChart = new CurrentLineChartInfo(InstrumentBatchType, "Long Term Stability", "NegDry Reagent Ions", TargetScanFileInfoListDic, CurrentLineChartInfo.Type.DWS).XYChartList[0]; ProcessButtonProgress += 1;

                                    DIPSourceSwitchStabilityReagentRSDChart = new RSDBarChartInfo(InstrumentBatchType, "Source Switch Stability", "Reagent Ions RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DIPSourceSwitchStabilityPosWetReagentChart = new OverlapLineChartInfo(InstrumentBatchType, "Source Switch Stability", "PosWet Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DIPSourceSwitchStabilityNegWetReagentChart = new OverlapLineChartInfo(InstrumentBatchType, "Source Switch Stability", "NegWet Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 1;
                                    DIPSourceSwitchStabilityNegDryReagentChart = new OverlapLineChartInfo(InstrumentBatchType, "Source Switch Stability", "NegDry Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 1;

                                    ICFChart = new ICFLineChartInfo(InstrumentBatchType, "ICF", "ICF", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 1;
                                    goto case Global.BatchType.DPIS_Coarse;
                                case Global.BatchType.Infinity:
                                    ProcessButtonProgress = 70;
                                    SensitiveChartList = new SensitiveBarChartInfo(InstrumentBatchType, "Sensitiveties and Impurities", "Sensitiveties", TargetScanFileInfoListDic, false, false).XYChartList; ProcessButtonProgress += 2;
                                    ImpurityChartList = new ImpurityBarChartInfo(InstrumentBatchType, "Sensitiveties and Impurities", "Impurities", TargetScanFileInfoListDic, false, false).XYChartList; ProcessButtonProgress += 2;

                                    StabilityConcentrationsRSDChartList = new RSDBarChartInfo(InstrumentBatchType, "Stability", "Concentrations RSD", TargetScanFileInfoListDic, RSDBarChartInfo.Type.Conc, false).XYChartList; ProcessButtonProgress += 3;
                                    StabilityConcentrationsChart = new TraceLineChartInfo(InstrumentBatchType, "Stability", "Concentrations", TargetScanFileInfoListDic, TraceLineChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    StabilityReagentIonsChart = new OverlapLineChartInfo(InstrumentBatchType, "Stability", "Reagent Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    StabilityProductIonsChart = new OverlapLineChartInfo(InstrumentBatchType, "Stability", "Product Ions", TargetScanFileInfoListDic, OverlapLineChartInfo.Type.CPS).XYChartList[0]; ProcessButtonProgress += 3;
                                    StabilityConcentrationswEOVChart = new TraceLineChartInfo(InstrumentBatchType, "Stability", "Concentrations wEOV", TargetScanFileInfoListDic, TraceLineChartInfo.Type.Conc).XYChartList[0]; ProcessButtonProgress += 3;
                                    StabilityReactionTimeEOVChart = new ReactionTimeLineChartInfo(InstrumentBatchType, "Stability", "Concentrations wEOV", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 3;

                                    LODsChart = new LODsBarChartInfo(InstrumentBatchType, "LODs", "LODs", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 2;

                                    InjectionScanPosWetChart = new InjectionLineChartInfo(InstrumentBatchType, "Injection Scan", "PosWet", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 1;
                                    InjectionScanNegWetChart = new InjectionLineChartInfo(InstrumentBatchType, "Injection Scan", "NegWet", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 1;
                                    InjectionScanNegDryChart = new InjectionLineChartInfo(InstrumentBatchType, "Injection Scan", "NegDry", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 1;

                                    ICFChart = new ICFLineChartInfo(InstrumentBatchType, "ICF", "ICF", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 1;

                                    BackgroundCompounds75Chart = new AConcBarChartInfo(InstrumentBatchType, "Background", "75 Compounds", TargetScanFileInfoListDic, AConcBarChartInfo.Type.Compounds75).XYChartList[0]; ProcessButtonProgress += 1;
                                    BackgroundCompounds51Chart = new AConcBarChartInfo(InstrumentBatchType, "Background", "51 Compounds", TargetScanFileInfoListDic, AConcBarChartInfo.Type.Compounds51_52).XYChartList[0]; ProcessButtonProgress += 1;
                                    BackgroundCompounds52Chart = new AConcBarChartInfo(InstrumentBatchType, "Background", "52 Compounds", TargetScanFileInfoListDic, AConcBarChartInfo.Type.Compounds51_52).XYChartList[0]; ProcessButtonProgress = 100;
                                    break;
                                case Global.BatchType.Infinity_EOB:
                                    ProcessButtonProgress = 70;
                                    SensitiveChartList = new SensitiveBarChartInfo(InstrumentBatchType, "Sensitiveties and Impurities", "Sensitiveties", TargetScanFileInfoListDic, true, true).XYChartList; ProcessButtonProgress += 10;
                                    ImpurityChartList = new ImpurityBarChartInfo(InstrumentBatchType, "Sensitiveties and Impurities", "Impurities", TargetScanFileInfoListDic, true, true).XYChartList; ProcessButtonProgress += 10;
                                    LODsChart = new LODsBarChartInfo(InstrumentBatchType, "LODs", "LODs", TargetScanFileInfoListDic).XYChartList[0]; ProcessButtonProgress += 10;
                                    break;
                                default:
                                    break;
                            }
                            ProcessButtonProgress = 0;
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
                });
            }
        }
        #endregion

        #region Save Command
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    //Close popup dialog
                    PopupBox.ClosePopupCommand.Execute(new object(), null);
                    //Open folder path selection dialog
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

                                if (SaveScanFilesEnable)
                                {
                                    //Now Create all of the directories
                                    foreach (string dirPath in Directory.GetDirectories("./temp/Batch_Analysis", "*", SearchOption.AllDirectories))
                                        Directory.CreateDirectory(dirPath.Replace("./temp/Batch_Analysis", folderDlg.FileName));

                                    //Copy all the files & Replaces any files with the same name
                                    foreach (string newPath in Directory.GetFiles("./temp/Batch_Analysis", "*.*", SearchOption.AllDirectories))
                                        File.Copy(newPath, newPath.Replace("./temp/Batch_Analysis", folderDlg.FileName), true);
                                }
                                PdfDocument pdf;
                                Document doc;
                                switch (InstrumentBatchType)
                                {
                                    //SPIS
                                    case Global.BatchType.SPIS_Overnight:
                                        if (NumberofBatches == 1)
                                        {
                                            pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_SPIS Overnight Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                            doc = new Document(pdf, PageSize.A4);
                                            doc.SetMargins(30, 10, 30, 10);
                                            PDFGenerator.PDFHead(ref doc, $"P3_SPIS Report\nOvernight");
                                            PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                            PDFGenerator.ChartTable(ref doc, true, "Overnight", 1,
                                                SPISOvernightConcentrationsRSDChart.SavePNGChart(),
                                                SPISOvernightConcentrationsChart.SavePNGChart(),
                                                SPISOvernightReagentIonsChart.SavePNGChart(),
                                                SPISOvernightProductIonsChart.SavePNGChart(),
                                                SPISOvernightQuadStabilityChart.SavePNGChart()
                                                );
                                            doc.Close();
                                        }
                                        else
                                        {
                                            pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_SPIS Double Overnight Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                            doc = new Document(pdf, PageSize.A4);
                                            doc.SetMargins(30, 10, 30, 10);
                                            PDFGenerator.PDFHead(ref doc, $"P3_SPIS Report\nDouble Overnight");
                                            PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                            PDFGenerator.ChartTable_SPISDoubleOvernight(ref doc, true,
                                                SPISOvernightConcentrationsRSDChartOverall.SavePNGChart(),
                                                SPISOvernightConcentrationsRSDChart.SavePNGChart(),
                                                SPISOvernightConcentrationsChart.SavePNGChart(),
                                                SPISOvernightReagentIonsChart.SavePNGChart(),
                                                SPISOvernightProductIonsChart.SavePNGChart(),
                                                SPISOvernightQuadStabilityChart.SavePNGChart(),
                                                SPISOvernightConcentrationsRSDChart2.SavePNGChart(),
                                                SPISOvernightConcentrationsChart2.SavePNGChart(),
                                                SPISOvernightReagentIonsChart2.SavePNGChart(),
                                                SPISOvernightProductIonsChart2.SavePNGChart(),
                                                SPISOvernightQuadStabilityChart2.SavePNGChart()
                                            );
                                            doc.Close();
                                        }
                                        break;
                                    case Global.BatchType.SPIS_OnOff:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_SPIS OnOff Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_SPIS Report\nOn/Off Cycle");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable(ref doc, true, "On/Off Cycle", 1,
                                            SPISOnOffConcentrationsRSDChart.SavePNGChart(),
                                            SPISOnOffConcentrationsChart.SavePNGChart(),
                                            SPISOnOffReagentIonsChart.SavePNGChart(),
                                            SPISOnOffProductIonsChart.SavePNGChart(),
                                            SPISOnOffQuadStabilityChart.SavePNGChart()
                                            );
                                        doc.Close();
                                        break;
                                    case Global.BatchType.SPIS_EOV:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_SPIS EOV Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_SPIS Report\nEffect of Validation");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable(ref doc, true, "Effect of Validation", 1,
                                            EOVConcentrationsRSDChart.SavePNGChart(),
                                            EOVConcentrationsChart.SavePNGChart(),
                                            EOVReagentIonsChart.SavePNGChart(),
                                            EOVProductIonsChart.SavePNGChart(),
                                            EOVReactionTimeEOVChart.SavePNGChart()
                                            );
                                        doc.Close();
                                        break;
                                    case Global.BatchType.SPIS_Weekend:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_SPIS Weekend Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_SPIS Report\nWeekend");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable_SPISDoubleOvernight(ref doc, true,
                                            SPISOvernightConcentrationsRSDChartOverall.SavePNGChart(),
                                            SPISOvernightConcentrationsRSDChart.SavePNGChart(),
                                            SPISOvernightConcentrationsChart.SavePNGChart(),
                                            SPISOvernightReagentIonsChart.SavePNGChart(),
                                            SPISOvernightProductIonsChart.SavePNGChart(),
                                            SPISOvernightQuadStabilityChart.SavePNGChart(),
                                            SPISOvernightConcentrationsRSDChart2.SavePNGChart(),
                                            SPISOvernightConcentrationsChart2.SavePNGChart(),
                                            SPISOvernightReagentIonsChart2.SavePNGChart(),
                                            SPISOvernightProductIonsChart2.SavePNGChart(),
                                            SPISOvernightQuadStabilityChart2.SavePNGChart()
                                        );
                                        PDFGenerator.ChartTable(ref doc, true, "Effect of Validation", 1,
                                            EOVConcentrationsRSDChart.SavePNGChart(),
                                            EOVConcentrationsChart.SavePNGChart(),
                                            EOVReagentIonsChart.SavePNGChart(),
                                            EOVProductIonsChart.SavePNGChart(),
                                            EOVReactionTimeEOVChart.SavePNGChart());
                                        doc.Close();
                                        break;
                                    //DPIS
                                    case Global.BatchType.DPIS_Coarse:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_DPIS_Coarse ({NumberofBatches}Bs) Overnight Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_DPIS Report\nCoarse Overnight ({NumberofBatches}Bs)");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable_DPISLongTerm(ref doc, true,
                                            DPISLongTermStabilityReagentRSDChartList.SavePNGChart(),
                                            DPISLongTermStabilityPosWetReagentChartList.SavePNGChart(),
                                            DPISLongTermStabilityPosWetProductChartList.SavePNGChart(),
                                            DPISLongTermStabilityNegWetReagentChartList.SavePNGChart(),
                                            DPISLongTermStabilityNegWetProductChartList.SavePNGChart(),
                                            DPISLongTermStabilityNegDryReagentChartList.SavePNGChart(),
                                            DPISLongTermStabilityNegDryProductChartList.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, true, "Short Term Stability", NumberofBatches,
                                            DPISShortTermStabilityReagentRSDChart.SavePNGChart(),
                                            DPISShortTermStabilityPosWetReagentChart.SavePNGChart(),
                                            DPISShortTermStabilityNegWetReagentChart.SavePNGChart(),
                                            DPISShortTermStabilityNegDryReagentChart.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, true, "Source Switch & Settle Time", NumberofBatches, SourceSwitchAndSettleTimeChart.SavePNGChart());
                                        PDFGenerator.ChartTable(ref doc, false, "Injection Scan", NumberofBatches,
                                            InjectionScanPosWetChart.SavePNGChart(),
                                            InjectionScanNegWetChart.SavePNGChart(),
                                            InjectionScanNegDryChart.SavePNGChart()
                                            );
                                        doc.Close();
                                        break;
                                    case Global.BatchType.DPIS_Fine:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_DPIS_Fine ({NumberofBatches}Bs) Overnight Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_DPIS Report\nFine Overnight ({NumberofBatches}Bs)");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable_SenAndImp(ref doc, true, NumberofBatches, SensitiveChartList.SavePNGChart(), ImpurityChartList.SavePNGChart());
                                        PDFGenerator.ChartTable_DPISLongTerm(ref doc, true,
                                            DPISLongTermStabilityReagentRSDChartList.SavePNGChart(),
                                            DPISLongTermStabilityPosWetReagentChartList.SavePNGChart(),
                                            DPISLongTermStabilityPosWetProductChartList.SavePNGChart(),
                                            DPISLongTermStabilityNegWetReagentChartList.SavePNGChart(),
                                            DPISLongTermStabilityNegWetProductChartList.SavePNGChart(),
                                            DPISLongTermStabilityNegDryReagentChartList.SavePNGChart(),
                                            DPISLongTermStabilityNegDryProductChartList.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, true, "Long Term Current", NumberofBatches,
                                            DPISLongTermStabilityPosWetUPSCurrentChart.SavePNGChart(),
                                            DPISLongTermStabilityPosWetDWSCurrentChart.SavePNGChart(),
                                            DPISLongTermStabilityNegWetUPSCurrentChart.SavePNGChart(),
                                            DPISLongTermStabilityNegWetDWSCurrentChart.SavePNGChart(),
                                            DPISLongTermStabilityNegDryUPSCurrentChart.SavePNGChart(),
                                            DPISLongTermStabilityNegDryDWSCurrentChart.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, true, "Short Term Stability", NumberofBatches,
                                            DPISShortTermStabilityReagentRSDChart.SavePNGChart(),
                                            DPISShortTermStabilityPosWetReagentChart.SavePNGChart(),
                                            DPISShortTermStabilityNegWetReagentChart.SavePNGChart(),
                                            DPISShortTermStabilityNegDryReagentChart.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, true, "Source Switch Stability", NumberofBatches,
                                            DIPSourceSwitchStabilityReagentRSDChart.SavePNGChart(),
                                            DIPSourceSwitchStabilityPosWetReagentChart.SavePNGChart(),
                                            DIPSourceSwitchStabilityNegWetReagentChart.SavePNGChart(),
                                            DIPSourceSwitchStabilityNegDryReagentChart.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, false, "Source Switch & Settle Time", NumberofBatches, SourceSwitchAndSettleTimeChart.SavePNGChart());
                                        PDFGenerator.ChartTable(ref doc, true, "Injection Scan", NumberofBatches,
                                            InjectionScanPosWetChart.SavePNGChart(),
                                            InjectionScanNegWetChart.SavePNGChart(),
                                            InjectionScanNegDryChart.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, false, "ICF", NumberofBatches, ICFChart.SavePNGChart());
                                        doc.Close();
                                        break;
                                    case Global.BatchType.DPIS_EOV:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_DPIS EOV Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_DPIS Report\nEffect of Validation");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable(ref doc, true, "Effect of Validation", 1,
                                            EOVConcentrationsRSDChart.SavePNGChart(),
                                            EOVConcentrationsChart.SavePNGChart(),
                                            EOVReagentIonsChart.SavePNGChart(),
                                            EOVProductIonsChart.SavePNGChart(),
                                            EOVReactionTimeEOVChart.SavePNGChart()
                                            );
                                        doc.Close();
                                        break;
                                    case Global.BatchType.DPIS_ColdStart:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_DPIS Cold Start Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_DPIS Report\nCold Start");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable(ref doc, true, "Cold Start", 1,
                                            ColdStartSensDevChart.SavePNGChart(),
                                            ColdStartMark3And4DevChart.SavePNGChart(),
                                            ColdStartBothEndsDevChart.SavePNGChart(),
                                            ColdStartSensitivetiesChart.SavePNGChart(),
                                            ColdStartReagentIonsChart.SavePNGChart()
                                            );
                                        doc.Close();
                                        break;
                                    //Infinity
                                    case Global.BatchType.Infinity:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_Infinity ({NumberofBatches}Bs) Overnight Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_Infinity Report\nOvernight ({NumberofBatches}Bs)");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable_SenAndImp(ref doc, true, NumberofBatches, SensitiveChartList.SavePNGChart(), ImpurityChartList.SavePNGChart());
                                        PDFGenerator.ChartTable_InfinityStability(ref doc, true, NumberofBatches,
                                            StabilityConcentrationsRSDChartList.SavePNGChart(),
                                            StabilityConcentrationsChart.SavePNGChart(),
                                            StabilityReagentIonsChart.SavePNGChart(),
                                            StabilityProductIonsChart.SavePNGChart(),
                                            StabilityConcentrationswEOVChart.SavePNGChart(),
                                            StabilityReactionTimeEOVChart.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, true, "LODs", NumberofBatches, LODsChart.SavePNGChart());
                                        PDFGenerator.ChartTable(ref doc, false, "Background", NumberofBatches,
                                            BackgroundCompounds75Chart.SavePNGChart(),
                                            BackgroundCompounds51Chart.SavePNGChart(),
                                            BackgroundCompounds52Chart.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, true, "Injection Scan", NumberofBatches,
                                            InjectionScanPosWetChart.SavePNGChart(),
                                            InjectionScanNegWetChart.SavePNGChart(),
                                            InjectionScanNegDryChart.SavePNGChart()
                                            );
                                        PDFGenerator.ChartTable(ref doc, false, "ICF", NumberofBatches, ICFChart.SavePNGChart());
                                        doc.Close();
                                        break;
                                    case Global.BatchType.Infinity_ColdStart:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_Infinity Cold Start Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_Infinity Report\nCold Start");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable(ref doc, true, "Cold Start", 1,
                                            ColdStartSensDevChart.SavePNGChart(),
                                            ColdStartMark3And4DevChart.SavePNGChart(),
                                            ColdStartBothEndsDevChart.SavePNGChart(),
                                            ColdStartSensitivetiesChart.SavePNGChart(),
                                            ColdStartReagentIonsChart.SavePNGChart()
                                            );
                                        doc.Close();
                                        break;
                                    case Global.BatchType.Infinity_EOB:
                                        pdf = new PdfDocument(new PdfWriter($"{folderDlg.FileName}/{instrument.Number}_{Global.UserName}_Infinity End of Benchmark Report_{DateTime.Now.ToString("yMMdd HHmmss")}.pdf"));
                                        doc = new Document(pdf, PageSize.A4);
                                        doc.SetMargins(30, 10, 30, 10);
                                        PDFGenerator.PDFHead(ref doc, $"P3_Infinity Report\nEnd of Benchmark ({NumberofBatches}Bs)");
                                        PDFGenerator.PDFInstrumentInfo(ref doc, instrument, Comments);
                                        PDFGenerator.ChartTable_SenAndImp(ref doc, true, NumberofBatches, SensitiveChartList.SavePNGChart(), ImpurityChartList.SavePNGChart());
                                        PDFGenerator.ChartTable(ref doc, false, "LODs", NumberofBatches, LODsChart.SavePNGChart());
                                        doc.Close();
                                        break;
                                    default:
                                        break;
                                }

                                //Delete temp/Setting_Check
                                if (Directory.Exists("./temp/Batch_Analysis/chart_temp"))
                                    new DirectoryInfo("./temp/Batch_Analysis/chart_temp").Delete(true);

                                ShowSaveButtonProcess = false;
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show($"{ex.Message}", "ERROR");
                                ShowSaveButtonProcess = false;
                            }
                        });
                    }
                });
            }
        }
        #endregion
    }
}
