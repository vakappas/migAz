using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    public class AzureSubscriptionResourceCache
    {
        private AzureSubscription _AzureSubscription;

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

        private AzureSubscriptionResourceCache() { }

        public AzureSubscriptionResourceCache(AzureSubscription azureSubscription)
        {
            _AzureSubscription = azureSubscription;
        }

        public void ClearCache()
        {
            _ArmVirtualMachines = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>>();
            _ArmAvailabilitySets = new Dictionary<Arm.ResourceGroup, List<Arm.AvailabilitySet>>();
            _ArmVirtualMachines = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>>();
            _ArmVirtualNetworks = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetwork>>();
            _ArmNetworkSecurityGroups = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkSecurityGroup>>();
            _ArmStorageAccounts = new Dictionary<Arm.ResourceGroup, List<Arm.StorageAccount>>();
            _ArmManagedDisks = new Dictionary<Arm.ResourceGroup, List<Arm.ManagedDisk>>();
            _ArmLoadBalancers = new Dictionary<Arm.ResourceGroup, List<Arm.LoadBalancer>>();
            _ArmNetworkInterfaces = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkInterface>>();
            _ArmVirtualNetworkGateways = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetworkGateway>>();
            _ArmPublicIPs = new Dictionary<Arm.ResourceGroup, List<Arm.PublicIP>>();
        }

    }
}
