using ChartDirector;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OvernightScan.ViewModels.ThreePhase
{
    public class ColdStartViewModel : INavigationAware
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
            SensDevChart = navigationContext.Parameters.GetValue<BaseChart>("SensDevChart");
            Mark3And4DevChart = navigationContext.Parameters.GetValue<BaseChart>("Mark3And4DevChart");
            BothEndsDevChart = navigationContext.Parameters.GetValue<BaseChart>("BothEndsDevChart");
            SensitivetiesChart = navigationContext.Parameters.GetValue<BaseChart>("SensitivetiesChart");
            ReagentIonsChart = navigationContext.Parameters.GetValue<BaseChart>("ReagentIonsChart");
        }

        #region Chart property
        public BaseChart SensDevChart { get; set; }
        public BaseChart Mark3And4DevChart { get; set; }
        public BaseChart BothEndsDevChart { get; set; }
        public BaseChart SensitivetiesChart { get; set; }
        public BaseChart ReagentIonsChart { get; set; }
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(ColdStartViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) => {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}
