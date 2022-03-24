using Public.Batch;
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

namespace Public.SFTP
{
    public class InstrumentServer : SFTPServices
    {
        private readonly string RemoteMethodsPath = "/usr/local/syft/lib/methods/";

        private readonly string LocalBatchPath = "./Temp/Batch/";
        private readonly string LocalBatchTempFile = "BatchTemp.xml";
        private string LocalBatchTempFilePath => LocalBatchPath + LocalBatchTempFile;
        public InstrumentServer(string ipAddress, Options options) : base(ipAddress, options.Port, options.User, options.Password)
        {
            // Check local directory
            if (!Directory.Exists(LocalBatchPath)) Directory.CreateDirectory(LocalBatchPath);
        }

        public ObservableCollection<string> GetBatchesList()
        {
            try
            {
                Connect();
                List<string> files = GetFileList(RemoteMethodsPath, "sba");
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

        public ObservableCollection<Method> GetMethodsList(string batchFile)
        {
            try
            {
                Connect();
                DownloadFile(RemoteMethodsPath + batchFile, LocalBatchTempFilePath);
                Disconnect();

                return new BatchFile(XElement.Load(LocalBatchTempFilePath)).GetMethodsList();
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
