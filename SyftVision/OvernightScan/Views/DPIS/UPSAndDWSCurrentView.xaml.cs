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

namespace OvernightScan.Views.DPIS
{
    /// <summary>
    /// Interaction logic for UPSAndDWSCurrentView.xaml
    /// </summary>
    public partial class UPSAndDWSCurrentView : UserControl
    {
        public UPSAndDWSCurrentView()
        {
            InitializeComponent();
        }

        private void PosWetUPSCurrentBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (PosWetUPSCurrentChartViewer != null)
                PosWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Visible;
            if (PosWetDWSCurrentChartViewer != null)
                PosWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetUPSCurrentChartViewer != null)
                NegWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetDWSCurrentChartViewer != null)
                NegWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryUPSCurrentChartViewer != null)
                NegDryUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryDWSCurrentChartViewer != null)
                NegDryDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void PosWetDWSCurrentBtn_Checked(object sender, RoutedEventArgs e)
        {
            PosWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegWetUPSCurrentBtn_Checked(object sender, RoutedEventArgs e)
        {
            PosWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegWetDWSCurrentBtn_Checked(object sender, RoutedEventArgs e)
        {
            PosWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegDryUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegDryUPSCurrentBtn_Checked(object sender, RoutedEventArgs e)
        {
            PosWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegDryDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegDryDWSCurrentBtn_Checked(object sender, RoutedEventArgs e)
        {
            PosWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryUPSCurrentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryDWSCurrentChartViewer.Visibility = System.Windows.Visibility.Visible;
        }

        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
