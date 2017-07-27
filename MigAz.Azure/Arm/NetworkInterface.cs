using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{

    public class NetworkInterface : ArmResource, INetworkInterface
    {
        private VirtualMachine _VirtualMachine;
        private List<NetworkInterfaceIpConfiguration> _NetworkInterfaceIpConfigurations = new List<NetworkInterfaceIpConfiguration>();

        private NetworkInterface() : base(null) { }

        public NetworkInterface(JToken resourceToken) : base(resourceToken)
        {
            foreach (JToken networkInterfaceIpConfigurationToken in ResourceToken["properties"]["ipConfigurations"])
            {
                NetworkInterfaceIpConfiguration networkInterfaceIpConfiguration = new NetworkInterfaceIpConfiguration(networkInterfaceIpConfigurationToken);
                _NetworkInterfaceIpConfigurations.Add(networkInterfaceIpConfiguration);
            }
        }

        public async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            foreach (NetworkInterfaceIpConfiguration networkInterfaceIpConfiguration in this.NetworkInterfaceIpConfigurations)
            {
                await networkInterfaceIpConfiguration.InitializeChildrenAsync(azureContext);
            }

            if (this.NetworkSecurityGroupId != String.Empty)
                this.NetworkSecurityGroup = await azureContext.AzureRetriever.GetAzureARMNetworkSecurityGroup(this.NetworkSecurityGroupId);
        }

        public string EnableIPForwarding => (string)ResourceToken["properties"]["enableIPForwarding"];
        public string EnableAcceleratedNetworking => (string)ResourceToken["properties"]["enableAcceleratedNetworking"];
        public Guid ResourceGuid => new Guid((string)ResourceToken["properties"]["resourceGuid"]);
        public bool IsPrimary => Convert.ToBoolean((string)ResourceToken["properties"]["primary"]);
        public VirtualMachine VirtualMachine
        {
            get { return _VirtualMachine; }
            set { _VirtualMachine = value; }
        }

        public List<NetworkInterfaceIpConfiguration> NetworkInterfaceIpConfigurations
        {
            get { return _NetworkInterfaceIpConfigurations; }
        }

        public NetworkInterfaceIpConfiguration PrimaryIpConfiguration
        {
            get
            {
                foreach (NetworkInterfaceIpConfiguration ipConfiguration in this._NetworkInterfaceIpConfigurations)
                {
                    if (ipConfiguration.IsPrimary)
                        return ipConfiguration;
                }

                return null;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        private string NetworkSecurityGroupId
        {
            get
            {
                if (this.ResourceToken["properties"]["networkSecurityGroup"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["properties"]["networkSecurityGroup"]["id"];
            }
        }

        public NetworkSecurityGroup NetworkSecurityGroup
        {
            get;
            private set;
        }
    }
}
