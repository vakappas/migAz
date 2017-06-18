using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualNetwork : ArmResource, IVirtualNetwork, IMigrationVirtualNetwork
    {
        private List<VirtualNetworkGateway> ResourceTokenGateways = new List<VirtualNetworkGateway>();
        private List<ISubnet> _Subnets = new List<ISubnet>();
        private List<string> _AddressPrefixes = new List<string>();
        private List<string> _DnsServers = new List<string>();

        private VirtualNetwork() : base(null) { }

        public VirtualNetwork(JToken resourceToken) : base(resourceToken)
        {
            var subnets = from vnet in ResourceToken["properties"]["subnets"]
                          select vnet;

            foreach (var subnet in subnets)
            {
                Subnet armSubnet = new Subnet(this, subnet);
                _Subnets.Add(armSubnet);
            }

            var addressPrefixes = from vnet in ResourceToken["properties"]["addressSpace"]["addressPrefixes"]
                                  select vnet;

            foreach (var addressPrefix in addressPrefixes)
            {
                _AddressPrefixes.Add(addressPrefix.ToString());
            }

            if (ResourceToken["properties"] != null)
            {
                if (ResourceToken["properties"]["dhcpOptions"] != null)
                {
                    if (ResourceToken["properties"]["dhcpOptions"]["dnsServers"] != null)
                    {
                        var dnsPrefixes = from vnet in ResourceToken["properties"]["dhcpOptions"]["dnsServers"]
                                          select vnet;

                        foreach (var dnsPrefix in dnsPrefixes)
                        {
                            _DnsServers.Add(dnsPrefix.ToString());
                        }
                    }
                }
            }
        }

        public new async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            await base.InitializeChildrenAsync(azureContext);

            foreach (Subnet subnet in this.Subnets)
            {
                await subnet.InitializeChildrenAsync(azureContext);
            }
        }

        public string Type => (string)ResourceToken["type"];
        public ISubnet GatewaySubnet
        {
            get
            {
                foreach (Subnet subnet in this.Subnets)
                {
                    if (subnet.Name == ArmConst.GatewaySubnetName)
                        return subnet;
                }

                return null;
            }
        }
        public List<ISubnet> Subnets => _Subnets;
        public List<string> AddressPrefixes => _AddressPrefixes;
        public List<string> DnsServers => _DnsServers;

        public List<VirtualNetworkGateway> VirtualNetworkGateways
        {
            get { return ResourceTokenGateways; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public bool HasNonGatewaySubnet
        {
            get
            {
                if ((this.Subnets.Count() == 0) ||
                    ((this.Subnets.Count() == 1) && (this.Subnets[0].Name == ArmConst.GatewaySubnetName)))
                    return false;

                return true;
            }
        }

        public bool HasGatewaySubnet
        {
            get
            {
                return this.GatewaySubnet != null;
            }
        }

        public static bool operator ==(VirtualNetwork lhs, VirtualNetwork rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                    ((object)lhs != null && (object)rhs != null && lhs.Id == rhs.Id))
            {
                status = true;
            }
            return status;
        }

        public static bool operator !=(VirtualNetwork lhs, VirtualNetwork rhs)
        {
            return !(lhs == rhs);
        }
    }
}
