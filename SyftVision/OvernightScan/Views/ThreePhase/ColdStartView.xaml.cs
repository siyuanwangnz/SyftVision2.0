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
    /// Interaction logic for ColdStartView.xaml
    /// </summary>
    public partial class ColdStartView : UserControl
    {
        public ColdStartView()
        {
            InitializeComponent();
        }

        private void SensDevBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (SensDevChartViewer != null) SensDevChartViewer.Visibility = System.Windows.Visibility.Visible;
            if (Mark3And4DevChartViewer != null) Mark3And4DevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (BothEndsDevChartViewer != null) BothEndsDevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (SensitivetiesChartViewer != null) SensitivetiesChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ReagentIonsChartViewer != null) ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Mark3And4DevBtn_Checked(object sender, RoutedEventArgs e)
        {
            SensDevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            Mark3And4DevChartViewer.Visibility = System.Windows.Visibility.Visible;
            BothEndsDevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            SensitivetiesChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void BothEndsDevBtn_Checked(object sender, RoutedEventArgs e)
        {
            SensDevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            Mark3And4DevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            BothEndsDevChartViewer.Visibility = System.Windows.Visibility.Visible;
            SensitivetiesChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void SensitivetiesBtn_Checked(object sender, RoutedEventArgs e)
        {
            SensDevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            Mark3And4DevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            BothEndsDevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            SensitivetiesChartViewer.Visibility = System.Windows.Visibility.Visible;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ReagentIonsBtn_Checked(object sender, RoutedEventArgs e)
        {
            SensDevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            Mark3And4DevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            BothEndsDevChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            SensitivetiesChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Visible;
        }

        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
