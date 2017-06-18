using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class AvailabilitySet : IMigrationTarget
    {
        private AzureContext _AzureContext;
        private IAvailabilitySetSource _SourceAvailabilitySet;
        private string _TargetName = String.Empty;

        private AvailabilitySet() { }

        public AvailabilitySet(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        public AvailabilitySet(AzureContext azureContext, String targetName)
        {
            _AzureContext = azureContext;
            this.TargetName = targetName;
        }

        public AvailabilitySet(AzureContext azureContext, Asm.CloudService asmCloudService)
        {
            _AzureContext = azureContext;
            _SourceAvailabilitySet = asmCloudService;
            this.TargetName = _SourceAvailabilitySet.Name;
        }

        public AvailabilitySet(AzureContext azureContext, Arm.AvailabilitySet availabilitySet)
        {
            _AzureContext = azureContext;
            _SourceAvailabilitySet = availabilitySet;

            this.TargetName = _SourceAvailabilitySet.Name;
        }

        public IAvailabilitySetSource SourceAvailabilitySet
        {
            get { return _SourceAvailabilitySet; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceAvailabilitySet == null)
                    return String.Empty;
                else
                    return this.SourceAvailabilitySet.ToString();
            }
        }
        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.TargetName + this._AzureContext.SettingsProvider.AvailabilitySetSuffix;
        }
    }
}
