using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Interface
{
    public class IVirtualNetworkTarget
    {
        private String _TargetPrivateIPAllocationMethod = "Dynamic";
        private String _TargetStaticIpAddress = String.Empty;

        public IMigrationVirtualNetwork TargetVirtualNetwork { get; set; }
        public IMigrationSubnet TargetSubnet { get; set; }
        public String TargetPrivateIPAllocationMethod
        {
            get { return _TargetPrivateIPAllocationMethod; }
            set
            {
                if (value == "Static" || value == "Dynamic")
                    _TargetPrivateIPAllocationMethod = value;
                else
                    throw new ArgumentException("Must be 'Static' or 'Dynamic'.");
            }
        }

        public String TargetPrivateIpAddress
        {
            get { return _TargetStaticIpAddress; }
            set
            {
                if (value == null)
                    _TargetStaticIpAddress = String.Empty;
                else
                    _TargetStaticIpAddress = value.Trim();
            }
        }

    }
}
