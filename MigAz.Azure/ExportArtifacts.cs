using MigAz.Azure.MigrationTarget;
using MigAz.Core.Interface;
using System.Collections.Generic;

namespace MigAz.Azure
{
    public class ExportArtifacts : IExportArtifacts
    {
        public ExportArtifacts()
        {
            NetworkSecurityGroups = new List<NetworkSecurityGroup>();
            StorageAccounts = new List<StorageAccount>();
            VirtualNetworks = new List<VirtualNetwork>();
            VirtualMachines = new List<VirtualMachine>();
            LoadBalancers = new List<LoadBalancer>();
            PublicIPs = new List<PublicIp>();
        }

        public ResourceGroup ResourceGroup { get; set; }
        public List<NetworkSecurityGroup> NetworkSecurityGroups { get; }
        public List<StorageAccount> StorageAccounts { get; }
        public List<VirtualNetwork> VirtualNetworks { get; }
        public List<VirtualMachine> VirtualMachines { get; }
        public List<LoadBalancer> LoadBalancers { get; }
        public List<PublicIp> PublicIPs { get; }

        public NetworkSecurityGroup SeekNetworkSecurityGroup(string sourceName)
        {
            foreach (NetworkSecurityGroup networkSecurityGroup in NetworkSecurityGroups)
            {
                if (networkSecurityGroup.ToString() == sourceName)
                    return networkSecurityGroup;
            }

            return null;
        }
    }
}