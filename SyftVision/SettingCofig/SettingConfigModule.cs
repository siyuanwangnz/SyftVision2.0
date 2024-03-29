﻿using SettingConfig.ViewModels;
using SettingConfig.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace SettingConfig
{
    public class SettingConfigModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(SettingConfigView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<TextLimitSetDialogView, TextLimitSetDialogViewModel>();
            containerRegistry.RegisterDialog<ValueLimitSetDialogView, ValueLimitSetDialogViewModel>();
            containerRegistry.RegisterDialog<OnOffLimitSetDialogView, OnOffLimitSetDialogViewModel>();
            containerRegistry.RegisterDialog<TableLimitSetDialogView, TableLimitSetDialogViewModel>();
            containerRegistry.RegisterDialog<MapLimitSetDialogView, MapLimitSetDialogViewModel>();
            containerRegistry.RegisterDialog<SyftSettingDialogView, SyftSettingDialogViewModel>();
            containerRegistry.RegisterDialog<InstruScanDialogView, InstruScanDialogViewModel>();
        }
    }
}