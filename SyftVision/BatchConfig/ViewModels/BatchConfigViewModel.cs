using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BatchConfig.ViewModels
{
    public class BatchConfigViewModel : BindableBase
    {
        public BatchConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

        }

        private readonly IEventAggregator _eventAggregator;

        private readonly IRegionManager _regionManager;
        
    }
}
