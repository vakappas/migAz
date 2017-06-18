using MigAz.Core.Interface;

namespace MigAz.Providers
{
    public class AppSettingsProvider : ISettingsProvider
    {
        public bool AllowTelemetry
        {
            get
            {
                return app.Default.AllowTelemetry;
            }

            set
            {
                app.Default.AllowTelemetry = value;
                app.Default.Save();
            }
        }

        public bool BuildEmpty
        {
            get
            {
                return app.Default.BuildEmpty;
            }

            set
            {
                app.Default.BuildEmpty = value;
                app.Default.Save();
            }
        }

        public string StorageAccountSuffix
        {
            get
            {
                return app.Default.StorageAccountSuffix;
            }

            set
            {
                app.Default.StorageAccountSuffix = value;
                app.Default.Save();
            }
        }
        public string AvailabilitySetSuffix
        {
            get
            {
                return app.Default.AvailabilitySetSuffix;
            }

            set
            {
                app.Default.AvailabilitySetSuffix = value;
                app.Default.Save();
            }
        }

        public string NetworkInterfaceCardSuffix
        {
            get
            {
                return app.Default.NetworkInterfaceCardSuffix;
            }

            set
            {
                app.Default.NetworkInterfaceCardSuffix = value;
                app.Default.Save();
            }
        }

        public string VirtualNetworkSuffix
        {
            get { return app.Default.VirtualNetworkSuffix; }
            set
            {
                app.Default.VirtualNetworkSuffix = value;
                app.Default.Save();
            }
        }
        public string ResourceGroupSuffix
        {
            get { return app.Default.ResourceGroupSuffix; }
            set
            {
                app.Default.ResourceGroupSuffix = value;
                app.Default.Save();
            }
        }

        public string VirtualNetworkGatewaySuffix
        {
            get { return app.Default.VirtualNetworkGatewaySuffix; }
            set
            {
                app.Default.VirtualNetworkGatewaySuffix = value;
                app.Default.Save();
            }
        }
        public string PublicIPSuffix
        {
            get { return app.Default.PublicIPSuffix; }
            set
            {
                app.Default.PublicIPSuffix = value;
                app.Default.Save();
            }
        }

        public string NetworkSecurityGroupSuffix
        {
            get { return app.Default.NetworkSecurityGroupSuffix; }
            set
            {
                app.Default.NetworkSecurityGroupSuffix = value;
                app.Default.Save();
            }
        }

        public string LoadBalancerSuffix
        {
            get { return app.Default.LoadBalancerSuffix; }
            set
            {
                app.Default.LoadBalancerSuffix = value;
                app.Default.Save();
            }
        }

        public string VirtualMachineSuffix
        {
            get { return app.Default.VirtualMachineSuffix; }
            set
            {
                app.Default.VirtualMachineSuffix = value;
                app.Default.Save();
            }
        }
    }
}
