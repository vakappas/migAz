using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class LoadBalancingRule
    {
        private Probe _Probe = null;
        private BackEndAddressPool _BackEndAddressPool = null;
        private String _Name = String.Empty;
        private Int32 _FrontEndPort = 0;
        private Int32 _BackEndPort = 0;
        private String _Protocol = "tcp";
        private LoadBalancer _ParentLoadBalancer = null;
        private bool _EnableFloatingIP = false;
        private Int32 _IdleTimeoutInMinutes = 4;

        private LoadBalancingRule() { }

        public LoadBalancingRule(LoadBalancer loadBalancer)
        {
            _ParentLoadBalancer = loadBalancer;
            loadBalancer.LoadBalancingRules.Add(this);
        }
        public LoadBalancingRule(LoadBalancer loadBalancer, Arm.LoadBalancingRule armLoadBalancingRule)
        {
            _ParentLoadBalancer = loadBalancer;

            this.Name = armLoadBalancingRule.Name;
            this.FrontEndPort = armLoadBalancingRule.FrontEndPort;
            this.BackEndPort = armLoadBalancingRule.BackEndPort;
            this.Protocol = armLoadBalancingRule.Protocol;
            this.EnableFloatingIP = armLoadBalancingRule.EnableFloatingIP;
            this.IdleTimeoutInMinutes = armLoadBalancingRule.IdleTimeoutInMinutes;

            if (armLoadBalancingRule.FrontEndIpConfiguration != null)
            {
                foreach (FrontEndIpConfiguration targetFrontEndIpConfiguration in loadBalancer.FrontEndIpConfigurations)
                {
                    if (targetFrontEndIpConfiguration.Name == armLoadBalancingRule.FrontEndIpConfiguration.Name)
                    {
                        this.FrontEndIpConfiguration = targetFrontEndIpConfiguration;
                        break;
                    }
                }
            }

            if (armLoadBalancingRule.BackEndAddressPool != null)
            {
                foreach (BackEndAddressPool targetBackEndAddressPool in loadBalancer.BackEndAddressPools)
                {
                    if (targetBackEndAddressPool.Name == armLoadBalancingRule.BackEndAddressPool.Name)
                    {
                        this.BackEndAddressPool = targetBackEndAddressPool;
                        break;
                    }
                }
            }

            if (armLoadBalancingRule.Probe != null)
            {
                foreach (Probe targetProbe in loadBalancer.Probes)
                {
                    if (targetProbe.Name == armLoadBalancingRule.Probe.Name)
                    {
                        this.Probe = targetProbe;
                        break;
                    }
                }
            }
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

        public Probe Probe
        {
            get { return _Probe; }
            set { _Probe = value; }
        }

        public bool EnableFloatingIP
        {
            get { return _EnableFloatingIP; }
            set { _EnableFloatingIP = value; }
        }

        public Int32 IdleTimeoutInMinutes
        {
            get { return _IdleTimeoutInMinutes; }
            set { _IdleTimeoutInMinutes = value; }
        }

        public BackEndAddressPool BackEndAddressPool
        {
            get { return _BackEndAddressPool; }
            set { _BackEndAddressPool = value; }
        }

        public Int32 FrontEndPort
        {
            get { return _FrontEndPort; }
            set { _FrontEndPort = value; }
        }

        public Int32 BackEndPort
        {
            get { return _BackEndPort; }
            set { _BackEndPort = value; }
        }

        public String Protocol
        {
            get { return _Protocol; }
            set { _Protocol = value; }
        }

        public FrontEndIpConfiguration FrontEndIpConfiguration
        {
            get; set;
        }

        public String Name
        {
            get { return _Name; }
            set { _Name = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
