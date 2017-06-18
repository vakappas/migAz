using MigAz.Core.Interface;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class Route : IRoute
    {
        private AzureContext _AzureContext;
        private XmlNode _XmlNode;
        private string _TargetName;

        public Route(AzureContext azureContext, XmlNode routeNode)
        {
            this._AzureContext = azureContext;
            this._XmlNode = routeNode;
            this.TargetName = this.Name;
        }

        #region Properties

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Replace(' ', '-'); }
        }

        public string AddressPrefix
        {
            get { return _XmlNode.SelectSingleNode("AddressPrefix").InnerText; }
        }

        public string NextHopType
        {
            get { return _XmlNode.SelectSingleNode("NextHopType/Type").InnerText; }
        }
        public string NextHopIpAddress
        {
            get
            {
                if (_XmlNode.SelectSingleNode("NextHopType/IpAddress") == null)
                    return string.Empty;

                return _XmlNode.SelectSingleNode("NextHopType/IpAddress").InnerText;
            }

        }
        
        #endregion
    }
}
