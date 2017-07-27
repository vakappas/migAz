using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class Subnet : IMigrationTarget, IMigrationSubnet
    {
        private AzureContext _AzureContext = null;
        private ISubnet _SourceSubnet;
        private String _TargetName = String.Empty;
        private MigrationTarget.VirtualNetwork _ParentVirtualNetwork;

        private Subnet() { }

        public Subnet(AzureContext azureContext, MigrationTarget.VirtualNetwork parentVirtualNetwork, ISubnet source, List<NetworkSecurityGroup> networkSecurityGroups)
        {
            _AzureContext = azureContext;
            _ParentVirtualNetwork = parentVirtualNetwork;
            _SourceSubnet = source;

            if (source.GetType() == typeof(Asm.Subnet))
            {
                Asm.Subnet asmSubnet = (Asm.Subnet)source;

                if (asmSubnet.NetworkSecurityGroup != null)
                {
                    this.NetworkSecurityGroup = SeekNetworkSecurityGroup(networkSecurityGroups, asmSubnet.NetworkSecurityGroup.ToString());
                }

                if (asmSubnet.RouteTable != null)
                {
                    this.RouteTable = new RouteTable(azureContext, asmSubnet.RouteTable);
                }
            }
            else if (source.GetType() == typeof(Arm.Subnet))
            {
                Arm.Subnet armSubnet = (Arm.Subnet)source;

                if (armSubnet.NetworkSecurityGroup != null)
                {
                    this.NetworkSecurityGroup = SeekNetworkSecurityGroup(networkSecurityGroups, armSubnet.NetworkSecurityGroup.ToString());
                }

                if (armSubnet.RouteTable != null)
                {
                    this.RouteTable = new RouteTable(azureContext, armSubnet.RouteTable);
                }

            }

            this.AddressPrefix = source.AddressPrefix;
            this.TargetName = source.Name;
        }

        public Subnet(MigrationTarget.VirtualNetwork parentVirtualNetwork, ISubnet sourceSubnet)
        {
            this._ParentVirtualNetwork = parentVirtualNetwork;
            this._SourceSubnet = sourceSubnet;
            this.TargetName = sourceSubnet.Name;
            this.AddressPrefix = sourceSubnet.AddressPrefix;
        }

        private NetworkSecurityGroup SeekNetworkSecurityGroup(List<NetworkSecurityGroup> networkSecurityGroups, string sourceName)
        {
            if (networkSecurityGroups == null || sourceName == null)
                return null;

            foreach (NetworkSecurityGroup networkSecurityGroup in networkSecurityGroups)
            {
                if (networkSecurityGroup.SourceName == sourceName)
                    return networkSecurityGroup;
            }

            return null;
        }

        public String AddressPrefix { get; set; }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty).Replace("-", String.Empty); }
        }

        public ISubnet SourceSubnet
        {
            get { return _SourceSubnet; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceSubnet == null)
                    return String.Empty;
                else
                    return this.SourceSubnet.ToString();
            }
        }

        public MigrationTarget.VirtualNetwork ParentVirtualNetwork
        {
            get { return _ParentVirtualNetwork; }
        }

        public override string ToString()
        {
            return this.TargetName;
        }

        public string TargetId
        {
            get { return "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + this.ParentVirtualNetwork.ToString() + "/subnets/" + this.TargetName + "')]"; }
        }

        public RouteTable RouteTable { get; set;  }
        public NetworkSecurityGroup NetworkSecurityGroup { get; set; }

        public bool IsGatewaySubnet
        {
            get { return this.TargetName == ArmConst.GatewaySubnetName; }
        }
    }
}
