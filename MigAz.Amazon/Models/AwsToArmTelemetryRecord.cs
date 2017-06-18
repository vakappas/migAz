using System;
using System.Collections.Generic;

namespace MigAz.AWS.Models
{
    public class AwsToArmTelemetryRecord
    {
        public Guid ExecutionId;
        public string AccessKeyId;
        public Dictionary<string, string> ProcessedResources;
        public string AWSRegion;
        public string SourceVersion;

    }
}
