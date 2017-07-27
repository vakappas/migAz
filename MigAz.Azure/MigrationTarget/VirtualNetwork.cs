using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class VirtualNetwork : IMigrationTarget, IMigrationVirtualNetwork
    {
        private AzureContext _AzureContext;
        private string _TargetName = String.Empty;
        private List<VirtualNetworkGateway> _TargetVirtualNetworkGateways = new List<VirtualNetworkGateway>();
        private List<Subnet> _TargetSubnets = new List<Subnet>();
        List<string> _AddressPrefixes = new List<string>();
        List<string> _DnsServers = new List<string>();

        private VirtualNetwork() { }

        public VirtualNetwork(AzureContext azureContext, Asm.VirtualNetwork virtualNetwork, List<NetworkSecurityGroup> networkSecurityGroups)
        {
            this._AzureContext = azureContext;
            this.SourceVirtualNetwork = virtualNetwork;
            this.TargetName = virtualNetwork.Name;

            if (virtualNetwork.Gateways2 != null)
            {
                foreach (Asm.VirtualNetworkGateway virtualNetworkGateway in virtualNetwork.Gateways2)
                {
                    TargetVirtualNetworkGateways.Add(new VirtualNetworkGateway(_AzureContext, virtualNetworkGateway));
                }
            }

            foreach (Asm.Subnet subnet in virtualNetwork.Subnets)
            {
                this.TargetSubnets.Add(new Subnet(azureContext, this, subnet, networkSecurityGroups));
            }

            foreach (String addressPrefix in virtualNetwork.AddressPrefixes)
            {
                this.AddressPrefixes.Add(addressPrefix);
            }

            foreach (String dnsServer in virtualNetwork.DnsServers)
            {
                this.DnsServers.Add(dnsServer);
            }
        }

        public VirtualNetwork(AzureContext azureContext, Arm.VirtualNetwork virtualNetwork, List<NetworkSecurityGroup> networkSecurityGroups)
        {
            this._AzureContext = azureContext;
            this.SourceVirtualNetwork = virtualNetwork;
            this.TargetName = virtualNetwork.Name;

            foreach (Arm.VirtualNetworkGateway virtualNetworkGateway in virtualNetwork.VirtualNetworkGateways)
            {
                TargetVirtualNetworkGateways.Add(new VirtualNetworkGateway(_AzureContext, virtualNetworkGateway));
            }

            foreach (Arm.Subnet subnet in virtualNetwork.Subnets)
            {
                this.TargetSubnets.Add(new Subnet(azureContext, this, subnet, networkSecurityGroups));
            }

            foreach (String addressPrefix in virtualNetwork.AddressPrefixes)
            {
                this.AddressPrefixes.Add(addressPrefix);
            }

            foreach (String dnsServer in virtualNetwork.DnsServers)
            {
                this.DnsServers.Add(dnsServer);
            }
        }

        public VirtualNetwork(IVirtualNetwork virtualNetwork)
        {
            this.SourceVirtualNetwork = virtualNetwork;
            this.TargetName = virtualNetwork.Name;
            foreach (String addressPrefix in virtualNetwork.AddressPrefixes)
            {
                this.AddressPrefixes.Add(addressPrefix);
            }
            foreach (String dnsServer in virtualNetwork.DnsServers)
            {
                this.DnsServers.Add(dnsServer);
            }

            foreach (ISubnet sourceSubnet in virtualNetwork.Subnets)
            {
                MigrationTarget.Subnet targetSubnet = new Subnet(this, sourceSubnet);
                this.TargetSubnets.Add(targetSubnet);
            }
        }

        public IVirtualNetwork SourceVirtualNetwork { get; }

        public String SourceName
        {
            get
            {
                if (this.SourceVirtualNetwork == null)
                    return String.Empty;
                else
                    return this.SourceVirtualNetwork.ToString();
            }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty).Replace("-", String.Empty); }
        }

        public List<VirtualNetworkGateway> TargetVirtualNetworkGateways
        {
            get { return _TargetVirtualNetworkGateways; }
        }

        public List<Subnet> TargetSubnets
        {
            get { return _TargetSubnets; }
        }

        public override string ToString()
        {
            if (_AzureContext == null || _AzureContext.SettingsProvider == null)
                return this.TargetName;
            else
                return this.TargetName + _AzureContext.SettingsProvider.VirtualNetworkSuffix;
        }

        public string TargetId
        {
            get { return "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + this.ToString() + "')]"; }
        }



        public List<string> AddressPrefixes
        {
            get
            {

                return _AddressPrefixes;
            }
        }

        public List<string> DnsServers
        {
            get
            {
                return _DnsServers;
            }
        }


    }
}
