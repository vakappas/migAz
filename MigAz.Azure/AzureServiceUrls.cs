using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    public class AzureServiceUrls
    {
        private AzureContext _AzureContext;

        private AzureServiceUrls() { }

        public AzureServiceUrls(AzureContext azureContext)
        {
            if (azureContext == null)
                throw new ArgumentException("Invalid Azure Context.");

            _AzureContext = azureContext;
        }

        private Dictionary<string, string[]> _serviceUrls = new Dictionary<string, string[]>
        {
            {  "AzureCloud", new[] { "https://management.core.windows.net/", "https://management.azure.com/", "https://login.microsoftonline.com/", "blob.core.windows.net", "https://graph.windows.net/" } },
            // China Endpoints - https://msdn.microsoft.com/en-us/library/azure/dn578439.aspx
            {  "AzureChinaCloud", new[] { "https://management.core.chinacloudapi.cn/", "https://management.chinacloudapi.cn/", "https://login.chinacloudapi.cn/", "blob.core.chinacloudapi.cn", "https://graph.chinacloudapi.cn/" } },
            // Germany Endpoints - https://blogs.msdn.microsoft.com/azuregermany/2016/08/18/endpoints-in-microsoft-cloud-germany/
            {  "AzureGermanCloud", new[] { "https://management.core.cloudapi.de/", "https://management.microsoftazure.de/",  "https://login.microsoftonline.de/", "blob.core.cloudapi.de", "https://graph.windows.net/" } },
            // US Gov Endpoints - https://docs.microsoft.com/en-us/azure/azure-government/documentation-government-developer-guide
            {  "AzureUSGovernment", new[] { "https://management.core.usgovcloudapi.net/", "https://management.usgovcloudapi.net/", "https://login-us.microsoftonline.com/", "blob.core.usgovcloudapi.net", "https://graph.windows.net/" } },
        };

        public string GetASMServiceManagementUrl()
        {
            if (_AzureContext != null && _AzureContext.LogProvider != null)
                _AzureContext.LogProvider.WriteLog("AzureServiceUrls", "GetASMServiceManagementUrl " + _AzureContext.AzureEnvironment.ToString() + " " + _serviceUrls[_AzureContext.AzureEnvironment.ToString()][0]);

            return _serviceUrls[_AzureContext.AzureEnvironment.ToString()][0];
        }

        public string GetARMServiceManagementUrl()
        {
            if (_AzureContext != null && _AzureContext.LogProvider != null)
                _AzureContext.LogProvider.WriteLog("AzureServiceUrls", "GetARMServiceManagementUrl " + _AzureContext.AzureEnvironment.ToString() + " " + _serviceUrls[_AzureContext.AzureEnvironment.ToString()][1]);

            return _serviceUrls[_AzureContext.AzureEnvironment.ToString()][1];
        }

        public string GetAzureLoginUrl()
        {
            if (_AzureContext != null && _AzureContext.LogProvider != null)
                _AzureContext.LogProvider.WriteLog("AzureServiceUrls", "GetAzureLoginUrl " + _AzureContext.AzureEnvironment.ToString() + " " + _serviceUrls[_AzureContext.AzureEnvironment.ToString()][2]);

            return _serviceUrls[_AzureContext.AzureEnvironment.ToString()][2];
        }

        public string GetBlobEndpointUrl()
        {
            if (_AzureContext != null && _AzureContext.LogProvider != null)
                _AzureContext.LogProvider.WriteLog("AzureServiceUrls", "GetBlobEndpointUrl " + _AzureContext.AzureEnvironment.ToString() + " " + _serviceUrls[_AzureContext.AzureEnvironment.ToString()][3]);

            return _serviceUrls[_AzureContext.AzureEnvironment.ToString()][3];
        }

        public string GetGraphApiUrl()
        {
            if (_AzureContext != null && _AzureContext.LogProvider != null)
                _AzureContext.LogProvider.WriteLog("AzureServiceUrls", "GetGraphApiUrl " + _AzureContext.AzureEnvironment.ToString() + " " + _serviceUrls[_AzureContext.AzureEnvironment.ToString()][4]);

            return _serviceUrls[_AzureContext.AzureEnvironment.ToString()][4];
        }
    }
}
