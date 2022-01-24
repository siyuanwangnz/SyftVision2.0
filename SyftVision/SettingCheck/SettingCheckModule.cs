using SettingCheck.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SettingCheck.Views.ThreePhase;

namespace SettingCheck
{
    public class SettingCheckModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(SettingCheckView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SourceView>();
            containerRegistry.RegisterForNavigation<PosWetView>();
            containerRegistry.RegisterForNavigation<NegWetView>();
            containerRegistry.RegisterForNavigation<NegDryView>();
            containerRegistry.RegisterForNavigation<DWSView>();
            containerRegistry.RegisterForNavigation<DWSSpecificView>();
            containerRegistry.RegisterForNavigation<DetectionView>();
            containerRegistry.RegisterForNavigation<SICFView>();
        }
    }
}