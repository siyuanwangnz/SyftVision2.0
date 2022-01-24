using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChartDirector;
using Prism.Mvvm;
using Prism.Regions;
using Public.Global;

namespace OvernightScan.ViewModels.SPIS
{
    class OvernightViewModel : BindableBase, INavigationAware
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
            if (navigationContext.Parameters.GetValue<int>("NumberofBatches") == 1 && navigationContext.Parameters.GetValue<Global.BatchType>("InstrumentBatchType") == Global.BatchType.SPIS_Overnight)
            {
                TabItemName = "Selected Ov";
                TabItemOv2Visibility = Visibility.Collapsed;
                TabItemOverallVisibility = Visibility.Collapsed;

                ConcentrationsRSDChart = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsRSDChart");
                ConcentrationsChart = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsChart");
                ReagentIonsChart = navigationContext.Parameters.GetValue<BaseChart>("ReagentIonsChart");
                ProductIonsChart = navigationContext.Parameters.GetValue<BaseChart>("ProductIonsChart");
                QuadStabilityChart = navigationContext.Parameters.GetValue<BaseChart>("QuadStabilityChart");
            }
            else
            {
                TabItemName = "Ov 1";
                TabItemOv2Visibility = Visibility.Visible;
                TabItemOverallVisibility = Visibility.Visible;

                ConcentrationsRSDChartOverall = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsRSDChartOverall");

                ConcentrationsRSDChart = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsRSDChart");
                ConcentrationsChart = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsChart");
                ReagentIonsChart = navigationContext.Parameters.GetValue<BaseChart>("ReagentIonsChart");
                ProductIonsChart = navigationContext.Parameters.GetValue<BaseChart>("ProductIonsChart");
                QuadStabilityChart = navigationContext.Parameters.GetValue<BaseChart>("QuadStabilityChart");

                ConcentrationsRSDChart2 = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsRSDChart2");
                ConcentrationsChart2 = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsChart2");
                ReagentIonsChart2 = navigationContext.Parameters.GetValue<BaseChart>("ReagentIonsChart2");
                ProductIonsChart2 = navigationContext.Parameters.GetValue<BaseChart>("ProductIonsChart2");
                QuadStabilityChart2 = navigationContext.Parameters.GetValue<BaseChart>("QuadStabilityChart2");
            }
        }

        #region Binding Property
        private string _tabItemName;
        public string TabItemName
        {
            get { return _tabItemName; }
            set { SetProperty(ref _tabItemName, value); }
        }
        private System.Windows.Visibility _tabItemOv2Visibility;
        public System.Windows.Visibility TabItemOv2Visibility
        {
            get { return _tabItemOv2Visibility; }
            set { SetProperty(ref _tabItemOv2Visibility, value); }
        }
        private System.Windows.Visibility _tabItemOverallVisibility;
        public System.Windows.Visibility TabItemOverallVisibility
        {
            get { return _tabItemOverallVisibility; }
            set { SetProperty(ref _tabItemOverallVisibility, value); }
        }
        #endregion

        #region Chart Property
        public BaseChart ConcentrationsRSDChartOverall { get; set; }

        public BaseChart ConcentrationsRSDChart { get; set; }
        public BaseChart ConcentrationsChart { get; set; }
        public BaseChart ReagentIonsChart { get; set; }
        public BaseChart ProductIonsChart { get; set; }
        public BaseChart QuadStabilityChart { get; set; }

        public BaseChart ConcentrationsRSDChart2 { get; set; }
        public BaseChart ConcentrationsChart2 { get; set; }
        public BaseChart ReagentIonsChart2 { get; set; }
        public BaseChart ProductIonsChart2 { get; set; }
        public BaseChart QuadStabilityChart2 { get; set; }
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(OvernightViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) =>
            {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}
