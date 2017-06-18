using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class AffinityGroup
    {
        #region Variables

        private AzureContext _AzureContext = null;
        private XmlNode _XmlNode = null;

        #endregion

        #region Constructors

        private AffinityGroup() { }

        internal AffinityGroup(AzureContext azureContext, XmlNode xmlNode)
        {
            _AzureContext = azureContext;
            _XmlNode = xmlNode;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }

        public string Location
        {
            get
            {
                return _XmlNode.SelectSingleNode("Location").InnerText;
            }
        }

      
        #endregion
    }
}
