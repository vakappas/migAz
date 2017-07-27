using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MigAz.Core.Interface;

namespace MigAz.Azure.Arm
{
    public class AvailabilitySet : ArmResource, IAvailabilitySetSource
    {
        private List<VirtualMachine> _VirtualMachines = new List<VirtualMachine>();

        public AvailabilitySet(JToken resourceToken) : base(resourceToken)
        {
        }

        public Int32 PlatformUpdateDomainCount => (Int32)ResourceToken["properties"]["platformUpdateDomainCount"];
        public Int32 PlatformFaultDomainCount => (Int32)ResourceToken["properties"]["platformFaultDomainCount"];
        public string SkuName => (string)ResourceToken["sku"]["name"];

        public List<VirtualMachine> VirtualMachines
        {
            get { return _VirtualMachines; }
        }

        public override string ToString()
        {
            return this.Name;
        }
        internal new async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            await base.InitializeChildrenAsync(azureContext);
        }


    }
}
