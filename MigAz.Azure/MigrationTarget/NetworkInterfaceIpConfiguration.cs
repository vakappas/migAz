using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkInterfaceIpConfiguration : IVirtualNetworkTarget, IMigrationTarget
    {
        private AzureContext _AzureContext;
        private INetworkInterfaceIpConfiguration _SourceIpConfiguration;
        private string _TargetName = String.Empty;
        private String _TargetPrivateIPAllocationMethod = "Dynamic";
        private String _TargetStaticIpAddress = String.Empty;

        private NetworkInterfaceIpConfiguration() { }

        public NetworkInterfaceIpConfiguration(AzureContext azureContext, Azure.Asm.NetworkInterfaceIpConfiguration ipConfiguration, List<VirtualNetwork> virtualNetworks)
        {
            _AzureContext = azureContext;
            _SourceIpConfiguration = ipConfiguration;

            this.TargetName = ipConfiguration.Name;
            this.TargetPrivateIPAllocationMethod = ipConfiguration.PrivateIpAllocationMethod;
            this.TargetPrivateIpAddress = ipConfiguration.PrivateIpAddress;

            #region Attempt to default Target Virtual Network and Target Subnet objects from source names

            this.TargetVirtualNetwork = SeekVirtualNetwork(virtualNetworks, ipConfiguration.VirtualNetworkName);
            if (this.TargetVirtualNetwork != null && this.TargetVirtualNetwork.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork)) // Should only be of this type, as we don't default to another existing ARM VNet (which would be of the base interface type also)
            {
                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)this.TargetVirtualNetwork;
                foreach (Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                {
                    if (targetSubnet.SourceName == ipConfiguration.SubnetName)
                    {
                        this.TargetSubnet = targetSubnet;
                        break;
                    }
                }
            }

            #endregion

        }

        public NetworkInterfaceIpConfiguration(AzureContext azureContext, Azure.Arm.NetworkInterfaceIpConfiguration ipConfiguration, List<VirtualNetwork> virtualNetworks)
        {
            _AzureContext = azureContext;
            _SourceIpConfiguration = ipConfiguration;

            #region Attempt to default Target Virtual Network and Target Subnet objects from source names

            this.TargetName = ipConfiguration.Name;
            this.TargetPrivateIPAllocationMethod = ipConfiguration.PrivateIpAllocationMethod;
            this.TargetPrivateIpAddress = ipConfiguration.PrivateIpAddress;
            this.TargetVirtualNetwork = SeekVirtualNetwork(virtualNetworks, ipConfiguration.VirtualNetworkName);
            if (this.TargetVirtualNetwork != null && this.TargetVirtualNetwork.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork)) // Should only be of this type, as we don't default to another existing ARM VNet (which would be of the base interface type also)
            {
                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)this.TargetVirtualNetwork;
                foreach (Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                {
                    if (targetSubnet.SourceName == ipConfiguration.SubnetName)
                    {
                        this.TargetSubnet = targetSubnet;
                        break;
                    }
                }
            }

            #endregion
            
        }

        private VirtualNetwork SeekVirtualNetwork(List<VirtualNetwork> virtualNetworks, string virtualNetworkName)
        {
            foreach (VirtualNetwork targetVirtualNetwork in virtualNetworks)
            {
                if (targetVirtualNetwork.SourceName == virtualNetworkName)
                    return targetVirtualNetwork;
            }

            return null;
        }

        public INetworkInterfaceIpConfiguration SourceIpConfiguration
        {
            get { return _SourceIpConfiguration; }
        }

        public String TargetPrivateIPAllocationMethod
        {
            get { return _TargetPrivateIPAllocationMethod; }
            set
            {
                if (value == "Static" || value == "Dynamic")
                    _TargetPrivateIPAllocationMethod = value;
                else
                    throw new ArgumentException("Must be 'Static' or 'Dynamic'.");
            }
        }

        public String TargetPrivateIpAddress
        {
            get { return _TargetStaticIpAddress; }
            set
            {
                if (value == null)
                    _TargetStaticIpAddress = String.Empty;
                else
                    _TargetStaticIpAddress = value.Trim();
            }
        }

        public string SourceName
        {
            get
            {
                if (this.SourceIpConfiguration == null)
                    return String.Empty;

                return this.SourceIpConfiguration.Name;
            }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public PublicIp TargetPublicIp { get; set; }
        public NetworkSecurityGroup TargetNetworkSecurityGroup { get; set; }

        public override string ToString()
        {
            return this.TargetName;
        }
    }
}
