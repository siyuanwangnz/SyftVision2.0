using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Public.ChartConfig;
using Public.SettingConfig;
using Public.SFTP;
using Public.TreeList;
using SettingCheck.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SettingCheck.ViewModels
{
    public class SettingCheckViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly SyftServer _syftServer;
        private InstrumentServer _instrumentServer;
        private SettingProp SettingProp;
        private Task Task;
        private bool TaskIsRunning()
        {
            if (Task != null && !Task.IsCompleted)
            {
                MessageBox.Show($"In the process", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            return false;
        }
        public SettingCheckViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _syftServer = new SyftServer();
        }

        #region Toolbar
        public DelegateCommand DownloadCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

                    //Open folder path selection dialog
                    CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
                    folderDlg.IsFolderPicker = true;
                    folderDlg.Title = "Select a Target Folder to Download Setting Config Files";
                    if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        try
                        {
                            _syftServer.DownloadSettingConfigFolder(folderDlg.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        }
        private bool _localSelectIsChecked;
        public bool LocalSelectIsChecked
        {
            get => _localSelectIsChecked;
            set => SetProperty(ref _localSelectIsChecked, value);
        }
        public DelegateCommand SelectCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TaskIsRunning()) return;

                    try
                    {
                        if (LocalSelectIsChecked) // Local Selection
                        {
                            //Open file path selection dialog
                            CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
                            folderDlg.Title = "Select a Local Setting Config File";
                            if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                SettingProp = new SettingProp(XElement.Load(folderDlg.FileName));

                                Tittle = SettingProp.Tittle;
                                SubTittle = SettingProp.SubTittle;
                                SettingList = SettingProp.SettingList;
                            }
                        }
                        else // Remote Selection
                        {
                            // Get tree nodes
                            ObservableCollection<TreeNode> treeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Setting);

                            // Navigate to dialog
                            DialogParameters param = new DialogParameters();
                            param.Add("treeNodes", treeNodes);
                            _dialogService.ShowDialog("SyftSettingDialogView", param, arg =>
                            {
                                if (arg.Result == ButtonResult.OK)
                                {
                                    TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                    SettingProp = _syftServer.DownloadSetting(treeNode);

                                    Tittle = SettingProp.Tittle;
                                    SubTittle = SettingProp.SubTittle;
                                    SettingList = SettingProp.SettingList;
                                }
                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        private string _tittle;
        public string Tittle
        {
            get => _tittle;
            set => SetProperty(ref _tittle, value);
        }
        private string _subTittle;
        public string SubTittle
        {
            get => _subTittle;
            set => SetProperty(ref _subTittle, value);
        }
        private string _ipAddress = "10.0.17.";
        public string IPAddress
        {
            get => _ipAddress;
            set => SetProperty(ref _ipAddress, value);
        }
        private bool _localModeIsChecked;
        public bool LocalModeIsChecked
        {
            get => _localModeIsChecked;
            set => SetProperty(ref _localModeIsChecked, value);
        }
        public DelegateCommand OpenCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        if (LocalModeIsChecked) // Local Selection
                        {
                            //Open file path selection dialog
                            CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
                            folderDlg.Title = "Select a Local Scan File";
                            if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                XElement.Load(folderDlg.FileName);

                            }
                        }
                        else // Remote Selection
                        {
                            _instrumentServer = new InstrumentServer(IPAddress, new Public.Global.Options());

                            // Get tree nodes
                            ObservableCollection<TreeNode> treeNodes = _instrumentServer.GetScanFileTreeNodes();

                            // Navigate to dialog
                            DialogParameters param = new DialogParameters();
                            param.Add("treeNodes", treeNodes);
                            _dialogService.ShowDialog("InstruScanDialogView", param, arg =>
                            {
                                if (arg.Result == ButtonResult.OK)
                                {
                                    TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                }
                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        if (LocalModeIsChecked) // Local Selection
                        {
                            //Open file path selection dialog
                            CommonOpenFileDialog folderDlg = new CommonOpenFileDialog();
                            folderDlg.Title = "Select a Local Scan File";
                            if (folderDlg.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                XElement.Load(folderDlg.FileName);

                            }
                        }
                        else // Remote Selection
                        {
                            _instrumentServer = new InstrumentServer(IPAddress, new Public.Global.Options());

                            // Get tree nodes
                            ObservableCollection<TreeNode> treeNodes = _instrumentServer.GetScanFileTreeNodes();

                            // Navigate to dialog
                            DialogParameters param = new DialogParameters();
                            param.Add("treeNodes", treeNodes);
                            _dialogService.ShowDialog("InstruScanDialogView", param, arg =>
                            {
                                if (arg.Result == ButtonResult.OK)
                                {
                                    TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                }
                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        private string _comments = "";
        public string Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }
        private ObservableCollection<SyftInfo> _syftInfoList;
        public ObservableCollection<SyftInfo> SyftInfoList
        {
            get => _syftInfoList;
            set => SetProperty(ref _syftInfoList, value);
        }
        #endregion

        private ObservableCollection<Setting> _settingList;
        public ObservableCollection<Setting> SettingList
        {
            get => _settingList;
            set => SetProperty(ref _settingList, value);
        }
        private Setting _selectedSetting;
        public Setting SelectedSetting
        {
            get => _selectedSetting;
            set => SetProperty(ref _selectedSetting, value);
        }
        public DelegateCommand SelectedCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    //// Navigate to dialog
                    //DialogParameters param = new DialogParameters();
                    //param.Add("SelectedSetting", SelectedSetting);
                    //_dialogService.ShowDialog(SelectedSetting.Type.LimitSetDialog, param, arg =>
                    //{
                    //    if (arg.Result == ButtonResult.OK)
                    //    {
                    //        switch (SelectedSetting.Type.Name)
                    //        {
                    //            case "Map":
                    //                SelectedSetting.MapSetList = arg.Parameters.GetValue<ObservableCollection<SettingMap>>("MapSetList");
                    //                break;
                    //            case "Table":
                    //                SelectedSetting.TableSetList = arg.Parameters.GetValue<ObservableCollection<SettingTable>>("TableSetList");
                    //                break;
                    //            case "OnOff":
                    //                SelectedSetting.OnOff = arg.Parameters.GetValue<SettingOnOff>("SettingOnOff");
                    //                break;
                    //            case "Value":
                    //                SelectedSetting.Value = arg.Parameters.GetValue<SettingValue>("SettingValue");
                    //                break;
                    //            case "Text":
                    //                SelectedSetting.Text = arg.Parameters.GetValue<SettingText>("SettingText");
                    //                break;
                    //        }
                    //    }
                    //});
                });
            }
        }
    }
}
