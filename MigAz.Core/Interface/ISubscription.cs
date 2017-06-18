using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core.Interface
{
    public enum AzureEnvironment
    {
        AzureCloud,
        AzureChinaCloud,
        AzureGermanCloud,
        AzureUSGovernment
    }

    public interface ISubscription
    {
            Guid SubscriptionId { get; }
            Guid AzureAdTenantId { get; }
            string offercategories { get; }
            AzureEnvironment AzureEnvironment { get; }
    }
}