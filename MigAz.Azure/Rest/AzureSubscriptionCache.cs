using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Rest
{
    public class AzureSubscriptionCache
    {
        #region Variables

        private AzureContext _AzureContext;

        private List<AzureTenant> _ArmTenants;
        private List<AzureSubscription> _ArmSubscriptions;

        // ASM Object Cache (Subscription Context Specific)
        private List<Asm.VirtualNetwork> _AsmVirtualNetworks;
        private List<Asm.StorageAccount> _AsmStorageAccounts;
        private List<Asm.CloudService> _AsmCloudServices;
        private List<Asm.ReservedIP> _AsmReservedIPs;

        // ARM Object Cache (Subscription Context Specific)
        private List<Arm.Location> _ArmLocations;
        private List<Arm.ResourceGroup> _ArmResourceGroups;
        private List<Arm.VirtualNetwork> _ArmVirtualNetworks;
        private List<Arm.StorageAccount> _ArmStorageAccounts;
        private List<Arm.AvailabilitySet> _ArmAvailabilitySets;
        private List<Arm.VirtualMachine> _ArmVirtualMachines;
        private List<Arm.ManagedDisk> _ArmManagedDisks;
        private List<Arm.NetworkInterface> _ArmNetworkInterfaces;
        private List<Arm.NetworkSecurityGroup> _ArmNetworkSecurityGroups;

        private List<MigrationTarget.AvailabilitySet> _MigrationAvailabilitySets;

        private Dictionary<string, AzureRestResponse> _RestApiCache = new Dictionary<string, AzureRestResponse>();

        #endregion

        #region Constructors

        internal IAzureRetriever() { }

        public IAzureRetriever(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        #endregion

        #region Properties

        internal AzureContext AzureContext
        {
            get { return _AzureContext; }
        }

        internal List<Arm.Location> ArmLocations
        {
            get { return _ArmLocations; }
            set { _ArmLocations = value; }
        }

        internal List<Arm.ManagedDisk> ArmManagedDisks
        {
            get { return _ArmManagedDisks; }
            set { _ArmManagedDisks = value; }
        }

        internal List<Arm.AvailabilitySet> ArmAvailabilitySets
        {
            get { return _ArmAvailabilitySets; }
            set { _ArmAvailabilitySets = value; }
        }

        internal List<Arm.NetworkInterface> ArmNetworkInterfaces
        {
            get { return _ArmNetworkInterfaces; }
            set { _ArmNetworkInterfaces = value; }
        }

        internal List<Arm.ResourceGroup> ArmResourceGroups
        {
            get { return _ArmResourceGroups; }
            set { _ArmResourceGroups = value; }
        }

        internal List<Arm.NetworkSecurityGroup> ArmNetworkSecurityGroups
        {
            get { return _ArmNetworkSecurityGroups; }
            set { _ArmNetworkSecurityGroups = value; }
        }

        internal List<AzureSubscription> ArmSubscriptions
        {
            get { return _ArmSubscriptions; }
            set { _ArmSubscriptions = value; }
        }

        internal List<Arm.VirtualMachine> ArmVirtualMachines
        {
            get { return _ArmVirtualMachines; }
            set { _ArmVirtualMachines = value; }
        }

        internal List<Arm.VirtualNetwork> ArmVirtualNetworks
        {
            get { return _ArmVirtualNetworks; }
            set { _ArmVirtualNetworks = value; }
        }

        internal List<Arm.StorageAccount> ArmStorageAccounts
        {
            get { return _ArmStorageAccounts; }
            set { _ArmStorageAccounts = value; }
        }

        internal List<MigrationTarget.AvailabilitySet> MigrationAvailabilitySets
        {
            get { return _MigrationAvailabilitySets; }
            set { _MigrationAvailabilitySets = value; }
        }

        internal List<Asm.VirtualNetwork> AsmVirtualNetworks
        {
            get { return _AsmVirtualNetworks; }
            set { _AsmVirtualNetworks = value; }
        }

        internal List<Asm.StorageAccount> AsmStorageAccounts
        {
            get { return _AsmStorageAccounts; }
            set { _AsmStorageAccounts = value; }
        }

        internal List<Asm.ReservedIP> AsmReservedIPs
        {
            get { return _AsmReservedIPs; }
            set { _AsmReservedIPs = value; }
        }

        internal List<Asm.CloudService> AsmCloudServices
        {
            get { return _AsmCloudServices; }
            set { _AsmCloudServices = value; }
        }

        internal List<AzureTenant> ArmTenants
        {
            get { return _ArmTenants; }
            set { _ArmTenants = value; }
        }

        internal Dictionary<string, AzureRestResponse> RestApiCache
        {
            get { return _RestApiCache; }
        }

        #endregion

        public void ClearCache()
        {
            _ArmTenants = null;
            _ArmSubscriptions = null;

            _AsmVirtualNetworks = null;
            _AsmStorageAccounts = null;
            _AsmCloudServices = null;
            _AsmReservedIPs = null;

            _ArmLocations = null;
            _ArmResourceGroups = null;
            _ArmVirtualNetworks = null;
            _ArmStorageAccounts = null;
            _ArmAvailabilitySets = null;
            _ArmNetworkInterfaces = null;
            _ArmNetworkSecurityGroups = null;
            _ArmVirtualMachines = null;
            _ArmManagedDisks = null;

            _MigrationAvailabilitySets = null;

            _RestApiCache = new Dictionary<string, AzureRestResponse>();
        }
    }
}
