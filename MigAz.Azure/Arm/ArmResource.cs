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
        private JToken _ResourceToken;
        private Location _Location;

        private ArmResource() { }

        internal ArmResource(JToken resourceToken)
        {
            _ResourceToken = resourceToken;
        }
        internal virtual async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            this.ResourceGroup = await azureContext.AzureRetriever.GetAzureARMResourceGroup(this.Id);
            this.Location = await azureContext.AzureRetriever.GetAzureARMLocation(this.LocationString);
            return;
        }

        public JToken ResourceToken => _ResourceToken;
        public string Name => (string)_ResourceToken["name"];
        public string Id => (string)_ResourceToken["id"];
        private string LocationString => (string)_ResourceToken["location"];

        public Location Location
        {
            get
            {
                return _Location;
            }
            private set
            {
                _Location = value;
            }
        }

        public ResourceGroup ResourceGroup { get; internal set; }
    }
}
