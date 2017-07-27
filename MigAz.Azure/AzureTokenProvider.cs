using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure
{
    public class AzureTokenProvider : ITokenProvider
    {
        private const string strReturnUrl = "urn:ietf:wg:oauth:2.0:oob";
        private const string strClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        private AuthenticationResult _AuthenticationResult = null;
        private ILogProvider _LogProvider;
        private AzureContext _AzureContext;

        private AzureTokenProvider() { }

        internal AzureTokenProvider(AzureContext azureContext)
        {
            _AzureContext = azureContext;
            _LogProvider = azureContext.LogProvider;
        }

        public AuthenticationResult AuthenticationResult
        {
            get { return _AuthenticationResult; }
            set { _AuthenticationResult = value; }
        }

        public string AccessToken
        {
            get
            {
                if (this.AuthenticationResult == null)
                    return String.Empty;

                return this.AuthenticationResult.AccessToken;
            }
        }

        public async Task<AuthenticationResult> GetToken(AzureSubscription azureSubscription)
        {
            _LogProvider.WriteLog("GetToken", "Start token request");

            if (azureSubscription == null)
            {
                _LogProvider.WriteLog("GetToken", "Azure Subscription cannot be null.");
                throw new ArgumentNullException("Azure Subscription cannot be null.");
            }

            _LogProvider.WriteLog("GetToken", "Azure Subscription: " + azureSubscription.ToString());

            string authenticationUrl = _AzureContext.AzureServiceUrls.GetAzureLoginUrl() + azureSubscription.AzureAdTenantId.ToString();
            _LogProvider.WriteLog("GetToken", "Authentication Url: " + authenticationUrl);

            _AuthenticationResult = null;
            AuthenticationContext context = new AuthenticationContext(authenticationUrl);

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Auto, null);
            _AuthenticationResult = await context.AcquireTokenAsync(_AzureContext.AzureServiceUrls.GetASMServiceManagementUrl(), strClientId, new Uri(strReturnUrl), platformParams);

            _LogProvider.WriteLog("GetToken", "End token request");

            return _AuthenticationResult;
        }

        public async Task<AuthenticationResult> LoginAzureProvider()
        {
            _LogProvider.WriteLog("LoginAzureProvider", "Start token request");
            _LogProvider.WriteLog("LoginAzureProvider", "Azure Environment: " + _AzureContext.AzureEnvironment.ToString());

            string authenticationUrl = _AzureContext.AzureServiceUrls.GetAzureLoginUrl() + "common";
            _LogProvider.WriteLog("LoginAzureProvider", "Authentication Url: " + authenticationUrl);

            AuthenticationContext context = new AuthenticationContext(authenticationUrl);

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Always, null);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(_AzureContext.AzureServiceUrls.GetASMServiceManagementUrl(), strClientId, new Uri(strReturnUrl), platformParams);
            if (authenticationResult == null)
            {
                _LogProvider.WriteLog("LoginAzureProvider", "Failed to obtain the token (null AuthenticationResult returned).");
            }

            _AuthenticationResult = authenticationResult;

            _LogProvider.WriteLog("LoginAzureProvider", "End token request for Azure Environment " + _AzureContext.AzureEnvironment.ToString());

            return _AuthenticationResult;
        }

        public async Task<AuthenticationResult> GetGraphToken(string tenantId)
        {
            _LogProvider.WriteLog("GetGraphToken", "Start token request");
            _LogProvider.WriteLog("GetGraphToken", "Azure Environment: " + _AzureContext.AzureEnvironment.ToString());
            _LogProvider.WriteLog("GetGraphToken", "Azure Tenant: " + tenantId);

            string authenticationUrl = _AzureContext.AzureServiceUrls.GetAzureLoginUrl() + tenantId;
            _LogProvider.WriteLog("GetGraphToken", "Authentication Url: " + authenticationUrl);

            AuthenticationContext context = new AuthenticationContext(authenticationUrl);

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Auto, null);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(_AzureContext.AzureServiceUrls.GetGraphApiUrl(), strClientId, new Uri(strReturnUrl), platformParams);
            if (authenticationResult == null)
            {
                _LogProvider.WriteLog("GetGraphToken", "Failed to obtain the token (null AuthenticationResult returned).");
            }

            _LogProvider.WriteLog("GetGraphToken", "End token request");

            return authenticationResult;
        }

        public async Task<AuthenticationResult> GetAzureToken(string tenantId)
        {
            _LogProvider.WriteLog("GetAzureToken", "Start token request");
            _LogProvider.WriteLog("GetAzureToken", "Azure Environment: " + _AzureContext.AzureEnvironment.ToString());
            _LogProvider.WriteLog("GetAzureToken", "Azure Tenant: " + tenantId);

            string authenticationUrl = _AzureContext.AzureServiceUrls.GetAzureLoginUrl() + tenantId;
            _LogProvider.WriteLog("GetAzureToken", "Authentication Url: " + authenticationUrl);

            AuthenticationContext context = new AuthenticationContext(authenticationUrl);

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Auto, null);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(_AzureContext.AzureServiceUrls.GetARMServiceManagementUrl(), strClientId, new Uri(strReturnUrl), platformParams);
            if (authenticationResult == null)
            {
                _LogProvider.WriteLog("GetAzureToken", "Failed to obtain the token (null AuthenticationResult returned).");
            }

            _LogProvider.WriteLog("GetAzureToken", "End token request for Azure Environment " + _AzureContext.AzureEnvironment.ToString() + " Tenant Id " + tenantId);

            return authenticationResult;
        }

        public static bool operator ==(AzureTokenProvider lhs, AzureTokenProvider rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                ((object)lhs != null && (object)rhs != null && lhs.AuthenticationResult == null && rhs.AuthenticationResult == null) ||
                    ((object)lhs != null && (object)rhs != null && lhs.AuthenticationResult == rhs.AuthenticationResult))
            {
                status = true;
            }
            return status;
        }

        public static bool operator !=(AzureTokenProvider lhs, AzureTokenProvider rhs)
        {
            return !(lhs == rhs);
        }
    }
}
