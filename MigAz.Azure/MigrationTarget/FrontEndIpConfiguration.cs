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
            this.TargetPrivateIPAllocationMethod = armFrontEndIpConfiguration.PrivateIPAllocationMethod;
            this.TargetPrivateIpAddress = armFrontEndIpConfiguration.PrivateIPAddress;
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

        public PublicIp PublicIp
        {
            get { return _PublicIp; }
            set
            {
                _PublicIp = value;

                if (value != null)
                    this.LoadBalancer.LoadBalancerType = LoadBalancerType.Public;
            }
        }
    }
}
