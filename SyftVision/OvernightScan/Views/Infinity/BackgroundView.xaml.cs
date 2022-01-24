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
    /// Interaction logic for BackgroundView.xaml
    /// </summary>
    public partial class BackgroundView : UserControl
    {
        public BackgroundView()
        {
            InitializeComponent();
        }
        private void ChartViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as WPFChartViewer).Chart != null)
                (sender as WPFChartViewer).ImageMap = (sender as WPFChartViewer).Chart.getHTMLImageMap("");
        }

        private void Compounds75Btn_Checked(object sender, RoutedEventArgs e)
        {
            if (Compounds75Chartviewer != null) Compounds75Chartviewer.Visibility = System.Windows.Visibility.Visible;
            if (Compounds51Chartviewer != null) Compounds51Chartviewer.Visibility = System.Windows.Visibility.Collapsed;
            if (Compounds52Chartviewer != null) Compounds52Chartviewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Compounds51Btn_Checked(object sender, RoutedEventArgs e)
        {
            Compounds75Chartviewer.Visibility = System.Windows.Visibility.Collapsed;
            Compounds51Chartviewer.Visibility = System.Windows.Visibility.Visible;
            Compounds52Chartviewer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Compounds52Btn_Checked(object sender, RoutedEventArgs e)
        {
            Compounds75Chartviewer.Visibility = System.Windows.Visibility.Collapsed;
            Compounds51Chartviewer.Visibility = System.Windows.Visibility.Collapsed;
            Compounds52Chartviewer.Visibility = System.Windows.Visibility.Visible;
        }
    }
}

