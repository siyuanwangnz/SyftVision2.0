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
            BatchesList = parameters.GetValue<ObservableCollection<Batch>>("batchesList");
        }
        public DelegateCommand SelectedCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    DialogParameters param = new DialogParameters();
                    param.Add("selectedBatch", SelectedBatch);
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
                });
            }
        }
        private ObservableCollection<Batch> _batchesList;
        public ObservableCollection<Batch> BatchesList
        {
            get => _batchesList;
            set => SetProperty(ref _batchesList, value);
        }
        private Batch _selectedBatch;
        public Batch SelectedBatch
        {
            get => _selectedBatch;
            set => SetProperty(ref _selectedBatch, value);
        }
    }
}
