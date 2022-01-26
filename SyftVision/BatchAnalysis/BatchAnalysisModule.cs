using BatchAnalysis.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BatchAnalysis
{
    public class BatchAnalysisModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(BatchAnalysisView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}