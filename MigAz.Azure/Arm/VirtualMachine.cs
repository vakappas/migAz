using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualMachine : ArmResource, IVirtualMachine
    {
        private List<IArmDisk> _DataDisks = new List<IArmDisk>();
        private IArmDisk _OSVirtualHardDisk;
        private List<NetworkInterface> _NetworkInterfaceCards = new List<NetworkInterface>();

        private VirtualMachine() : base(null) { }

        public VirtualMachine(JToken resourceToken) : base(resourceToken)
        {
            _OSVirtualHardDisk = new Disk(ResourceToken["properties"]["storageProfile"]["osDisk"]);
            foreach (JToken dataDiskToken in ResourceToken["properties"]["storageProfile"]["dataDisks"])
            {
                _DataDisks.Add(new Disk(dataDiskToken));
            }
        }

        public bool HasPlan
        {
            get
            {
                return ResourceToken["plan"] != null;
            }
        }

        public string Type => (string)ResourceToken["type"];
        public Guid VmId => new Guid((string)ResourceToken["properties"]["vmId"]);
        public string VmSize => (string)ResourceToken["properties"]["hardwareProfile"]["vmSize"];
        public string OSVirtualHardDiskOS => (string)ResourceToken["properties"]["storageProfile"]["osDisk"]["osType"];

        internal string AvailabilitySetId
        {
            get
            {
                try
                {
                    return (string)ResourceToken["properties"]["availabilitySet"]["id"];
                }
                catch (NullReferenceException)
                {
                    return String.Empty;
                }
            }
        }

        public List<IArmDisk> DataDisks => _DataDisks;
        public IArmDisk OSVirtualHardDisk => _OSVirtualHardDisk;
        public List<NetworkInterface> NetworkInterfaces => _NetworkInterfaceCards;

        public AvailabilitySet AvailabilitySet
        {
            get; private set;
        }
        public NetworkInterface PrimaryNetworkInterface
        {
            get
            {
                foreach (NetworkInterface networkInterface in this.NetworkInterfaces)
                {
                    if (networkInterface.IsPrimary)
                        return networkInterface;
                }

                return null;
            }
        }
        internal new async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            await base.InitializeChildrenAsync(azureContext);

            if (this.AvailabilitySetId != String.Empty)
                this.AvailabilitySet = azureContext.AzureRetriever.GetAzureARMAvailabilitySet(azureContext.AzureSubscription, this.AvailabilitySetId);

            if (this.AvailabilitySet != null)
                this.AvailabilitySet.VirtualMachines.Add(this);

            await this.OSVirtualHardDisk.InitializeChildrenAsync(azureContext);

            foreach (Disk dataDisk in this.DataDisks)
            {
                await dataDisk.InitializeChildrenAsync(azureContext);
            }

            foreach (JToken networkInterfaceToken in ResourceToken["properties"]["networkProfile"]["networkInterfaces"])
            {
                NetworkInterface networkInterface = await azureContext.AzureRetriever.GetAzureARMNetworkInterface((string)networkInterfaceToken["id"]);
                networkInterface.VirtualMachine = this;
                _NetworkInterfaceCards.Add(networkInterface);
            }

            return;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
