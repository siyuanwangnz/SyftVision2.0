using ChartConfig.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;

namespace ChartConfig.ViewModels
{
    public class ChartConfigViewModel : BindableBase
    {
        public ChartConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            ChartTypeList = new ObservableCollection<ChartType> {
                new ChartType("Sensitivities",new Component(true)),
                new ChartType("Impurities",new Component(true)),
                new ChartType("LODs_Conc",new Component(true)),
                new ChartType("LODs_AConc",new Component(true,Component.Type.CompoundOnly)),
                new ChartType("AConc",new Component(true,Component.Type.CompoundOnly)),

                new ChartType("RSD_Conc",new Component(true)),
                new ChartType("RSD_CPS",new Component(true)),

                new ChartType("DEV_CPS",new Component(true)),
                new ChartType("DEV_Conc",new Component(true)),

                new ChartType("Overlap_Conc",new Component(false)),
                new ChartType("Overlap_CPS",new Component(false)),
                new ChartType("Trace_Conc",new Component(false)),
                new ChartType("Trace_CPS",new Component(false)),

                new ChartType("Current_UPS",new Component(false,Component.Type.ReagentOnly)),
                new ChartType("Current_DWS",new Component(false,Component.Type.ReagentOnly)),
                new ChartType("Injection",new Component(false)),
                new ChartType("ReactionTime",new Component(false)),
                new ChartType("Mass",new Component(false))
            };

        }

        private readonly IEventAggregator _eventAggregator;

        private readonly IRegionManager _regionManager;

        #region Tool Bar
        private ObservableCollection<ChartType> _chartTypeList;
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

        public DelegateCommand NewCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedChartType != null)
                    {
                        ComponentsList = new ObservableCollection<Component> { };
                        ComponentsList.Add(SelectedChartType.Component);
                    }
                });
            }
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
                    ComponentsList.Insert(ComponentsList.IndexOf(SelectedComponent), SelectedChartType.Component);
                });
            }
        }
        public DelegateCommand AddDown
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentsList.Insert(ComponentsList.IndexOf(SelectedComponent) + 1, SelectedChartType.Component);
                });
            }
        }
        #endregion

    }
}
