using SyftVision.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using SettingCheck;
using OvernightScan;

namespace SyftVision
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //Add Setting Check Module
            var moduleSettingCheckType = typeof(SettingCheckModule);
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = moduleSettingCheckType.Name,
                ModuleType = moduleSettingCheckType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.OnDemand
            });
            //Add Overnight Scan Module
            var moduleOvernightScanType = typeof(OvernightScanModule);
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = moduleOvernightScanType.Name,
                ModuleType = moduleOvernightScanType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.OnDemand
            });
        }
    }
}
