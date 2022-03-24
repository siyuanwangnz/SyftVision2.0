using Public.BatchConfig;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.Batch
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

        public ObservableCollection<Method> GetMethodsList()
        {
            ObservableCollection<Method> methodList = new ObservableCollection<Method>();
            if (ItemList != null)
            {
                ObservableCollection<Method> methodGroupList = new ObservableCollection<Method>();
                string groupScanCount = "";
                string groupDelayBetween = "";

                foreach (var item in ItemList)
                {
                    // Get method name
                    Method method = new Method();
                    method.MethodName = item.Method;

                    if (groupScanCount == "" && groupDelayBetween == "" && methodGroupList.Count == 0)
                    {
                        if (item.DelayBetween == "" || item.ScanCount == "") // No delay between
                        {
                            // Get scan count
                            int scanCount = 1;
                            if (item.ScanCount != "") scanCount = int.Parse(item.ScanCount);
                            // Add methods
                            for (int i = 0; i < scanCount; i++)
                                methodList.Add(method);
                        }
                        else // Has delay between
                        {
                            // Add to group
                            methodGroupList.Add(method);

                            groupScanCount = item.ScanCount;
                            groupDelayBetween = item.DelayBetween;
                        }

                    }
                    else
                    {
                        if (item.DelayBetween == "" || item.ScanCount == "") // No delay between
                        {
                            for (int i = 0; i < int.Parse(groupScanCount); i++)
                                methodList.AddRange(methodGroupList);

                            methodGroupList = new ObservableCollection<Method>() { };
                            groupScanCount = "";
                            groupDelayBetween = "";

                            // Get scan count
                            int scanCount = 1;
                            if (item.ScanCount != "") scanCount = int.Parse(item.ScanCount);
                            // Add methods
                            for (int i = 0; i < scanCount; i++)
                                methodList.Add(method);
                        }
                        else // Has delay between
                        {
                            if (groupScanCount == item.ScanCount && groupDelayBetween == item.DelayBetween)
                            {
                                methodGroupList.Add(method);
                            }
                            else
                            {
                                for (int i = 0; i < int.Parse(groupScanCount); i++)
                                    methodList.AddRange(methodGroupList);

                                methodGroupList = new ObservableCollection<Method>() { method };
                                groupScanCount = item.ScanCount;
                                groupDelayBetween = item.DelayBetween;
                            }
                        }
                    }
                }

                if (groupScanCount != "" && groupDelayBetween != "" && methodGroupList.Count != 0)
                {
                    for (int i = 0; i < int.Parse(groupScanCount); i++)
                        methodList.AddRange(methodGroupList);
                }

            }
            return methodList;
        }
    }
}
