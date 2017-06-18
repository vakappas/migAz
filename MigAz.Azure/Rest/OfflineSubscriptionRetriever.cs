using MigAz.Azure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Rest
{
    public class OfflineSubscriptionRetriever : AzureSubscriptionRetriever
    {
        private AzureContext _AzureContext;

        private OfflineSubscriptionRetriever() { }

        public OfflineSubscriptionRetriever(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }
    }
}
