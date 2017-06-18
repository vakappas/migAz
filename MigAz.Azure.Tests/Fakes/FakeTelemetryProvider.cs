using MigAz.Azure.Generator.AsmToArm;
using MigAz.Core.Interface;

namespace MigAz.Tests.Fakes
{
    class FakeTelemetryProvider : ITelemetryProvider
    {
        public void PostTelemetryRecord(AzureGenerator templateResult)
        {
        }
    }
}
