using Public.ChartConfig;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SFTP
{
    public class SyftServer : SFTPServices
    {
        public enum Type { Chart, Batch }
        public readonly string LocalChartPath = "./Temp/ChartConfig/";
        public readonly string LocalChartTempFile = "ChartTemp.xml";

        public readonly string LocalBatchPath = "./Temp/BatchConfig/";
        public readonly string LocalBatchTempFile = "BatchTemp.xml";

        public readonly string RemoteChartPath = "/home/sftp/files/syft-vision2/ChartConfig/";
        public readonly string RemoteBatchPath = "/home/sftp/files/syft-vision2/BatchConfig/";

        public SyftServer() : base("tools.syft.com", "22", "sftp", "MuhPEzxNchfr8nyZ") { }

        public ObservableCollection<TreeNode> GetTreeNodes(Type type)
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
                }

                Connect();
                List<string> folders = GetDirectoryList(remotePath);
                ObservableCollection<TreeNode> treeNodes = new ObservableCollection<TreeNode>();
                foreach (var folder in folders)
                {
                    if (folder == "." || folder == "..") continue;
                    // Set name
                    TreeNode treeNode = new TreeNode();
                    treeNode.Name = folder;
                    // Set child nodes
                    List<string> files = GetFileList(remotePath + folder, "xml");
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
    }
}
