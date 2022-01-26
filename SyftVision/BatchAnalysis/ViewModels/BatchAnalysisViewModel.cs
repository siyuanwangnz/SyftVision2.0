using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BatchAnalysis.ViewModels
{
    public class BatchAnalysisViewModel : BindableBase
    {
        public BatchAnalysisViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

        }

        private readonly IEventAggregator _eventAggregator;

        private readonly IRegionManager _regionManager;
        
    }
}
