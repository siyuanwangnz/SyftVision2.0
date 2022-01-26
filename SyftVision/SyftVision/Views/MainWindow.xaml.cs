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

        public MainWindow(IModuleManager moduleManager, IRegionManager regionManager)
        {
            InitializeComponent();

            //Version check
            AutoUpdater.Start("http://tools.syft.com:3453/downloads/syft-vision/VersionInfo.xml");

            _moduleManager = moduleManager;
            _regionManager = regionManager;

            //Load modules
            _moduleManager.LoadModule("ChartConfigModule");
            _moduleManager.LoadModule("BatchConfigModule");
            _moduleManager.LoadModule("BatchAnalysisModule");

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
    }
}
