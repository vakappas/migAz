using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class RouteTable : IRouteTable
    {
        private List<Route> _Routes = new List<Route>();

        public string Name => "TODO";
        public List<Route> Routes => _Routes; // todo  routenode in resource.properties.routes)
    }
}
