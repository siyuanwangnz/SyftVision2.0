using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChartDirector;
using Prism.Mvvm;
using Prism.Regions;

namespace OvernightScan.ViewModels.DPISCommon
{
    class LongTermStabilityViewModel : BindableBase, INavigationAware
    {
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            switch (navigationContext.Parameters.GetValue<int>("NumberofBatches"))
            {
                default:
                case 1:
                    TabItemName = "Selected Batch";
                    TabItemOverallVisibility = Visibility.Collapsed;
                    TabItem2Visibility = Visibility.Collapsed;
                    TabItem3Visibility = Visibility.Collapsed;
                    TabItem4Visibility = Visibility.Collapsed;
                    TabItem5Visibility = Visibility.Collapsed;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?.Count == 1
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?.Count == 1
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?.Count == 1
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?.Count == 1
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?.Count == 1
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?.Count == 1
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?.Count == 1)
                    { 
                        ReagentRSDChart = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[0];
                        PosWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[0];
                        PosWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[0];
                        NegWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[0];
                        NegWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[0];
                        NegDryReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[0];
                        NegDryProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[0];
                    }
                    
                    break;
                case 2:
                    TabItemName = "Batch 1";
                    TabItemOverallVisibility = Visibility.Visible;
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Collapsed;
                    TabItem4Visibility = Visibility.Collapsed;
                    TabItem5Visibility = Visibility.Collapsed;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?.Count == 3
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?.Count == 2
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?.Count == 2
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?.Count == 2
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?.Count == 2
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?.Count == 2
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?.Count == 2)
                    {
                        ReagentRSDChartOverall = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[0];

                        ReagentRSDChart = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[1];
                        PosWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[0];
                        PosWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[0];
                        NegWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[0];
                        NegWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[0];
                        NegDryReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[0];
                        NegDryProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[0];

                        ReagentRSDChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[2];
                        PosWetReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[1];
                        PosWetProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[1];
                        NegWetReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[1];
                        NegWetProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[1];
                        NegDryReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[1];
                        NegDryProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[1];
                    }
                    break;
                case 3:
                    TabItemName = "Batch 1";
                    TabItemOverallVisibility = Visibility.Visible;
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Visible;
                    TabItem4Visibility = Visibility.Collapsed;
                    TabItem5Visibility = Visibility.Collapsed;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?.Count == 4
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?.Count == 3
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?.Count == 3
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?.Count == 3
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?.Count == 3
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?.Count == 3
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?.Count == 3)
                    {
                        ReagentRSDChartOverall = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[0];

                        ReagentRSDChart = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[1];
                        PosWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[0];
                        PosWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[0];
                        NegWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[0];
                        NegWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[0];
                        NegDryReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[0];
                        NegDryProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[0];

                        ReagentRSDChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[2];
                        PosWetReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[1];
                        PosWetProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[1];
                        NegWetReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[1];
                        NegWetProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[1];
                        NegDryReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[1];
                        NegDryProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[1];

                        ReagentRSDChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[3];
                        PosWetReagentChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[2];
                        PosWetProductChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[2];
                        NegWetReagentChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[2];
                        NegWetProductChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[2];
                        NegDryReagentChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[2];
                        NegDryProductChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[2];
                    }
                    break;
                case 4:
                    TabItemName = "Batch 1";
                    TabItemOverallVisibility = Visibility.Visible;
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Visible;
                    TabItem4Visibility = Visibility.Visible;
                    TabItem5Visibility = Visibility.Collapsed;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?.Count == 5
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?.Count == 4
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?.Count == 4
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?.Count == 4
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?.Count == 4
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?.Count == 4
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?.Count == 4)
                    {
                        ReagentRSDChartOverall = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[0];

                        ReagentRSDChart = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[1];
                        PosWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[0];
                        PosWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[0];
                        NegWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[0];
                        NegWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[0];
                        NegDryReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[0];
                        NegDryProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[0];

                        ReagentRSDChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[2];
                        PosWetReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[1];
                        PosWetProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[1];
                        NegWetReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[1];
                        NegWetProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[1];
                        NegDryReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[1];
                        NegDryProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[1];

                        ReagentRSDChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[3];
                        PosWetReagentChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[2];
                        PosWetProductChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[2];
                        NegWetReagentChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[2];
                        NegWetProductChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[2];
                        NegDryReagentChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[2];
                        NegDryProductChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[2];

                        ReagentRSDChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[4];
                        PosWetReagentChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[3];
                        PosWetProductChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[3];
                        NegWetReagentChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[3];
                        NegWetProductChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[3];
                        NegDryReagentChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[3];
                        NegDryProductChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[3];
                    }
                    break;
                case 5:
                    TabItemName = "Batch 1";
                    TabItemOverallVisibility = Visibility.Visible;
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Visible;
                    TabItem4Visibility = Visibility.Visible;
                    TabItem5Visibility = Visibility.Visible;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?.Count == 6
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?.Count == 5
                        && navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?.Count == 5
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?.Count == 5
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?.Count == 5
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?.Count == 5
                        && navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?.Count == 5)
                    {
                        ReagentRSDChartOverall = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[0];

                        ReagentRSDChart = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[1];
                        PosWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[0];
                        PosWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[0];
                        NegWetReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[0];
                        NegWetProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[0];
                        NegDryReagentChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[0];
                        NegDryProductChart = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[0];

                        ReagentRSDChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[2];
                        PosWetReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[1];
                        PosWetProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[1];
                        NegWetReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[1];
                        NegWetProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[1];
                        NegDryReagentChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[1];
                        NegDryProductChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[1];

                        ReagentRSDChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[3];
                        PosWetReagentChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[2];
                        PosWetProductChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[2];
                        NegWetReagentChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[2];
                        NegWetProductChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[2];
                        NegDryReagentChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[2];
                        NegDryProductChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[2];

                        ReagentRSDChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[4];
                        PosWetReagentChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[3];
                        PosWetProductChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[3];
                        NegWetReagentChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[3];
                        NegWetProductChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[3];
                        NegDryReagentChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[3];
                        NegDryProductChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[3];

                        ReagentRSDChart5 = navigationContext.Parameters.GetValue<List<XYChart>>("ReagentRSDChartList")?[5];
                        PosWetReagentChart5 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetReagentChartList")?[4];
                        PosWetProductChart5 = navigationContext.Parameters.GetValue<List<XYChart>>("PosWetProductChartList")?[4];
                        NegWetReagentChart5 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetReagentChartList")?[4];
                        NegWetProductChart5 = navigationContext.Parameters.GetValue<List<XYChart>>("NegWetProductChartList")?[4];
                        NegDryReagentChart5 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryReagentChartList")?[4];
                        NegDryProductChart5 = navigationContext.Parameters.GetValue<List<XYChart>>("NegDryProductChartList")?[4];
                    }
                    break;
            }
        }

        #region Binding Property
        private string _tabItemName;
        public string TabItemName
        {
            get { return _tabItemName; }
            set { SetProperty(ref _tabItemName, value); }
        }
        private System.Windows.Visibility _tabItemOverallVisibility;
        public System.Windows.Visibility TabItemOverallVisibility
        {
            get => _tabItemOverallVisibility;
            set => SetProperty(ref _tabItemOverallVisibility, value);
        }
        private System.Windows.Visibility _tabItem2Visibility;
        public System.Windows.Visibility TabItem2Visibility
        {
            get => _tabItem2Visibility;
            set => SetProperty(ref _tabItem2Visibility, value);
        }
        private System.Windows.Visibility _tabItem3Visibility;
        public System.Windows.Visibility TabItem3Visibility
        {
            get => _tabItem3Visibility;
            set => SetProperty(ref _tabItem3Visibility, value);
        }
        private System.Windows.Visibility _tabItem4Visibility;
        public System.Windows.Visibility TabItem4Visibility
        {
            get => _tabItem4Visibility;
            set => SetProperty(ref _tabItem4Visibility, value);
        }
        private System.Windows.Visibility _tabItem5Visibility;
        public System.Windows.Visibility TabItem5Visibility
        {
            get => _tabItem5Visibility;
            set => SetProperty(ref _tabItem5Visibility, value);
        }
        #endregion

        #region Chart property
        public BaseChart ReagentRSDChartOverall { get; set; }

        public BaseChart ReagentRSDChart { get; set; }
        public BaseChart PosWetReagentChart { get; set; }
        public BaseChart PosWetProductChart { get; set; }
        public BaseChart NegWetReagentChart { get; set; }
        public BaseChart NegWetProductChart { get; set; }
        public BaseChart NegDryReagentChart { get; set; }
        public BaseChart NegDryProductChart { get; set; }

        public BaseChart ReagentRSDChart2 { get; set; }
        public BaseChart PosWetReagentChart2 { get; set; }
        public BaseChart PosWetProductChart2 { get; set; }
        public BaseChart NegWetReagentChart2 { get; set; }
        public BaseChart NegWetProductChart2 { get; set; }
        public BaseChart NegDryReagentChart2 { get; set; }
        public BaseChart NegDryProductChart2 { get; set; }

        public BaseChart ReagentRSDChart3 { get; set; }
        public BaseChart PosWetReagentChart3 { get; set; }
        public BaseChart PosWetProductChart3 { get; set; }
        public BaseChart NegWetReagentChart3 { get; set; }
        public BaseChart NegWetProductChart3 { get; set; }
        public BaseChart NegDryReagentChart3 { get; set; }
        public BaseChart NegDryProductChart3 { get; set; }

        public BaseChart ReagentRSDChart4 { get; set; }
        public BaseChart PosWetReagentChart4 { get; set; }
        public BaseChart PosWetProductChart4 { get; set; }
        public BaseChart NegWetReagentChart4 { get; set; }
        public BaseChart NegWetProductChart4 { get; set; }
        public BaseChart NegDryReagentChart4 { get; set; }
        public BaseChart NegDryProductChart4 { get; set; }

        public BaseChart ReagentRSDChart5 { get; set; }
        public BaseChart PosWetReagentChart5 { get; set; }
        public BaseChart PosWetProductChart5 { get; set; }
        public BaseChart NegWetReagentChart5 { get; set; }
        public BaseChart NegWetProductChart5 { get; set; }
        public BaseChart NegDryReagentChart5 { get; set; }
        public BaseChart NegDryProductChart5 { get; set; }
        #endregion

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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(LongTermStabilityViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) => {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}
