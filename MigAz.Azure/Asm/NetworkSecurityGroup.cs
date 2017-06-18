using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class NetworkSecurityGroup : INetworkSecurityGroup
    {
        #region Variables

        private AzureContext _AzureContext = null;
        private XmlNode _XmlNode = null;
        private List<NetworkSecurityGroupRule> _Rules;

        #endregion

        #region Constructors

        private NetworkSecurityGroup() { }

        public NetworkSecurityGroup(AzureContext azureContext, XmlNode xmlNode)
        {
            _AzureContext = azureContext;
            _XmlNode = xmlNode;

            _Rules = new List<NetworkSecurityGroupRule>();
            foreach (XmlNode rule in _XmlNode.SelectNodes("//Rules/Rule"))
            {
                _Rules.Add(new NetworkSecurityGroupRule(_AzureContext, rule));
            }

        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }

        public string Location
        {
            get { return _XmlNode.SelectSingleNode("Location").InnerText; }
        }

        public List<NetworkSecurityGroupRule> Rules
        {
            get { return _Rules; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
