using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Renci.SshNet;

namespace Public.SFTP
{
    public class SFTPServices
    {
        private readonly SftpClient sftp;
        //status of connection
        public bool Connected { get { return sftp.IsConnected; } }

        public SFTPServices(string ip, string port, string user, string pwd)
        {
            try
            {
                sftp = new SftpClient(ip, int.Parse(port), user, pwd);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR");
            }
        }

        //success return true
        public bool Connect()
        {
            try
            {
                if (!Connected)
                {
                    sftp.Connect();
                }
                return true;
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), $"Connection failed，Reason：{ex.Message}");
                throw new Exception($"Connection failed，Reason：{ex.Message}");
            }
        }

        public void Disconnect()
        {
            try
            {
                if (sftp != null && Connected)
                {
                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), $"Disconnection failed，Reason：{ex.Message}");
                throw new Exception($"Disconnection failed，Reason：{ex.Message}");
            }
        }

        public void DownloadFile(string remotePath, string localPath)
        {
            try
            {
                var byt = sftp.ReadAllBytes(remotePath);
                File.WriteAllBytes(localPath, byt);
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), $"Download failed，Reason：{ex.Message}");
                throw new Exception($"Download File failed，Reason：{ex.Message}");
            }

        }

        public void DownloadDirectory(string remotePath, string localPath)
        {
            try
            {
                Directory.CreateDirectory(localPath);
                foreach (var file in sftp.ListDirectory(remotePath))
                {
                    if ((file.Name != ".") && (file.Name != ".."))
                    {
                        string sourceFilePath = remotePath + "/" + file.Name;
                        string destFilePath = Path.Combine(localPath, file.Name);
                        if (file.IsDirectory)
                        {
                            DownloadDirectory(sourceFilePath, destFilePath);
                        }
                        else
                        {
                            using (Stream fileStream = File.Create(destFilePath))
                            {
                                sftp.DownloadFile(sourceFilePath, fileStream);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Download Directory failed，Reason：{ex.Message}");
            }
        }

        public List<string> GetFileList(string remotePath, string fileSuffix)
        {
            try
            {
                var files = sftp.ListDirectory(remotePath);
                List<string> FileList = new List<string>();
                foreach (var file in files)
                {
                    string name = file.Name;
                    if (name.Length > fileSuffix.Length + 1 && fileSuffix == name.Substring(name.Length - fileSuffix.Length))
                    {
                        FileList.Add(name);
                    }
                }
                return FileList;
            }
            catch (Exception ex)
            {
                // TxtLog.WriteTxt(CommonMethod.GetProgramName(), $"File list collection failed，Reason：{ex.Message}");
                throw new Exception($"File list collection failed，Reason：{ex.Message}");
            }
        }

        public List<string> GetDirectoryList(string remotePath)
        {
            try
            {
                var directories = sftp.ListDirectory(remotePath);
                List<string> DirectoriesList = new List<string>();
                foreach (var directory in directories)
                {
                    string name;
                    if (directory.IsDirectory)
                    {
                        name = directory.Name;
                        DirectoriesList.Add(name);
                    }
                }
                return DirectoriesList;
            }
            catch (Exception ex)
            {
                // TxtLog.WriteTxt(CommonMethod.GetProgramName(), $"File list collection failed，Reason：{ex.Message}");
                throw new Exception($"File list collection failed，Reason：{ex.Message}");
            }
        }

    }
}
