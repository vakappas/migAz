using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class RouteTable : IMigrationTarget
    {
        private AzureContext _AzureContext;
        private IRouteTable _SourceRouteTable;
        private String _TargetName = String.Empty;
        private List<Route> _Routes = new List<Route>();

        private RouteTable() { }

        public RouteTable(AzureContext azureContext)
        {
            this._AzureContext = azureContext;
        }
        public RouteTable(AzureContext azureContext, IRouteTable source)
        {
            _AzureContext = azureContext;
            _SourceRouteTable = source;
            this.TargetName = source.Name;
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

        public List<Route> Routes
        {
            get { return _Routes; }
        }
    }
}
