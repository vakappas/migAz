using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Asm;
using MigAz.Azure.Interface;
using MigAz.Azure;

namespace MigAz.Tests.Fakes
{
    class FakeTokenProvider : ITokenProvider
    {
        public async Task<AuthenticationResult> GetToken(AzureSubscription azureSubscription)
        {
            return null;
        }

        public async Task<AuthenticationResult> GetCommonToken(string azureEnvironment)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AzureSubscription>> GetSubscriptions()
        {
            return new List<AzureSubscription>();
        }
    }
}
