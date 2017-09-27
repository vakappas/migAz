using MigAz.Azure;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class ResourceGroup
    {
        private JObject _ResourceGroupJson;
        private AzureEnvironment _AzureEnvironment;
        private AzureSubscription _AzureSubscription;

        internal ResourceGroup(JObject resourceGroupJson, AzureEnvironment azureEnvironment, AzureSubscription azureSubscription)
        {
            _ResourceGroupJson = resourceGroupJson;
            _AzureEnvironment = azureEnvironment;
            _AzureSubscription = azureSubscription;
        }

        public async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            this.Location = await azureContext.AzureRetriever.GetAzureARMLocation(this.LocationString);
        }

        private ResourceGroup() { }

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
        }

        public string Name => (string)_ResourceGroupJson["name"];
        private string LocationString => (string)_ResourceGroupJson["location"];
        public string Id => (string)_ResourceGroupJson["id"];

        public Location Location
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return this.Name + " (" + this.Location + ")";
        }
    }
}
