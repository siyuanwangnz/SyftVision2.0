﻿using Public.Instrument;
using Public.BatchConfig;
using Public.ChartConfig;
using Public.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Public.TreeList;
using Public.SettingConfig;

namespace Public.SFTP
{
    public class InstrumentServer : SFTPServices
    {
        private readonly string RemoteMethodPath = "/usr/local/syft/lib/methods/";

        private readonly string LocalBatchPath = "./Temp/Batch/";
        private readonly string LocalBatchTempFile = "BatchTemp.xml";
        private string LocalBatchTempFilePath => LocalBatchPath + LocalBatchTempFile;

        private readonly string LocalSettingScanPath = "./Temp/SettingScan/";
        private readonly string LocalSettingScanTempFile = "SettingScanTemp.xml";
        private string LocalSettingScanTempFilePath => LocalSettingScanPath + LocalSettingScanTempFile;

        private readonly string LocalLoadedSettingScanPath = "./Temp/SettingScan/LoadedSettingScan/";

        private readonly string RemoteScanPath = "/usr/local/syft/data/";
        private static readonly string LocalScanPath = "./Temp/Scan/";

        public InstrumentServer(string ipAddress, Options options) : base(ipAddress, options.Port, options.User, options.Password)
        {
            // Check local directory
            if (!Directory.Exists(LocalBatchPath)) Directory.CreateDirectory(LocalBatchPath);
            if (!Directory.Exists(LocalScanPath)) Directory.CreateDirectory(LocalScanPath);
            if (!Directory.Exists(LocalSettingScanPath)) Directory.CreateDirectory(LocalSettingScanPath);
            if (!Directory.Exists(LocalLoadedSettingScanPath)) Directory.CreateDirectory(LocalLoadedSettingScanPath);
        }

        // Batch file for batch config
        public List<string> GetBatchFileList()
        {
            try
            {
                Connect();
                List<string> files = GetFileList(RemoteMethodPath, "sba");
                Disconnect();
                return files;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        public List<Method> GetMethodListFromBatchFile(string batchFile)
        {
            try
            {
                Connect();
                DownloadFile(RemoteMethodPath + batchFile, LocalBatchTempFilePath);
                Disconnect();

                return new BatchFile(XElement.Load(LocalBatchTempFilePath)).GetMethodList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

        }

        // Scan file for batch analysis
        public List<ScanFile> GetScanFileList(DateTime date, DateTime time)
        {
            try
            {
                // Get date and time
                string _date = date.ToString("yyyy-MM-dd");
                string _time = time.ToString("HHmmss");

                Connect();

                // Get folder list that folder's format is xxxx-xx-xx and >= date
                List<string> folderList = GetDirectoryList(RemoteScanPath);
                folderList = folderList.Where(x => Regex.IsMatch(x, @"^\d{4}-\d{2}-\d{2}$") && string.CompareOrdinal(x, _date) >= 0).ToList();

                // Get scan list that >= time
                List<ScanFile> scanFileList = new List<ScanFile>();
                foreach (string folder in folderList)
                {
                    List<string> fileList = GetFileList(RemoteScanPath + folder, "xml");
                    foreach (var file in fileList)
                    {
                        ScanFile scanFile = new ScanFile(file);
                        scanFile.RemoteFolder = folder;
                        scanFileList.Add(scanFile);
                    }
                }

                Disconnect();

                scanFileList = scanFileList.Where(x => !(string.CompareOrdinal(x.RemoteFolder, _date) == 0 && string.CompareOrdinal(x.Time, _time) < 0)).ToList();

                // Order scan list (ascending comparing by Date Time and then ID)
                scanFileList.Sort((a, b) => a.Date_Time.CompareTo(b.Date_Time) == 0 ? a.ID.CompareTo(b.ID) : a.Date_Time.CompareTo(b.Date_Time));

                return scanFileList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

        }

        public void ClearLocalScanPath()
        {
            if (Directory.Exists(LocalScanPath)) new DirectoryInfo(LocalScanPath).Delete(true);
        }

        public void DownloadScanFileList(List<ScanFile> scanFileList, Action progress)
        {
            try
            {
                // Create directory
                string folder = scanFileList.First().Date_Time;
                Directory.CreateDirectory(LocalScanPath + folder);

                Connect();

                foreach (var scanFile in scanFileList)
                {
                    scanFile.FullLocalFolder = LocalScanPath + folder;
                    DownloadFile(RemoteScanPath + scanFile.RemoteFilePath, scanFile.FullLocalFilePath);
                    progress.Invoke();
                }

                Disconnect();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        public static void CopyScanFile(string folderPath)
        {
            // Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(LocalScanPath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(LocalScanPath, folderPath + "/"));

            // Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(LocalScanPath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(LocalScanPath, folderPath + "/"), true);
        }

        // Scan file for setting config
        public List<TreeNode> GetScanFileTreeNodes()
        {
            try
            {
                Connect();
                List<string> folders = GetDirectoryList(RemoteScanPath);

                // Filter and re-order folders
                folders = folders.Where(x => Regex.IsMatch(x, @"^\d{4}-\d{2}-\d{2}$")).ToList();
                folders.Sort((a, b) => b.CompareTo(a));

                List<TreeNode> treeNodes = new List<TreeNode>();
                foreach (var folder in folders)
                {
                    if (folder == "." || folder == "..") continue;
                    // Set name
                    TreeNode treeNode = new TreeNode();
                    treeNode.Name = folder;
                    // Set child nodes
                    List<string> files = GetFileList(RemoteScanPath + folder, "xml");

                    // Re-order files
                    files.Sort((a, b) => Regex.Match(b, @"-(\d{8}-\d{6})\.xml$").Groups[1].Value.CompareTo(Regex.Match(a, @"-(\d{8}-\d{6})\.xml$").Groups[1].Value));

                    List<TreeNode> treeChildNodes = new List<TreeNode>();
                    foreach (string file in files)
                    {
                        TreeNode treeChildNode = new TreeNode();
                        treeChildNode.Name = file;
                        treeChildNode.Parent = folder;
                        treeChildNodes.Add(treeChildNode);
                    }
                    treeNode.ChildNodes = treeChildNodes;

                    treeNodes.Add(treeNode);
                }
                Disconnect();

                return treeNodes;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        public List<Setting> GetSettingListFromScanFile(TreeNode treeNode, List<FilterOff> filterOffList)
        {
            try
            {
                Connect();
                DownloadFile(RemoteScanPath + treeNode.Parent + "/" + treeNode.Name, LocalSettingScanTempFilePath);
                Disconnect();

                return Setting.GetSettingList(XElement.Load(LocalSettingScanTempFilePath), filterOffList);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        public void ClearLocalLoadedSettingScanPath()
        {
            if (Directory.Exists(LocalLoadedSettingScanPath))
            {
                foreach (FileInfo file in new DirectoryInfo(LocalLoadedSettingScanPath).GetFiles())
                {
                    file.Delete();
                }
            }
        }

        public ScanFile GetScanFile(TreeNode treeNode)
        {
            try
            {
                Connect();
                DownloadFile(RemoteScanPath + treeNode.Parent + "/" + treeNode.Name, LocalLoadedSettingScanPath + treeNode.Name);
                Disconnect();

                ScanFile scanFile = new ScanFile(treeNode.Name);
                scanFile.FullLocalFolder = LocalLoadedSettingScanPath;
                return scanFile;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
