namespace MigAz.Core.Interface
{
    public interface ISettingsProvider
    {
        bool AllowTelemetry { get; set; }
        bool BuildEmpty { get; set; }
        string StorageAccountSuffix { get; set; }

        string AvailabilitySetSuffix { get; set; }
        string NetworkInterfaceCardSuffix { get; set; }
        string VirtualNetworkSuffix { get; set; }
        string ResourceGroupSuffix { get; set; }
        string VirtualNetworkGatewaySuffix { get; set; }

        string PublicIPSuffix { get; set; }

        string NetworkSecurityGroupSuffix { get; set; }

        string LoadBalancerSuffix { get; set; }

        string VirtualMachineSuffix { get; set; }
    }
}