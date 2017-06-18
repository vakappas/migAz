using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class LoadBalancingRule : ArmResource
    {
        private LoadBalancer _ParentLoadBalancer = null;
        private Probe _Probe = null;
        private BackEndAddressPool _BackEndAddressPool = null;

        public LoadBalancingRule(LoadBalancer loadBalancer, JToken loadBalancingRuleToken) : base(loadBalancingRuleToken)
        {
            _ParentLoadBalancer = loadBalancer;
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

        public Probe Probe
        {
            get { return _Probe; }
            internal set { _Probe = value; }
        }

        public BackEndAddressPool BackEndAddressPool
        {
            get { return _BackEndAddressPool; }
            internal set { _BackEndAddressPool = value; }
        }

        public bool EnableFloatingIP
        {
            get { return Convert.ToBoolean((string)this.ResourceToken["properties"]["enableFloatingIP"]); }
        }

        public Int32 IdleTimeoutInMinutes
        {
            get { return Convert.ToInt32((string)this.ResourceToken["properties"]["idleTimeoutInMinutes"]); }
        }

        public Int32 FrontEndPort
        {
            get { return Convert.ToInt32((string)this.ResourceToken["properties"]["frontendPort"]); }
        }

        public Int32 BackEndPort
        {
            get { return Convert.ToInt32((string)this.ResourceToken["properties"]["backendPort"]); }
        }

        public String Protocol
        {
            get { return (string)this.ResourceToken["properties"]["protocol"]; }
        }

        public FrontEndIpConfiguration FrontEndIpConfiguration
        {
            get;
            internal set;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
