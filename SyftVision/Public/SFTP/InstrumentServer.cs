using Public.ChartConfig;
using Public.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.SFTP
{
    public class InstrumentServer : SFTPServices
    {
        private readonly string RemoteMethodsPath = "/usr/local/syft/lib/methods/";
        public InstrumentServer(string ipAddress, Options options) : base(ipAddress, options.Port, options.User, options.Password) { }

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
    }
}
