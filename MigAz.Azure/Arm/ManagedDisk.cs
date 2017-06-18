using MigAz.Azure.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class ManagedDisk : ArmResource, IArmDisk
    {
        private ManagedDisk() : base(null) { }

        public ManagedDisk(JToken resourceToken) : base(resourceToken)
        {
        }

        public async Task InitializeChildrenAsync(AzureContext azureContext)
        {

        }

        #region Properties

        public string Type
        {
            get { return (string)ResourceToken["type"]; }
        }

        public string DiskSizeGB
        {
            get { return (string)ResourceToken["properties"]["diskSizeGB"]; }
        }

        public string OwnerId
        {
            get { return (string)ResourceToken["properties"]["ownerId"]; }
        }

        public string ProvisioningState
        {
            get { return (string)ResourceToken["properties"]["provisioningState"]; }
        }

        public string DiskState
        {
            get { return (string)ResourceToken["properties"]["diskState"]; }
        }
        public string TimeCreated
        {
            get { return (string)ResourceToken["properties"]["timeCreated"]; }
        }
        public string AccountType
        {
            get { return (string)ResourceToken["properties"]["accountType"]; }
        }
        #endregion

    }
}
