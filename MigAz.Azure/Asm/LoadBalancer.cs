using System;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class LoadBalancer
    {
        private AzureContext _AzureContext;
        private VirtualNetwork _AsmVirtualNetwork;
        private Subnet _AsmSubnet;
        private XmlNode _XmlNode;
        private string _TargetName = String.Empty;
        
        public LoadBalancer(AzureContext azureContext, VirtualNetwork asmVirtualNetwork, XmlNode loadBalancerXml)
        {
            this._AzureContext = azureContext;
            this._AsmVirtualNetwork = asmVirtualNetwork;
            this._XmlNode = loadBalancerXml;

            this.TargetName = this.SubnetName;

            if (_AsmVirtualNetwork != null)
            {
                foreach (Subnet subnet in _AsmVirtualNetwork.Subnets)
                {
                    if (subnet.Name == this.SubnetName)
                    {
                        _AsmSubnet = subnet;
                        break;
                    }
                }
            }
        }

        public Subnet Subnet
        {
            get { return _AsmSubnet; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value; }
        }

        public string ToString()
        {
            return this.TargetName + this._AzureContext.SettingsProvider.LoadBalancerSuffix;
        }

        private string SubnetName
        {
            get { return _XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/SubnetName").InnerText; }
        }

        public string Type
        {
            get { return _XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/Type").InnerText; }
        }
    }
}
