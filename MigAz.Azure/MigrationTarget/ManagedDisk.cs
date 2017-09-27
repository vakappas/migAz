using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget 
{
    public class ManagedDisk : IStorageTarget
    {
        private AzureContext _AzureContext;
        private StorageAccountType _StorageAccountType = StorageAccountType.Premium;
        private string _TargetName = "todo"; // String.Empty;
        private Azure.Arm.ManagedDisk _SourceManagedDisk;

        private ManagedDisk() { }

        public ManagedDisk(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        public ManagedDisk(Azure.Arm.ManagedDisk sourceManagedDisk)
        {
            _SourceManagedDisk = sourceManagedDisk;
            this.TargetName = sourceManagedDisk.Name;
        }

        public Azure.Arm.ManagedDisk SourceManagedDisk
        {
            get { return _SourceManagedDisk; }
        }

        public string SourceName
        {
            get
            {
                if (_SourceManagedDisk == null)
                    return String.Empty;

                return _SourceManagedDisk.Name;
            }
        }

        public StorageAccountType StorageAccountType
        {
            get { return _StorageAccountType; }
            set { _StorageAccountType = value; }
        }

        public string BlobStorageNamespace
        {
            get
            {
                return _AzureContext.AzureServiceUrls.GetBlobEndpointUrl();
            }
        }
        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public string ReferenceId
        {
            get
            {
                return "[resourceId('Microsoft.Compute/disks/', '" + this.ToString() + "')]";
            }
        }

        public override string ToString()
        {
            return this.TargetName;
        }
    }
}
