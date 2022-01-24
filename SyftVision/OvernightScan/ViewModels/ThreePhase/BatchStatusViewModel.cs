using OvernightScan.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace OvernightScan.ViewModels.ThreePhase
{
    public class BatchStatusViewModel : BindableBase, INavigationAware
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
            switch (navigationContext.Parameters.GetValue<int>("NumberofBatches"))
            {
                default:
                case 1:
                    TabItemName = "Selected Batch";
                    TabItem2Visibility = Visibility.Collapsed;
                    TabItem3Visibility = Visibility.Collapsed;
                    TabItem4Visibility = Visibility.Collapsed;
                    TabItem5Visibility = Visibility.Collapsed;
                    List1 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[0];
                    break;
                case 2:
                    TabItemName = "Batch 1";
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Collapsed;
                    TabItem4Visibility = Visibility.Collapsed;
                    TabItem5Visibility = Visibility.Collapsed;
                    List1 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[0];
                    List2 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[1];
                    break;
                case 3:
                    TabItemName = "Batch 1";
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Visible;
                    TabItem4Visibility = Visibility.Collapsed;
                    TabItem5Visibility = Visibility.Collapsed;
                    List1 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[0];
                    List2 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[1];
                    List3 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[2];
                    break;
                case 4:
                    TabItemName = "Batch 1";
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Visible;
                    TabItem4Visibility = Visibility.Visible;
                    TabItem5Visibility = Visibility.Collapsed;
                    List1 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[0];
                    List2 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[1];
                    List3 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[2];
                    List4 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[3];
                    break;
                case 5:
                    TabItemName = "Batch 1";
                    TabItem2Visibility = Visibility.Visible;
                    TabItem3Visibility = Visibility.Visible;
                    TabItem4Visibility = Visibility.Visible;
                    TabItem5Visibility = Visibility.Visible;
                    List1 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[0];
                    List2 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[1];
                    List3 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[2];
                    List4 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[3];
                    List5 = navigationContext.Parameters.GetValue<Dictionary<string, List<ScanStatus>>>("batchScansStatus")?.Values.ToList()?[4];
                    break;
            }
        }

        #region Binding Property
        private List<ScanStatus> _list1;
        public List<ScanStatus> List1
        {
            get { return _list1; }
            set { SetProperty(ref _list1, value); }
        }
        private List<ScanStatus> _list2;
        public List<ScanStatus> List2
        {
            get { return _list2; }
            set { SetProperty(ref _list2, value); }
        }
        private List<ScanStatus> _list3;
        public List<ScanStatus> List3
        {
            get { return _list3; }
            set { SetProperty(ref _list3, value); }
        }
        private List<ScanStatus> _list4;
        public List<ScanStatus> List4
        {
            get { return _list4; }
            set { SetProperty(ref _list4, value); }
        }
        private List<ScanStatus> _list5;
        public List<ScanStatus> List5
        {
            get { return _list5; }
            set { SetProperty(ref _list5, value); }
        }
        private string _tabItemName;
        public string TabItemName
        {
            get { return _tabItemName; }
            set { SetProperty(ref _tabItemName, value); }
        }
        private System.Windows.Visibility _tabItem2Visibility;
        public System.Windows.Visibility TabItem2Visibility
        {
            get { return _tabItem2Visibility; }
            set { SetProperty(ref _tabItem2Visibility, value); }
        }
        private System.Windows.Visibility _tabItem3Visibility;
        public System.Windows.Visibility TabItem3Visibility
        {
            get { return _tabItem3Visibility; }
            set { SetProperty(ref _tabItem3Visibility, value); }
        }
        private System.Windows.Visibility _tabItem4Visibility;
        public System.Windows.Visibility TabItem4Visibility
        {
            get { return _tabItem4Visibility; }
            set { SetProperty(ref _tabItem4Visibility, value); }
        }
        private System.Windows.Visibility _tabItem5Visibility;
        public System.Windows.Visibility TabItem5Visibility
        {
            get { return _tabItem5Visibility; }
            set { SetProperty(ref _tabItem5Visibility, value); }
        }
        #endregion
    }
}
