using SettingCheck.ViewModels;
using SettingCheck.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

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
            containerRegistry.RegisterDialog<SyftSettingDialogView, SyftSettingDialogViewModel>();
            containerRegistry.RegisterDialog<InstruScanDialogView, InstruScanDialogViewModel>();
            containerRegistry.RegisterDialog<ChartDialogView, ChartDialogViewModel>();
        }
    }
}