using System.Windows.Controls;
using ChartDirector;

namespace SettingCheck.Views.ThreePhase
{
    /// <summary>
    /// Interaction logic for DetectionView.xaml
    /// </summary>
    public partial class DetectionView : UserControl
    {
        public DetectionView()
        {
            InitializeComponent();
        }

        private void DetectionChartDVChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }

        private void DetectionChartDSChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }

        private void DetectionChartSTChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
