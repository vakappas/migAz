using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class Probe : ArmResource
    {
        private LoadBalancer _LoadBalancer;
        private List<LoadBalancingRule> _LoadBalancingRules = new List<LoadBalancingRule>();

        public Probe(LoadBalancer loadBalancer, JToken probeToken) : base(probeToken)
        {
            _LoadBalancer = loadBalancer;
        }

        public String Protocol
        {
            get { return (string)this.ResourceToken["properties"]["protocol"]; }
        }

        public Int32 IntervalInSeconds
        {
            get { return Convert.ToInt32((string)this.ResourceToken["properties"]["intervalInSeconds"]); }
        }
        public Int32 NumberOfProbes
        {
            get { return Convert.ToInt32((string)this.ResourceToken["properties"]["numberOfProbes"]); }
        }
        public Int32 Port
        {
            get { return Convert.ToInt32((string)this.ResourceToken["properties"]["port"]); }
        }

        public String RequestPath
        {
            get { return (string)this.ResourceToken["properties"]["requestPath"]; }
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
                        loadBalancingRuleIds.Add((string)loadBalancingRuleId["id"]);
                    }
                }

                return loadBalancingRuleIds;
            }
        }
    }
}
