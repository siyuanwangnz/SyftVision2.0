using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BatchConfig.ViewModels
{
    public class BatchConfigViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        public BatchConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

        }

        
    }
}
