using OvernightScan.Views;
using OvernightScan.Views.DPIS;
using OvernightScan.Views.DPISCommon;
using OvernightScan.Views.Infinity;
using OvernightScan.Views.SPIS;
using OvernightScan.Views.ThreePhase;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace OvernightScan
{
    public class OvernightScanModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(OvernightScanView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Batch Status
            containerRegistry.RegisterForNavigation<BatchStatusView>();
            //DPIS
            containerRegistry.RegisterForNavigation<SourceSwitchStabilityView>();
            containerRegistry.RegisterForNavigation<UPSAndDWSCurrentView>();
            //DPIS Common
            containerRegistry.RegisterForNavigation<LongTermStabilityView>();
            containerRegistry.RegisterForNavigation<ShortTermStabilityView>();
            containerRegistry.RegisterForNavigation<SourceSwitchAndSettleTimeView>();
            //Infinity
            containerRegistry.RegisterForNavigation<LODsView>();
            containerRegistry.RegisterForNavigation<StabilityView>();
            containerRegistry.RegisterForNavigation<BackgroundView>();
            //SPIS
            containerRegistry.RegisterForNavigation<OnOffView>();
            containerRegistry.RegisterForNavigation<OvernightView>();
            //Three Phase
            containerRegistry.RegisterForNavigation<ColdStartView>();
            containerRegistry.RegisterForNavigation<EffectofValidationView>();
            containerRegistry.RegisterForNavigation<ICFView>();
            containerRegistry.RegisterForNavigation<InjectionScanView>();
            containerRegistry.RegisterForNavigation<SensitiveAndImpurityView>();
        }
    }
}