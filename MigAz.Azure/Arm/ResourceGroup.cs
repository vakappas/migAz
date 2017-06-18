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

        internal ResourceGroup(JObject resourceGroupJson, AzureEnvironment azureEnvironment)
        {
            _ResourceGroupJson = resourceGroupJson;
            _AzureEnvironment = azureEnvironment;
        }

        private ResourceGroup() { }

        public string Name => (string)_ResourceGroupJson["name"];
        public string Location => (string)_ResourceGroupJson["location"];
        public string Id => (string)_ResourceGroupJson["id"];

        public override string ToString()
        {
            return this.Name + " (" + this.Location + ")";
        }
    }
}
