using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class RoleSize
    {
        private AzureContext _AzureContext;
        private XmlNode _XmlNode;

        public RoleSize(AzureContext azureContext, XmlNode roleSizeXml)
        {
            this._AzureContext = azureContext;
            this._XmlNode = roleSizeXml;
        }

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }

        public string Label
        {
            get { return _XmlNode.SelectSingleNode("Label").InnerText; }
        }
        public Int32 Cores
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("Cores").InnerText); }
        }
        public Int32 MemoryInMb
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MemoryInMb").InnerText); }
        }
        public bool SupportedByWebWorkerRoles
        {
            get { return Convert.ToBoolean(_XmlNode.SelectSingleNode("SupportedByWebWorkerRoles").InnerText); }
        }
        public bool SupportedByVirtualMachines
        {
            get { return Convert.ToBoolean(_XmlNode.SelectSingleNode("SupportedByVirtualMachines").InnerText); }
        }
        public Int32 MaxDataDiskCount
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxDataDiskCount").InnerText); }
        }
        public string WebWorkerResourceDiskSizeInMb
        {
            get { return _XmlNode.SelectSingleNode("WebWorkerResourceDiskSizeInMb").InnerText; }
        }
        public string VirtualMachineResourceDiskSizeInMb
        {
            get { return _XmlNode.SelectSingleNode("VirtualMachineResourceDiskSizeInMb").InnerText; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
