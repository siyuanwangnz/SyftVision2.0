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

            ChartTypeList = new ObservableCollection<ChartProp> {
                new ChartProp(ChartProp.Type.Bar,"Sensitivities"),
                new ChartProp(ChartProp.Type.Bar,"Impurities"),
                new ChartProp(ChartProp.Type.Bar,"LODs_Conc"),
                new ChartProp(ChartProp.Type.Bar,"LODs_AConc"),
                new ChartProp(ChartProp.Type.Bar,"AConc"),

                new ChartProp(ChartProp.Type.Bar,"RSD_Conc"),
                new ChartProp(ChartProp.Type.Bar,"RSD_CPS"),

                new ChartProp(ChartProp.Type.Bar,"DEV_CPS"),
                new ChartProp(ChartProp.Type.Bar,"DEV_Conc"),

                new ChartProp(ChartProp.Type.Line,"Overlap_Conc"),
                new ChartProp(ChartProp.Type.Line,"Overlap_CPS"),
                new ChartProp(ChartProp.Type.Line,"Trace_Conc"),
                new ChartProp(ChartProp.Type.Line,"Trace_CPS"),

                new ChartProp(ChartProp.Type.Line,"Current_UPS"),
                new ChartProp(ChartProp.Type.Line,"Current_DWS"),
                new ChartProp(ChartProp.Type.Line,"Injection"),
                new ChartProp(ChartProp.Type.Line,"ReactionTime"),
                new ChartProp(ChartProp.Type.Line,"Mass")
            };
        }

        private readonly IEventAggregator _eventAggregator;

        private readonly IRegionManager _regionManager;

        private ObservableCollection<ChartProp> _chartTypeList;
        public ObservableCollection<ChartProp> ChartTypeList
        {
            get { return _chartTypeList; }
            set { SetProperty(ref _chartTypeList, value); }
        }

        private ChartProp _selectedChartType;
        public ChartProp SelectedChartType
        {
            get { return _selectedChartType; }
            set { SetProperty(ref _selectedChartType, value); }
        }

    }
}
