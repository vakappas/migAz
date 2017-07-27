using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;

namespace MigAz.AWS.MigrationSource
{
    public class VirtualNetwork : IVirtualNetwork
    {
        private Vpc _Source;
        List<ISubnet> _Subnets = new List<ISubnet>();
        private List<DhcpOptions> _DhcpOptions;

        public string Id => _Source.VpcId;

        public ISubnet GatewaySubnet => throw new NotImplementedException();

        private VirtualNetwork() { }
        public VirtualNetwork(AwsObjectRetriever awsObjectRetriever, Vpc sourceVpc)
        {
            _Source = sourceVpc;
            _DhcpOptions = awsObjectRetriever.getDhcpOptions(_Source.DhcpOptionsId);

            foreach (Amazon.EC2.Model.Subnet amazonSubnet in awsObjectRetriever.GetSubnets(this.Id))
            {
                Subnet sourceSubnet = new Subnet(awsObjectRetriever, amazonSubnet);
                _Subnets.Add(sourceSubnet);
            }
        }

        public String Location { get { return "TODO"; } }

        public List<String> AddressPrefixes
        {
            get
            {
                List<String> addressPrefixes = new List<String>();
                addressPrefixes.Add(_Source.CidrBlock);
                return addressPrefixes;
            }
        }

        public List<String> DnsServers
        {
            get
            {
                List<String> dnsServers = new List<String>();
                
                if (_DhcpOptions != null)
                {
                    foreach (var dnsserver in _DhcpOptions)
                    {
                        foreach (var item in dnsserver.DhcpConfigurations)
                        {
                            if ((item.Key == "domain-name-servers") && (item.Values[0] != "AmazonProvidedDNS"))
                            {
                                foreach (var value in item.Values)
                                    dnsServers.Add(value);
                            }
                        }
                    }
                }
                
                return dnsServers;
            }
        }

        public List<ISubnet> Subnets
        {
            get
            {
                return _Subnets;
            }
        }

        public String Name
        { 
            get
            {
                foreach (var tag in _Source.Tags)
                {
                    if (tag.Key == "Name")
                    {
                        return tag.Value;
                    }
                }

                return this.Id;
            }
        }
    }
}







//private void BuildVirtualNetworkObject(Amazon.EC2.Model.Vpc vpc, List<Amazon.EC2.Model.Subnet> subnetNode, List<Amazon.EC2.Model.DhcpOptions> dhcpNode)


////DnsServers
//List<string> dnsservers = new List<string>();
//foreach (var dnsserver in dhcpNode)
//{
//    foreach (var item in dnsserver.DhcpConfigurations)
//    {
//        if ((item.Key == "domain-name-servers") && (item.Values[0] != "AmazonProvidedDNS"))
//        {
//            foreach (var value in item.Values)
//                dnsservers.Add(value);
//        }
//    }
//}



//    //var nodes = networkAcls.SelectSingleNode("DescribeNetworkAclsResponse ").SelectSingleNode("networkAclSet").SelectNodes("item");

//    if (networkAcls.Count > 0)
//    {
//        NetworkSecurityGroup networksecuritygroup = BuildNetworkSecurityGroup(networkAcls[0]);

//        //NetworkSecurityGroup networksecuritygroup = BuildNetworkSecurityGroup(subnet.name);

//        // Add NSG reference to the subnet
//        Reference networksecuritygroup_ref = new Reference();
//        networksecuritygroup_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/networkSecurityGroups/" + networksecuritygroup.name + "')]";

//        properties.networkSecurityGroup = networksecuritygroup_ref;


//    }

//    if (routeTable.Count > 0)
//    {
//        RouteTable routetable = BuildRouteTable(routeTable[0]);

//        if (routetable.properties != null)
//        {
//            // Add Route Table reference to the subnet
//            Reference routetable_ref = new Reference();
//            routetable_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/routeTables/" + routetable.name + "')]";

//            properties.routeTable = routetable_ref;


//        }
//    }

//}