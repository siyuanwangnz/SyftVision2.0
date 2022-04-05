using Public.Instrument;
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

namespace Public.SFTP
{
    public class InstrumentServer : SFTPServices
    {
        private readonly string RemoteMethodPath = "/usr/local/syft/lib/methods/";

        private readonly string LocalBatchPath = "./Temp/Batch/";
        private readonly string LocalBatchTempFile = "BatchTemp.xml";
        private string LocalBatchTempFilePath => LocalBatchPath + LocalBatchTempFile;

        private readonly string RemoteScanPath = "/usr/local/syft/data/";

        public InstrumentServer(string ipAddress, Options options) : base(ipAddress, options.Port, options.User, options.Password)
        {
            // Check local directory
            if (!Directory.Exists(LocalBatchPath)) Directory.CreateDirectory(LocalBatchPath);
        }

        public ObservableCollection<string> GetBatchFileList()
        {
            try
            {
                Connect();
                List<string> files = GetFileList(RemoteMethodPath, "sba");
                Disconnect();
                return new ObservableCollection<string>(files);
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

        public ObservableCollection<Method> GetMethodListFromBatchFile(string batchFile)
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
                        scanFileList.Add(new ScanFile(file, folder));
                }

                Disconnect();

                scanFileList = scanFileList.Where(x => string.CompareOrdinal(x.Time, _time) >= 0).ToList();

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
    }
}
