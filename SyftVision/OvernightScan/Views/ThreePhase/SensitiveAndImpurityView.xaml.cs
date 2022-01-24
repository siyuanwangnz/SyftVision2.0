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
    /// Interaction logic for SensitiveAndImpurity.xaml
    /// </summary>
    public partial class SensitiveAndImpurityView : UserControl
    {
        public SensitiveAndImpurityView()
        {
            InitializeComponent();
        }

        private void SensitiveBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (SensitiveChartViewer != null) SensitiveChartViewer.Visibility = System.Windows.Visibility.Visible;
            if (ImpurityChartViewer != null) ImpurityChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            if (SensitiveChartViewer2 != null) SensitiveChartViewer2.Visibility = System.Windows.Visibility.Visible;
            if (ImpurityChartViewer2 != null) ImpurityChartViewer2.Visibility = System.Windows.Visibility.Collapsed;

            if (SensitiveChartViewer3 != null) SensitiveChartViewer3.Visibility = System.Windows.Visibility.Visible;
            if (ImpurityChartViewer3 != null) ImpurityChartViewer3.Visibility = System.Windows.Visibility.Collapsed;

            if (SensitiveChartViewer4 != null) SensitiveChartViewer4.Visibility = System.Windows.Visibility.Visible;
            if (ImpurityChartViewer4 != null) ImpurityChartViewer4.Visibility = System.Windows.Visibility.Collapsed;

            if (SensitiveChartViewer5 != null) SensitiveChartViewer5.Visibility = System.Windows.Visibility.Visible;
            if (ImpurityChartViewer5 != null) ImpurityChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ImpurityBtn_Checked(object sender, RoutedEventArgs e)
        {
            SensitiveChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ImpurityChartViewer.Visibility = System.Windows.Visibility.Visible;

            SensitiveChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ImpurityChartViewer2.Visibility = System.Windows.Visibility.Visible;

            SensitiveChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            ImpurityChartViewer3.Visibility = System.Windows.Visibility.Visible;

            SensitiveChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            ImpurityChartViewer4.Visibility = System.Windows.Visibility.Visible;

            SensitiveChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            ImpurityChartViewer5.Visibility = System.Windows.Visibility.Visible;
        }
        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
