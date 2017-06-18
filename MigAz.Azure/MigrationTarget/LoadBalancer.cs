using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public enum LoadBalancerType
    {
        Public,
        Internal
    }

    public class LoadBalancer : IMigrationTarget
    {
        private String _SourceName = String.Empty;
        private string _TargetName = String.Empty;
        private ILoadBalancer _source;
        private List<FrontEndIpConfiguration> _FrontEndIpConfiguration = new List<FrontEndIpConfiguration>();
        private List<BackEndAddressPool> _BackEndAddressPools = new List<BackEndAddressPool>();
        private List<LoadBalancingRule> _LoadBalancingRules = new List<LoadBalancingRule>();
        private List<InboundNatRule> _InboundNatRules = new List<InboundNatRule>();
        private List<Probe> _Probes = new List<Probe>();
        private LoadBalancerType _LoadBalancerType = LoadBalancerType.Internal;

        public LoadBalancer(Arm.LoadBalancer sourceLoadBalancer)
        {
            this.Source = sourceLoadBalancer;
            this.Name = sourceLoadBalancer.Name;

            foreach (Arm.FrontEndIpConfiguration armFrontEndIpConfiguration in sourceLoadBalancer.FrontEndIpConfigurations)
            {
                FrontEndIpConfiguration targetFrontEndIpConfiguration = new FrontEndIpConfiguration(this, armFrontEndIpConfiguration);
                _FrontEndIpConfiguration.Add(targetFrontEndIpConfiguration);

                if (armFrontEndIpConfiguration.PublicIP != null)
                    this.LoadBalancerType = LoadBalancerType.Public;
            }

            foreach (Arm.BackEndAddressPool armBackendAddressPool in sourceLoadBalancer.BackEndAddressPools)
            {
                BackEndAddressPool targetBackendAddressPool = new BackEndAddressPool(this, armBackendAddressPool);
                _BackEndAddressPools.Add(targetBackendAddressPool);
            }

            foreach (Arm.Probe armProbe in sourceLoadBalancer.Probes)
            {
                Probe targetProbe = new Probe(this, armProbe);
                _Probes.Add(targetProbe);
            }

            foreach (Arm.LoadBalancingRule armLoadBalancingRule in sourceLoadBalancer.LoadBalancingRules)
            {
                LoadBalancingRule targetLoadBalancingRule = new LoadBalancingRule(this, armLoadBalancingRule);
                _LoadBalancingRules.Add(targetLoadBalancingRule);
            }
        }

        public LoadBalancer()
        {
        }

        public LoadBalancerType LoadBalancerType
        {
            get { return _LoadBalancerType; }
            set { _LoadBalancerType = value; }
        }

        public ILoadBalancer Source
        {
            get { return _source; }
            set
            {
                _source = value;
            }
        }

        public String SourceName
        {
            get
            {
                if (this.Source == null)
                    return _SourceName;
                else
                    return this.Source.Name;
            }
            set
            {
                _SourceName = value;
            }
        }

        public List<BackEndAddressPool> BackEndAddressPools
        {
            get { return _BackEndAddressPools; }
            set { _BackEndAddressPools = value; }
        }
        public List<FrontEndIpConfiguration> FrontEndIpConfigurations
        {
            get { return _FrontEndIpConfiguration; }
            set { _FrontEndIpConfiguration = value; }
        }

        public List<InboundNatRule> InboundNatRules
        {
            get { return _InboundNatRules; }
        }

        public List<Probe> Probes
        {
            get { return _Probes; }
        }

        public List<LoadBalancingRule> LoadBalancingRules
        {
            get { return _LoadBalancingRules; }
        }

        public String StaticVirtualNetworkIPAddress
        {
            get;set;
        }

        public string Name
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
