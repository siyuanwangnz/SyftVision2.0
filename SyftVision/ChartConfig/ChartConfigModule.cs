using ChartConfig.Views;
using ChartConfig.Views.Components;
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
            containerRegistry.RegisterForNavigation<ACComponentsView>();
            containerRegistry.RegisterForNavigation<RPComponentsView>();
        }
    }
}