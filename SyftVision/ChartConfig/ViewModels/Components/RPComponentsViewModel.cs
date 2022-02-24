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
    class RPComponentsViewModel : BindableBase, INavigationAware
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
            ComponentsList = new ObservableCollection<RPComponent>() {
            new RPComponent("ttt","rt",false),
            new RPComponent("rtrt","rt",false),
            new RPComponent("trt","rt",false),
            new RPComponent("rtr","rt",false)
            };
        }

        public DelegateCommand Delete
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentsList.Remove(_selectedComponent);
                    if (ComponentsList.Count == 0)
                        ComponentsList.Add(new RPComponent("rt", "", false));
                });
            }
        }

        public DelegateCommand AddUp
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentsList.Insert(ComponentsList.IndexOf(_selectedComponent), new RPComponent("rt", "", false));
                });
            }
        }

        public DelegateCommand AddDown
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ComponentsList.Insert(ComponentsList.IndexOf(_selectedComponent) + 1, new RPComponent("rtr", "", false));
                });
            }
        }
        private ObservableCollection<RPComponent> _componentsList;
        public ObservableCollection<RPComponent> ComponentsList
        {
            get { return _componentsList; }
            set { SetProperty(ref _componentsList, value); }
        }

        private RPComponent _selectedComponent;
        public RPComponent SelectedComponent
        {
            get { return _selectedComponent; }
            set { SetProperty(ref _selectedComponent, value); }
        }
    }
}
