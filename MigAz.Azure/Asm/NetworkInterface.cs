using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class NetworkInterface : INetworkInterface
    {
        #region Variables

        private AzureContext _AzureContext;
        private VirtualMachine _AsmVirtualMachine;
        private String _Name = "NetworkInterface1";
        private List<NetworkInterfaceIpConfiguration> _NetworkInterfaceIpConfigurations = new List<NetworkInterfaceIpConfiguration>();

        #endregion

        #region Constructors

        private NetworkInterface() { }

        public NetworkInterface(AzureContext azureContext, VirtualMachine asmVirtualMachine)
        {
            _AzureContext = azureContext;
            _AsmVirtualMachine = asmVirtualMachine;

            this.IsPrimary = false;
        }

        #endregion

        #region Properties

        public VirtualMachine VirtualMachine
        {
            get { return _AsmVirtualMachine; }
        }

        public bool IsPrimary { get; set; }

        public string Name
        {
            get { return _Name; }
            set { _Name = value.Replace(" ", String.Empty); }
        }

        public bool EnableIpForwarding { get; internal set; }

        public List<NetworkInterfaceIpConfiguration> NetworkInterfaceIpConfigurations
        {
            get { return _NetworkInterfaceIpConfigurations; }
        }

        public override string ToString()
        {
            return this.Name;
        }


        #endregion
    }
}
