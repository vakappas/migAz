using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class StorageAccount : ArmResource, IStorageAccount, IStorageTarget
    {
        private List<StorageAccountKey> ResourceTokenKeys = new List<StorageAccountKey>();
        private AzureEnvironment _AzureEnvironment;

        private StorageAccount() : base(null) { }

        public StorageAccount(JToken resourceToken, AzureEnvironment azureEnvironment) : base(resourceToken)
        {
            _AzureEnvironment = azureEnvironment;
        }

        public List<StorageAccountKey> Keys
        {
            get { return ResourceTokenKeys; }
        }

        public string AccountType
        {
            get { return this.SkuName; }
        }

        public string Kind
        {
            get { return (string)ResourceToken["kind"]; }
        }

        public string CreationTime
        {
            get { return (string)ResourceToken["properties"]["creationTime"]; }
        }

        public string PrimaryLocation
        {
            get { return (string)ResourceToken["properties"]["primaryLocation"]; }
        }

        public string PrimaryEndpointBlob
        {
            get { return (string)ResourceToken["properties"]["primaryEndpoints"]["blob"]; }
        }

        public string PrimaryEndpointFile
        {
            get { return (string)ResourceToken["properties"]["primaryEndpoints"]["file"]; }
        }

        public string PrimaryEndpointQueue
        {
            get { return (string)ResourceToken["properties"]["primaryEndpoints"]["queue"]; }
        }

        public string PrimaryEndpointTable
        {
            get { return (string)ResourceToken["properties"]["primaryEndpoints"]["table"]; }
        }

        public string PrimaryStatus
        {
            get { return (string)ResourceToken["properties"]["statusOfPrimary"]; }
        }

        public string SecondaryLocation
        {
            get { return (string)ResourceToken["properties"]["secondaryLocation"]; }
        }

        public string SecondaryEndpointBlob
        {
            get { return (string)ResourceToken["properties"]["secondaryEndpoints"]["blob"]; }
        }

        public string SecondaryEndpointQueue
        {
            get { return (string)ResourceToken["properties"]["secondaryEndpoints"]["queue"]; }
        }

        public string SecondaryEndpointTable
        {
            get { return (string)ResourceToken["properties"]["secondaryEndpoints"]["table"]; }
        }

        public string SecondaryStatus
        {
            get { return (string)ResourceToken["properties"]["statusOfSecondary"]; }
        }

        public string SkuName
        {
            get { return (string)ResourceToken["sku"]["name"]; }
        }

        public string SkuTier
        {
            get { return (string)ResourceToken["sku"]["tier"]; }
        }

        public string BlobStorageNamespace
        {
            get
            {
                return AzureServiceUrls.GetBlobEndpointUrl(_AzureEnvironment);
            }
        }

        public StorageAccountType StorageAccountType
        {
            get { return MigrationTarget.StorageAccount.GetStorageAccountType(this.AccountType); }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
