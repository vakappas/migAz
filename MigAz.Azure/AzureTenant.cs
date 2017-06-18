using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    public class AzureTenant
    {
        private JObject _TenantJson;
        private AzureContext _AzureContext;
        private List<AzureDomain> _Domains;
        private List<AzureSubscription> _Subscriptions;

        internal AzureTenant(JObject tenantsJson, AzureContext azureContext)
        {
            _TenantJson = tenantsJson;
            _AzureContext = azureContext;
        }

        public string Id
        {
            get { return (string)_TenantJson["id"]; }
        }
        public Guid TenantId
        {
            get { return new Guid((string)_TenantJson["tenantId"]); }
        }
        public AzureDomain DefaultDomain
        {
            get
            {
                if (Domains == null)
                    return null;

                foreach (AzureDomain azureDomain in Domains)
                {
                    if (azureDomain.IsDefault)
                        return azureDomain;
                }

                return null;
            }
        }

        public override string ToString()
        {
            if (DefaultDomain == null)
                return TenantId.ToString();
            else
                return DefaultDomain.Name + " (" + TenantId + ")";
        }

        public List<AzureDomain> Domains
        {
            get { return _Domains; }
        }

        public List<AzureSubscription> Subscriptions
        {
            get { return _Subscriptions; }
        }

        public async Task InitializeChildren()
        {
            _Domains = await _AzureContext.AzureRetriever.GetAzureARMDomains(this);
            _Subscriptions = await _AzureContext.AzureRetriever.GetAzureARMSubscriptions(this);
        }

        public static bool operator ==(AzureTenant lhs, AzureTenant rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                    ((object)lhs != null && (object)rhs != null && lhs.TenantId == rhs.TenantId))
            {
                status = true;
            }
            return status;
        }

        public static bool operator !=(AzureTenant lhs, AzureTenant rhs)
        {
            return !(lhs == rhs);
        }
    }
}
