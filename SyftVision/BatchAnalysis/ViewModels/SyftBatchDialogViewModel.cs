﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Public.TreeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.ViewModels
{
    public class SyftBatchDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Select A Target Batch Config File";
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
            TreeNodes = new ObservableCollection<TreeNode>(parameters.GetValue<List<TreeNode>>("treeNodes"));
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
