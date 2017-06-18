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
        public IMigrationVirtualNetwork TargetVirtualNetwork { get; set; }
        public IMigrationSubnet TargetSubnet { get; set; }
    }
}
