using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    public class AzureDomain
    {
        private JObject _DomainJson;
        private AzureContext _AzureContext;
        private List<AzureDomain> _Domains;

        public AzureDomain(JObject domainJson, AzureContext azureContext)
        {
            _DomainJson = domainJson;
            _AzureContext = azureContext;
        }

        public string Name
        {
            get { return (string)_DomainJson["name"]; }
        }

        public bool IsDefault
        {
            get { return Convert.ToBoolean(_DomainJson["isDefault"]); }
        }
        public bool IsVerified
        {
            get { return Convert.ToBoolean(_DomainJson["isVerified"]); }
        }
        public bool IsRoot
        {
            get { return Convert.ToBoolean(_DomainJson["isRoot"]); }
        }
        public bool IsInitial
        {
            get { return Convert.ToBoolean(_DomainJson["isInitial"]); }
        }
        public bool IsAdminManaged
        {
            get { return Convert.ToBoolean(_DomainJson["isAdminManaged"]); }
        }

        // Not yet added to properties
          //"authenticationType": "Managed",
          //"availabilityStatus": null,
          //"supportedServices": [
          //  "Email",
          //  "OfficeCommunicationsOnline",
          //  "OrgIdAuthentication"
          //],
          //"forceDeleteState": null,
          //"state": null
    }
}
