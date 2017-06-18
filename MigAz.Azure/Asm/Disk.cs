using MigAz.Core.Interface;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class Disk : IDisk
    {
        private XmlNode _DataDiskNode;
        private AzureContext _AzureContext;
        private StorageAccount _SourceStorageAccount;
        private String _TargetName = String.Empty;

        public Disk(AzureContext azureContext, XmlNode dataDiskNode)
        {
            this._AzureContext = azureContext;
            this._DataDiskNode = dataDiskNode;
        }

        public async Task InitializeChildren()
        {
            _SourceStorageAccount = await _AzureContext.AzureRetriever.GetAzureAsmStorageAccount(StorageAccountName);
        }

        #region Properties

        public string SourceImageName
        {
            get { return _DataDiskNode.SelectSingleNode("SourceImageName").InnerText; }
        }

        public string MediaLink
        {
            get { return _DataDiskNode.SelectSingleNode("MediaLink").InnerText; }
        }

        public string DiskName
        {
            get { return _DataDiskNode.SelectSingleNode("DiskName").InnerText; }
        }

        public Int64? Lun
        {
            get
            {
                if (_DataDiskNode.SelectSingleNode("Lun") == null)
                    return null;

                return Int64.Parse(_DataDiskNode.SelectSingleNode("Lun").InnerText);
            }
        }

        public string HostCaching
        {
            get { return _DataDiskNode.SelectSingleNode("HostCaching").InnerText; }
        }

        public Int64? DiskSizeInGB
        {
            get
            {
                if (_DataDiskNode.SelectSingleNode("LogicalDiskSizeInGB") == null)
                    return null;

                return Int64.Parse(_DataDiskNode.SelectSingleNode("LogicalDiskSizeInGB").InnerText);
            }
        }

        public string StorageAccountName
        {
            get
            {
                return MediaLink.Split(new char[] { '/' })[2].Split(new char[] { '.' })[0];
            }
        }

        public string StorageAccountContainer
        {
            get
            {
                return MediaLink.Split(new char[] { '/' })[3];
            }
        }

        public string StorageAccountBlob
        {
            get
            {
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
                if (this.SourceStorageAccount == null || this.SourceStorageAccount.Keys == null)
                    return String.Empty;

                return this.SourceStorageAccount.Keys.Primary;
            }
        }

        #endregion

        public override string ToString()
        {
            return this.DiskName;
        }
    }
}
