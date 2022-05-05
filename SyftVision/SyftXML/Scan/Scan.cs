using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SyftXML
{
    /// <summary>
    /// give scan path and get all data from this class 
    /// </summary>
    public class Scan
    {
        /// <summary>
        /// scan phase
        /// </summary>
        public enum Phase { Preparation, Background, Sample, All }
        /// <summary>
        /// GetRP_Data speed mode
        /// </summary>
        public enum FastMode
        {
            CPS = 0x01,
            R_CPS = 0x02,
            Conc = 0x04,
            Off = CPS | R_CPS | Conc,
            Sensitivity = CPS | Conc,
            Impurity = CPS | R_CPS
        }
        /// <summary>
        /// declare scan with scan file path to get data of scan 
        /// </summary>
        /// <param name="path">file path: "./infinity +- performance check_V5-13014-20210526-073716.xml"</param>
        public Scan(string path)
        {
            try
            {
                RootNode = XElement.Load(path);
            }
            catch (Exception ex)
            {
                throw new Exception($"(Scan: {path}) {ex.Message} ");
            }
            
        }
        /// <summary>
        /// declare scan with scan root node to get data of scan 
        /// </summary>
        /// <param name="rootNode">XElement type root node</param>
        public Scan(XElement rootNode)
        {
            RootNode = rootNode;
        }
        /// <summary>
        /// root node of scan file
        /// </summary>
        public XElement RootNode { get; private set; }

        /// <summary>
        /// Scan Result
        /// </summary>
        /// <returns>Scan Result</returns>
        public string Result() => XMLDataService.GetSettingNodeValueByAttributeName(RootNode, "job.report.result");
        /// <summary>
        /// Scan Status
        /// </summary>
        /// <returns>Scan Status</returns>
        public string Status() => XMLDataService.GetSettingNodeValueByAttributeName(RootNode, "job.report.status");
        /// <summary>
        /// get file
        /// </summary>
        /// <returns>file info</returns>
        public File GetFileInfo() => new File(RootNode);
        /// <summary>
        /// get instrument info
        /// </summary>
        /// <returns>instrument info</returns>
        public Instrument GetInstrumentInfo() => new Instrument(RootNode);
        /// <summary>
        /// get injection scan
        /// </summary>
        /// <returns>injection scan data</returns>
        public Inj_Data GetInj_Data()
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> m_targetNodes =
                from target in RootNode.Descendants("measurements")
                where target?.Attribute("phase")?.Value == "PRECURSOR"
                from subtarget in target?.Descendants("datum")
                select subtarget;

            List<M_Datum> m_DatumList = new List<M_Datum>();
            foreach (var node in m_targetNodes)
            {
                m_DatumList.Add(new M_Datum(node));
            }
            return new Inj_Data(m_DatumList);
        }
        /// <summary>
        /// get icf table
        /// </summary>
        /// <returns>Dictionary &lt;mass, factor&gt;</returns>
        public Dictionary<double, double> GetICF_Table()
        {
            if (RootNode == null) return null;

            Dictionary<double, double> ICF = new Dictionary<double, double>();

            string massTable = $"{XMLDataService.GetSettingNodeValueByAttributeName(RootNode, "icf.converter")};"; //TODO: Should remove ; for 3.4.4

            while (massTable.Contains(";"))
            {
                //Get xxx,xxx;
                string temp = massTable.Substring(0, massTable.IndexOf(";") + 1);
                //Add xxx,xxx; to dictionary
                try
                {
                    ICF.Add(double.Parse(temp.Substring(0, temp.IndexOf(","))), double.Parse(temp.Substring(temp.IndexOf(",") + 1, temp.IndexOf(";") - temp.IndexOf(",") - 1)));
                }
                catch (Exception)
                {

                }
                //Trim off front xxx,xxx;
                massTable = massTable.Remove(0, massTable.IndexOf(";") + 1);
            }
            return ICF;
        }
        /// <summary>
        /// get reaction time, unit: ms
        /// </summary>
        /// <returns>reaction time</returns>
        public double GetReactionTime()
        {
            if (RootNode == null) return 0;

            IEnumerable<XElement> m_targetNodes =
                from target in RootNode.Descendants("measurements")?.Elements("datum")
                select target;

            if (m_targetNodes.Count() == 0) return 0;
            return Double.Parse(m_targetNodes?.First()?.Attribute("reactionTime")?.Value);
        }

        /// <summary>
        /// get required components
        /// </summary>
        /// <param name="rpList">new List &lt;string&gt; {"H3O+29","O2+32"}</param>
        /// <param name="phase">scan phase in enum Scan.Phase</param>
        /// <param name="fastMode">scan phase in enum Scan.FastMode</param>
        /// <returns>list of required found components</returns>
        public List<RP_Data> GetRP_DataList(List<string> rpList, Phase phase, FastMode fastMode = FastMode.Off)
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> m_targetNodes;
            IEnumerable<XElement> c_targetNodes;
            switch (phase)
            {
                case Phase.Preparation:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.Background:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.Sample:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.All:
                default:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")?.Elements("datum")
                        select target;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")?.Descendants("datum")
                        select target;
                    break;
            }

            List<RP_Data> rp_DataList = new List<RP_Data>();
            List<string> rpCodeList = rpList.ConvertAll(a => a.ToLower().Replace(" ", ""));
            foreach (var rpCode in rpCodeList)
            {
                //Get CPS
                List<M_Datum> m_DatumList = new List<M_Datum>();
                string reagent = "";
                string product = "";
                string r_product = "";
                string r_rpCode = "";
                if (fastMode == FastMode.Conc)
                {
                    foreach (var node in m_targetNodes)
                    {
                        M_Datum m_Datum = new M_Datum(node);
                        if (rpCode == m_Datum.RPCode)
                        {
                            reagent = m_Datum.Reagent;
                            product = m_Datum.Product.Substring(0, m_Datum.Product.IndexOf("."));
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var node in m_targetNodes)
                    {
                        M_Datum m_Datum = new M_Datum(node);
                        if (rpCode == m_Datum.RPCode)
                        {
                            reagent = m_Datum.Reagent;
                            product = m_Datum.Product.Substring(0, m_Datum.Product.IndexOf("."));
                            if ((fastMode & FastMode.R_CPS) == FastMode.R_CPS)
                            {
                                r_product = Array.Find(R2P_Table.Content, a => a.Reagent == reagent).Product;
                                r_rpCode = Array.Find(R2P_Table.Content, a => a.Reagent == reagent).RPCode;
                            }
                            m_DatumList.Add(m_Datum);
                        }
                    }
                }

                //Get Reagent CPS
                List<M_Datum> r_m_DatumList = new List<M_Datum>();
                if ((fastMode & FastMode.R_CPS) == FastMode.R_CPS)
                {
                    if (r_rpCode == rpCode)
                        r_m_DatumList = m_DatumList;
                    else
                    {
                        foreach (var node in m_targetNodes)
                        {
                            M_Datum r_m_Datum = new M_Datum(node);
                            if (r_rpCode == r_m_Datum.RPCode)
                                r_m_DatumList.Add(r_m_Datum);
                        }
                    }
                }

                //Get Concentration
                List<C_Datum> c_DatumList = new List<C_Datum>();
                if ((fastMode & FastMode.Conc) == FastMode.Conc)
                {
                    string formula = XMLDataService.GetFormulaByReagentandProduct(RootNode, reagent, product);
                    if (formula != null)
                    {
                        string rfCode = $"{reagent}{formula}".ToLower().Replace(" ", "");
                        foreach (var node in c_targetNodes)
                        {
                            C_Datum c_Datum = new C_Datum(node);
                            if (rfCode == c_Datum.RFCode)
                                c_DatumList.Add(c_Datum);
                        }
                    }
                }
                RP_Data temp = new RP_Data(reagent, product, r_product, m_DatumList, r_m_DatumList, c_DatumList);
                if (temp.IsAvailable)
                    rp_DataList.Add(temp);
            }
            return rp_DataList;
        }
        /// <summary>
        /// get all components
        /// </summary>
        /// <param name="phase">scan phase in enum Scan.Phase</param>
        /// <param name="fastMode">scan phase in enum Scan.FastMode</param>
        /// <returns>list of all components</returns>
        public List<RP_Data> GetRP_DataList(Phase phase, FastMode fastMode = FastMode.Off)
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> m_targetNodes;
            IEnumerable<XElement> c_targetNodes;
            switch (phase)
            {
                case Phase.Preparation:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.Background:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.Sample:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.All:
                default:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")?.Elements("datum")
                        select target;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")?.Descendants("datum")
                        select target;
                    break;
            }

            List<RP_Data> rp_DataList = new List<RP_Data>();
            foreach (var tempNode in m_targetNodes)
            {
                M_Datum tempM_Datum = new M_Datum(tempNode);
                string rpCode = "";
                if (!rp_DataList.Select(a => a.RPCode).ToList().Contains(tempM_Datum.RPCode))
                {
                    rpCode = tempM_Datum.RPCode;

                    //Get CPS
                    List<M_Datum> m_DatumList = new List<M_Datum>();
                    string reagent = "";
                    string product = "";
                    string r_product = "";
                    string r_rpCode = "";
                    if (fastMode == FastMode.Conc)
                    {
                        foreach (var node in m_targetNodes)
                        {
                            M_Datum m_Datum = new M_Datum(node);
                            if (rpCode == m_Datum.RPCode)
                            {
                                reagent = m_Datum.Reagent;
                                product = m_Datum.Product.Substring(0, m_Datum.Product.IndexOf("."));
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var node in m_targetNodes)
                        {
                            M_Datum m_Datum = new M_Datum(node);
                            if (rpCode == m_Datum.RPCode)
                            {
                                reagent = m_Datum.Reagent;
                                product = m_Datum.Product.Substring(0, m_Datum.Product.IndexOf("."));
                                if ((fastMode & FastMode.R_CPS) == FastMode.R_CPS)
                                {
                                    r_product = Array.Find(R2P_Table.Content, a => a.Reagent == reagent).Product;
                                    r_rpCode = Array.Find(R2P_Table.Content, a => a.Reagent == reagent).RPCode;
                                }
                                m_DatumList.Add(m_Datum);
                            }
                        }
                    }

                    //Get Reagent CPS
                    List<M_Datum> r_m_DatumList = new List<M_Datum>();
                    if ((fastMode & FastMode.R_CPS) == FastMode.R_CPS)
                    {
                        if (r_rpCode == rpCode)
                            r_m_DatumList = m_DatumList;
                        else
                        {
                            foreach (var node in m_targetNodes)
                            {
                                M_Datum r_m_Datum = new M_Datum(node);
                                if (r_rpCode == r_m_Datum.RPCode)
                                    r_m_DatumList.Add(r_m_Datum);
                            }
                        }
                    }
                    //Get Concentration
                    List<C_Datum> c_DatumList = new List<C_Datum>();
                    if ((fastMode & FastMode.Conc) == FastMode.Conc)
                    {
                        string formula = XMLDataService.GetFormulaByReagentandProduct(RootNode, reagent, product);
                        if (formula != null)
                        {
                            string rfCode = $"{reagent}{formula}".ToLower().Replace(" ", "");
                            foreach (var node in c_targetNodes)
                            {
                                C_Datum c_Datum = new C_Datum(node);
                                if (rfCode == c_Datum.RFCode)
                                    c_DatumList.Add(c_Datum);
                            }
                        }
                    }
                    RP_Data temp = new RP_Data(reagent, product, r_product, m_DatumList, r_m_DatumList, c_DatumList);
                    if (temp.IsAvailable)
                        rp_DataList.Add(temp);
                }
            }
            return rp_DataList;
        }
        /// <summary>
        /// get required single  component
        /// </summary>
        /// <param name="rp">"H3O+29"</param>
        /// <param name="phase">scan phase in enum Scan.Phase</param>
        /// <param name="fastMode">scan phase in enum Scan.FastMode</param>
        /// <returns>required single component</returns>
        public RP_Data GetRP_Data(string rp, Phase phase, FastMode fastMode = FastMode.Off)
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> m_targetNodes;
            IEnumerable<XElement> c_targetNodes;
            switch (phase)
            {
                case Phase.Preparation:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.Background:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.Sample:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.All:
                default:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")?.Elements("datum")
                        select target;
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")?.Descendants("datum")
                        select target;
                    break;
            }

            string rpCode = rp.ToLower().Replace(" ", "");

            //Get CPS
            List<M_Datum> m_DatumList = new List<M_Datum>();
            string reagent = "";
            string product = "";
            string r_product = "";
            string r_rpCode = "";
            if (fastMode == FastMode.Conc)
            {
                foreach (var node in m_targetNodes)
                {
                    M_Datum m_Datum = new M_Datum(node);
                    if (rpCode == m_Datum.RPCode)
                    {
                        reagent = m_Datum.Reagent;
                        product = m_Datum.Product.Substring(0, m_Datum.Product.IndexOf("."));
                        break;
                    }
                }
            }
            else
            {
                foreach (var node in m_targetNodes)
                {
                    M_Datum m_Datum = new M_Datum(node);
                    if (rpCode == m_Datum.RPCode)
                    {
                        reagent = m_Datum.Reagent;
                        product = m_Datum.Product.Substring(0, m_Datum.Product.IndexOf("."));
                        if ((fastMode & FastMode.R_CPS) == FastMode.R_CPS)
                        {
                            r_product = Array.Find(R2P_Table.Content, a => a.Reagent == reagent).Product;
                            r_rpCode = Array.Find(R2P_Table.Content, a => a.Reagent == reagent).RPCode;
                        }
                        m_DatumList.Add(m_Datum);
                    }
                }
            }

            //Get Reagent CPS
            List<M_Datum> r_m_DatumList = new List<M_Datum>();
            if ((fastMode & FastMode.R_CPS) == FastMode.R_CPS)
            {
                if (r_rpCode == rpCode)
                    r_m_DatumList = m_DatumList;
                else
                {
                    foreach (var node in m_targetNodes)
                    {
                        M_Datum r_m_Datum = new M_Datum(node);
                        if (r_rpCode == r_m_Datum.RPCode)
                            r_m_DatumList.Add(r_m_Datum);
                    }
                }
            }

            //Get Concentration
            List<C_Datum> c_DatumList = new List<C_Datum>();
            if ((fastMode & FastMode.Conc) == FastMode.Conc)
            {
                string formula = XMLDataService.GetFormulaByReagentandProduct(RootNode, reagent, product);
                if (formula != null)
                {
                    string rfCode = $"{reagent}{formula}".ToLower().Replace(" ", "");
                    foreach (var node in c_targetNodes)
                    {
                        C_Datum c_Datum = new C_Datum(node);
                        if (rfCode == c_Datum.RFCode)
                            c_DatumList.Add(c_Datum);
                    }
                }
            }
            return new RP_Data(reagent, product, r_product, m_DatumList, r_m_DatumList, c_DatumList);
        }

        /// <summary>
        /// get required reagents current
        /// </summary>
        /// <param name="rList">new List &lt;string&gt; {"H3O+","O2-"}</param>
        /// <returns>list of required found reagents current, not found return null</returns>
        public List<RC_Data> GetRC_DataList(List<string> rList)
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> m_targetNodes =
                from target in RootNode.Descendants("measurements")?.Elements("datum")
                select target;

            List<RC_Data> r_DataList = new List<RC_Data>();
            List<string> rCodeList = rList.ConvertAll(a => a.ToLower().Replace(" ", ""));
            foreach (var rCode in rCodeList)
            {
                List<M_Datum> m_DatumList = new List<M_Datum>();
                string reagent = "";
                string product = "";
                string rpCode = Array.Find(R2P_Table.Content, a => a.RCode == rCode).RPCode;
                foreach (var node in m_targetNodes)
                {
                    M_Datum m_Datum = new M_Datum(node);
                    if (rpCode == m_Datum.RPCode)
                    {
                        reagent = m_Datum.Reagent;
                        product = m_Datum.Product.Substring(0, m_Datum.Product.IndexOf("."));
                        m_DatumList.Add(m_Datum);
                    }
                }
                if (m_DatumList.Count == 0) continue;
                r_DataList.Add(new RC_Data(reagent, product, m_DatumList));
            }
            if (r_DataList.Count == 0) return null;
            return r_DataList;
        }
        /// <summary>
        /// get all reagents current
        /// </summary>
        /// <returns>list of all reagents current, no reagents current return null</returns>
        public List<RC_Data> GetRC_DataList()
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> m_targetNodes =
                from target in RootNode.Descendants("measurements")?.Elements("datum")
                select target;

            List<RC_Data> r_DataList = new List<RC_Data>();
            foreach (var tempNode in m_targetNodes)
            {
                M_Datum tempM_Datum = new M_Datum(tempNode);
                string r = "";
                if (!r_DataList.Select(a => a.Reagent).ToList().Contains(tempM_Datum.Reagent))
                {
                    r = tempM_Datum.Reagent;

                    List<M_Datum> m_DatumList = new List<M_Datum>();
                    string reagent = "";
                    string product = "";
                    string rpCode = Array.Find(R2P_Table.Content, a => a.Reagent == r).RPCode;
                    foreach (var node in m_targetNodes)
                    {
                        M_Datum m_Datum = new M_Datum(node);
                        if (rpCode == m_Datum.RPCode)
                        {
                            reagent = m_Datum.Reagent;
                            product = m_Datum.Product.Substring(0, m_Datum.Product.IndexOf("."));
                            m_DatumList.Add(m_Datum);
                        }
                    }
                    if (m_DatumList.Count == 0) continue;
                    r_DataList.Add(new RC_Data(reagent, product, m_DatumList));
                }
            }
            if (r_DataList.Count == 0) return null;
            return r_DataList;
        }
        /// <summary>
        /// get required single reagent current
        /// </summary>
        /// <param name="r">"H3O+"</param>
        /// <returns>required single reagent current, not found return null</returns>
        public RC_Data GetRC_Data(string r)
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> m_targetNodes =
                from target in RootNode.Descendants("measurements")?.Elements("datum")
                select target;

            string rCode = r.ToLower().Replace(" ", "");

            List<M_Datum> m_DatumList = new List<M_Datum>();
            string reagent = "";
            string product = "";
            string rpCode = Array.Find(R2P_Table.Content, a => a.RCode == rCode).RPCode;
            foreach (var node in m_targetNodes)
            {
                M_Datum m_Datum = new M_Datum(node);
                if (rpCode == m_Datum.RPCode)
                {
                    reagent = m_Datum.Reagent;
                    product = m_Datum.Product.Substring(0, m_Datum.Product.IndexOf("."));
                    m_DatumList.Add(m_Datum);
                }
            }
            if (m_DatumList.Count == 0) return null;
            return new RC_Data(reagent, product, m_DatumList);
        }

        /// <summary>
        /// get list of required compounds
        /// </summary>
        /// <param name="cList">new List &lt;string&gt; {"1-octanol","2-butoxyethanol"}</param>
        /// <param name="phase">scan phase in enum Scan.Phase</param>
        /// <returns>list of required found compounds, not found return null</returns>
        public List<AC_Data> GetAC_DataList(List<string> cList, Phase phase)
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> c_targetNodes;
            switch (phase)
            {
                case Phase.Preparation:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("concentration")
                        select subtarget;
                    break;
                case Phase.Background:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("concentration")
                        select subtarget;
                    break;
                case Phase.Sample:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("concentration")
                        select subtarget;
                    break;
                case Phase.All:
                default:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")?.Descendants("concentration")
                        select target;
                    break;
            }

            List<AC_Data> ac_DataList = new List<AC_Data>();
            List<string> cCodeList = cList.ConvertAll(a => a.ToLower().Replace(" ", ""));
            foreach (var cCode in cCodeList)
            {
                Dictionary<string, List<C_Datum>> C_DatumListDic = new Dictionary<string, List<C_Datum>>();
                string compound = "";
                foreach (var concnode in c_targetNodes)
                {
                    if (concnode?.Attribute("compound")?.Value.ToLower().Replace(" ", "") == cCode)
                    {
                        compound = concnode?.Attribute("compound")?.Value;
                        foreach (var datumnode in concnode.Descendants("datum"))
                        {
                            C_Datum c_Datum = new C_Datum(datumnode);
                            List<C_Datum> temp;
                            if (C_DatumListDic.TryGetValue($"{c_Datum.Reagent}/{c_Datum.Formula}", out temp))
                            {
                                temp.Add(c_Datum);
                                C_DatumListDic[$"{c_Datum.Reagent}/{c_Datum.Formula}"] = temp;
                            }
                            else
                                C_DatumListDic.Add($"{c_Datum.Reagent}/{c_Datum.Formula}", new List<C_Datum> { c_Datum });
                        }
                    }
                }
                if (C_DatumListDic.Count == 0) continue;
                ac_DataList.Add(new AC_Data(compound, C_DatumListDic));
            }
            if (ac_DataList.Count == 0) return null;
            return ac_DataList;
        }
        /// <summary>
        /// get all compounds
        /// </summary>
        /// <param name="phase">scan phase in enum Scan.Phase</param>
        /// <returns>list of all compounds, no compounds return null</returns>
        public List<AC_Data> GetAC_DataList(Phase phase)
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> c_targetNodes;
            switch (phase)
            {
                case Phase.Preparation:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("concentration")
                        select subtarget;
                    break;
                case Phase.Background:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("concentration")
                        select subtarget;
                    break;
                case Phase.Sample:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("concentration")
                        select subtarget;
                    break;
                case Phase.All:
                default:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")?.Descendants("concentration")
                        select target;
                    break;
            }

            List<AC_Data> ac_DataList = new List<AC_Data>();
            foreach (var tempNode in c_targetNodes)
            {
                string c = "";
                if (!ac_DataList.Select(a => a.Compound).ToList().Contains(tempNode?.Attribute("compound")?.Value))
                {
                    c = tempNode?.Attribute("compound")?.Value;

                    Dictionary<string, List<C_Datum>> C_DatumListDic = new Dictionary<string, List<C_Datum>>();
                    string compound = "";
                    foreach (var concnode in c_targetNodes)
                    {
                        if (concnode?.Attribute("compound")?.Value == c)
                        {
                            compound = concnode?.Attribute("compound")?.Value;
                            foreach (var datumnode in concnode.Descendants("datum"))
                            {
                                C_Datum c_Datum = new C_Datum(datumnode);
                                List<C_Datum> temp;
                                if (C_DatumListDic.TryGetValue($"{c_Datum.Reagent}/{c_Datum.Formula}", out temp))
                                {
                                    temp.Add(c_Datum);
                                    C_DatumListDic[$"{c_Datum.Reagent}/{c_Datum.Formula}"] = temp;
                                }
                                else
                                    C_DatumListDic.Add($"{c_Datum.Reagent}/{c_Datum.Formula}", new List<C_Datum> { c_Datum });
                            }
                        }
                    }
                    if (C_DatumListDic.Count == 0) continue;
                    ac_DataList.Add(new AC_Data(compound, C_DatumListDic));
                }
            }
            if (ac_DataList.Count == 0) return null;
            return ac_DataList;
        }
        /// <summary>
        /// get required single compound
        /// </summary>
        /// <param name="c">"1-octanol"</param>
        /// <param name="phase">scan phase in enum Scan.Phase</param>
        /// <returns>required single compound, not found return null</returns>
        public AC_Data GetAC_Data(string c, Phase phase)
        {
            if (RootNode == null) return null;

            IEnumerable<XElement> c_targetNodes;
            switch (phase)
            {
                case Phase.Preparation:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("concentration")
                        select subtarget;
                    break;
                case Phase.Background:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("concentration")
                        select subtarget;
                    break;
                case Phase.Sample:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("concentration")
                        select subtarget;
                    break;
                case Phase.All:
                default:
                    c_targetNodes =
                        from target in RootNode.Descendants("analysis")?.Descendants("concentration")
                        select target;
                    break;
            }

            string cCode = c.ToLower().Replace(" ", "");

            Dictionary<string, List<C_Datum>> C_DatumListDic = new Dictionary<string, List<C_Datum>>();
            string compound = "";
            foreach (var concnode in c_targetNodes)
            {
                if (concnode?.Attribute("compound")?.Value.ToLower().Replace(" ", "") == cCode)
                {
                    compound = concnode?.Attribute("compound")?.Value;
                    foreach (var datumnode in concnode.Descendants("datum"))
                    {
                        C_Datum c_Datum = new C_Datum(datumnode);
                        List<C_Datum> temp;
                        if (C_DatumListDic.TryGetValue($"{c_Datum.Reagent}/{c_Datum.Formula}", out temp))
                        {
                            temp.Add(c_Datum);
                            C_DatumListDic[$"{c_Datum.Reagent}/{c_Datum.Formula}"] = temp;
                        }
                        else
                            C_DatumListDic.Add($"{c_Datum.Reagent}/{c_Datum.Formula}", new List<C_Datum> { c_Datum });
                    }
                }
            }
            if (C_DatumListDic.Count == 0) return null;
            return new AC_Data(compound, C_DatumListDic);
        }

        /// <summary>
        /// get required single mass
        /// </summary>
        /// <param name="reagent">"H3O+"</param>
        /// <param name="product">"78"</param>
        /// <param name="phase">scan phase in enum Scan.Phase</param>
        /// <returns>required single mass, not found return null</returns>
        public Mass_Data GetMass_Data(string reagent, string product, Phase phase)
        {
            if (RootNode == null) return null;
            IEnumerable<XElement> m_targetNodes;
            switch (phase)
            {
                case Phase.Preparation:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "PREPARATION"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.Background:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "BACKGROUND"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.Sample:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")
                        where target?.Attribute("phase")?.Value == "SAMPLE"
                        from subtarget in target?.Descendants("datum")
                        select subtarget;
                    break;
                case Phase.All:
                default:
                    m_targetNodes =
                        from target in RootNode.Descendants("measurements")?.Elements("datum")
                        select target;
                    break;
            }

            List<string> rpCodeList = new List<string>();
            double dproduct = double.Parse(product) - 0.5;
            while (dproduct <= double.Parse(product) + 0.5)
            {
                rpCodeList.Add($"{reagent}{dproduct:N}".ToLower().Replace(" ", ""));
                dproduct = dproduct + 0.05;
            }

            List<M_Datum> m_DatumList = new List<M_Datum>();
            foreach (var node in m_targetNodes)
            {
                M_Datum m_Datum = new M_Datum(node);
                if (rpCodeList.Contains(m_Datum.RPRawCode))
                    m_DatumList.Add(m_Datum);
            }
            if (m_DatumList.Count == 0) return null;
            return new Mass_Data(reagent, product, m_DatumList);
        }
    }
}
