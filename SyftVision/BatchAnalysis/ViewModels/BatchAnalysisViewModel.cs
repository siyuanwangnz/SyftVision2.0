using BatchAnalysis.Models;
using ChartDirector;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows;

namespace BatchAnalysis.ViewModels
{
    public class BatchAnalysisViewModel : BindableBase
    {
        public BatchAnalysisViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            #region chart list test
            // The data for the bar chart
            double[] data = { 85, 156, 179.5, 211, 123 };

            // The labels for the bar chart
            string[] labels = { "Mon", "Tue", "Wed", "Thu", "Fri" };

            // Create a XYChart object of size 250 x 250 pixels
            XYChart c = new XYChart(250, 250);

            // Set the plotarea at (30, 20) and of size 200 x 200 pixels
            c.setPlotArea(30, 20, 200, 200);

            // Add a bar chart layer using the given data
            c.addBarLayer(data);

            // Set the labels on the x axis.
            c.xAxis().setLabels(labels);

            // The data for the bar chart
            data = new double[] { 100, 200, 150, 250, 300 };

            // The labels for the bar chart
            labels = new string[] { "Mon1", "Tue1", "Wed1", "Thu1", "Fri1" };

            // Create a XYChart object of size 250 x 250 pixels
            XYChart c1 = new XYChart(250, 250);

            // Set the plotarea at (30, 20) and of size 200 x 200 pixels
            c1.setPlotArea(30, 20, 200, 200);

            // Add a bar chart layer using the given data
            c1.addBarLayer(data);

            // Set the labels on the x axis.
            c1.xAxis().setLabels(labels);

            ResultChart chart = new ResultChart(c);
            ResultChart chart1 = new ResultChart(c1);

            ChartList = new ObservableCollection<ResultChart>() { chart, chart1 };
            #endregion

        }

        private readonly IEventAggregator _eventAggregator;

        private readonly IRegionManager _regionManager;

        private ObservableCollection<ResultChart> _chartList;
        public ObservableCollection<ResultChart> ChartList
        {
            get => _chartList;
            set => SetProperty(ref _chartList, value);
        }

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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(BatchAnalysisViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) =>
            {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion

    }
}
