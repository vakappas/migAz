using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class VirtualNetworkGateway : IMigrationTarget, IMigrationVirtualNetworkGateway
    {
        private AzureContext _AzureContext;
        private string _TargetName = String.Empty;
        private IVirtualNetworkGateway _SourceVirtualNetworkGateway;
        private List<Subnet> _TargetSubnets = new List<Subnet>();

        private VirtualNetworkGateway() { }

        public VirtualNetworkGateway(AzureContext azureContext, IVirtualNetworkGateway virtualNetworkGateway)
        {
            this._AzureContext = azureContext;
            this._SourceVirtualNetworkGateway = virtualNetworkGateway;
        }

        public IVirtualNetworkGateway SourceVirtualNetworkGateway { get; }

        public String SourceName
        {
            get
            {
                if (this.SourceVirtualNetworkGateway == null)
                    return String.Empty;
                else
                    return this.SourceVirtualNetworkGateway.ToString();
            }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.TargetName + _AzureContext.SettingsProvider.VirtualNetworkSuffix;
        }


    }
}
