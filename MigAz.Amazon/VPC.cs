using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.AWS
{
    public class VPC
    {
        private string _vpcId, _vpcName;
        public VPC(string vpcId)
        {
            _vpcId = vpcId;
        }

        public VPC(string vpcId, string vpcName)
        {
            _vpcId = vpcId;
            _vpcName = vpcName;
        }
        public string VpcId
        {
            get
            {
                return _vpcId;
            }
            private set
            {
                _vpcId = value;
            }
                
        }
        public string VpcName
        {
            get
            {
                return _vpcName;
            }
            private set
            {
                _vpcName = value;
            }
        }
    }
}
