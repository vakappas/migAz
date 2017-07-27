using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml;

namespace MigAz.Azure
{
    public class AzureSubscription : ISubscription
    {
        private XmlNode _XmlNode;
        private JObject _SubscriptionJson;
        private AzureEnvironment _AzureEnvironment;
        private AzureTenant _ParentTenant;

        private List<Arm.ResourceGroup> _ArmResourceGroups = new List<Arm.ResourceGroup>();
        private Dictionary<Arm.ResourceGroup, List<Arm.AvailabilitySet>> _ArmAvailabilitySets = new Dictionary<Arm.ResourceGroup, List<Arm.AvailabilitySet>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>> _ArmVirtualMachines = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetwork>> _ArmVirtualNetworks = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetwork>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.NetworkSecurityGroup>> _ArmNetworkSecurityGroups = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkSecurityGroup>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.StorageAccount>> _ArmStorageAccounts = new Dictionary<Arm.ResourceGroup, List<Arm.StorageAccount>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.ManagedDisk>> _ArmManagedDisks = new Dictionary<Arm.ResourceGroup, List<Arm.ManagedDisk>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.LoadBalancer>> _ArmLoadBalancers = new Dictionary<Arm.ResourceGroup, List<Arm.LoadBalancer>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.NetworkInterface>> _ArmNetworkInterfaces = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkInterface>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetworkGateway>> _ArmVirtualNetworkGateways = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetworkGateway>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.PublicIP>> _ArmPublicIPs = new Dictionary<Arm.ResourceGroup, List<Arm.PublicIP>>();


        private AzureSubscription() { }

        internal AzureSubscription(XmlNode xmlNode, AzureEnvironment azureEnvironment)
        {
            _XmlNode = xmlNode;
            _AzureEnvironment = azureEnvironment;
        }

        internal AzureSubscription(JObject subscriptionJson, AzureTenant parentAzureTenant, AzureEnvironment azureEnvironment)
        {
            _SubscriptionJson = subscriptionJson;
            _ParentTenant = parentAzureTenant;
            _AzureEnvironment = azureEnvironment;
        }

        public string Name
        {
            get
            {
                if (_XmlNode != null)
                    return _XmlNode.SelectSingleNode("SubscriptionName").InnerText;
                else if (_SubscriptionJson != null)
                    return (string)_SubscriptionJson["displayName"];
                else
                    return String.Empty;
            }
        }

        public AzureTenant Parent
        {
            get { return _ParentTenant; }
        }

        public AzureEnvironment AzureEnvironment
        {
            get { return _AzureEnvironment; }
        }

        public Guid AzureAdTenantId
        {
            get
            {
                if (this.Parent != null)
                    return this.Parent.TenantId;
                else if (_XmlNode != null)
                    return new Guid(_XmlNode.SelectSingleNode("AADTenantID").InnerText);
                else
                    return Guid.Empty;
            }
        }

        public Guid SubscriptionId
        {
            get
            {
                if (_XmlNode != null)
                    return new Guid(_XmlNode.SelectSingleNode("SubscriptionID").InnerText);
                else if (_SubscriptionJson != null)
                    return new Guid((string)_SubscriptionJson["subscriptionId"]);
                else
                    return Guid.Empty;

            }
        }

        public string offercategories
        {
            get
            {
                if (_XmlNode != null && _XmlNode.SelectSingleNode("OfferCategories") != null)
                    return _XmlNode.SelectSingleNode("OfferCategories").InnerText;
                else return String.Empty;
            }
        }

        public string SubscriptionStatus
        {
            get
            {
                if (_XmlNode != null && _XmlNode.SelectSingleNode("SubscriptionStatus") != null)
                    return _XmlNode.SelectSingleNode("SubscriptionStatus").InnerText;
                else return String.Empty;
            }
        }

        public string AccountAdminLiveEmailId
        {
            get { return _XmlNode.SelectSingleNode("AccountAdminLiveEmailId").InnerText; }
        }

        public string ServiceAdminLiveEmailId
        {
            get { return _XmlNode.SelectSingleNode("ServiceAdminLiveEmailId").InnerText; }
        }

        public Int32 MaxCoreCount
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxCoreCount").InnerText); }
        }

        public Int32 MaxStorageAccounts
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxStorageAccounts").InnerText); }
        }

        public Int32 MaxHostedServices
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxHostedServices").InnerText); }
        }

        public Int32 CurrentCoreCount
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentCoreCount").InnerText); }
        }

        public Int32 CurrentHostedServices
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentHostedServices").InnerText); }
        }

        public Int32 CurrentStorageAccounts
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentStorageAccounts").InnerText); }
        }

        public Int32 MaxVirtualNetworkSites
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxVirtualNetworkSites").InnerText); }
        }

        public Int32 CurrentVirtualNetworkSites
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentVirtualNetworkSites").InnerText); }
        }

        public Int32 MaxLocalNetworkSites
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxLocalNetworkSites").InnerText); }
        }

        public Int32 MaxDnsServers
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxDnsServers").InnerText); }
        }

        public Int32 CurrentDnsServers
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentDnsServers").InnerText); }
        }

        public Int32 MaxExtraVIPCount
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxExtraVIPCount").InnerText); }
        }

        public Int32 MaxReservedIPs
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxReservedIPs").InnerText); }
        }

        public Int32 CurrentReservedIPs
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentReservedIPs").InnerText); }
        }

        public Int32 MaxPublicIPCount
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxPublicIPCount").InnerText); }
        }

        public Int32 CurrentNetworkSecurityGroups
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentNetworkSecurityGroups").InnerText); }
        }

        public Int32 MaxNetworkSecurityGroups
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxNetworkSecurityGroups").InnerText); }
        }

        public Int32 MaxNetworkSecurityRulesPerGroup
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxNetworkSecurityRulesPerGroup").InnerText); }
        }

        public Int32 MaxRouteTables
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxRouteTables").InnerText); }
        }

        public Int32 MaxRoutesPerRouteTable
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxRoutesPerRouteTable").InnerText); }
        }

        public Int32 MaxRoutesBackendPerRouteTable
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxRoutesBackendPerRouteTable").InnerText); }
        }

        public DateTime CreatedTime
        {
            get { return Convert.ToDateTime(_XmlNode.SelectSingleNode("CreatedTime").InnerText); }
        }

        public override string ToString()
        {
            return Name + " (" + SubscriptionId + ")";
        }

        public static bool operator ==(AzureSubscription lhs, AzureSubscription rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) || 
                    ((object)lhs != null && (object)rhs != null && lhs.SubscriptionId   == rhs.SubscriptionId))
            {
                status = true;
            }
            return status;
        }

        public static bool operator !=(AzureSubscription lhs, AzureSubscription rhs)
        {
            return !(lhs == rhs);
        }

        public void ClearCache()
        {
            _ArmVirtualMachines = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>>();
            _ArmAvailabilitySets = new Dictionary<Arm.ResourceGroup, List<Arm.AvailabilitySet>>();
            _ArmVirtualNetworks = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetwork>>();
            _ArmNetworkSecurityGroups = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkSecurityGroup>>();
            _ArmStorageAccounts = new Dictionary<Arm.ResourceGroup, List<Arm.StorageAccount>>();
            _ArmManagedDisks = new Dictionary<Arm.ResourceGroup, List<Arm.ManagedDisk>>();
            _ArmLoadBalancers = new Dictionary<Arm.ResourceGroup, List<Arm.LoadBalancer>>();
            _ArmNetworkInterfaces = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkInterface>>();
            _ArmVirtualNetworkGateways = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetworkGateway>>();
            _ArmPublicIPs = new Dictionary<Arm.ResourceGroup, List<Arm.PublicIP>>();
            _ArmResourceGroups = new List<Arm.ResourceGroup>();
        }

        internal Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>> ArmVirtualMachines
        {
            get { return _ArmVirtualMachines; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.AvailabilitySet>> ArmAvailabilitySets
        {
            get { return _ArmAvailabilitySets; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetwork>> ArmVirtualNetworks
        {
            get { return _ArmVirtualNetworks; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.StorageAccount>> ArmStorageAccounts
        {
            get { return _ArmStorageAccounts; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.NetworkSecurityGroup>> ArmNetworkSecurityGroups
        {
            get { return _ArmNetworkSecurityGroups; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.ManagedDisk>> ArmManagedDisks
        {
            get { return _ArmManagedDisks; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.LoadBalancer>> ArmLoadBalancers
        {
            get { return _ArmLoadBalancers; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.NetworkInterface>> ArmNetworkInterfaces
        {
            get { return _ArmNetworkInterfaces; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetworkGateway>> ArmVirtualNetworkGateways
        {
            get { return _ArmVirtualNetworkGateways; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.PublicIP>> ArmPublicIPs
        {
            get { return _ArmPublicIPs; }
        }
        internal List<Arm.ResourceGroup> ArmResourceGroups
        {
            get { return _ArmResourceGroups; }
        }
    }
}
