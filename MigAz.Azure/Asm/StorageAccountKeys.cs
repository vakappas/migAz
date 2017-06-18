using System.Xml;

namespace MigAz.Azure.Asm
{
    public class StorageAccountKeys
    {
        private AzureContext _AzureContext;
        private XmlDocument _StorageAccountKeysXml;

        public StorageAccountKeys(AzureContext azureContext, XmlDocument storageAccountKeysXml)
        {
            this._AzureContext = azureContext;
            this._StorageAccountKeysXml = storageAccountKeysXml;
        }

        public string Primary
        {
            get { return _StorageAccountKeysXml.SelectSingleNode("//StorageServiceKeys/Primary").InnerText; }
        }

        public string Secondary
        {
            get { return _StorageAccountKeysXml.SelectSingleNode("//StorageServiceKeys/Secondary").InnerText; }
        }
    }
}
