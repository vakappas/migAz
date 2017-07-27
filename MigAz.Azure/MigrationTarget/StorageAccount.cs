using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class StorageAccount : IStorageTarget, IMigrationTarget
    {
        private AzureContext _AzureContext = null;
        private IStorageAccount _Source;
        private string _TargetName = String.Empty;
        private string _BlobStorageNamespace = String.Empty;

        private StorageAccount() { }

        public StorageAccount(AzureContext azureContext, IStorageAccount source)
        {
            _AzureContext = azureContext;
            _Source = source;
            this.TargetName = source.Name;
            this.AccountType = source.AccountType;
            this._BlobStorageNamespace = azureContext.AzureServiceUrls.GetBlobEndpointUrl();
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public string AccountType { get; set; }
        public string BlobStorageNamespace { get { return _BlobStorageNamespace; } }
        public IStorageAccount SourceAccount
        {
            get { return _Source; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceAccount == null)
                    return String.Empty;
                else
                    return this.SourceAccount.ToString();
            }
        }

        public StorageAccountType StorageAccountType
        {
            get { return MigrationTarget.StorageAccount.GetStorageAccountType(this.AccountType); }
        }

        public override string ToString()
        {
            if (this.TargetName.Length + this._AzureContext.SettingsProvider.StorageAccountSuffix.Length > 24)
                return this.TargetName.Substring(0, 24 - this._AzureContext.SettingsProvider.StorageAccountSuffix.Length) + this._AzureContext.SettingsProvider.StorageAccountSuffix;
            else
                return this.TargetName + this._AzureContext.SettingsProvider.StorageAccountSuffix;
        }

        public static StorageAccountType GetStorageAccountType(string storageAccountType)
        {
            // https://msdn.microsoft.com/en-us/library/azure/hh264518.aspx

            if (String.Compare("Premium_LRS", storageAccountType, true) == 0)
                return StorageAccountType.Premium;

            return StorageAccountType.Standard;
        }
    }
}
