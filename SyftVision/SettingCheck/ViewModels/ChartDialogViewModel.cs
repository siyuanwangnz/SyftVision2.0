using ChartDirector;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Public.ChartBuilder.XY;
using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SettingCheck.ViewModels
{
    public class ChartDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Setting Chart";
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Chart = parameters.GetValue<BaseChart>("Chart");
            ScanFileList = new ObservableCollection<ScanFile>(parameters.GetValue<List<ScanFile>>("ScanFileList"));
            XYLegendList = new ObservableCollection<XYLegend>(parameters.GetValue<List<XYLegend>>("XYLegendList"));
        }

        private BaseChart _chart;
        public BaseChart Chart
        {
            get => _chart;
            set => SetProperty(ref _chart, value);
        }
        private ObservableCollection<ScanFile> _scanFileList;
        public ObservableCollection<ScanFile> ScanFileList
        {
            get => _scanFileList;
            set => SetProperty(ref _scanFileList, value);
        }
        private ObservableCollection<XYLegend> _xyLegendList;
        public ObservableCollection<XYLegend> XYLegendList
        {
            get => _xyLegendList;
            set => SetProperty(ref _xyLegendList, value);
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
            DependencyProperty.RegisterAttached("AttachedChart", typeof(BaseChart), typeof(ChartDialogViewModel), new PropertyMetadata(null, new PropertyChangedCallback((s, e) =>
            {
                var chartviewer = s as WPFChartViewer;
                chartviewer.Chart = e.NewValue as BaseChart;
            })));
        #endregion
    }
}