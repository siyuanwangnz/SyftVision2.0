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

namespace OvernightScan.Views.ThreePhase
{
    /// <summary>
    /// Interaction logic for EffectofValidationView.xaml
    /// </summary>
    public partial class EffectofValidationView : UserControl
    {
        public EffectofValidationView()
        {
            InitializeComponent();
        }

        private void ConcentrationsRSDBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (ConcentrationsRSDChartViewer != null) ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Visible;
            if (ConcentrationsChartViewer != null) ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ReagentIonsChartViewer != null) ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ProductIonsChartViewer != null) ProductIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            if (ReactionTimeEOVChartViewer != null) ReactionTimeEOVChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ConcentrationsBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Visible;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReactionTimeEOVChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ReagentIonsBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Visible;
            ProductIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReactionTimeEOVChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ProductIonsBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartViewer.Visibility = System.Windows.Visibility.Visible;
            ReactionTimeEOVChartViewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ReactionTimeEOVBtn_Checked(object sender, RoutedEventArgs e)
        {
            ConcentrationsRSDChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ConcentrationsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReagentIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ProductIonsChartViewer.Visibility = System.Windows.Visibility.Collapsed;
            ReactionTimeEOVChartViewer.Visibility = System.Windows.Visibility.Visible;
        }

        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }
    }
}
