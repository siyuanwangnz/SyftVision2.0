using System.Windows;
using ChartDirector;
using Prism.Regions;

namespace SettingCheck.ViewModels.ThreePhase
{
    class DWSSpecificViewModel : INavigationAware
    {
        #region Navigation Message transfer
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            DWSSpecificChartPF = navigationContext.Parameters.GetValue<BaseChart>("DWSSpecificChartPF");
            DWSSpecificChartAB = navigationContext.Parameters.GetValue<BaseChart>("DWSSpecificChartAB");
            DWSSpecificChartLens5 = navigationContext.Parameters.GetValue<BaseChart>("DWSSpecificChartLens5");
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
        private BaseChart _dwsspecificchartpf;
        public BaseChart DWSSpecificChartPF
        {
            get { return _dwsspecificchartpf; }
            set { _dwsspecificchartpf = value; }
        }

        private BaseChart _dwsspecificchartab;
        public BaseChart DWSSpecificChartAB
        {
            get { return _dwsspecificchartab; }
            set { _dwsspecificchartab = value; }
        }

        private BaseChart _dwsspecificchartlens5;
        public BaseChart DWSSpecificChartLens5
        {
            get { return _dwsspecificchartlens5; }
            set { _dwsspecificchartlens5 = value; }
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(DWSSpecificViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) => {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}
