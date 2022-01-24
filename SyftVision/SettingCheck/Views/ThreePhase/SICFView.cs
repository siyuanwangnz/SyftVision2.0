using ChartDirector;
using System.Windows.Controls;

namespace SettingCheck.Views.ThreePhase
{
    /// <summary>
    /// Interaction logic for ICFView
    /// </summary>
    public partial class SICFView : UserControl
    {
        public SICFView()
        {
            InitializeComponent();
        }

        private void ICFChartChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
