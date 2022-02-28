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
        public ChartConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _sftpServices = new SFTPServices("tools.syft.com", "22", "sftp", "MuhPEzxNchfr8nyZ");
        }

        #region Tool Bar
        public DelegateCommand OpenCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    _dialogService.ShowDialog("OpenDialogView");
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

                    string localPath = "./Temp/ChartConfig/";
                    string remotePath = "/home/sftp/files/syft-vision2/ChartConfig/" + Tittle + "/";
                    string fileName = SubTittle + ".xml";

                    // Create local file
                    if (!Directory.Exists(localPath)) Directory.CreateDirectory(localPath);
                    ChartProp chartProp = new ChartProp(SelectedChartType, Tittle, SubTittle, SelectedExpectedRange, SelectedPhase, ComponentsList);
                    chartProp.XMLGeneration().Save(localPath + fileName);

                    // Upload file
                    _sftpServices.Connect();
                    if (!_sftpServices.Exist(remotePath)) _sftpServices.CreateDirectory(remotePath);
                    _sftpServices.UploadFile(remotePath + fileName, localPath + fileName);
                    _sftpServices.Disconnect();

                    MessageBox.Show("Chart has been saved", "INFO");
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
