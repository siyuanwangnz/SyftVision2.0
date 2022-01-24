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

namespace OvernightScan.Views.DPISCommon
{
    /// <summary>
    /// Interaction logic for ShortTermStabilityView.xaml
    /// </summary>
    public partial class ShortTermStabilityView : UserControl
    {
        public ShortTermStabilityView()
        {
            InitializeComponent();
        }

        private void ReagentRSDBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (ReagentRSDChartViewer != null) ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Visible;
            if (PosWetReagentChartViewer != null) PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetReagentChartViewer != null) NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryReagentChartViewer != null) NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void PosWetReagentBtn_Checked(object sender, RoutedEventArgs e)
        {
            ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegWetReagentBtn_Checked(object sender, RoutedEventArgs e)
        {
            ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegDryReagentBtn_Checked(object sender, RoutedEventArgs e)
        {
            ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Visible;
        }
        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
