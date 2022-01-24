using Public.Global;
using SyftXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OvernightScan.Services
{
    class GetRootNode
    {
        public static XElement BatchConfigRootNode(Global.BatchType batchType)
        {
            try
            {
                switch (batchType)
                {
                    //SPIS
                    case Global.BatchType.SPIS_Overnight:
                        return XElement.Load($"./Config/P3_Batch_SPIS_Overnight_Config.xml");
                    case Global.BatchType.SPIS_OnOff:
                        return XElement.Load($"./Config/P3_Batch_SPIS_OnOff_Config.xml");
                    case Global.BatchType.SPIS_EOV:
                        return XElement.Load($"./Config/P3_Batch_SPIS_EOV_Config.xml");
                    case Global.BatchType.SPIS_Weekend:
                        return XElement.Load($"./Config/P3_Batch_SPIS_Weekend_Config.xml");
                    //DPIS
                    case Global.BatchType.DPIS_Coarse:
                        return XElement.Load($"./Config/P3_Batch_DPIS_Config.xml");
                    case Global.BatchType.DPIS_Fine:
                        return XElement.Load($"./Config/P3_Batch_DPIS_Config.xml");
                    case Global.BatchType.DPIS_EOV:
                        return XElement.Load($"./Config/P3_Batch_DPIS_EOV_Config.xml");
                    case Global.BatchType.DPIS_ColdStart:
                        return XElement.Load($"./Config/P3_Batch_DPIS_ColdStart_Config.xml");
                    case Global.BatchType.DPIS_SensAndImpu:
                        return XElement.Load($"./Config/P3_Batch_DPIS_SensitivityAndImpurity_Config.xml");
                    //Infinity
                    case Global.BatchType.Infinity:
                        return XElement.Load($"./Config/P3_Batch_Infinity_Config.xml");
                    case Global.BatchType.Infinity_ColdStart:
                        return XElement.Load($"./Config/P3_Batch_Infinity_ColdStart_Config.xml");
                    case Global.BatchType.Infinity_EOB:
                        return XElement.Load($"./Config/P3_Batch_Infinity_EOB_Config.xml");
                    case Global.BatchType.Infinity_SensAndImpu:
                        return XElement.Load($"./Config/P3_Batch_Infinity_SensitivityAndImpurity_Config.xml");
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Load {batchType} config scan list failed，Reason：{ex.Message}");
            }
        }
    }
}