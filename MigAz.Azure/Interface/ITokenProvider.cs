using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MigAz.Azure.Interface
{
    public interface ITokenProvider
    {
        Task<List<AzureSubscription>> GetSubscriptions();
        Task<AuthenticationResult> GetToken(AzureSubscription azureSubscription);
        Task<AuthenticationResult> GetCommonToken(string azureEnvironment);
    }
}
