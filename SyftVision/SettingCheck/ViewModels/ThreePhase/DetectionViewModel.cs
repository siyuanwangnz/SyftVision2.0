using System.Windows;
using ChartDirector;
using Prism.Regions;

namespace SettingCheck.ViewModels.ThreePhase
{
    class DetectionViewModel : INavigationAware
    {
        #region Navigation Message transfer
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            DetectionChartDV = navigationContext.Parameters.GetValue<BaseChart>("DetectionChartDV");
            DetectionChartDS = navigationContext.Parameters.GetValue<BaseChart>("DetectionChartDS");
            DetectionChartST = navigationContext.Parameters.GetValue<BaseChart>("DetectionChartST");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion

        #region Chart property
        private BaseChart _detectionchartdv;
        public BaseChart DetectionChartDV
        {
            get { return _detectionchartdv; }
            set { _detectionchartdv = value; }
        }

        private BaseChart _detectionchartds;
        public BaseChart DetectionChartDS
        {
            get { return _detectionchartds; }
            set { _detectionchartds = value; }
        }

        private BaseChart _detectionchartst;
        public BaseChart DetectionChartST
        {
            get { return _detectionchartst; }
            set { _detectionchartst = value; }
        }
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(DetectionViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) => {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion

    }
}
