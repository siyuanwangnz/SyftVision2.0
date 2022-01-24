using System.Windows.Controls;
using ChartDirector;

namespace SettingCheck.Views.ThreePhase
{
    /// <summary>
    /// Interaction logic for GPosWetView.xaml
    /// </summary>
    public partial class PosWetView : UserControl
    {
        public PosWetView()
        {
            InitializeComponent();
        }

        private void PosWetChartChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
