using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core.ArmTemplate
{
    public class ArmConst
    {
        public const string ResourceGroupId = "resourceGroup().id";
        public const string Locations = "/locations";
        public const string ProviderVirtualNetwork = "/providers/Microsoft.Network/virtualNetworks/";
        public const string ProviderLoadBalancers = "/providers/Microsoft.Network/loadBalancers/";
        public const string ProviderPublicIpAddress = "/providers/Microsoft.Network/publicIPAddresses/";
        public const string ProviderNetworkSecurityGroups = "/providers/Microsoft.Network/networkSecurityGroups/";
        public const string ProviderRouteTables = "/providers/Microsoft.Network/routeTables/";
        public const string ProviderLocalNetworkGateways = "/providers/Microsoft.Network/localNetworkGateways/";
        public const string ProviderVirtualNetworkGateways = "/providers/Microsoft.Network/virtualNetworkGateways/";
        public const string ProviderNetworkInterfaces = "/providers/Microsoft.Network/networkInterfaces/";
        public const string ProviderAvailabilitySets = "/providers/Microsoft.Compute/availabilitySets/";
        public const string ProviderStorageAccounts = "/providers/Microsoft.Storage/storageAccounts/";
        public const string ProviderExpressRouteCircuits = "/providers/Microsoft.Network/expressRouteCircuits/";
        public const string ProviderVirtualMachines = "/providers/Microsoft.Compute/virtualMachines/";
        public const string ProviderManagedDisks = "/providers/Microsoft.Compute/disks/";
        public const string ProviderGatewayConnection = "/providers/Microsoft.Network/connections/";
        public const string ProviderVMSizes = "/providers/Microsoft.Compute/locations/{0}/vmSizes/";
        public const string GatewaySubnetName = "GatewaySubnet";
    }
}
