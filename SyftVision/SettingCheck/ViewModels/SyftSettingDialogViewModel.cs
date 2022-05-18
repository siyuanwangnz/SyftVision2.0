using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Public.ChartConfig;
using Public.TreeList;
using System;
using System.Collections.ObjectModel;

namespace SettingCheck.ViewModels
{
    public class SyftSettingDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Select A Target Setting Config File";
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            TreeNodes = parameters.GetValue<ObservableCollection<TreeNode>>("treeNodes");
        }
        public DelegateCommand SelectedCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedTreeNode.Parent == null) return;
                    DialogParameters param = new DialogParameters();
                    param.Add("selectedTreeNode", SelectedTreeNode);
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
                });
            }
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

    }
}
