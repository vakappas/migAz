using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    public class AzureRestResponse
    {

        private AzureRestRequest _AzureRestRequest;

        public Guid RequestGuid { get; private set; }
        public String Url { get; private set; }
        public String AccessToken { get; private set; }
        public String Response { get; private set; }

        private AzureRestResponse() { }

        public AzureRestResponse(AzureRestRequest azureRestRequest, string response)
        {
            this._AzureRestRequest = azureRestRequest;

            if (this._AzureRestRequest != null)
            {
                this.RequestGuid = _AzureRestRequest.RequestGuid;
                this.Url = _AzureRestRequest.Url;
                this.AccessToken = _AzureRestRequest.AccessToken;
            }

            this.Response = response;
        }

    }
}
