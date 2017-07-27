using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;
using System.Threading.Tasks;
using MigAz.Core.Interface;

namespace MigAz.Azure.Interface
{
    public interface ITokenProvider
    {
        AuthenticationResult AuthenticationResult { get; set; }
        string AccessToken { get; }

        // Task<List<AzureSubscription>> GetSubscriptions();
        Task<AuthenticationResult> GetToken(AzureSubscription azureSubscription);
        //Task<AuthenticationResult> GetCommonToken(string azureEnvironment);
        Task<AuthenticationResult> LoginAzureProvider(AzureEnvironment azureEnvironment);
        Task<AuthenticationResult> GetGraphToken(AzureEnvironment azureEnvironment, string v);
        Task<AuthenticationResult> GetAzureToken(AzureEnvironment azureEnvironment, string v);
    }
}
