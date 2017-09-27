using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VMSize
    {
        private JToken _VMSizeToken;

        public VMSize(JToken resourceToken)
        {
            _VMSizeToken = resourceToken;
        }

        public string Name => (string)_VMSizeToken["name"];
        public Int32 NumberOfCores => (Int32)_VMSizeToken["numberOfCores"];
        public Int32 osDiskSizeInMB => (Int32)_VMSizeToken["osDiskSizeInMB"];
        public Int32 resourceDiskSizeInMB => (Int32)_VMSizeToken["resourceDiskSizeInMB"];
        public Int32 memoryInMB => (Int32)_VMSizeToken["memoryInMB"];
        public Int32 maxDataDiskCount => (Int32)_VMSizeToken["maxDataDiskCount"];

        public override string ToString()
        {
            return this.Name;
        }

    }
}
