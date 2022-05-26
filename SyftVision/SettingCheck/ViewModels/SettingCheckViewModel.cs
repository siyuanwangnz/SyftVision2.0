using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Public.ChartConfig;
using Public.Instrument;
using Public.SettingConfig;
using Public.SFTP;
using Public.TreeList;
using SettingCheck.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
                    CommonOpenFileDialog dlg = new CommonOpenFileDialog();
                    dlg.IsFolderPicker = true;
                    dlg.Title = "Select a Target Folder to Download Setting Config Files";
                    if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        try
                        {
                            _syftServer.DownloadSettingConfigFolder(dlg.FileName);
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
                            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
                            dlg.Title = "Select a Local Setting Config File";
                            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                SettingProp = new SettingProp(XElement.Load(dlg.FileName));

                                Tittle = SettingProp.Tittle;
                                SubTittle = SettingProp.SubTittle;
                            }
                        }
                        else // Remote Selection
                        {
                            // Get tree nodes
                            List<TreeNode> treeNodes = _syftServer.GetTreeNodes(SyftServer.Type.Setting);

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
            set
            {
                SetProperty(ref _localModeIsChecked, value);
                if (_localModeIsChecked == true) ScanFileCollectionIsChecked = false;
            }
        }
        public DelegateCommand OpenCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SettingProp == null)
                    {
                        MessageBox.Show($"Please select a Setting Config", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    try
                    {
                        if (LocalModeIsChecked) // Local Selection
                        {
                            //Open file path selection dialog
                            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
                            dlg.Title = "Select a Local Scan File";
                            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                FileInfo fileInfo = new FileInfo(dlg.FileName);
                                SettingProp.SetContentForSettingList(XElement.Load(dlg.FileName), new ScanFile(fileInfo.Name) { FullLocalFolder = fileInfo.Directory.FullName });

                                SettingList = new ObservableCollection<Setting>(SettingProp.SettingList);

                                SyftInfoList = new ObservableCollection<SyftInfo>(SettingProp.SyftInfoList);
                            }
                        }
                        else // Remote Selection
                        {
                            _instrumentServer = new InstrumentServer(IPAddress, new Public.Global.Options());

                            // Get tree nodes
                            List<TreeNode> treeNodes = _instrumentServer.GetScanFileTreeNodes();

                            // Navigate to dialog
                            DialogParameters param = new DialogParameters();
                            param.Add("treeNodes", treeNodes);
                            _dialogService.ShowDialog("InstruScanDialogView", param, arg =>
                            {
                                if (arg.Result == ButtonResult.OK)
                                {
                                    TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                    _instrumentServer.ClearLocalLoadedSettingScanPath();

                                    ScanFile scanFile = _instrumentServer.GetScanFile(treeNode);

                                    SettingProp.SetContentForSettingList(XElement.Load(scanFile.FullLocalFilePath), scanFile);

                                    SettingList = new ObservableCollection<Setting>(SettingProp.SettingList);

                                    SyftInfoList = new ObservableCollection<SyftInfo>(SettingProp.SyftInfoList);
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
                    if (SettingProp == null)
                    {
                        MessageBox.Show($"Please select a Setting Config", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (SettingProp != null && SettingProp.HasSet == false)
                    {
                        MessageBox.Show($"Please open a scan first", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    try
                    {
                        if (LocalModeIsChecked) // Local Selection
                        {
                            //Open file path selection dialog
                            CommonOpenFileDialog dlg = new CommonOpenFileDialog();
                            dlg.Title = "Select a Local Scan File";
                            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                FileInfo fileInfo = new FileInfo(dlg.FileName);
                                SettingProp.AddContentForSettingList(XElement.Load(dlg.FileName), new ScanFile(fileInfo.Name));

                                SettingList = new ObservableCollection<Setting>(SettingProp.SettingList);
                            }
                        }
                        else // Remote Selection
                        {
                            _instrumentServer = new InstrumentServer(IPAddress, new Public.Global.Options());

                            // Get tree nodes
                            List<TreeNode> treeNodes = _instrumentServer.GetScanFileTreeNodes();

                            // Navigate to dialog
                            DialogParameters param = new DialogParameters();
                            param.Add("treeNodes", treeNodes);
                            _dialogService.ShowDialog("InstruScanDialogView", param, arg =>
                            {
                                if (arg.Result == ButtonResult.OK)
                                {
                                    TreeNode treeNode = arg.Parameters.GetValue<TreeNode>("selectedTreeNode");

                                    ScanFile scanFile = _instrumentServer.GetScanFile(treeNode);

                                    SettingProp.AddContentForSettingList(XElement.Load(scanFile.FullLocalFilePath), scanFile);

                                    SettingList = new ObservableCollection<Setting>(SettingProp.SettingList);
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
        private bool _scanFileCollectionIsChecked;
        public bool ScanFileCollectionIsChecked
        {
            get => _scanFileCollectionIsChecked;
            set => SetProperty(ref _scanFileCollectionIsChecked, value);
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    if (SettingProp == null || !SettingProp.HasSet) return;

                    //Open folder path selection dialog
                    CommonOpenFileDialog dlg = new CommonOpenFileDialog();
                    dlg.IsFolderPicker = true;
                    dlg.Title = "Select a Target Folder to Save";
                    if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        try
                        {
                            if (ScanFileCollectionIsChecked)
                            {
                                ScanFile scanFile = SettingProp.SettingList.First().ScanList.First();
                                if (scanFile.RemoteFilePath != "") File.Copy(scanFile.FullLocalFilePath, dlg.FileName + scanFile.File, true);
                            }

                            new SyftPDF(SettingProp, Comments, dlg.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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
                    // Navigate to dialog
                    DialogParameters param = new DialogParameters();
                    param.Add("Chart", SelectedSetting.Chart);
                    param.Add("ScanFileList", SelectedSetting.ScanList);
                    param.Add("XYLegendList", SelectedSetting.XYLegendList);
                    _dialogService.ShowDialog("ChartDialogView", param, arg =>
                    {
                    });
                });
            }
        }
    }
}
