using ChartDirector;
using System.Windows.Controls;

namespace BatchAnalysis.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class BatchAnalysisView : UserControl
    {
        public BatchAnalysisView()
        {
            InitializeComponent();
        }

        private void WPFChartViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
