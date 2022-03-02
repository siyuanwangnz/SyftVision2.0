using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Public.ChartConfig;
using Public.SFTP;
using System.Collections.ObjectModel;

namespace BatchConfig.ViewModels
{
    public class BatchConfigViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly SyftServer _syftServer;
        public BatchConfigViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _syftServer = new SyftServer();

        }
        #region Chart Config
        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    TreeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Chart);
                });
            }
        }
        public DelegateCommand SelectedCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedTreeNode.Parent == null) return;
                    ChartTittle = SelectedTreeNode.Parent;
                    ChartSubTittle = SelectedTreeNode.Name;
                });
            }
        }
        private string _chartTittle;
        public string ChartTittle
        {
            get => _chartTittle;
            set => SetProperty(ref _chartTittle, value);
        }
        private string _chartSubTittle;
        public string ChartSubTittle
        {
            get => _chartSubTittle;
            set => SetProperty(ref _chartSubTittle, value);
        }
        private ObservableCollection<TreeNode> _treeNodes;
        public ObservableCollection<TreeNode> TreeNodes
        {
            get => _treeNodes;
            set => SetProperty(ref _treeNodes, value);
        }
        private TreeNode _selectedTreeNode;
        public TreeNode SelectedTreeNode
        {
            get => _selectedTreeNode;
            set => SetProperty(ref _selectedTreeNode, value);
        }
        #endregion

        #region Batch Config

        #endregion

    }
}
