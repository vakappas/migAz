using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class Route : ArmResource, IRoute
    {
        private Route() : base(null) { }

        private Route(JToken resourceToken) : base(resourceToken)
        {

        }

        public string AddressPrefix => "TODO";
        public string NextHopType => "TODO";
        public string NextHopIpAddress => "TODO";
    }
}
