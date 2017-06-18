using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class PublicIp : IMigrationTarget
    {
        private String _SourceName = String.Empty;
        private String _Name = String.Empty;
        private String _DomainNameLabel = String.Empty;

        public PublicIp() { }

        public PublicIp(Arm.PublicIP armPublicIP)
        {
            this.SourceName = armPublicIP.Name;
            this.Name = armPublicIP.Name;
            this.DomainNameLabel = armPublicIP.DomainNameLabel;
        }

        public String DomainNameLabel
        {
            get { return _DomainNameLabel; }
            set { _DomainNameLabel = value.ToLower(); }
        }


        public String SourceName
        {
            get { return _SourceName; }
            set { _SourceName = value; }
        }

        public String Name
        {
            get { return _Name; }
            set { _Name = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
