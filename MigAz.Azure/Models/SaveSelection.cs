using System;
using System.Collections.Generic;

namespace MigAz.Azure.Models
{
    public class SaveSelection
    {
        public Guid SubscriptionId;
        public List<SaveSelectionVirtualNetwork> VirtualNetworks;
        public List<SaveSelectioStorageAccount> StorageAccounts;
        public List<SaveSelectionVirtualMachine> VirtualMachines;
    }

    public class SaveSelectionVirtualNetwork
    {
        public string VirtualNetworkName;
    }

    public class SaveSelectioStorageAccount
    {
        public string StorageAccountName;
        public string TargetStorageAccountName;
    }

    public class SaveSelectionVirtualMachine
    {
        public string CloudService;
        public string VirtualMachine;
        public string TargetVirtualNetwork;
        public string TargetSubnet;
        public Dictionary<string, string> TargetDiskStorageAccounts = new Dictionary<string, string>();
    }
}
