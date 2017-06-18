using MigAz.Azure.Arm;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class VirtualMachine : IVirtualMachine
    {
        #region Variables

        private CloudService _AsmCloudService;
        private AzureContext _AzureContext;
        private XmlNode _XmlNode;
        private Hashtable _VmDetails;
        Disk _OSVirtualHardDisk;
        List<Disk> _DataDisks;
        List<LoadBalancerRule> _LoadBalancerRules = new List<LoadBalancerRule>();
        List<NetworkInterface> _NetworkInterfaces = new List<NetworkInterface>();
        private VirtualNetwork _SourceVirtualNetwork;
        private Subnet _SourceSubnet;
        private String _NetworkSecurityGroupName = String.Empty;
        private NetworkSecurityGroup _AsmNetworkSecurityGroup = null;

        #endregion

        #region Constructors

        private VirtualMachine() { }

        public VirtualMachine(AzureContext azureContext, CloudService asmCloudService, ISettingsProvider settingsProvider, XmlNode virtualMachineXml, Hashtable vmDetails)
        {
            this._AsmCloudService = asmCloudService;
            this._AzureContext = azureContext;
            this._XmlNode = virtualMachineXml;
            this._VmDetails = vmDetails;

            _OSVirtualHardDisk = new Disk(azureContext, _XmlNode.SelectSingleNode("//OSVirtualHardDisk"));

            _DataDisks = new List<Disk>();
            foreach (XmlNode dataDiskNode in _XmlNode.SelectNodes("//DataVirtualHardDisks/DataVirtualHardDisk"))
            {
                Disk asmDisk = new Disk(azureContext, dataDiskNode);
                _DataDisks.Add(asmDisk);
            }

            foreach (XmlNode loadBalancerRuleNode in _XmlNode.SelectNodes("//ConfigurationSets/ConfigurationSet/InputEndpoints/InputEndpoint"))
            {
                _LoadBalancerRules.Add(new LoadBalancerRule(_AzureContext, loadBalancerRuleNode));
            }

            #region Primary Network Interface

            NetworkInterface primaryNetworkInterface = new NetworkInterface(_AzureContext, this);
            this.NetworkInterfaces.Add(primaryNetworkInterface);

            primaryNetworkInterface.IsPrimary = true;
            primaryNetworkInterface.Name = this.RoleName + "-NIC1";

            NetworkInterfaceIpConfiguration primaryNetworkInterfaceIpConfiguration = new NetworkInterfaceIpConfiguration(_AzureContext);
            primaryNetworkInterface.NetworkInterfaceIpConfigurations.Add(primaryNetworkInterfaceIpConfiguration);

            primaryNetworkInterfaceIpConfiguration.VirtualNetworkName = vmDetails["virtualnetworkname"].ToString();

            // code note, unclear why this is index [1] on the ConfigurationSet ... couldn't it be a different order?
            if (_XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/SubnetNames/SubnetName") != null)
            {
                primaryNetworkInterfaceIpConfiguration.SubnetName = _XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/SubnetNames/SubnetName").InnerText;
            }

            if (_XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/StaticVirtualNetworkIPAddress") != null)
            {
                primaryNetworkInterfaceIpConfiguration.PrivateIpAllocationMethod = "Static";
                primaryNetworkInterfaceIpConfiguration.PrivateIpAddress = _XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/StaticVirtualNetworkIPAddress").InnerText;
            }

            if (_XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/NetworkSecurityGroup") != null)
            {
                _NetworkSecurityGroupName = _XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/NetworkSecurityGroup").InnerText;
            }

            #endregion


            #region Additional Network Interfaces

            foreach (XmlNode additionalNetworkInterfaceXml in _XmlNode.SelectNodes("//ConfigurationSets/ConfigurationSet/NetworkInterfaces/NetworkInterface"))
            {
                NetworkInterface additionalNetworkInterface = new NetworkInterface(_AzureContext, this);
                this.NetworkInterfaces.Add(additionalNetworkInterface);

                additionalNetworkInterface.Name = this.RoleName + "-" + additionalNetworkInterfaceXml.SelectSingleNode("Name").InnerText;

                if (additionalNetworkInterfaceXml.SelectNodes("IPForwarding").Count > 0)
                {
                    additionalNetworkInterface.EnableIpForwarding = true;
                }

                NetworkInterfaceIpConfiguration ipConfiguration = new NetworkInterfaceIpConfiguration(_AzureContext);
                additionalNetworkInterface.NetworkInterfaceIpConfigurations.Add(ipConfiguration);

                ipConfiguration.Name = "ipconfig1";
                ipConfiguration.VirtualNetworkName = vmDetails["virtualnetworkname"].ToString();

                if (_XmlNode.SelectSingleNode("IPConfigurations/IPConfiguration/SubnetName") != null)
                {
                    ipConfiguration.SubnetName = additionalNetworkInterfaceXml.SelectSingleNode("IPConfigurations/IPConfiguration/SubnetName").InnerText;
                }

                if (additionalNetworkInterfaceXml.SelectSingleNode("IPConfigurations/IPConfiguration/StaticVirtualNetworkIPAddress") != null)
                {
                    ipConfiguration.PrivateIpAllocationMethod = "Static";
                    ipConfiguration.PrivateIpAddress = additionalNetworkInterfaceXml.SelectSingleNode("IPConfigurations/IPConfiguration/StaticVirtualNetworkIPAddress").InnerText;
                }
            }

            #endregion
        }

        #endregion

        public async Task InitializeChildren()
        {
            if (this.VirtualNetworkName != String.Empty)
            {
                _SourceVirtualNetwork = await _AzureContext.AzureRetriever.GetAzureAsmVirtualNetwork(this.VirtualNetworkName);

                if (_SourceVirtualNetwork != null)
                {
                    foreach (Subnet asmSubnet in _SourceVirtualNetwork.Subnets)
                    {
                        if (asmSubnet.Name == this.SubnetName)
                        {
                            _SourceSubnet = asmSubnet;
                            break;
                        }
                    }
                }
            }

            await _OSVirtualHardDisk.InitializeChildren();
            foreach (Disk asmDisk in this.DataDisks)
            {
                await asmDisk.InitializeChildren();
            }

            if (this.NetworkSecurityGroupName != String.Empty)
                _AsmNetworkSecurityGroup = await _AzureContext.AzureRetriever.GetAzureAsmNetworkSecurityGroup(this.NetworkSecurityGroupName);
        }

        #region Properties

        public XmlNode ResourceXml
        {
            get { return _XmlNode; }
        }

        public CloudService Parent
        {
            get { return _AsmCloudService; }
        }

        public List<Disk> DataDisks
        {
            get { return _DataDisks; }
        }

        public List<LoadBalancerRule> LoadBalancerRules
        {
            get { return _LoadBalancerRules; }
        }

        public List<NetworkInterface> NetworkInterfaces
        {
            get { return _NetworkInterfaces; }
        }

        public NetworkInterface PrimaryNetworkInterface
        {
            get
            {
                foreach (NetworkInterface networkInterface in _NetworkInterfaces)
                {
                    if (networkInterface.IsPrimary)
                        return networkInterface;
                }

                return null;
            }
        }

        public string AvailabilitySetName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//AvailabilitySetName") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//AvailabilitySetName").InnerText;
            }
        }

        public NetworkSecurityGroup NetworkSecurityGroup
        {
            get { return _AsmNetworkSecurityGroup; }
        }



        public string RoleName
        {
            get { return _XmlNode.SelectSingleNode("//RoleName").InnerText; }
        }

        public string RoleSize
        {
            get { return _XmlNode.SelectSingleNode("//RoleSize").InnerText; }
        }

        public bool EnabledIpForwarding
        {
            get { return _XmlNode.SelectNodes("//ConfigurationSets/ConfigurationSet/IPForwarding").Count > 0; }
        }

        private string NetworkSecurityGroupName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet/NetworkSecurityGroup") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet/NetworkSecurityGroup").InnerText;
            }
        }

        public string StaticVirtualNetworkIPAddress
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/StaticVirtualNetworkIPAddress") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/StaticVirtualNetworkIPAddress").InnerText;
            }
        }

        public string SubnetName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet/SubnetNames/SubnetName") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet/SubnetNames[1]/SubnetName").InnerText;
            }
        }

        public string OSVirtualHardDiskOS
        {
            get { return _XmlNode.SelectSingleNode("//OSVirtualHardDisk/OS").InnerText; }
        }

        public Disk OSVirtualHardDisk
        {
            get { return _OSVirtualHardDisk; }
        }

        public string VirtualNetworkName
        {
            get { return _VmDetails["virtualnetworkname"].ToString(); }
        }

        public string LoadBalancerName
        {
            get { return _VmDetails["loadbalancername"].ToString(); }
        }

        public string CloudServiceName
        {
            get { return _VmDetails["cloudservicename"].ToString(); }
        }

        public bool HasPublicIPs
        {
            get
            {
                return _XmlNode.SelectNodes("//ConfigurationSets/ConfigurationSet/PublicIPs").Count > 0;
            }
        }


        public Subnet SourceSubnet
        {
            get { return _SourceSubnet; }
        }
        public VirtualNetwork SourceVirtualNetwork
        {
            get { return _SourceVirtualNetwork; }
        }

        public override string ToString()
        {
            return this.RoleName;
        }

        #endregion
    }
}
