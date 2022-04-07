using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.ViewModels
{
    public class TroubleshootDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "Match Troubleshoot";
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
            string sourceScanList = "";
            foreach (var line in parameters.GetValue<List<string>>("sourceScanList"))
                sourceScanList = sourceScanList + line + "\r\n";
            SourceScanList = sourceScanList;

            string referScanList = "";
            foreach (var line in parameters.GetValue<List<string>>("referScanList"))
                referScanList = referScanList + line + "\r\n";
            ReferScanList = referScanList;
        }
        private string _sourceScanList;
        public string SourceScanList
        {
            get => _sourceScanList;
            set => SetProperty(ref _sourceScanList, value);
        }
        private string _referScanList;
        public string ReferScanList
        {
            get => _referScanList;
            set => SetProperty(ref _referScanList, value);
        }

    }
}
