using System.Windows.Controls;
using ChartDirector;

namespace SettingCheck.Views.ThreePhase
{
    /// <summary>
    /// Interaction logic for GNegDryView.xaml
    /// </summary>
    public partial class NegDryView : UserControl
    {
        public NegDryView()
        {
            InitializeComponent();
        }

        private void NegDryChartChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
