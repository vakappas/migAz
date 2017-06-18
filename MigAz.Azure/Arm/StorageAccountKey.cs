using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class StorageAccountKey
    {
        private JToken _Key;
        private List<Subnet> _Subnets = new List<Subnet>();

        private StorageAccountKey() { }

        public StorageAccountKey(JToken key)
        {
            _Key = key;
        }

        public string Name
        {
            get { return (string)_Key["keyName"]; }
        }

        public string Permissions
        {
            get { return (string)_Key["permissions"]; }
        }

        public string Value
        {
            get { return (string)_Key["value"]; }
        }


    }
}
