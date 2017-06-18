using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class ArmResource
    {
        private JToken _ResourceToken = null;

        private ArmResource() { }

        internal ArmResource(JToken resourceToken)
        {
            _ResourceToken = resourceToken;
        }

        public JToken ResourceToken => _ResourceToken;
        public string Name => (string)_ResourceToken["name"];
        public string Id => (string)_ResourceToken["id"];
        public string Location => (string)_ResourceToken["location"];

        public ResourceGroup ResourceGroup { get; set; }

        internal virtual async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            this.ResourceGroup = await azureContext.AzureRetriever.GetAzureARMResourceGroup(this.Id);
            return;
        }

    }
}
