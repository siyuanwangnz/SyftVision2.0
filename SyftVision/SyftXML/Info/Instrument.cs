using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyftXML
{
    /// <summary>
    /// instrument information
    /// </summary>
    public class Instrument
    {
        public Instrument(XElement rootNode)
        {
            Model = XMLDataService.GetSettingNodeValueByAttributeName(rootNode, "instrument.model");
            Number = Regex.Match(XMLDataService.GetSettingNodeValueByAttributeName(rootNode, "instrument.name"), @"(\d{4})").Groups[1].Value;
            SN = XMLDataService.GetSettingNodeValueByAttributeName(rootNode, "instrument.serial");
            KioskVersion = XMLDataService.GetSettingNodeValueByAttributeName(rootNode, "version.firmware");
        }
        /// <summary>
        /// model: "Voice200 Infinity"
        /// </summary>
        public string Model { get; private set; }
        /// <summary>
        /// number: "3390"
        /// </summary>
        public string Number { get; private set; }
        /// <summary>
        /// serial number: "3601229"
        /// </summary>
        public string SN { get; private set; }
        /// <summary>
        /// kiosk version: "3.3.23"
        /// </summary>
        public string KioskVersion { get; private set; }
    }
}
