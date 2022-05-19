using BatchConfig.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchConfig.ViewModels
{
    public class InstruBatchDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Select A Target Batch File";
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
            List<string> batchFileList = parameters.GetValue<List<string>>("batchFileList");

            BatchList = new ObservableCollection<InstruBatch>(batchFileList.ConvertAll(a => new InstruBatch(a)));
        }
        public DelegateCommand SelectedCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    DialogParameters param = new DialogParameters();
                    param.Add("selectedBatchFile", SelectedBatch.File);
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
                });
            }
        }
        private ObservableCollection<InstruBatch> _batchList;
        public ObservableCollection<InstruBatch> BatchList
        {
            get => _batchList;
            set => SetProperty(ref _batchList, value);
        }
        private InstruBatch _selectedBatch;
        public InstruBatch SelectedBatch
        {
            get => _selectedBatch;
            set => SetProperty(ref _selectedBatch, value);
        }
    }
}
