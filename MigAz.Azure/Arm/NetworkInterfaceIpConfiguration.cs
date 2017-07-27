using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class NetworkInterfaceIpConfiguration : ArmResource, INetworkInterfaceIpConfiguration
    {
        private VirtualNetwork _VirtualNetwork;
        private Subnet _Subnet;
        private PublicIP _PublicIP;


        public NetworkInterfaceIpConfiguration(JToken networkInterfaceIpConfiguration) : base(networkInterfaceIpConfiguration)
        {
        }

        internal override async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            _VirtualNetwork = azureContext.AzureRetriever.GetAzureARMVirtualNetwork(azureContext.AzureSubscription, this.SubnetId);

            if (_VirtualNetwork != null)
            {
                foreach (Subnet subnet in _VirtualNetwork.Subnets)
                {
                    if (subnet.Name == this.SubnetName)
                    {
                        _Subnet = subnet;
                        break;
                    }
                }
            }

            if (this.PublicIpAddressId != String.Empty)
            {
                _PublicIP = await azureContext.AzureRetriever.GetAzureARMPublicIP(this.PublicIpAddressId);
            }
        }

        public string ProvisioningState => (string)this.ResourceToken["properties"]["provisioningState"];
        public string PrivateIpAddress => (string)this.ResourceToken["properties"]["privateIPAddress"];
        public string PrivateIpAddressVersion => (string)this.ResourceToken["properties"]["privateIPAddressVersion"];
        public string PrivateIpAllocationMethod => (string)this.ResourceToken["properties"]["privateIPAllocationMethod"];
        public bool IsPrimary => Convert.ToBoolean((string)this.ResourceToken["properties"]["primary"]);
        public string SubnetId => (string)this.ResourceToken["properties"]["subnet"]["id"];
        private string PublicIpAddressId
        {
            get
            {
                if (this.ResourceToken["properties"]["publicIPAddress"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["properties"]["publicIPAddress"]["id"];
            }
        }

        

        public string VirtualNetworkName
        {
            get { return SubnetId.Split('/')[8]; }
        }

        public string SubnetName
        {
            get { return SubnetId.Split('/')[10]; }
        }

        public VirtualNetwork VirtualNetwork => _VirtualNetwork;
        public Subnet Subnet => _Subnet;
        public BackEndAddressPool BackEndAddressPool
        {
            get;
            internal set;
        }

        public PublicIP PublicIP
        {
            get { return _PublicIP; }
            private set { _PublicIP = value; }
        }



    }
}
