using System.Collections.Generic;

namespace MigAz.Core.Interface
{
    public interface IVirtualNetwork
    {
        string Id { get; }
        List<ISubnet> Subnets { get; }
        ISubnet GatewaySubnet { get; }
        List<string> AddressPrefixes { get; }
        List<string> DnsServers { get; }
        string Name { get; }
        string Location { get; }
    }
}
