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

namespace OvernightScan.Views.Infinity
{
    /// <summary>
    /// Interaction logic for Stability.xaml
    /// </summary>
    public partial class StabilityView : UserControl
    {
        public StabilityView()
        {
            InitializeComponent();
        }

        private void ConcentrationsRSDBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (ConcentrationsRSDChartviewer != null) ConcentrationsRSDChartviewer.Visibility = System.Windows.Visibility.Visible;
            if (ConcentrationsChartviewer != null) ConcentrationsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ReagentIonsChartviewer != null) ReagentIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ProductIonsChartviewer != null) ProductIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ConcentrationswEOVChartviewer != null) ConcentrationswEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ReactionTimeEOVChartviewer != null) ReactionTimeEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ConcentrationsBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartviewer.Visibility = System.Windows.Visibility.Visible;
            ReagentIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationswEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ReactionTimeEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ReagentIonsBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartviewer.Visibility = System.Windows.Visibility.Visible;
            ProductIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationswEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ReactionTimeEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ProductIonsBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartviewer.Visibility = System.Windows.Visibility.Visible;
            ConcentrationswEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ReactionTimeEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ConcentrationswEOVBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationswEOVChartviewer.Visibility = System.Windows.Visibility.Visible;
            ReactionTimeEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void ReactionTimeEOVBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationswEOVChartviewer.Visibility = System.Windows.Visibility.Collapsed;
            ReactionTimeEOVChartviewer.Visibility = System.Windows.Visibility.Visible;
        }
        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
