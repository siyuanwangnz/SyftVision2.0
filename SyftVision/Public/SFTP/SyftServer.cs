using Public.BatchConfig;
using Public.ChartConfig;
using Public.SettingConfig;
using Public.TreeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Public.SFTP
{
    public class SyftServer : SFTPServices
    {
        public enum Type { Chart, Batch, Setting }

        private readonly string LocalChartPath = "./Temp/ChartConfig/";
        private readonly string LocalChartTempFile = "ChartTemp.xml";

        private readonly string LocalBatchPath = "./Temp/BatchConfig/";
        private readonly string LocalBatchTempFile = "BatchTemp.xml";

        private readonly string LocalSettingPath = "./Temp/SettingConfig/";
        private readonly string LocalSettingTempFile = "SettingTemp.xml";

        private readonly string RemoteChartPath = "/home/sftp/files/syft-vision2/ChartConfig/";
        private readonly string RemoteBatchPath = "/home/sftp/files/syft-vision2/BatchConfig/";
        private readonly string RemoteSettingPath = "/home/sftp/files/syft-vision2/SettingConfig/";

        private string LocalChartTempFilePath => LocalChartPath + LocalChartTempFile;
        private string LocalBatchTempFilePath => LocalBatchPath + LocalBatchTempFile;
        private string LocalSettingTempFilePath => LocalSettingPath + LocalSettingTempFile;

        public SyftServer() : base("tools.syft.com", "22", "sftp", "MuhPEzxNchfr8nyZ")
        {
            // Check local directory
            if (!Directory.Exists(LocalChartPath)) Directory.CreateDirectory(LocalChartPath);
            if (!Directory.Exists(LocalBatchPath)) Directory.CreateDirectory(LocalBatchPath);
            if (!Directory.Exists(LocalSettingPath)) Directory.CreateDirectory(LocalSettingPath);
        }

        public List<TreeNode> GetTreeNodes(Type type)
        {
            try
            {
                string remotePath = "";
                switch (type)
                {
                    case Type.Chart:
                        remotePath = RemoteChartPath;
                        break;
                    case Type.Batch:
                        remotePath = RemoteBatchPath;
                        break;
                    case Type.Setting:
                        remotePath = RemoteSettingPath;
                        break;
                }

                Connect();
                List<string> folders = GetDirectoryList(remotePath);
                folders.Sort();
                List<TreeNode> treeNodes = new List<TreeNode>();
                foreach (var folder in folders)
                {
                    if (folder == "." || folder == "..") continue;
                    // Set name
                    TreeNode treeNode = new TreeNode();
                    treeNode.Name = folder;
                    // Set child nodes
                    List<string> files = GetFileList(remotePath + folder, "xml");
                    files.Sort();
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

        public ChartProp DownloadChart(TreeNode treeNode)
        {
            try
            {
                Connect();
                DownloadFile(RemoteChartPath + treeNode.Parent + "/" + treeNode.Name, LocalChartTempFilePath);
                Disconnect();

                return new ChartProp(XElement.Load(LocalChartTempFilePath));
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

        public void UploadChart(ChartProp chartProp)
        {
            try
            {
                string remoteFolderPath = RemoteChartPath + chartProp.Tittle + "/";

                chartProp.XMLGeneration().Save(LocalChartTempFilePath);

                Connect();
                // Check existing file
                if (Exist(remoteFolderPath + chartProp.File))
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show($"The chart file already exists, do you want to replace it?", "QUESTION", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.No) return;
                }

                // Upload file
                if (!Exist(remoteFolderPath)) CreateDirectory(remoteFolderPath);
                UploadFile(remoteFolderPath + chartProp.File, LocalChartTempFilePath);
                Disconnect();
                MessageBox.Show("Chart has been saved", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public BatchProp DownloadBatch(TreeNode treeNode)
        {
            try
            {
                Connect();
                DownloadFile(RemoteBatchPath + treeNode.Parent + "/" + treeNode.Name, LocalBatchTempFilePath);
                Disconnect();

                return new BatchProp(XElement.Load(LocalBatchTempFilePath));
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

        public void UploadBatch(BatchProp batchProp)
        {
            try
            {
                string remoteFolderPath = RemoteBatchPath + batchProp.Tittle + "/";

                batchProp.XMLGeneration().Save(LocalBatchTempFilePath);

                Connect();
                // Check existing file
                if (Exist(remoteFolderPath + batchProp.File))
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show($"The batch file already exists, do you want to replace it?", "QUESTION", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.No) return;
                }

                // Upload file
                if (!Exist(remoteFolderPath)) CreateDirectory(remoteFolderPath);
                UploadFile(remoteFolderPath + batchProp.File, LocalBatchTempFilePath);
                Disconnect();
                MessageBox.Show("Batch has been saved", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public SettingProp DownloadSetting(TreeNode treeNode)
        {
            try
            {
                Connect();
                DownloadFile(RemoteSettingPath + treeNode.Parent + "/" + treeNode.Name, LocalSettingTempFilePath);
                Disconnect();

                return new SettingProp(XElement.Load(LocalSettingTempFilePath));
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

        public void UploadSetting(SettingProp settingProp)
        {
            try
            {
                string remoteFolderPath = RemoteSettingPath + settingProp.Tittle + "/";

                settingProp.XMLGeneration().Save(LocalSettingTempFilePath);

                Connect();
                // Check existing file
                if (Exist(remoteFolderPath + settingProp.File))
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show($"The setting file already exists, do you want to replace it?", "QUESTION", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.No) return;
                }

                // Upload file
                if (!Exist(remoteFolderPath)) CreateDirectory(remoteFolderPath);
                UploadFile(remoteFolderPath + settingProp.File, LocalSettingTempFilePath);
                Disconnect();
                MessageBox.Show("Setting has been saved", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public void DownloadBatchConfigFolder(string targetFolder)
        {
            try
            {
                string localFolderPath = targetFolder + "/LocalBatchConfig/";
                if (!Directory.Exists(localFolderPath)) Directory.CreateDirectory(localFolderPath);
                Connect();
                DownloadDirectory(RemoteBatchPath, localFolderPath);
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
        public void DownloadSettingConfigFolder(string targetFolder)
        {
            try
            {
                string localFolderPath = targetFolder + "/LocalSettingConfig/";
                if (!Directory.Exists(localFolderPath)) Directory.CreateDirectory(localFolderPath);
                Connect();
                DownloadDirectory(RemoteSettingPath, localFolderPath);
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
    }
}
