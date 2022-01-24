using OvernightScan.Models;
using Public.Global;
using Public.SFTP;
using SyftXML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OvernightScan.Services
{
    class GetScans
    {
        public static Dictionary<string, List<TargetScanInfo>> BatchScans(Global.BatchType batchType, int number, int level, bool tail, DateTime date, DateTime time, string ip, out Dictionary<string, List<ScanStatus>> batchScansStatus, Action action1, Action action2)
        {
            SFTPServices sftp = new SFTPServices(ip, "22", "root", Global.Password);
            batchScansStatus = new Dictionary<string, List<ScanStatus>>();
            Dictionary<string, List<ScanInfo>> fullBatchScansDic = new Dictionary<string, List<ScanInfo>>();
            Dictionary<string, List<TargetScanInfo>> targetScanInfoListDic = new Dictionary<string, List<TargetScanInfo>>();

            try
            {
                //Create temp folder
                if (!Directory.Exists("./temp"))
                {
                    Directory.CreateDirectory("./temp");
                }

                //Delete temp/Setting_Check
                if (Directory.Exists("./temp/Batch_Analysis"))
                {
                    new DirectoryInfo("./temp/Batch_Analysis").Delete(true);
                }

                sftp.Connect();
                //Init Target Scan Files List - all scan files from start date
                List<ScanInfo> initScanInfoList = new List<ScanInfo>();
                //Get Date
                string Date = date.ToString("yyyy-MM-dd");
                //Get Time
                string Time = time.ToString("HHmmss");
                //Get Directory List
                List<string> directoryList = sftp.GetDirectoryList("/usr/local/syft/data/");
                foreach (var directory in directoryList)
                {
                    //Get Directory that formate is xxxx-xx-xx
                    if (Regex.IsMatch(directory, @"^\d{4}-\d{2}-\d{2}$"))
                    {
                        //Get Directory that >= date
                        if (string.CompareOrdinal(directory, Date) >= 0)
                        {
                            //Get all scan files from Directory Date List
                            List<string> fileList = sftp.GetFileList($"/usr/local/syft/data/{directory}", "xml");
                            //Set Target File List 
                            if (directory == Date)
                            {
                                //Compare time
                                foreach (var file in fileList)
                                {
                                    if (string.CompareOrdinal(Regex.Match(file, @"-\d{8}-(\d{6})\.xml$")?.Groups[1].Value, Time) >= 0)
                                        initScanInfoList.Add(new ScanInfo(file, directory));
                                }
                            }
                            else
                            {
                                foreach (var file in fileList)
                                {
                                    initScanInfoList.Add(new ScanInfo(file, directory));
                                }
                            }
                        }
                    }
                }
                //upadate Prograss
                action1.Invoke();
                //Sort Init Target File List - ascending
                initScanInfoList.Sort((a, b) => a.Date.CompareTo(b.Date));
                //upadate Prograss
                action1.Invoke();
                //Adjust sequence bug caused by same date
                for (int i = 0; i < initScanInfoList.Count - 1; i++)
                {
                    if (initScanInfoList[i].Date == initScanInfoList[i + 1].Date && initScanInfoList[i].ID > initScanInfoList[i + 1].ID)
                    {
                        var temp = initScanInfoList[i];
                        initScanInfoList[i] = initScanInfoList[i + 1];
                        initScanInfoList[i + 1] = temp;
                    }
                }
                //upadate Prograss
                action1.Invoke();
                //Get Full Config Method Info List
                List<ConfigMethodInfo> fullConfigMethodInfoList = GetFullConfigMethodList(batchType);
                //upadate Prograss
                action1.Invoke();
                //Get matched index
                List<int> indexList = new List<int>();
                switch (level)
                {
                    case 0:
                        indexList = GetMatchedIndexDependslist_Level0(initScanInfoList.Select(t => t.NameCode.GetHashCode()).ToList(), fullConfigMethodInfoList.Select(a => a.NameCode.GetHashCode()).ToList());
                        break;
                    case 1:
                        indexList = GetMatchedIndexDependslist_Level1(initScanInfoList.Select(t => t.NameCode.GetHashCode()).ToList(), fullConfigMethodInfoList.Select(a => a.NameCode.GetHashCode()).ToList());
                        break;
                    case 2:
                        indexList = GetMatchedIndexDependslist_Level2(initScanInfoList.Select(t => t.NameCode.GetHashCode()).ToList(), fullConfigMethodInfoList.Select(a => a.NameCode.GetHashCode()).ToList());
                        break;
                    default:
                        break;
                }
                //upadate Prograss
                action1.Invoke();

                if (indexList.Count >= number)
                {
                    if (tail)
                    {
                        int batchNumber = 0;
                        for (int i = indexList.Count - number; i < indexList.Count; i++)
                        {
                            batchNumber++;
                            List<ScanInfo> singleFullScanInfoList = initScanInfoList.GetRange(indexList[i], fullConfigMethodInfoList.Count);
                            string batch = $"{batchType}_{batchNumber} - ({singleFullScanInfoList[0].Date})";
                            fullBatchScansDic.Add(batch, singleFullScanInfoList);
                            targetScanInfoListDic.Add(batch, ConvertToTargetScanInfoList(singleFullScanInfoList, fullConfigMethodInfoList, batch));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < number; i++)
                        {
                            List<ScanInfo> singleFullScanInfoList = initScanInfoList.GetRange(indexList[i], fullConfigMethodInfoList.Count);
                            string batch = $"{batchType}_{i + 1} - ({singleFullScanInfoList[0].Date})";
                            fullBatchScansDic.Add(batch, singleFullScanInfoList);
                            targetScanInfoListDic.Add(batch, ConvertToTargetScanInfoList(singleFullScanInfoList, fullConfigMethodInfoList, batch));
                        }
                    }
                    //upadate Prograss
                    action1.Invoke();
                    //Create Batch_Analysis Folder
                    if (!Directory.Exists("./temp/Batch_Analysis"))
                    {
                        Directory.CreateDirectory("./temp/Batch_Analysis");
                    }
                    //Download Full Scans
                    foreach (var dicItem in fullBatchScansDic)
                    {
                        foreach (var scanInfo in dicItem.Value)
                        {
                            string tempSaveFolderPath = $"./temp/Batch_Analysis/Full Scans/{dicItem.Key}";
                            if (!Directory.Exists(tempSaveFolderPath))
                            {
                                Directory.CreateDirectory(tempSaveFolderPath);
                            }
                            sftp.DownloadFile($"/usr/local/syft/data/{scanInfo.FolderName}/{scanInfo.FileName}", $"{tempSaveFolderPath}/{scanInfo.FileName}");
                            //upadate Prograss
                            action2.Invoke();
                        }
                    }
                    //Download Target Scans
                    foreach (var dicItem in targetScanInfoListDic)
                    {
                        foreach (var targetScanInfo in dicItem.Value)
                        {
                            foreach (var item in targetScanInfo.MarkList)
                            {
                                string tempSaveFolderPath = $"./temp/Batch_Analysis/{dicItem.Key}/{item.Main}/{item.Sub}";
                                if (!Directory.Exists(tempSaveFolderPath))
                                {
                                    Directory.CreateDirectory(tempSaveFolderPath);
                                }
                                System.IO.File.Copy($"./temp/Batch_Analysis/Full Scans/{dicItem.Key}/{targetScanInfo.FileName}", $"{tempSaveFolderPath}/{targetScanInfo.FileName}");
                                //sftp.DownloadFile($"/usr/local/syft/data/{targetScanInfo.FolderName}/{targetScanInfo.FileName}", $"{tempSaveFolderPath}/{targetScanInfo.FileName}");
                            }
                        }
                        //upadate Prograss
                        action2.Invoke();
                    }
                }

                sftp.Disconnect();

                //Get Batch Scans Status
                foreach (var batch in fullBatchScansDic)
                {
                    List<ScanStatus> scanList = new List<ScanStatus>();
                    foreach (var scan in batch.Value)
                    {
                        Scan scanData = new Scan($"./temp/Batch_Analysis/Full Scans/{batch.Key}/{scan.FileName}");
                        scanList.Add(new ScanStatus(scan.FileName, scanData.Status(), scanData.Result()));
                    }
                    batchScansStatus.Add(batch.Key, scanList);
                    //upadate Prograss
                    action1.Invoke();
                }


                if (indexList.Count == 0 || indexList.Count < number)
                {
                    //Create log file
                    using (StreamWriter writer = new StreamWriter($"./temp/Scan Matching Error Log.txt"))
                    {
                        writer.WriteLine($"***Use Scan Name Code To Check Manually By Text Compare Tool (https://www.diffchecker.com/)***\n");
                        writer.WriteLine($"~~~~~~~~~~~~~~~~~~~~(Name Code) Scan List Start From Selected Date & Time~~~~~~~~~~~~~~~~~~~~");
                        foreach (var scan in initScanInfoList)
                            writer.WriteLine($"{scan.NameCode}");
                        writer.WriteLine($"~~~~~~~~~~~~~~~~~~~~(Name Code) Reference Scan List~~~~~~~~~~~~~~~~~~~~");
                        foreach (var method in fullConfigMethodInfoList)
                            writer.WriteLine($"{method.NameCode}");
                        writer.WriteLine($"\n***Automatically Check by Program***");
                        List<string> fileList = initScanInfoList.Select(a => a.FileName).ToList();
                        switch (level)
                        {
                            case 0:
                                writer.WriteLine($"\n**Match Level Is Already 0, Automatically Check by Program Is Not Avaiable**");
                                break;
                            case 1:
                                writer.WriteLine($"\n**Match Level Is 1, Try To Match by Level 0**\n");
                                List<int> indexListTemp = GetMatchedIndexDependslist_Level0(initScanInfoList.Select(t => t.NameCode.GetHashCode()).ToList(), fullConfigMethodInfoList.Select(a => a.NameCode.GetHashCode()).ToList());
                                if (indexListTemp.Count > 0)
                                {
                                    foreach (var pos in indexListTemp)
                                    {
                                        List<string> fileListTemp = fileList.GetRange(pos, fullConfigMethodInfoList.Count).ConvertAll(a => a = $" -> {a}");
                                        fileListTemp[0] = $"{fileListTemp[0]} ++++++++++++++++++++";
                                        fileListTemp[fileListTemp.Count - 1] = $"{fileListTemp[fileListTemp.Count - 1]} --------------------";
                                        fileList.RemoveRange(pos, fullConfigMethodInfoList.Count);
                                        fileList.InsertRange(pos, fileListTemp);
                                    }
                                    writer.WriteLine($"====================(File Name) Scan List Start From Selected Date & Time (Level 0) ({indexListTemp.Count} Batches Are Available)====================");
                                    foreach (var file in fileList)
                                        writer.WriteLine(file);
                                }
                                else
                                    writer.WriteLine($"Level 0 Match Failed");
                                break;
                            case 2:
                                writer.WriteLine($"\n**Match Level Is 2, Try To Match by Level 1**\n");
                                List<int> indexListTemp1 = GetMatchedIndexDependslist_Level1(initScanInfoList.Select(t => t.NameCode.GetHashCode()).ToList(), fullConfigMethodInfoList.Select(a => a.NameCode.GetHashCode()).ToList());
                                if (indexListTemp1.Count > 0)
                                {
                                    foreach (var pos in indexListTemp1)
                                    {
                                        List<string> fileListTemp = fileList.GetRange(pos, fullConfigMethodInfoList.Count).ConvertAll(a => a = $" -> {a}");
                                        fileListTemp[0] = $"{fileListTemp[0]} ++++++++++++++++++++";
                                        fileListTemp[fileListTemp.Count - 1] = $"{fileListTemp[fileListTemp.Count - 1]} --------------------";
                                        fileList.RemoveRange(pos, fullConfigMethodInfoList.Count);
                                        fileList.InsertRange(pos, fileListTemp);
                                    }
                                    writer.WriteLine($"====================(File Name) Scan List Start From Selected Date & Time (Level 1) ({indexListTemp1.Count} Batches Are Available)====================");
                                    foreach (var file in fileList)
                                        writer.WriteLine(file);
                                }
                                else
                                    writer.WriteLine($"Level 1 Match Failed");
                                goto case 1;
                            default:
                                break;
                        }
                    }
                    if (indexList.Count == 0) throw new Exception($"Not Find Available Batches");
                    else if (indexList.Count < number) throw new Exception($"Not Find Required Number of Batches, Please Try {indexList.Count} as Maximum Number of Batches");
                }

                return targetScanInfoListDic;
            }
            catch (Exception ex)
            {
                throw new Exception($"Batch Matching Failed，Reason：{ex.Message}");
            }
            finally
            {
                sftp.Disconnect();
            }
        }

        private static List<ConfigMethodInfo> GetFullConfigMethodList(Global.BatchType batchType)
        {
            List<ConfigMethodInfo> configMethodInfoList = new List<ConfigMethodInfo>();
            //get config file root node
            XElement rootNode = GetRootNode.BatchConfigRootNode(batchType);
            //index all elements under Methods
            foreach (var node in rootNode?.Element("Methods")?.Elements())
            {
                //check group
                if (node.Name.ToString() == "Group")
                {
                    List<ConfigMethodInfo> configMethodInfoGroupList = new List<ConfigMethodInfo>();
                    //get group repeat number
                    int repeat = int.Parse(node.Attribute("repeat")?.Value ?? "1");
                    //get to group list
                    foreach (var subNode in node.Elements("Method"))
                    {
                        //get method repeat number
                        int r = int.Parse(subNode.Attribute("repeat")?.Value ?? "1");
                        //get mark
                        List<MarkInfo> markList = new List<MarkInfo>();
                        foreach (var item in subNode.Elements("Mark"))
                        {
                            string main = item.Attribute("main")?.Value;
                            string sub = item.Attribute("sub")?.Value;
                            if (main != null && sub != null)
                                markList.Add(new MarkInfo(main, sub));
                        }
                        //add to group list
                        while (r > 0)
                        {
                            configMethodInfoGroupList.Add(new ConfigMethodInfo(subNode.Attribute("name").Value, markList));
                            r--;
                        }
                    }
                    //add group list to list
                    while (repeat > 0)
                    {
                        configMethodInfoList.InsertRange(configMethodInfoList.Count, configMethodInfoGroupList);
                        repeat--;
                    }
                }
                else//not a group
                {
                    //get method repeat number
                    int repeat = int.Parse(node.Attribute("repeat")?.Value ?? "1");
                    //get mark
                    List<MarkInfo> markList = new List<MarkInfo>();
                    foreach (var item in node.Elements("Mark"))
                    {
                        string main = item.Attribute("main")?.Value;
                        string sub = item.Attribute("sub")?.Value;
                        if (main != null && sub != null)
                            markList.Add(new MarkInfo(main, sub));
                    }
                    //add to list
                    while (repeat > 0)
                    {
                        configMethodInfoList.Add(new ConfigMethodInfo(node.Attribute("name").Value, markList));
                        repeat--;
                    }
                }
            }
            return configMethodInfoList;
        }

        private static List<int> GetMatchedIndexDependslist_Level2(List<int> sourceList, List<int> referenceList)
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < sourceList.Count; i++)
            {
                if (sourceList.Skip(i).Take(referenceList.Count).SequenceEqual(referenceList))
                    indexList.Add(i);
            }
            return indexList;
        }

        private static List<int> GetMatchedIndexDependslist_Level1(List<int> sourceList, List<int> referenceList)
        {
            List<int> indexList = new List<int>();
            int scanNmuber = referenceList.Count;
            for (int i = 0; i < sourceList.Count; i++)
            {
                if (sourceList[i] == referenceList[0])
                {
                    if ((i + scanNmuber) <= sourceList.Count && sourceList[i + scanNmuber - 1] == referenceList[scanNmuber - 1])
                    {
                        if (sourceList.Skip(i).Take(referenceList.Count).OrderBy(e => e).SequenceEqual(referenceList.OrderBy(e => e)))
                            indexList.Add(i);
                    }
                }
            }
            return indexList;
        }

        private static List<int> GetMatchedIndexDependslist_Level0(List<int> sourceList, List<int> referenceList)
        {
            List<int> indexList = new List<int>();
            int scanNmuber = referenceList.Count;
            for (int i = 0; i < sourceList.Count; i++)
            {
                if (sourceList[i] == referenceList[0])
                {
                    if ((i + scanNmuber) <= sourceList.Count && sourceList[i + scanNmuber - 1] == referenceList[scanNmuber - 1])
                        indexList.Add(i);
                }
            }
            return indexList;
        }

        private static List<TargetScanInfo> ConvertToTargetScanInfoList(List<ScanInfo> singleFullScanInfoList, List<ConfigMethodInfo> fullconfigMethodInfoList, string batch)
        {
            List<TargetScanInfo> targetScanInfoList = new List<TargetScanInfo>();

            for (int i = 0; i < singleFullScanInfoList.Count; i++)
            {
                if (singleFullScanInfoList[i].NameCode == fullconfigMethodInfoList[i].NameCode)
                    if (fullconfigMethodInfoList[i].MarkList.Count != 0)
                        targetScanInfoList.Add(new TargetScanInfo(singleFullScanInfoList[i].FileName, singleFullScanInfoList[i].FolderName, fullconfigMethodInfoList[i].MarkList, batch));
            }

            return targetScanInfoList;
        }
    }
}
