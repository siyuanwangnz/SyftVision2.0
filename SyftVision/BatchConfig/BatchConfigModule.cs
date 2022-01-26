using BatchConfig.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BatchConfig
{
    public class BatchConfigModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(BatchConfigView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}