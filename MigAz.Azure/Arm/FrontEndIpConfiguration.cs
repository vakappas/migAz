using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class FrontEndIpConfiguration : ArmResource
    {
        private List<LoadBalancingRule> _LoadBalancingRules = new List<LoadBalancingRule>();
        private VirtualNetwork _VirtualNetwork;
        private Subnet _Subnet;
        private PublicIP _PublicIP;

        private LoadBalancer _ParentLoadBalancer = null;

        public FrontEndIpConfiguration(LoadBalancer loadBalancer, JToken frontEndIpConfigurationToken) : base(frontEndIpConfigurationToken)
        {
            _ParentLoadBalancer = loadBalancer;
        }

        internal override async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            if (this.PublicIpId != String.Empty)
                this.PublicIP = await azureContext.AzureRetriever.GetAzureARMPublicIP(this.PublicIpId);
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }


        public string PrivateIPAllocationMethod
        {
            get { return (string)this.ResourceToken["properties"]["privateIPAllocationMethod"]; }
        }

        public string PrivateIPAddress
        {
            get { return (string)this.ResourceToken["properties"]["privateIPAddress"]; }
        }

        public VirtualNetwork VirtualNetwork
        {
            get { return _VirtualNetwork; }
            set { _VirtualNetwork = value; }
        }

        public Subnet Subnet
        {
            get { return _Subnet; }
            set { _Subnet = value; }
        }

        public List<LoadBalancingRule> LoadBalancingRules
        {
            get { return _LoadBalancingRules; }
        }

        internal List<String> LoadBalancingRuleIds
        {
            get
            {
                List<String> loadBalancingRuleIds = new List<string>();

                if (this.ResourceToken["properties"]["loadBalancingRules"] != null)
                {
                    foreach (JToken loadBalancingRuleId in this.ResourceToken["properties"]["loadBalancingRules"])
                    {
                        loadBalancingRuleIds.Add((string) loadBalancingRuleId["id"]);
                    }
                }

                return loadBalancingRuleIds;
            }
        }

        private String PublicIpId
        {
            get
            {
                if (this.ResourceToken == null || this.ResourceToken["properties"]["publicIPAddress"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["properties"]["publicIPAddress"]["id"];
            }
        }

        public PublicIP PublicIP
        {
            get { return _PublicIP; }
            private set { _PublicIP = value; }
        }
    }
}
