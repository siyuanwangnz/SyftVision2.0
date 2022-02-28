using ChartConfig.Models;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.ViewModels
{
    public class OpenDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Selct A Target Chart Config File";
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

        }
        public OpenDialogViewModel()
        {
            TreeNode ChildTreeNode = new TreeNode(); ChildTreeNode.Name = "ChildTreeNode";
            TreeNode ChildTreeNode1 = new TreeNode(); ChildTreeNode1.Name = "ChildTreeNode1";
            TreeNode ChildTreeNode2 = new TreeNode(); ChildTreeNode2.Name = "ChildTreeNode2";

            TreeNode TreeNode = new TreeNode(); TreeNode.Name = "TreeNode";
            TreeNode TreeNode1 = new TreeNode(); TreeNode1.Name = "TreeNode1";

            TreeNode.ChildNodes = new List<TreeNode>() { };
            TreeNode1.ChildNodes = new List<TreeNode>() { };

            ChildTreeNode.Parent = TreeNode;
            ChildTreeNode1.Parent = TreeNode;
            ChildTreeNode2.Parent = TreeNode1;

            TreeNode.ChildNodes.Add(ChildTreeNode);
            TreeNode.ChildNodes.Add(ChildTreeNode1);
            TreeNode1.ChildNodes.Add(ChildTreeNode2);

            TreeNodes = new ObservableCollection<TreeNode>() { };
            TreeNodes.Add(TreeNode);
            TreeNodes.Add(TreeNode1);

        }
        private ObservableCollection<TreeNode> _treeNodes;
        public ObservableCollection<TreeNode> TreeNodes
        {
            get => _treeNodes;
            set => SetProperty(ref _treeNodes, value);
        }
        private TreeNode _selectedTreeItem;
        public TreeNode SelectedTreeItem
        {
            get => _selectedTreeItem;
            set
            {
                SetProperty(ref _selectedTreeItem, value);

                if (_selectedTreeItem.Parent != null)
                {
                    Console.WriteLine(_selectedTreeItem.Parent.Name + "/" + _selectedTreeItem.Name);
                }
                else
                {
                    Console.WriteLine(_selectedTreeItem.Name);
                }

            }
        }


    }
}
