using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChartDirector;
using Prism.Mvvm;
using Prism.Regions;

namespace OvernightScan.ViewModels.ThreePhase
{
    class SensitiveAndImpurityViewModel : BindableBase, INavigationAware
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
                    TabItem2Visibility = Visibility.Collapsed;
                    TabItem3Visibility = Visibility.Collapsed;
                    TabItem4Visibility = Visibility.Collapsed;
                    TabItem5Visibility = Visibility.Collapsed;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?.Count == 1
                        && navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?.Count == 1)
                    { 
                        SensitiveChart = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[0];
                        ImpurityChart = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[0];
                    }
                    
                    break;
                case 2:
                    TabItemName = "Batch 1";
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Collapsed;
                    TabItem4Visibility = Visibility.Collapsed;
                    TabItem5Visibility = Visibility.Collapsed;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?.Count == 2
                        && navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?.Count == 2)
                    {
                        SensitiveChart = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[0];
                        ImpurityChart = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[0];

                        SensitiveChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[1];
                        ImpurityChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[1];
                    }
                    break;
                case 3:
                    TabItemName = "Batch 1";
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Visible;
                    TabItem4Visibility = Visibility.Collapsed;
                    TabItem5Visibility = Visibility.Collapsed;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?.Count == 3
                        && navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?.Count == 3)
                    {
                        SensitiveChart = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[0];
                        ImpurityChart = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[0];

                        SensitiveChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[1];
                        ImpurityChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[1];

                        SensitiveChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[2];
                        ImpurityChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[2];
                    }
                    break;
                case 4:
                    TabItemName = "Batch 1";
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Visible;
                    TabItem4Visibility = Visibility.Visible;
                    TabItem5Visibility = Visibility.Collapsed;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?.Count == 4
                        && navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?.Count == 4)
                    {
                        SensitiveChart = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[0];
                        ImpurityChart = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[0];

                        SensitiveChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[1];
                        ImpurityChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[1];

                        SensitiveChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[2];
                        ImpurityChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[2];

                        SensitiveChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[3];
                        ImpurityChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[3];
                    }
                    break;
                case 5:
                    TabItemName = "Batch 1";
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Visible;
                    TabItem4Visibility = Visibility.Visible;
                    TabItem5Visibility = Visibility.Visible;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?.Count == 5
                        && navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?.Count == 5)
                    {
                        SensitiveChart = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[0];
                        ImpurityChart = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[0];

                        SensitiveChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[1];
                        ImpurityChart2 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[1];

                        SensitiveChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[2];
                        ImpurityChart3 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[2];

                        SensitiveChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[3];
                        ImpurityChart4 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[3];

                        SensitiveChart5 = navigationContext.Parameters.GetValue<List<XYChart>>("SensitiveChartList")?[4];
                        ImpurityChart5 = navigationContext.Parameters.GetValue<List<XYChart>>("ImpurityChartList")?[4];
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
        private System.Windows.Visibility _tabItem2Visibility;
        public System.Windows.Visibility TabItem2Visibility
        {
            get { return _tabItem2Visibility; }
            set { SetProperty(ref _tabItem2Visibility, value); }
        }
        private System.Windows.Visibility _tabItem3Visibility;
        public System.Windows.Visibility TabItem3Visibility
        {
            get { return _tabItem3Visibility; }
            set { SetProperty(ref _tabItem3Visibility, value); }
        }
        private System.Windows.Visibility _tabItem4Visibility;
        public System.Windows.Visibility TabItem4Visibility
        {
            get { return _tabItem4Visibility; }
            set { SetProperty(ref _tabItem4Visibility, value); }
        }
        private System.Windows.Visibility _tabItem5Visibility;
        public System.Windows.Visibility TabItem5Visibility
        {
            get { return _tabItem5Visibility; }
            set { SetProperty(ref _tabItem5Visibility, value); }
        }
        #endregion

        #region Chart property
        public BaseChart SensitiveChart { get; set; }
        public BaseChart ImpurityChart { get; set; }

        public BaseChart SensitiveChart2 { get; set; }
        public BaseChart ImpurityChart2 { get; set; }

        public BaseChart SensitiveChart3 { get; set; }
        public BaseChart ImpurityChart3 { get; set; }

        public BaseChart SensitiveChart4 { get; set; }
        public BaseChart ImpurityChart4 { get; set; }

        public BaseChart SensitiveChart5 { get; set; }
        public BaseChart ImpurityChart5 { get; set; }
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(SensitiveAndImpurityViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) => {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}
