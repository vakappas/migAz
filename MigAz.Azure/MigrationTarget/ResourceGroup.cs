using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class ResourceGroup : IMigrationTarget
    {
        private String _TargetName = String.Empty;
        private ILocation _TargetLocation;

        public ResourceGroup()
        {
            _TargetName = "NewResourceGroup";
        }

        public String SourceName
        {
            get { return String.Empty; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public ILocation TargetLocation
        {
            get { return _TargetLocation; }
            set { _TargetLocation = value; }
        }

        public override string ToString()
        {
            return this.TargetName;
        }
    }
}
