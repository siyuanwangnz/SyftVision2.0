using System.Windows.Controls;
using ChartDirector;

namespace SettingCheck.Views.ThreePhase
{
    /// <summary>
    /// Interaction logic for GNegWetView.xaml
    /// </summary>
    public partial class NegWetView : UserControl
    {
        public NegWetView()
        {
            InitializeComponent();
        }

        private void NegWetChartChartChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
