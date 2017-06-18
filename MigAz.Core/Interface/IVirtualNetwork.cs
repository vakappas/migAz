using System.Collections.Generic;

namespace MigAz.Core.Interface
{
    public interface IVirtualNetwork
    {
        string Id { get; }
        List<ISubnet> Subnets { get; }
        ISubnet GatewaySubnet { get; }
        List<string> AddressPrefixes { get; }
        string Name { get; }
    }
}
