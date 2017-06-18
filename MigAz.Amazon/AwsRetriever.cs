using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using MigAz.Core.Interface;

namespace MigAz.AWS
{
    public class AwsRetriever
    {
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;

        public Dictionary<string, XmlDocument> _documentCache = new Dictionary<string, XmlDocument>();

        public AwsRetriever(ILogProvider logProvider, IStatusProvider statusProvider)
        {
            _logProvider = logProvider;
            _statusProvider = statusProvider;
        }

        public XmlDocument GetAwsResources(string endpoint, string resourceType, string resourceId)
        {
            _logProvider.WriteLog("GetAwsResources", "Start");

            ParamComparer pc = new ParamComparer();
            SortedDictionary<string, string> sortedRequestPars =
                             new SortedDictionary<string, string>(pc);

            string action = "", resourceName = "", filterName = "", filterValue = "";

            getActionAndId(resourceType, out action, out resourceName, out filterValue, out filterName);
            if (resourceName != "" && filterName != "" && filterValue != "")
            {
                sortedRequestPars.Add(filterName, resourceName);
                sortedRequestPars.Add(filterValue, resourceId);
            }
            sortedRequestPars.Add("Action", action);
            sortedRequestPars.Add("SignatureMethod", "HmacSHA1");
            sortedRequestPars.Add("Version", "2010-11-15");
            sortedRequestPars.Add("SignatureVersion", "2");
            string date = GetEC2Date();
            sortedRequestPars.Add("Timestamp", date);
            sortedRequestPars.Add("AWSAccessKeyId", "AKIAI72AH2JEMPVR3I2Q");

            var stringToSign = ComposeStringToSign(sortedRequestPars, "GET", endpoint, "/");
            var signature = GetAWS3_SHA1AuthorizationValue("AKIAI72AH2JEMPVR3I2Q", "l8SKjLvjHxFv5KVrEX4B3WXTfrkaYBkBrQUmx0yB", stringToSign);
            string queryString = "https://" + endpoint + "/?" + GetSortedParamsAsString(sortedRequestPars, false) + "&Signature=" + signature;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(queryString);
            request.Method = "GET";
            request.ContentType = "application/text";

            string xml = "";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                xml = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //xml = new StreamReader(response.GetResponseStream()).ReadToEnd();
                _logProvider.WriteLog("GetAwsResources", "RESPONSE " + response.StatusCode);
            }
            catch (Exception exception)
            {
                _logProvider.WriteLog("GetAwsResources", "EXCEPTION " + exception.Message);
                //DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                xml = "";
                //Application.ExitThread();
            }

            if (xml != "")
            {
                XmlDocument xmlDoc = RemoveXmlns(xml);

                _logProvider.WriteLog("GetAzureASMResources", "End");
                // writeXMLtoFile(url, xml);

                _documentCache.Add(queryString, xmlDoc);
                return xmlDoc;
            }
            else
            {
                //XmlNodeList xmlnode = null;
                //return xmlnode;
                XmlDocument xmlDoc = new XmlDocument();

                _logProvider.WriteLog("GetAzureASMResources", "End");
                writeXMLtoFile(queryString, "");
                return null;
            }

        }

        private void getActionAndId(string resourceType, out string action, out string resourceName, out string filterValue, out string filterName)
        {
            switch (resourceType.ToLower())
            {
                case ("vpcs"):
                    action = "DescribeVpcs";
                    resourceName = "vpc-id";
                    filterValue = "Filter.1.Value.1";
                    filterName = "Filter.1.Name";
                    break;
                //check
                case ("instances"):
                    action = "DescribeInstances";
                    resourceName = "instance-id";
                    filterValue = "Filter.1.Value.1";
                    filterName = "Filter.1.Name";
                    break;
                case ("subnets"):
                    action = "DescribeSubnets";
                    resourceName = "vpc-id";
                    filterValue = "Filter.1.Value.1";
                    filterName = "Filter.1.Name";
                    break;
                //check
                case ("route tables"):
                    action = "DescribeRouteTables";
                    resourceName = "vpc-id";
                    filterValue = "";
                    filterName = "";
                    break;
                //check
                case ("security groups"):
                    action = "DescribeSecurityGroups";
                    resourceName = "vpc-id";
                    filterValue = "";
                    filterName = "Filter.1.Name";
                    break;
                case ("vpn gateways"):
                    action = "DescribeVpnGateways";
                    resourceName = "";
                    filterValue = "";
                    filterName = "";
                    break;
                //check
                case ("internet gateways"):
                    action = "DescribeInternetGateways";
                    resourceName = "";
                    filterValue = "";
                    filterName = "";
                    break;
                //check
                case ("nat gateways"):
                    action = "DescribeNatGateways";
                    resourceName = "";
                    filterValue = "";
                    filterName = "";
                    break;
                case ("dhcp options"):
                    action = "DescribeDhcpOptions";
                    resourceName = "dhcp-options-id";
                    filterValue = "Filter.1.Value.1";
                    filterName = "Filter.1.Name";
                    break;
                case ("network acls"):
                    action = "DescribeNetworkAcls";
                    resourceName = "association.subnet-id";
                    filterValue = "Filter.1.Value.1";
                    filterName = "Filter.1.Name";
                    break;
                default:
                    action = "";
                    resourceName = "";
                    filterValue = "";
                    filterName = "";
                    break;
            }
        }
        static public string ComposeStringToSign(SortedDictionary<string, string> listPars,
             string method, string host, string resourcePath)
        {
            String stringToSign = null;

            stringToSign = method + "\n";
            stringToSign += host + "\n";
            stringToSign += resourcePath + "\n";
            stringToSign += GetSortedParamsAsString(listPars, true);

            return stringToSign;
        }

        static public string GetEC2Date()
        {
            //string httpDate = DateTime.UtcNow.ToString("s") + "Z"; 
            string httpDate = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                              DateTimeFormatInfo.InvariantInfo);
            return httpDate;
        }

        private static string PercentEncodeRfc3986(string str)
        {
            str = HttpUtility.UrlEncode(str, System.Text.Encoding.UTF8);
            str = str.Replace("'", "%27").Replace("(", "%28").Replace(")",
                              "%29").Replace("*", "%2A").Replace("!",
                              "%21").Replace("%7e", "~").Replace("+", "%20");
            StringBuilder sbuilder = new StringBuilder(str);

            for (int i = 0; i < sbuilder.Length; i++)
            {
                if (sbuilder[i] == '%')
                {
                    if (Char.IsLetter(sbuilder[i + 1]) ||
                        Char.IsLetter(sbuilder[i + 2]))
                    {
                        sbuilder[i + 1] = Char.ToUpper(sbuilder[i + 1]);
                        sbuilder[i + 2] = Char.ToUpper(sbuilder[i + 2]);
                    }
                }
            }
            return sbuilder.ToString();
        }

        public static String GetSortedParamsAsString(SortedDictionary<String, String> paras, bool isCanonical)
        {
            String sParams = "";
            String sKey = null;
            String sValue = null;
            String separator = "";

            foreach (KeyValuePair<string, String> entry in paras)
            {
                sKey = entry.Key;
                sValue = entry.Value;

                if (isCanonical)
                {
                    sKey = PercentEncodeRfc3986(sKey);
                    sValue = PercentEncodeRfc3986(sValue);
                }
                sParams += separator + sKey + "=" + sValue;
                separator = "&";
            }
            return sParams;
        }

        public static string GetAWS3_SHA1AuthorizationValue(string AWSAccessKeyId,
              string AWSSecretAccessKey, string stringToSign)
        {
            System.Security.Cryptography.HMACSHA1 MySigner =
                   new System.Security.Cryptography.HMACSHA1(
                   System.Text.Encoding.UTF8.GetBytes(AWSSecretAccessKey));

            string SignatureValue = Convert.ToBase64String(
                   MySigner.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringToSign)));

            return SignatureValue;
        }

        protected XmlDocument RemoveXmlns(String xml)
        {
            if (!xml.StartsWith("<"))
            {
                xml = $"<data>{xml}</data>";
            }
            XDocument d = XDocument.Parse(xml);
            d.Root.DescendantsAndSelf().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in d.Descendants())
                elem.Name = elem.Name.LocalName;

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(d.CreateReader());

            return xmlDocument;
        }

        private void writeXMLtoFile(string url, string xml)
        {
            string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MIGAZ\\MIGAZ-XML-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
            string text = DateTime.Now.ToString() + "   " + url + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
            text = xml + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
            text = Environment.NewLine;
            File.AppendAllText(logfilepath, text);
        }

    }
}
