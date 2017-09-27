using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class Location : ILocation
    {
        #region Properties

        private XmlNode _XmlNode;
        private AzureContext _AzureContext;

        #endregion

        #region Constructors

        private Location() { }

        public Location(AzureContext azureContext, XmlNode xmlNode)
        {
            _XmlNode = xmlNode;
            _AzureContext = azureContext;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }

        public string DisplayName
        {
            get { return _XmlNode.SelectSingleNode("DisplayName").InnerText; }
        }

        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }
}

