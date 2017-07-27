﻿using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class LoadBalancer : ArmResource, ILoadBalancer
    {
        private List<FrontEndIpConfiguration> _FrontEndIpConfigurations = new List<FrontEndIpConfiguration>();
        private List<BackEndAddressPool> _BackEndAddressPool = new List<BackEndAddressPool>();
        private List<LoadBalancingRule> _LoadBalancingRules = new List<LoadBalancingRule>();
        private List<Probe> _Probes = new List<Probe>();

        public LoadBalancer(JToken resourceToken) : base(resourceToken)
        {
            // Create objects
            foreach (JToken frontEndIpConfigurationToken in ResourceToken["properties"]["frontendIPConfigurations"])
            {
                FrontEndIpConfiguration frontEndIpConfiguration = new FrontEndIpConfiguration(this, frontEndIpConfigurationToken);
                _FrontEndIpConfigurations.Add(frontEndIpConfiguration);
            }

            foreach (JToken backendAddressPoolToken in ResourceToken["properties"]["backendAddressPools"])
            {
                BackEndAddressPool backEndAddressPool = new BackEndAddressPool(this, backendAddressPoolToken);
                _BackEndAddressPool.Add(backEndAddressPool);
            }

            foreach (JToken loadBalancingRuleToken in ResourceToken["properties"]["loadBalancingRules"])
            {
                LoadBalancingRule loadBalancingRule = new LoadBalancingRule(this, loadBalancingRuleToken);
                _LoadBalancingRules.Add(loadBalancingRule);
            }

            foreach (JToken probeToken in ResourceToken["properties"]["probes"])
            {
                Probe probe = new Probe(this, probeToken);
                _Probes.Add(probe);
            }

            // Bind object relations
            foreach (FrontEndIpConfiguration frontEndIpConfiguration in this.FrontEndIpConfigurations)
            {
                foreach (String loadBalancingRuleId in frontEndIpConfiguration.LoadBalancingRuleIds)
                {
                    foreach (LoadBalancingRule loadBalancingRule in this.LoadBalancingRules)
                    {
                        if (String.Compare(loadBalancingRule.Id, loadBalancingRuleId) == 0)
                        {
                            frontEndIpConfiguration.LoadBalancingRules.Add(loadBalancingRule);
                            loadBalancingRule.FrontEndIpConfiguration = frontEndIpConfiguration;
                        }
                    }
                }
            }

            foreach (BackEndAddressPool backEndAddressPool in this.BackEndAddressPools)
            {
                foreach (String loadBalancingRuleId in backEndAddressPool.LoadBalancingRuleIds)
                {
                    foreach (LoadBalancingRule loadBalancingRule in this.LoadBalancingRules)
                    {
                        if (String.Compare(loadBalancingRule.Id, loadBalancingRuleId) == 0)
                        {
                            backEndAddressPool.LoadBalancingRules.Add(loadBalancingRule);
                            loadBalancingRule.BackEndAddressPool = backEndAddressPool;
                        }
                    }
                }
            }

            foreach (Probe probe in this.Probes)
            {
                foreach (String loadBalancingRuleId in probe.LoadBalancingRuleIds)
                {
                    foreach (LoadBalancingRule loadBalancingRule in this.LoadBalancingRules)
                    {
                        if (String.Compare(loadBalancingRule.Id, loadBalancingRuleId) == 0)
                        {
                            probe.LoadBalancingRules.Add(loadBalancingRule);
                            loadBalancingRule.Probe = probe;
                        }
                    }
                }
            }
        }

        internal override async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            await base.InitializeChildrenAsync(azureContext);

            foreach (FrontEndIpConfiguration frontEndIpConfiguration in this.FrontEndIpConfigurations)
            {
                await frontEndIpConfiguration.InitializeChildrenAsync(azureContext);
            }

            foreach (BackEndAddressPool backEndAddressPool in this.BackEndAddressPools)
            {
                await backEndAddressPool.InitializeChildrenAsync(azureContext);
            }
        }


        public List<FrontEndIpConfiguration> FrontEndIpConfigurations
        {
            get { return _FrontEndIpConfigurations; }
        }

        public List<BackEndAddressPool> BackEndAddressPools
        {
            get { return _BackEndAddressPool; }
        }

        public List<LoadBalancingRule> LoadBalancingRules
        {
            get { return _LoadBalancingRules; }
        }

        public List<Probe> Probes
        {
            get { return _Probes; }
        }
    }
}
