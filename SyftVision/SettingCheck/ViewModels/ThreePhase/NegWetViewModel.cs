using System.Windows;
using ChartDirector;
using Prism.Regions;

namespace SettingCheck.ViewModels.ThreePhase
{
    class NegWetViewModel : INavigationAware
    {
        #region Navigation Message transfer
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            NegWetChart = navigationContext.Parameters.GetValue<BaseChart>("NegWetChart");
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
        private BaseChart _negwetchart;
        public BaseChart NegWetChart
        {
            get { return _negwetchart; }
            set { _negwetchart = value; }
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(NegWetViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) => {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}
