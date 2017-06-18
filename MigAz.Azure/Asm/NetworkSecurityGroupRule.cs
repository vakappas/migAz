using System;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class NetworkSecurityGroupRule
    {
        private AzureContext _AzureContext;
        private XmlNode _ruleNode;

        public NetworkSecurityGroupRule(AzureContext azureContext, XmlNode ruleNode)
        {
            this._AzureContext = azureContext;
            this._ruleNode = ruleNode;
        }

        #region Properties

        public string Name
        {
            get { return _ruleNode.SelectSingleNode("Name").InnerText.Replace(' ', '-'); }
        }

        public string Type
        {
            get { return _ruleNode.SelectSingleNode("Type").InnerText; }
        }

        public long Priority
        {
            get { return long.Parse(_ruleNode.SelectSingleNode("Priority").InnerText); }
        }

        public string Action
        {
            get { return _ruleNode.SelectSingleNode("Action").InnerText; }
        }

        public string SourceAddressPrefix
        {
            get { return _ruleNode.SelectSingleNode("SourceAddressPrefix").InnerText.Replace("_", String.Empty); }
        }

        public string DestinationAddressPrefix
        {
            get { return _ruleNode.SelectSingleNode("DestinationAddressPrefix").InnerText.Replace("_", String.Empty); }
        }

        public string SourcePortRange
        {
            get { return _ruleNode.SelectSingleNode("SourcePortRange").InnerText; }
        }

        public string DestinationPortRange
        {
            get { return _ruleNode.SelectSingleNode("DestinationPortRange").InnerText; }
        }

        public string Protocol
        {
            get { return _ruleNode.SelectSingleNode("Protocol").InnerText; }
        }

        public bool IsSystemRule
        {
            get { return _ruleNode.SelectNodes("IsDefault").Count > 0; }
        }

        #endregion
    }
}