using MigAz.Azure;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;

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

        private ResourceGroup() { }

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
        }

        public string Name => (string)_ResourceGroupJson["name"];
        public string Location => (string)_ResourceGroupJson["location"];
        public string Id => (string)_ResourceGroupJson["id"];

        public override string ToString()
        {
            return this.Name + " (" + this.Location + ")";
        }
    }
}
