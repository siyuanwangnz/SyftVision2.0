using System.Windows;
using ChartDirector;
using Prism.Mvvm;
using Prism.Regions;
using Public.Global;

namespace SettingCheck.ViewModels.ThreePhase
{
    class SourceViewModel : BindableBase, INavigationAware
    {
        #region Navigation Message transfer
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            if (navigationContext.Parameters.GetValue<Global.InstrumentType>("InstrumentType") == Global.InstrumentType.SPIS)
                MeshIsVisible = System.Windows.Visibility.Collapsed;
            else
                MeshIsVisible = System.Windows.Visibility.Visible;
            SourceChartPS = navigationContext.Parameters.GetValue<BaseChart>("SourceChartPS");
            SourceChartMV = navigationContext.Parameters.GetValue<BaseChart>("SourceChartMV");
            SourceChartMESH = navigationContext.Parameters.GetValue<BaseChart>("SourceChartMESH");
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
        private BaseChart _sourcechartps;
        public BaseChart SourceChartPS
        {
            get { return _sourcechartps; }
            set { _sourcechartps = value; }
        }

        private BaseChart _sourcechartmv;
        public BaseChart SourceChartMV
        {
            get { return _sourcechartmv; }
            set { _sourcechartmv = value; }
        }

        private BaseChart _sourcechartmesh;
        public BaseChart SourceChartMESH
        {
            get { return _sourcechartmesh; }
            set { _sourcechartmesh = value; }
        }
        #endregion

        #region Binding Property
        private System.Windows.Visibility _meshIsVisible = System.Windows.Visibility.Visible;
        public System.Windows.Visibility MeshIsVisible
        {
            get { return _meshIsVisible; }
            set { SetProperty(ref _meshIsVisible, value); }
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(SourceViewModel), new PropertyMetadata(null,new PropertyChangedCallback((s, e) => {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}
