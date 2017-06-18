using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class FrontEndIpConfiguration : IVirtualNetworkTarget
    {
        private String _Name = "default";

        private String _PrivateIPAllocationMethod = "Dynamic";
        private String _PrivateIPAddress = String.Empty;

        private PublicIp _PublicIp = null;

        private LoadBalancer _ParentLoadBalancer = null;
        private Arm.FrontEndIpConfiguration _Source;

        private FrontEndIpConfiguration() { }

        public FrontEndIpConfiguration(LoadBalancer loadBalancer)
        {
            _ParentLoadBalancer = loadBalancer;
            loadBalancer.FrontEndIpConfigurations.Add(this);
        }

        public FrontEndIpConfiguration(LoadBalancer loadBalancer, Arm.FrontEndIpConfiguration armFrontEndIpConfiguration)
        {
            _ParentLoadBalancer = loadBalancer;
            _Source = armFrontEndIpConfiguration;

            this.Name = armFrontEndIpConfiguration.Name;
            this.PrivateIPAllocationMethod = armFrontEndIpConfiguration.PrivateIPAllocationMethod;
            this.PrivateIPAddress = armFrontEndIpConfiguration.PrivateIPAddress;
            this.TargetVirtualNetwork = armFrontEndIpConfiguration.VirtualNetwork;
            this.TargetSubnet = armFrontEndIpConfiguration.Subnet;
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

        public Arm.FrontEndIpConfiguration Source
        {
            get { return _Source; }
        }

        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public String PrivateIPAllocationMethod
        {
            get { return _PrivateIPAllocationMethod; }
            set { _PrivateIPAllocationMethod = value; }
        }

        public String PrivateIPAddress
        {
            get { return _PrivateIPAddress; }
            set { _PrivateIPAddress = value; }
        }

        public PublicIp PublicIp
        {
            get { return _PublicIp; }
            set { _PublicIp = value; }
        }
    }
}
