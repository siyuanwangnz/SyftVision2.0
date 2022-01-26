using ChartDirector;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Public.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ChartConfig.ViewModels
{
    public class ChartConfigViewModel : BindableBase
    {
        public ChartConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

        }

        private readonly IEventAggregator _eventAggregator;

        private readonly IRegionManager _regionManager;
        
    }
}
