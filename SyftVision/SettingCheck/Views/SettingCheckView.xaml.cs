using System.Windows.Controls;

namespace SettingCheck.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class SettingCheckView : UserControl
    {
        public SettingCheckView()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbx = sender as ComboBox;
            var item = cbx.SelectedItem as ComboBoxItem;
            string selectedItem;
            if (item != null)
            {
                selectedItem = item.Content.ToString();

                if (selectedItem == "SPIS")
                {
                    BtnSource.Visibility = System.Windows.Visibility.Visible;
                    BtnPW.Visibility = System.Windows.Visibility.Visible;
                    BtnNW.Visibility = System.Windows.Visibility.Collapsed;
                    BtnND.Visibility = System.Windows.Visibility.Collapsed;
                    BtnDWS.Visibility = System.Windows.Visibility.Visible;
                    BtnDWSS.Visibility = System.Windows.Visibility.Visible;
                    BtnDetetion.Visibility = System.Windows.Visibility.Visible;
                    BtnICF.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    BtnSource.Visibility = System.Windows.Visibility.Visible;
                    BtnPW.Visibility = System.Windows.Visibility.Visible;
                    BtnNW.Visibility = System.Windows.Visibility.Visible;
                    BtnND.Visibility = System.Windows.Visibility.Visible;
                    BtnDWS.Visibility = System.Windows.Visibility.Visible;
                    BtnDWSS.Visibility = System.Windows.Visibility.Visible;
                    BtnDetetion.Visibility = System.Windows.Visibility.Visible;
                    BtnICF.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }
    }
}
