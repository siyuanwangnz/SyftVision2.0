using Public.Global;
using Public.SFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace SettingCheck.Services
{
    static class GetRootNode
    {
        public static XElement Config()
        {
            try
            {
                return XElement.Load($"./Config/P3_SettingCheck_Config.xml");
            }
            catch (Exception ex)
            {
                throw new Exception($"Load config file failed，Reason：{ex.Message}");
            }
        }

        public static XElement LatestScan(string IP, out string FileName)
        {
            string CurrentDirectory = Directory.GetCurrentDirectory();
            if (!Directory.Exists($"{CurrentDirectory}/temp/Setting_Check"))
            {
                Directory.CreateDirectory($"{CurrentDirectory}/temp/Setting_Check");
            }

            SFTPServices sftp = new SFTPServices(IP, "22", "root", Global.Password);

            try
            {
                sftp.Connect();
                //Get Directory List
                List<string> DirectoryList = sftp.GetDirectoryList("/usr/local/syft/data/");
                //Get Directory Date List xxxx-xx-xx
                List<string> DirectoryDateList = new List<string>();
                foreach (var directory in DirectoryList)
                {
                    if (Regex.IsMatch(directory, @"^\d{4}-\d{2}-\d{2}$"))
                    {
                        DirectoryDateList.Add(directory);
                    }
                }
                ////Get File List
                List<string> FileList = sftp.GetFileList($"/usr/local/syft/data/{DirectoryDateList.Max()}", "xml");
                //Get File Date List xxxxxxxx-xxxxxx
                List<string> FileDateList = new List<string>();
                foreach (var File in FileList)
                {
                    Match match = Regex.Match(File, @"-(\d{8}-\d{6})\.xml$");
                    FileDateList.Add(match.Groups[1].Value); //Groups[0]: full string, Groups[1]: selected() string 
                }
                //Get Target File
                string TargetFile = "";
                foreach (var File in FileList)
                {
                    if (File.Contains(FileDateList.Max()))
                    {
                        TargetFile = File;
                    }
                }
                FileName = TargetFile;
                //Download Targer File
                sftp.DownloadFile($"/usr/local/syft/data/{DirectoryDateList.Max()}/{TargetFile}", $"{CurrentDirectory}/temp/Setting_Check/{TargetFile}");
                sftp.Disconnect();

                return XElement.Load($"{CurrentDirectory}/temp/Setting_Check/{TargetFile}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Load scan file failed，Reason：{ex.Message}");
            }
            finally
            {
                sftp.Disconnect();
            }
        }

        //FilePath is full name of file include path
        public static XElement SavedScan(string FilePath)
        {
            try
            {
                return XElement.Load($"{FilePath}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Load saved scan file failed，Reason：{ex.Message}");
            }
        }
    }
}
