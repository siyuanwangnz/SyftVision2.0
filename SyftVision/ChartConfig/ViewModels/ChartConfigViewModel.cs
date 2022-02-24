using ChartConfig.Models;
using ChartDirector;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Public.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ChartConfig.ViewModels
{
    public class ChartConfigViewModel : BindableBase
    {
        public ChartConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            ChartTypeList = new ObservableCollection<string> {
            "1",
            "2",
            "3",
            "4",
            "5"

            };
        }

        private readonly IEventAggregator _eventAggregator;

        private readonly IRegionManager _regionManager;

        private ObservableCollection<string> _chartTypeList;
        public ObservableCollection<string> ChartTypeList
        {
            get { return _chartTypeList; }
            set { SetProperty(ref _chartTypeList, value); }
        }

        private string _selectedChartType;
        public string SelectedChartType
        {
            get { return _selectedChartType; }
            set
            {
                SetProperty(ref _selectedChartType, value);
                if (_selectedChartType == "1")
                {
                    NavigationParameters param = new NavigationParameters();
                    _regionManager.RequestNavigate("ComponentsContentRegion", "ACComponentsView", param);
                }
                else if (_selectedChartType == "2")
                {
                    NavigationParameters param = new NavigationParameters();
                    _regionManager.RequestNavigate("ComponentsContentRegion", "RPComponentsView", param);
                }
            }
        }

    }
}
