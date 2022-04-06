using Public.BatchConfig;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.Instrument
{
    public class BatchFile
    {
        public BatchFile(XElement rootNode)
        {
            try
            {
                ItemList = new ObservableCollection<BatchItem>();
                foreach (var item in rootNode.Elements("item"))
                {
                    BatchItem batchItem = new BatchItem();
                    batchItem.Method = item.Element("method").Value;
                    batchItem.ScanCount = item.Element("scanCount")?.Value ?? "";
                    batchItem.DelayBetween = item.Element("delayBetween")?.Value ?? "";

                    ItemList.Add(batchItem);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ObservableCollection<BatchItem> ItemList { get; private set; }

        public ObservableCollection<Method> GetMethodList()
        {
            List<string> mainList = new List<string>();
            if (ItemList != null)
            {
                List<string> groupList = new List<string>();
                string groupScanCount = "";
                string groupDelayBetween = "";

                foreach (var item in ItemList)
                {
                    // Get method name
                    string methodName = item.Method;

                    if (groupScanCount == "" && groupDelayBetween == "" && groupList.Count == 0)
                    {
                        if (item.DelayBetween == "" || item.ScanCount == "") // No delay between
                        {
                            // Get scan count
                            int scanCount = 1;
                            if (item.ScanCount != "") scanCount = int.Parse(item.ScanCount);
                            // Add methods
                            for (int i = 0; i < scanCount; i++)
                                mainList.Add(methodName);
                        }
                        else // Has delay between
                        {
                            // Add to group
                            groupList.Add(methodName);

                            groupScanCount = item.ScanCount;
                            groupDelayBetween = item.DelayBetween;
                        }

                    }
                    else
                    {
                        if (item.DelayBetween == "" || item.ScanCount == "") // No delay between
                        {
                            for (int i = 0; i < int.Parse(groupScanCount); i++)
                                mainList.AddRange(groupList);

                            groupList = new List<string>();
                            groupScanCount = "";
                            groupDelayBetween = "";

                            // Get scan count
                            int scanCount = 1;
                            if (item.ScanCount != "") scanCount = int.Parse(item.ScanCount);
                            // Add methods
                            for (int i = 0; i < scanCount; i++)
                                mainList.Add(methodName);
                        }
                        else // Has delay between
                        {
                            if (groupScanCount == item.ScanCount && groupDelayBetween == item.DelayBetween)
                            {
                                groupList.Add(methodName);
                            }
                            else
                            {
                                for (int i = 0; i < int.Parse(groupScanCount); i++)
                                    mainList.AddRange(groupList);

                                groupList = new List<string>() { methodName };
                                groupScanCount = item.ScanCount;
                                groupDelayBetween = item.DelayBetween;
                            }
                        }
                    }
                }

                if (groupScanCount != "" && groupDelayBetween != "" && groupList.Count != 0)
                {
                    for (int i = 0; i < int.Parse(groupScanCount); i++)
                        mainList.AddRange(groupList);
                }

            }

            ObservableCollection<Method> methodsList = new ObservableCollection<Method>();
            foreach (var methodName in mainList)
            {
                Method method = new Method();
                method.Name = methodName;
                methodsList.Add(method);
            }
            return methodsList;
        }
    }
}
