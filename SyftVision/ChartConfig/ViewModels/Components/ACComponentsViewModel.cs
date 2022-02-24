using ChartConfig.Models;
using ChartConfig.Models.Components;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.ViewModels.Components
{
    class ACComponentsViewModel : BindableBase, INavigationAware
    {
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ComponentsList = new ObservableCollection<ACComponent>() {
            new ACComponent("1asd",true),
            new ACComponent("2asd",false),
            new ACComponent("3asd",false),
            new ACComponent("4asd",false)
            };

        }

        public DelegateCommand Delete
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentsList.Remove(SelectedComponent);
                    if (ComponentsList.Count == 0)
                        ComponentsList.Add(new ACComponent("acsd", false));
                });
            }
        }

        public DelegateCommand AddUp
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentsList.Insert(ComponentsList.IndexOf(SelectedComponent), new ACComponent("acasd", false));
                });
            }
        }

        public DelegateCommand AddDown
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentsList.Insert(ComponentsList.IndexOf(SelectedComponent) + 1, new ACComponent("acasd", false));
                });
            }
        }
        private ObservableCollection<ACComponent> _componentsList;
        public ObservableCollection<ACComponent> ComponentsList
        {
            get { return _componentsList; }
            set { SetProperty(ref _componentsList, value); }
        }

        private ACComponent _selectedComponent;
        public ACComponent SelectedComponent
        {
            get { return _selectedComponent; }
            set { SetProperty(ref _selectedComponent, value); }
        }
    }
}
