using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChartDirector;
using Prism.Regions;

namespace OvernightScan.ViewModels.ThreePhase
{
    class EffectofValidationViewModel : INavigationAware
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
            ConcentrationsRSDChart = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsRSDChart");
            ConcentrationsChart = navigationContext.Parameters.GetValue<BaseChart>("ConcentrationsChart");
            ReagentIonsChart = navigationContext.Parameters.GetValue<BaseChart>("ReagentIonsChart");
            ProductIonsChart = navigationContext.Parameters.GetValue<BaseChart>("ProductIonsChart");
            ReactionTimeEOVChart = navigationContext.Parameters.GetValue<BaseChart>("ReactionTimeEOVChart");
        }

        #region Chart property
        public BaseChart ConcentrationsRSDChart { get; set; }
        public BaseChart ConcentrationsChart { get; set; }
        public BaseChart ReagentIonsChart { get; set; }
        public BaseChart ProductIonsChart { get; set; }
        public BaseChart ReactionTimeEOVChart { get; set; }
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(EffectofValidationViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) => {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}
