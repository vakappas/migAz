using System;
using System.Collections.Generic;

namespace MigAz.Azure.Models
{
    public class ArmToArmTelemetryRecord
    {
        public Guid ExecutionId;
        public string TenantId;
        public System.Guid SubscriptionId;
        public Dictionary<string, string> ProcessedResources;
        public string OfferCategories;
        public string SourceVersion;
    }
}
