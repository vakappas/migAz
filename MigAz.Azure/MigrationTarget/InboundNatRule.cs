using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class InboundNatRule
    {
        private String _Name = String.Empty;
        private Int32 _FrontEndPort = 0;
        private Int32 _BackEndPort = 0;
        private String _Protocol = "tcp";
        private LoadBalancer _ParentLoadBalancer = null;

        private InboundNatRule() { }

        public InboundNatRule(LoadBalancer loadBalancer)
        {
            _ParentLoadBalancer = loadBalancer;
            loadBalancer.InboundNatRules.Add(this);
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }
        public String Name
        {
            get { return _Name; }
            set { _Name = value.Trim().Replace(" ", String.Empty); }
        }

        public Int32 FrontEndPort
        {
            get { return _FrontEndPort; }
            set { _FrontEndPort = value; }
        }

        public Int32 BackEndPort
        {
            get { return _BackEndPort; }
            set { _BackEndPort = value; }
        }

        public String Protocol
        {
            get { return _Protocol; }
            set { _Protocol = value; }
        }

        public FrontEndIpConfiguration FrontEndIpConfiguration
        {
            get;set;
        }
    }
}
