﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingConfig.ViewModels
{
    public class OnOffLimitSetDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; } = "OnOff";
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
        public DelegateCommand SelectedCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                });
            }
        }


    }
}