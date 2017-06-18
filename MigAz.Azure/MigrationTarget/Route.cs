using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class Route : IMigrationTarget
    {
        private AzureContext _AzureContext;
        private String _TargetName = String.Empty;
        private String _NextHopType = String.Empty;
        private String _AddressPrefix = String.Empty;
        private String _NextHopIpAddress = String.Empty;
        

        private Route() { }

        public Route(AzureContext azureContext)
        {
            this._AzureContext = azureContext;
        }

        public String NextHopType
        {
            get { return _NextHopType; }
            set { _NextHopType = value.Trim(); }
        }

        public String AddressPrefix
        {
            get { return _AddressPrefix; }
            set { _AddressPrefix = value.Trim(); }
        }
        public String NextHopIpAddress
        {
            get { return _NextHopIpAddress; }
            set { _NextHopIpAddress = value.Trim(); }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public String SourceName
        {
            get { return String.Empty; }
        }

        public override string ToString()
        {
            return this.TargetName;
        }

    }
}
