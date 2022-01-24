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
    /// Interaction logic for LongTermStabilityView.xaml
    /// </summary>
    public partial class LongTermStabilityView : UserControl
    {
        public LongTermStabilityView()
        {
            InitializeComponent();
        }

        private void ReagentRSDBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (ReagentRSDChartViewer != null) ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Visible;
            if (PosWetReagentChartViewer != null) PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (PosWetProductChartViewer != null) PosWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetReagentChartViewer != null) NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetProductChartViewer != null) NegWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryReagentChartViewer != null) NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryProductChartViewer != null) NegDryProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            if (ReagentRSDChartViewer2 != null) ReagentRSDChartViewer2.Visibility = System.Windows.Visibility.Visible;
            if (PosWetReagentChartViewer2 != null) PosWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            if (PosWetProductChartViewer2 != null) PosWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetReagentChartViewer2 != null) NegWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetProductChartViewer2 != null) NegWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryReagentChartViewer2 != null) NegDryReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryProductChartViewer2 != null) NegDryProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;

            if (ReagentRSDChartViewer3 != null) ReagentRSDChartViewer3.Visibility = System.Windows.Visibility.Visible;
            if (PosWetReagentChartViewer3 != null) PosWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            if (PosWetProductChartViewer3 != null) PosWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetReagentChartViewer3 != null) NegWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetProductChartViewer3 != null) NegWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryReagentChartViewer3 != null) NegDryReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryProductChartViewer3 != null) NegDryProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;

            if (ReagentRSDChartViewer4 != null) ReagentRSDChartViewer4.Visibility = System.Windows.Visibility.Visible;
            if (PosWetReagentChartViewer4 != null) PosWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            if (PosWetProductChartViewer4 != null) PosWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetReagentChartViewer4 != null) NegWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetProductChartViewer4 != null) NegWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryReagentChartViewer4 != null) NegDryReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryProductChartViewer4 != null) NegDryProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;

            if (ReagentRSDChartViewer5 != null) ReagentRSDChartViewer5.Visibility = System.Windows.Visibility.Visible;
            if (PosWetReagentChartViewer5 != null) PosWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            if (PosWetProductChartViewer5 != null) PosWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetReagentChartViewer5 != null) NegWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            if (NegWetProductChartViewer5 != null) NegWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryReagentChartViewer5 != null) NegDryReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            if (NegDryProductChartViewer5 != null) NegDryProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void PosWetReagentBtn_Checked(object sender, RoutedEventArgs e)
        {
            ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Visible;
            PosWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer2.Visibility = System.Windows.Visibility.Visible;
            PosWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer3.Visibility = System.Windows.Visibility.Visible;
            PosWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer4.Visibility = System.Windows.Visibility.Visible;
            PosWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer5.Visibility = System.Windows.Visibility.Visible;
            PosWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void PosWetProductBtn_Checked(object sender, RoutedEventArgs e)
        {
            ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer2.Visibility = System.Windows.Visibility.Visible;
            NegWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer3.Visibility = System.Windows.Visibility.Visible;
            NegWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer4.Visibility = System.Windows.Visibility.Visible;
            NegWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer5.Visibility = System.Windows.Visibility.Visible;
            NegWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegWetReagentBtn_Checked(object sender, RoutedEventArgs e)
        {
            ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer2.Visibility = System.Windows.Visibility.Visible;
            NegWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer3.Visibility = System.Windows.Visibility.Visible;
            NegWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer4.Visibility = System.Windows.Visibility.Visible;
            NegWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer5.Visibility = System.Windows.Visibility.Visible;
            NegWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegWetProductBtn_Checked(object sender, RoutedEventArgs e)
        {
            ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer2.Visibility = System.Windows.Visibility.Visible;
            NegDryReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer3.Visibility = System.Windows.Visibility.Visible;
            NegDryReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer4.Visibility = System.Windows.Visibility.Visible;
            NegDryReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer5.Visibility = System.Windows.Visibility.Visible;
            NegDryReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegDryReagentBtn_Checked(object sender, RoutedEventArgs e)
        {
            ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Visible;
            NegDryProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer2.Visibility = System.Windows.Visibility.Visible;
            NegDryProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer3.Visibility = System.Windows.Visibility.Visible;
            NegDryProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer4.Visibility = System.Windows.Visibility.Visible;
            NegDryProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;

            ReagentRSDChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer5.Visibility = System.Windows.Visibility.Visible;
            NegDryProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void NegDryProductBtn_Checked(object sender, RoutedEventArgs e)
        {
            ReagentRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer.Visibility = System.Windows.Visibility.Visible;

            ReagentRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer2.Visibility = System.Windows.Visibility.Visible;

            ReagentRSDChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer3.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer3.Visibility = System.Windows.Visibility.Visible;

            ReagentRSDChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer4.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer4.Visibility = System.Windows.Visibility.Visible;

            ReagentRSDChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            PosWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegWetProductChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryReagentChartViewer5.Visibility = System.Windows.Visibility.Collapsed;
            NegDryProductChartViewer5.Visibility = System.Windows.Visibility.Visible;
        }
        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
