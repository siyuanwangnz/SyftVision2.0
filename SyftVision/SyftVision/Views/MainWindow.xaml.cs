using System.Windows;
using AutoUpdaterDotNET;
using Prism.Modularity;
using Prism.Regions;
using System;
using Public.Global;
using System.Windows.Controls;
using System.Xml.Linq;
using MaterialDesignThemes.Wpf;

namespace SyftVision.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IModuleManager _moduleManager;
        private readonly IRegionManager _regionManager;
        private Options Options;

        public MainWindow(IModuleManager moduleManager, IRegionManager regionManager)
        {
            InitializeComponent();

            //Version check
            AutoUpdater.Start("http://tools.syft.com:3453/downloads/syft-vision/VersionInfo.xml");

            _moduleManager = moduleManager;
            _regionManager = regionManager;

            //Load modules
            _moduleManager.LoadModule("BatchAnalysisModule");
            _moduleManager.LoadModule("ChartConfigModule");
            _moduleManager.LoadModule("BatchConfigModule");

            //Get options settings
            try
            {
                Options = new Options();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR");
            }
            finally
            {
                SetOptions();
            }

        }

        private void OptionsSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Options.Set(UserText.Text, PortText.Text, PasswordPSD.Password, OperatorText.Text);

                //Close popup dialog
                PopupBox.ClosePopupCommand.Execute(new object(), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR");
            }
        }

        private void OptionsResetBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Options.Reset();

                SetOptions();

                //Close popup dialog
                PopupBox.ClosePopupCommand.Execute(new object(), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR");
            }
        }

        private void C_Btn_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to chart config view
            _regionManager.RequestNavigate("ContentRegion", "ChartConfigView");
        }

        private void B_Btn_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to batch config view
            _regionManager.RequestNavigate("ContentRegion", "BatchConfigView");
        }

        private void BA_Btn_Click(object sender, RoutedEventArgs e)
        {
            //Navigare to batch analysis view
            _regionManager.RequestNavigate("ContentRegion", "BatchAnalysisView");
        }

        private void SetOptions()
        {
            UserText.Text = Options.User;
            PortText.Text = Options.Port;
            PasswordPSD.Password = Options.Password;
            OperatorText.Text = Options.Operator;
        }
    }
}
