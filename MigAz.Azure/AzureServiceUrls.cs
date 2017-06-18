using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    static class AzureServiceUrls
    {
        private static Dictionary<string, string[]> _serviceUrls = new Dictionary<string, string[]>
        {
            {  "AzureCloud", new[] { "https://management.core.windows.net/", "https://management.azure.com/", "https://login.microsoftonline.com/", "blob.core.windows.net", "https://graph.windows.net/" } },
            // China Endpoints - https://msdn.microsoft.com/en-us/library/azure/dn578439.aspx
            {  "AzureChinaCloud", new[] { "https://management.core.chinacloudapi.cn/", "https://management.chinacloudapi.cn/", "https://login.chinacloudapi.cn/", "blob.core.chinacloudapi.cn", "https://graph.windows.net/" } },
            // Germany Endpoints - https://blogs.msdn.microsoft.com/azuregermany/2016/08/18/endpoints-in-microsoft-cloud-germany/
            {  "AzureGermanCloud", new[] { "https://management.core.cloudapi.de/", "https://management.microsoftazure.de/",  "https://login.microsoftonline.de/", "blob.core.cloudapi.de", "https://graph.windows.net/" } },
            // US Gov Endpoints - https://docs.microsoft.com/en-us/azure/azure-government/documentation-government-developer-guide
            {  "AzureUSGovernment", new[] { "https://management.core.usgovcloudapi.net/", "https://management.usgovcloudapi.net/", "https://login-us.microsoftonline.com/", "blob.core.usgovcloudapi.net", "https://graph.windows.net/" } },
        };

        public static string GetASMServiceManagementUrl(AzureEnvironment azureEnvironment)
        {
            return _serviceUrls[azureEnvironment.ToString()][0];
        }

        public static string GetARMServiceManagementUrl(AzureEnvironment azureEnvironment)
        {
            return _serviceUrls[azureEnvironment.ToString()][1];
        }

        public static string GetAzureLoginUrl(AzureEnvironment azureEnvironment)
        {
            return _serviceUrls[azureEnvironment.ToString()][2];
        }

        public static string GetBlobEndpointUrl(AzureEnvironment azureEnvironment)
        {
            return _serviceUrls[azureEnvironment.ToString()][3];
        }

        public static string GetGraphApiUrl(AzureEnvironment azureEnvironment)
        {
            return _serviceUrls[azureEnvironment.ToString()][4];
        }
    }
}
