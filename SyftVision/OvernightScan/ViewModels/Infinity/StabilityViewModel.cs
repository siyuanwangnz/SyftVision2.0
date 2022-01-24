using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChartDirector;
using Prism.Mvvm;
using Prism.Regions;

namespace OvernightScan.ViewModels.Infinity
{
    class StabilityViewModel : BindableBase, INavigationAware
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

                    if(navigationContext.Parameters.GetValue<List<XYChart>>("ConcentrationsRSDChartList")?.Count==1)
                        ConcentrationsRSDChart = navigationContext.Parameters.GetValue<List<XYChart>>("ConcentrationsRSDChartList")?[0];
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    TabItemName = "The Last Batch";
                    TabItemOverallVisibility = Visibility.Visible;

                    if (navigationContext.Parameters.GetValue<List<XYChart>>("ConcentrationsRSDChartList")?.Count >=3 )
                    {
                        ConcentrationsRSDChartOverall = navigationContext.Parameters.GetValue<List<XYChart>>("ConcentrationsRSDChartList")?[0];
                        ConcentrationsRSDChart = navigationContext.Parameters.GetValue<List<XYChart>>("ConcentrationsRSDChartList")?.Last();
                    }
                    break;
            }
            ConcentrationsChart = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsChart");
            ReagentIonsChart = navigationContext.Parameters.GetValue<BaseChart>("ReagentIonsChart");
            ProductIonsChart = navigationContext.Parameters.GetValue<BaseChart>("ProductIonsChart");
            ConcentrationswEOVChart = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationswEOVChart");
            ReactionTimeEOVChart = navigationContext.Parameters.GetValue<BaseChart>("ReactionTimeEOVChart");
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
            get { return _tabItemOverallVisibility; }
            set { SetProperty(ref _tabItemOverallVisibility, value); }
        }
        #endregion

        #region Chart property
        public BaseChart ConcentrationsRSDChartOverall { get; set; }

        public BaseChart ConcentrationsRSDChart { get; set; }
        public BaseChart ConcentrationsChart { get; set; }
        public BaseChart ReagentIonsChart { get; set; }
        public BaseChart ProductIonsChart { get; set; }
        public BaseChart ConcentrationswEOVChart { get; set; }
        public BaseChart ReactionTimeEOVChart{ get; set; }
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
                DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(StabilityViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) => {
                    var chartviewer = s as WPFChartViewer;
                    chartviewer.Chart = e.NewValue as BaseChart;
                })));
            #endregion
    }
}
