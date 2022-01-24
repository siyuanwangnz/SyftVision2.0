using ChartDirector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OvernightScan.Views.ThreePhase
{
    /// <summary>
    /// Interaction logic for InjectionScanView.xaml
    /// </summary>
    public partial class InjectionScanView : UserControl
    {
        public InjectionScanView()
        {
            InitializeComponent();
        }

        private void PosWetBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (PosWetChartViewer != null) PosWetChartViewer.Visibility = System.Windows.Visibility.Visible;
            if (NegWetChartViewer != null) NegWetChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryChartViewer != null) NegDryChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegWetBtn_Checked(object sender, RoutedEventArgs e)
        {
            PosWetChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegDryChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegDryBtn_Checked(object sender, RoutedEventArgs e)
        {
            PosWetChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryChartViewer.Visibility = System.Windows.Visibility.Visible;
        }
        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
