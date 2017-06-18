using MigAz.Core.Interface;
using System.Collections.Generic;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class RouteTable : IRouteTable
    {
        private List<Route> _Routes;
        private AzureContext _AzureContext;
        private XmlNode _XmlNode;

        public RouteTable(AzureContext azureContext, XmlNode routeTableNode)
        {
            this._AzureContext = azureContext;
            this._XmlNode = routeTableNode;

            _Routes = new List<Route>();
            foreach (XmlNode routeNode in _XmlNode.SelectNodes("//RouteList/Route"))
            {
                _Routes.Add(new Route(this._AzureContext, routeNode));
            }
        }

        #region Properties

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("//Name").InnerText; }
        }

        public string Location
        {
            get
            {
                return _XmlNode.SelectSingleNode("//Location").InnerText;
            }
        }

        public List<Route> Routes
        {
            get { return _Routes; }
        }

        #endregion

    }
}