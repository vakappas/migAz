using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class VirtualMachine : IMigrationTarget
    {
        private AvailabilitySet _TargetAvailabilitySet = null;
        private string _TargetName = String.Empty;
        private AzureContext _AzureContext;
        private String _TargetSize = String.Empty;
        private List<NetworkInterface> _NetworkInterfaces = new List<NetworkInterface>();
        private List<Disk> _DataDisks = new List<Disk>();
        private Dictionary<string, string> _PlanAttributes;

        private VirtualMachine() { }

        public VirtualMachine(AzureContext azureContext, Asm.VirtualMachine virtualMachine, List<VirtualNetwork> targetVirtualNetworks, List<StorageAccount> targetStorageAccounts, List<NetworkSecurityGroup> networkSecurityGroups)
        {
            this._AzureContext = azureContext;
            this.Source = virtualMachine;
            this.TargetName = virtualMachine.RoleName;
            this._TargetSize = virtualMachine.RoleSize;
            this.OSVirtualHardDisk = new Disk(virtualMachine.OSVirtualHardDisk);
            this.OSVirtualHardDiskOS = virtualMachine.OSVirtualHardDiskOS;
            this.OSVirtualHardDisk.TargetStorageAccount = SeekTargetStorageAccount(targetStorageAccounts, virtualMachine.OSVirtualHardDisk.StorageAccountName);

            foreach (Asm.Disk asmDataDisk in virtualMachine.DataDisks)
            {
                Disk targetDataDisk = new Disk(asmDataDisk);
                targetDataDisk.TargetStorageAccount = SeekTargetStorageAccount(targetStorageAccounts, asmDataDisk.StorageAccountName);
                this.DataDisks.Add(targetDataDisk);
            }

            foreach (Asm.NetworkInterface asmNetworkInterface in virtualMachine.NetworkInterfaces)
            {
                Azure.MigrationTarget.NetworkInterface migrationNetworkInterface = new Azure.MigrationTarget.NetworkInterface(_AzureContext, virtualMachine, asmNetworkInterface, targetVirtualNetworks, networkSecurityGroups);
                this.NetworkInterfaces.Add(migrationNetworkInterface);
            }
        }

        public VirtualMachine(AzureContext azureContext, Arm.VirtualMachine virtualMachine, List<VirtualNetwork> targetVirtualNetworks, List<StorageAccount> targetStorageAccounts, List<NetworkSecurityGroup> networkSecurityGroups)
        {
            this._AzureContext = azureContext;
            this.Source = virtualMachine;
            this.TargetName = virtualMachine.Name;
            this._TargetSize = virtualMachine.VmSize;
            this.OSVirtualHardDiskOS = virtualMachine.OSVirtualHardDiskOS;

            this.OSVirtualHardDisk = new Disk(virtualMachine.OSVirtualHardDisk);

            if (virtualMachine.OSVirtualHardDisk.GetType() == typeof(Arm.Disk))
            {
                Arm.Disk armDisk = (Arm.Disk)virtualMachine.OSVirtualHardDisk;
                this.OSVirtualHardDisk.TargetStorageAccount = SeekTargetStorageAccount(targetStorageAccounts, armDisk.StorageAccountName);
            }

            foreach (IArmDisk dataDisk in virtualMachine.DataDisks)
            {
                Disk targetDataDisk = new Disk(dataDisk);
                this.DataDisks.Add(targetDataDisk);

                if (dataDisk.GetType() == typeof(Arm.Disk))
                {
                    Arm.Disk armDisk = (Arm.Disk)dataDisk;
                    targetDataDisk.TargetStorageAccount = SeekTargetStorageAccount(targetStorageAccounts, armDisk.StorageAccountName);
                }
            }

            foreach (Arm.NetworkInterface armNetworkInterface in virtualMachine.NetworkInterfaces)
            {
                Azure.MigrationTarget.NetworkInterface migrationNetworkInterface = new Azure.MigrationTarget.NetworkInterface(_AzureContext, armNetworkInterface, targetVirtualNetworks, networkSecurityGroups);
                this.NetworkInterfaces.Add(migrationNetworkInterface);
            }

            if (virtualMachine.HasPlan)
            {
                _PlanAttributes = new Dictionary<string, string>();

                foreach (JProperty planAttribute in virtualMachine.ResourceToken["plan"])
                {
                    _PlanAttributes.Add(planAttribute.Name, planAttribute.Value.ToString());
                }
            }
        }

        private StorageAccount SeekTargetStorageAccount(List<StorageAccount> storageAccounts, string sourceAccountName)
        {
            foreach (StorageAccount targetStorageAccount in storageAccounts)
            {
                if (targetStorageAccount.SourceName == sourceAccountName)
                    return targetStorageAccount;
            }

            return null;
        }

        public Disk OSVirtualHardDisk
        {
            get; set;
        }

        public List<Disk> DataDisks
        {
            get { return _DataDisks; }
        }

        public IVirtualMachine Source { get; set; }

        public List<NetworkInterface> NetworkInterfaces
        {
            get { return _NetworkInterfaces; }
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

        public String TargetSize
        {
            get { return _TargetSize; }
            set { _TargetSize = value.Trim(); }
        }


        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public Dictionary<string, string> PlanAttributes
        {
            get { return _PlanAttributes; }
            set { _PlanAttributes = value; }
        }

        public string SourceName
        {
            get
            {
                if (this.Source == null)
                    return String.Empty;
                else
                    return this.Source.ToString();
            }
        }

        public string OSVirtualHardDiskOS
        {
            get; set;
        }

        public override string ToString()
        {
            return this.TargetName + _AzureContext.SettingsProvider.VirtualMachineSuffix;
        }

        public AvailabilitySet TargetAvailabilitySet
        {
            get { return _TargetAvailabilitySet; }
            set { _TargetAvailabilitySet = value; }
        }
    }
}
