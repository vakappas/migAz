using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class BackEndAddressPool : ArmResource
    {
        private LoadBalancer _ParentLoadBalancer = null;
        private List<LoadBalancingRule> _LoadBalancingRules = new List<LoadBalancingRule>();
        private List<NetworkInterfaceIpConfiguration> _NetworkInterfaceIpConfigurations = new List<NetworkInterfaceIpConfiguration>();


        public BackEndAddressPool(LoadBalancer loadBalancer, JToken backEndAddressPoolToken) : base(backEndAddressPoolToken)
        {
            _ParentLoadBalancer = loadBalancer;
        }

        internal override async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            foreach (string backEndIpConfigurationId in this.BackEndIPConfigurationIds)
            {
                NetworkInterface networkInterface = await azureContext.AzureRetriever.GetAzureARMNetworkInterface(backEndIpConfigurationId);
                if (networkInterface != null)
                {
                    foreach (NetworkInterfaceIpConfiguration networkInterfaceIpConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
                    {
                        if (String.Compare(networkInterfaceIpConfiguration.Id, backEndIpConfigurationId) == 0)
                        {
                            networkInterfaceIpConfiguration.BackEndAddressPool = this;
                            this.NetworkInterfaceIpConfigurations.Add(networkInterfaceIpConfiguration);
                            break;
                        }
                    }
                }
            }
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

        public List<NetworkInterfaceIpConfiguration> NetworkInterfaceIpConfigurations
        {
            get { return _NetworkInterfaceIpConfigurations; }
        }

        public List<LoadBalancingRule> LoadBalancingRules
        {
            get { return _LoadBalancingRules; }
        }

        internal List<String> BackEndIPConfigurationIds
        {
            get
            {
                List<String> backEndIPConfigurationIds = new List<string>();

                if (this.ResourceToken["properties"]["backendIPConfigurations"] != null)
                {
                    foreach (JToken backEndIPConfiguration in this.ResourceToken["properties"]["backendIPConfigurations"])
                    {
                        backEndIPConfigurationIds.Add((string)backEndIPConfiguration["id"]);
                    }
                }

                return backEndIPConfigurationIds;
            }
        }

        internal List<String> LoadBalancingRuleIds
        {
            get
            {
                List<String> loadBalancingRuleIds = new List<string>();

                if (this.ResourceToken["properties"]["loadBalancingRules"] != null)
                {
                    foreach (JToken loadBalancingRule in this.ResourceToken["properties"]["loadBalancingRules"])
                    {
                        loadBalancingRuleIds.Add((string)loadBalancingRule["id"]);
                    }
                }

                return loadBalancingRuleIds;
            }
        }
    }
}
