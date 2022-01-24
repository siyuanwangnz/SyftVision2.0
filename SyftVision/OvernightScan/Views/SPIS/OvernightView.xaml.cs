﻿using ChartDirector;
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

namespace OvernightScan.Views.SPIS
{
    /// <summary>
    /// Interaction logic for OvernightView.xaml
    /// </summary>
    public partial class OvernightView : UserControl
    {
        public OvernightView()
        {
            InitializeComponent();
        }

        private void ConcentrationsRSDBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (ConcentrationsRSDChartViewer != null) ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Visible;
            if (ConcentrationsChartViewer != null) ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ReagentIonsChartViewer != null) ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ProductIonsChartViewer != null) ProductIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (QuadStabilityChartViewer != null) QuadStabilityChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            if (ConcentrationsRSDChartViewer2 != null) ConcentrationsRSDChartViewer2.Visibility = System.Windows.Visibility.Visible;
            if (ConcentrationsChartViewer2 != null) ConcentrationsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            if (ReagentIonsChartViewer2 != null) ReagentIonsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            if (ProductIonsChartViewer2 != null) ProductIonsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            if (QuadStabilityChartViewer2 != null) QuadStabilityChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ConcentrationsBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Visible;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            QuadStabilityChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            ConcentrationsRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer2.Visibility = System.Windows.Visibility.Visible;
            ReagentIonsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            QuadStabilityChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ReagentIonsBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Visible;
            ProductIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            QuadStabilityChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            ConcentrationsRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer2.Visibility = System.Windows.Visibility.Visible;
            ProductIonsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            QuadStabilityChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ProductIonsBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartViewer.Visibility = System.Windows.Visibility.Visible;
            QuadStabilityChartViewer.Visibility = System.Windows.Visibility.Collapsed;

            ConcentrationsRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartViewer2.Visibility = System.Windows.Visibility.Visible;
            QuadStabilityChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void QuadStabilityBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            QuadStabilityChartViewer.Visibility = System.Windows.Visibility.Visible;

            ConcentrationsRSDChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartViewer2.Visibility = System.Windows.Visibility.Collapsed;
            QuadStabilityChartViewer2.Visibility = System.Windows.Visibility.Visible;
        }

        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
