using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class PublicIP : ArmResource
    {
        public PublicIP(JToken resourceToken) : base(resourceToken)
        {
        }

        public string IpAddress => (string)this.ResourceToken["properties"]["ipAddress"];
        public string PublicIPAddressVersion => (string)this.ResourceToken["properties"]["publicIPAddressVersion"];
        public string PublicIPAllocationMethod => (string)this.ResourceToken["properties"]["publicIPAllocationMethod"];
        public string IdleTimeoutInMinutes => (string)this.ResourceToken["properties"]["idleTimeoutInMinutes"];
        public string DomainNameLabel
        {
            get
            {
                if (this.ResourceToken["properties"]["dnsSettings"] == null || this.ResourceToken["properties"]["dnsSettings"]["domainNameLabel"] == null)
                    return String.Empty;

                return (string) this.ResourceToken["properties"]["dnsSettings"]["domainNameLabel"];
            }
        }

        public string FQDN
        {
            get
            {
                if (this.ResourceToken["properties"]["dnsSettings"] == null || this.ResourceToken["properties"]["dnsSettings"]["fqdn"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["properties"]["dnsSettings"]["fqdn"];
            }
        }
    }
}
