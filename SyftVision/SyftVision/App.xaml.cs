using SyftVision.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using ChartConfig;
using BatchAnalysis;
using BatchConfig;

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
            //Add Chart Config Module
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = typeof(ChartConfigModule).Name,
                ModuleType = typeof(ChartConfigModule).AssemblyQualifiedName,
                InitializationMode = InitializationMode.OnDemand
            });
            //Add Batch Config Module
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = typeof(BatchConfigModule).Name,
                ModuleType = typeof(BatchConfigModule).AssemblyQualifiedName,
                InitializationMode = InitializationMode.OnDemand
            });
            //Add Batch Analysis Module
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = typeof(BatchAnalysisModule).Name,
                ModuleType = typeof(BatchAnalysisModule).AssemblyQualifiedName,
                InitializationMode = InitializationMode.OnDemand
            });
        }
    }
}
