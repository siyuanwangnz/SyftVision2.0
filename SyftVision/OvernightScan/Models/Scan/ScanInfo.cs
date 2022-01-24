using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OvernightScan.Models
{
    public class ScanInfo
    {
        public ScanInfo(string filename, string foldername)
        {
            FileName = filename;
            FolderName = foldername;
        }
        public string FileName { get; private set; }
        public string FolderName { get; private set; }
        public string Name { get => Regex.Match(FileName, @"^(.*)-\d+-\d{8}-\d{6}\.xml$")?.Groups[1].Value ?? ""; }
        public string NameCode { get => Name?.ToLower().Replace(" ", "") ?? ""; }
        public int ID { get => int.Parse(Regex.Match(FileName, @"-(\d+)-\d{8}-\d{6}\.xml$")?.Groups[1].Value ?? "0"); }
        public string Date { get => Regex.Match(FileName, @"-(\d{8}-\d{6})\.xml$")?.Groups[1].Value ?? ""; }
    }
}
