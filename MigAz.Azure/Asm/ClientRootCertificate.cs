using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MigAz.Azure;

namespace MigAz.Azure.Asm
{
    public class ClientRootCertificate
    {
        private AzureContext _AzureContext;
        private VirtualNetwork _AsmVirtualNetwork;
        private XmlNode _ClientRootCertificateXml;
        private XmlDocument _AsmClientRootCertificateDataXml;

        public ClientRootCertificate(AzureContext azureContext, VirtualNetwork asmVirtualNetwork, XmlNode clientRootCertificateXml)
        {
            this._AzureContext = azureContext;
            this._AsmVirtualNetwork = asmVirtualNetwork;
            this._ClientRootCertificateXml = clientRootCertificateXml;
        }

        public async Task InitializeChildrenAsync()
        {
            _AsmClientRootCertificateDataXml = await _AzureContext.AzureRetriever.GetAzureAsmClientRootCertificateData(_AsmVirtualNetwork, this.Thumbprint);
        }

        public string Thumbprint
        {
            get { return _ClientRootCertificateXml.SelectSingleNode("Thumbprint").InnerText; }
        }

        public string Subject
        {
            get { return _ClientRootCertificateXml.SelectSingleNode("Subject").InnerText; }
        }

        public string TargetSubject
        {
            get { return this.Subject.Replace("CN=", String.Empty).Replace(" ", "-"); }
        }

        public string PublicCertData
        {
            get
            {
                if (_AsmClientRootCertificateDataXml == null)
                    return null;

                return Convert.ToBase64String(StrToByteArray(_AsmClientRootCertificateDataXml.InnerText));
            }
        }

        // convert an hex string into byte array
        private static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }

        private static string Base64Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
