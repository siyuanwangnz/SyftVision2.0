using System.Windows;
using AutoUpdaterDotNET;
using OvernightScan.Views;
using Prism.Modularity;
using Prism.Regions;
using SettingCheck.Views;
using SyftVision.ViewModels;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        //Module Manager
        private readonly IModuleManager _moduleManager;
        //Region Manager
        private readonly IRegionManager _regionManager;

        public MainWindow(IModuleManager moduleManager, IRegionManager regionManager)
        {
            InitializeComponent();
            //Version check
            AutoUpdater.Start("http://tools.syft.com:3453/downloads/syft-vision/VersionInfo.xml");
            //Get Module Manager
            _moduleManager = moduleManager;
            //get Region Manager
            _regionManager = regionManager;
            //Load Setting Check Module
            _moduleManager.LoadModule("SettingCheckModule");
            //Load Setting Overnight Module
            _moduleManager.LoadModule("OvernightScanModule");
            //Get options settings
            try
            {
                XElement RootNode = XElement.Load($"./Config/Options_Config.xml");

                userText.Text = RootNode.Element("User")?.Value;
                portText.Text=  RootNode.Element("Port")?.Value;
                passwordPSD.Password = RootNode.Element("Password")?.Value;
                operatorText.Text = RootNode.Element("Operator")?.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR");
            }
            
        }

        private void Button_SettingCheck_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to Setting Check View
            _regionManager.RequestNavigate("ContentRegion", "SettingCheckView");
        }

        private void Button_OvernightScan_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to Overnight Scan view
            _regionManager.RequestNavigate("ContentRegion", "OvernightScanView");
        }

        private void optionsSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XElement RootNode = XElement.Load($"./Config/Options_Config.xml");

                RootNode.Element("User")?.SetValue(userText.Text); 
                RootNode.Element("Port")?.SetValue(portText.Text); 
                RootNode.Element("Password")?.SetValue(passwordPSD.Password); 
                RootNode.Element("Operator")?.SetValue(operatorText.Text);

                RootNode.Save($"./Config/Options_Config.xml");

                //Close popup dialog
                PopupBox.ClosePopupCommand.Execute(new object(), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR");
            }
        }

        private void optionsResetBtn_Click(object sender, RoutedEventArgs e)
        {
            userText.Text = Global.USER;
            portText.Text = Global.PORT;
            passwordPSD.Password = Global.PASSWORD;
            operatorText.Text = Global.OPERATOR;

            optionsSaveBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
