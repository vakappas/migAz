using System;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class ReservedIP
    {
        private XmlNode _ReservedIPNode;
        private AzureContext _AzureContext;

        public ReservedIP(AzureContext azureContext, XmlNode reservedIPNode)
        {
            this._AzureContext = azureContext;
            this._ReservedIPNode = reservedIPNode;
        }

        public string ServiceName
        {
            get
            {
                if (_ReservedIPNode.SelectNodes("ServiceName").Count == 0)
                    return String.Empty;

                return _ReservedIPNode.SelectSingleNode("ServiceName").InnerText;
            }
        }
    }
}
