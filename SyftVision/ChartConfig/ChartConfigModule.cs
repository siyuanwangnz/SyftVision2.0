using ChartConfig.ViewModels;
using ChartConfig.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ChartConfig
{
    public class ChartConfigModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(ChartConfigView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<SyftChartDialogView, SyftChartDialogViewModel>();
        }
    }
}