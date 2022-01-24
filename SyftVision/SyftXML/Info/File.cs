using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyftXML
{
    /// <summary>
    /// file information
    /// </summary>
    public class File
    {
        public File(XElement rootNode)
        {
            FileName = XMLDataService.GetSettingNodeValueByAttributeName(rootNode,"job.filename");
            ScanID = XMLDataService.GetSettingNodeValueByAttributeName(rootNode, "job.id");
            ScanName = XMLDataService.GetSettingNodeValueByAttributeName(rootNode, "job.name");
            ScanDate = XMLDataService.GetSettingNodeValueByAttributeName(rootNode, "job.start.date");
        }
        /// <summary>
        /// file name: "infinity +- performance check_V5-13014-20210526-073716.xml"
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// scan id: "13014"
        /// </summary>
        public string ScanID { get; private set; }
        /// <summary>
        /// scan name: "infinity +- performance check_V5"
        /// </summary>
        public string ScanName { get; private set; }
        /// <summary>
        /// scan date: "20210526-073716"
        /// </summary>
        public string ScanDate { get; private set; }
    }
}
