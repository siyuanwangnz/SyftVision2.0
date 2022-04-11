using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Public.Instrument
{
    public class ScanFile
    {
        public ScanFile(string file, string remoteFolder)
        {
            File = file;

            RemoteFolder = remoteFolder;

            Name = Regex.Match(File, @"^(.*)-\d+-\d{8}-\d{6}\.xml$").Groups[1].Value;

            NameCode = Name.ToLower().Replace(" ", "");

            NameHashCode = NameCode.GetHashCode();

            string id = Regex.Match(File, @"-(\d+)-\d{8}-\d{6}\.xml$").Groups[1].Value;
            ID = int.Parse(id == "" ? "0" : id);

            Date_Time = Regex.Match(File, @"-(\d{8}-\d{6})\.xml$").Groups[1].Value;

            Date = Regex.Match(Date_Time, @"^(\d{8})-\d{6}$").Groups[1].Value;

            Time = Regex.Match(Date_Time, @"^\d{8}-(\d{6})$").Groups[1].Value;
        }
        public string File { get; }
        public string RemoteFolder { get; }
        public string RemoteFilePath { get => $"{RemoteFolder}/{File}"; }
        public string FullLocalFolder { get; set; } = "";
        public string FullLocalFilePath { get => $"{FullLocalFolder}/{File}"; }
        public Scan Scan { get => new Scan(FullLocalFilePath); }
        public string Name { get; }
        public string NameCode { get; }
        public int NameHashCode { get; }
        public int ID { get; }
        public string Date_Time { get; }
        public string Date { get; }
        public string Time { get; }
    }
}
