using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkSecurityGroup : IMigrationTarget
    {
        private AzureContext _AzureContext = null;
        private INetworkSecurityGroup _SourceNetworkSecurityGroup;
        private string _TargetName = String.Empty;
        private List<NetworkSecurityGroupRule> _Rules = new List<NetworkSecurityGroupRule>();

        private NetworkSecurityGroup() { }

        public NetworkSecurityGroup(AzureContext azureContext, Asm.NetworkSecurityGroup source)
        {
            _AzureContext = azureContext;
            _SourceNetworkSecurityGroup = source;
            this.TargetName = source.Name;

            foreach (Asm.NetworkSecurityGroupRule sourceRule in source.Rules)
            {
                NetworkSecurityGroupRule targetRule = new NetworkSecurityGroupRule(azureContext, sourceRule);
                this.Rules.Add(targetRule);
            }
        }

        public NetworkSecurityGroup(AzureContext azureContext, Arm.NetworkSecurityGroup source)
        {
            _AzureContext = azureContext;
            _SourceNetworkSecurityGroup = source;
            this.TargetName = source.Name;

            foreach (Arm.NetworkSecurityGroupRule sourceRule in source.Rules)
            {
                NetworkSecurityGroupRule targetRule = new NetworkSecurityGroupRule(azureContext, sourceRule);
                this.Rules.Add(targetRule);
            }
        }

        public static NetworkSecurityGroup SeekNetworkSecurityGroup(List<NetworkSecurityGroup> networkSecurityGroups, string sourceName)
        {
            foreach (NetworkSecurityGroup networkSecurityGroup in networkSecurityGroups)
            {
                if (networkSecurityGroup.SourceName == sourceName)
                    return networkSecurityGroup;
            }

            return null;
        }


        public List<NetworkSecurityGroupRule> Rules
        {
            get { return _Rules; }
        }
        
        public INetworkSecurityGroup SourceNetworkSecurityGroup
        {
            get { return _SourceNetworkSecurityGroup; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceNetworkSecurityGroup == null)
                    return String.Empty;
                else
                    return this.SourceNetworkSecurityGroup.ToString();
            }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.TargetName + _AzureContext.SettingsProvider.NetworkSecurityGroupSuffix;
        }
    }
}
