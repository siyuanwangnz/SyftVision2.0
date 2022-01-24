using System.Windows.Controls;
using ChartDirector;

namespace SettingCheck.Views.ThreePhase
{
    /// <summary>
    /// Interaction logic for GeneralSettingCheckView.xaml
    /// </summary>
    public partial class SourceView : UserControl
    {
        public SourceView()
        {
            InitializeComponent();
        }

        private void SourceChartPSChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if((sender as WPFChartViewer).Chart!=null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }

        private void SourceChartMVChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }

        private void SourceChartMESHChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
