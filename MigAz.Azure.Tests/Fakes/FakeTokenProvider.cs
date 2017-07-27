using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Asm;
using MigAz.Azure.Interface;
using MigAz.Azure;
using MigAz.Core.Interface;

namespace MigAz.Tests.Fakes
{
    class FakeTokenProvider : ITokenProvider
    {
        public AuthenticationResult AuthenticationResult
        {
            get { return null; }
            set => throw new NotImplementedException();
        }

        public string AccessToken => "FakeTokenValue";

        public async Task<AuthenticationResult> GetToken(AzureSubscription azureSubscription)
        {
            return null;
        }

        public async Task<AuthenticationResult> GetCommonToken(string azureEnvironment)
        {
            return null;
        }

        public async Task<List<AzureSubscription>> GetSubscriptions()
        {
            return new List<AzureSubscription>();
        }

        public Task<AuthenticationResult> LoginAzureProvider()
        {
            return null;
        }

        public Task<AuthenticationResult> GetGraphToken(string v)
        {
            return null;
        }

        public Task<AuthenticationResult> GetAzureToken(string v)
        {
            return null;
        }
    }
}
