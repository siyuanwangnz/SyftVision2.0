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
            //Set Password
            PasswordBox.Password = Global.Password;
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

        private void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = ShowArea as FrameworkElement;
            CopyUIElementToClipboard(element);
        }

        private static void CopyUIElementToClipboard(FrameworkElement element)
        {
            double width = element.ActualWidth;
            double height = element.ActualHeight;
            RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(element);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Size(width, height)));
            }
            bmpCopied.Render(dv);
            Clipboard.SetImage(bmpCopied);
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = Global.PASSWORD;
            Global.Password = Global.PASSWORD;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Global.Password = PasswordBox.Password;
        }
    }
}
