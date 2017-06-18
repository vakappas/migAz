using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class NetworkSecurityGroupRule
    {
        private JToken _NetworkSecurityGroupRuleToken;

        private NetworkSecurityGroupRule() { }

        public NetworkSecurityGroupRule(JToken networkSecurityGroupRuleToken)
        {
            _NetworkSecurityGroupRuleToken = networkSecurityGroupRuleToken;
        }

        public string Id => (string)_NetworkSecurityGroupRuleToken["id"];
        public string Name => (string)_NetworkSecurityGroupRuleToken["name"];
        public Int32 Priority => Convert.ToInt32((string)_NetworkSecurityGroupRuleToken["properties"]["priority"]);
        public string Direction => (string)_NetworkSecurityGroupRuleToken["properties"]["direction"];
        public string Access => (string)_NetworkSecurityGroupRuleToken["properties"]["access"];
        public string SourceAddressPrefix => (string)_NetworkSecurityGroupRuleToken["properties"]["sourceAddressPrefix"];
        public string DestinationAddressPrefix => (string)_NetworkSecurityGroupRuleToken["properties"]["destinationAddressPrefix"];
        public string SourcePortRange => (string)_NetworkSecurityGroupRuleToken["properties"]["sourcePortRange"];
        public string DestinationPortRange => (string)_NetworkSecurityGroupRuleToken["properties"]["destinationPortRange"];
        public string Protocol => (string)_NetworkSecurityGroupRuleToken["properties"]["protocol"];


    }
}
