using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChartDirector;
using Prism.Mvvm;
using Prism.Regions;

namespace OvernightScan.ViewModels.DPIS
{
    class UPSAndDWSCurrentViewModel : BindableBase, INavigationAware
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
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    TabItemName = "The Last Batch";
                    break;
            }
            PosWetUPSCurrentChart = navigationContext.Parameters.GetValue<BaseChart>("PosWetUPSCurrentChart");
            PosWetDWSCurrentChart = navigationContext.Parameters.GetValue<BaseChart>("PosWetDWSCurrentChart");
            NegWetUPSCurrentChart = navigationContext.Parameters.GetValue<BaseChart>("NegWetUPSCurrentChart");
            NegWetDWSCurrentChart = navigationContext.Parameters.GetValue<BaseChart>("NegWetDWSCurrentChart");
            NegDryUPSCurrentChart = navigationContext.Parameters.GetValue<BaseChart>("NegDryUPSCurrentChart");
            NegDryDWSCurrentChart = navigationContext.Parameters.GetValue<BaseChart>("NegDryDWSCurrentChart");
        }

        #region Binding Property
        private string _tabItemName;
        public string TabItemName
        {
            get { return _tabItemName; }
            set { SetProperty(ref _tabItemName, value); }
        }
        #endregion

        #region Chart property
        public BaseChart PosWetUPSCurrentChart { get; set; }
        public BaseChart PosWetDWSCurrentChart { get; set; }
        public BaseChart NegWetUPSCurrentChart { get; set; }
        public BaseChart NegWetDWSCurrentChart { get; set; }
        public BaseChart NegDryUPSCurrentChart { get; set; }
        public BaseChart NegDryDWSCurrentChart { get; set; }
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(UPSAndDWSCurrentViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) => {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}
