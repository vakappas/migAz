using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MigAz.Azure.Interface;

namespace MigAz.Azure.Arm
{
    public class Disk : ArmResource, IArmDisk
    {
        private StorageAccount _SourceStorageAccount = null;

        public Disk(JToken resourceToken) : base(resourceToken)
        {
        }

        private Disk() : base(null) { }

        public async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            _SourceStorageAccount = azureContext.AzureRetriever.GetAzureARMStorageAccount(azureContext.AzureSubscription, StorageAccountName);
        }

        public string CreateOption => (string)this.ResourceToken["createOption"];
        public string Caching => (string)this.ResourceToken["caching"];
        public int DiskSizeGb => Convert.ToInt32((string)this.ResourceToken["diskSizeGB"]);

        public int Lun => Convert.ToInt32((string)ResourceToken["lun"]);

        public string MediaLink
        {
            get
            {
                if (this.ResourceToken["vhd"] == null)
                    return String.Empty;
                if (this.ResourceToken["vhd"]["uri"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["vhd"]["uri"];
            }
        } 

        public string StorageAccountName
        {
            get
            {
                if (this.MediaLink == String.Empty)
                    return String.Empty;

                return MediaLink.Split(new char[] { '/' })[2].Split(new char[] { '.' })[0];
            }
        }

        public string StorageAccountContainer
        {
            get
            {
                if (this.MediaLink == String.Empty)
                    return String.Empty;

                return MediaLink.Split(new char[] { '/' })[3];
            }
        }

        public string StorageAccountBlob
        {
            get
            {
                if (this.MediaLink == String.Empty)
                    return String.Empty;

                return MediaLink.Split(new char[] { '/' })[4];
            }
        }

        public StorageAccount SourceStorageAccount
        {
            get { return _SourceStorageAccount; }
        }

        public String StorageKey
        {
            get
            {
                if (this.SourceStorageAccount == null || this.SourceStorageAccount.Keys[0] == null)
                    return String.Empty;

                return this.SourceStorageAccount.Keys[0].Value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
