using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.ElasticLoadBalancing;
using Amazon.ElasticLoadBalancing.Model;
using MigAz.Core.Interface;
using System.Windows.Forms;

namespace MigAz.AWS
{
    public class AwsObjectRetriever
    {
        #region Vartiables

        public string REGION = null;
        ILogProvider _logProvider;
        IStatusProvider _statusProvider;
        static IAmazonEC2 _service;
        static IAmazonElasticLoadBalancing _LBservice;
        DescribeVpcsResponse _vpcs;
        DescribeInstancesResponse _instances;
        DescribeVolumesResponse _volumes;
        DescribeLoadBalancersResponse _Loadbalancers;
        //public Dictionary<string, XmlDocument> _documentCache = new Dictionary<string, XmlDocument>();

        #endregion

        #region Constructors

        private AwsObjectRetriever() { }

        public AwsObjectRetriever(string accessKeyID, string secretKeyID, Amazon.RegionEndpoint region, ILogProvider logProvider, IStatusProvider statusProvider)
        {
            _logProvider = logProvider;
            _statusProvider = statusProvider;

            createEC2Client(accessKeyID, secretKeyID, region);
            createLBClient(accessKeyID, secretKeyID, region);
            REGION = region.SystemName;

            _vpcs = GetAllVpcs();
            _instances = GetAllInstances();
            _volumes = GetAllVolumes();
            Application.DoEvents();
            _Loadbalancers = GetAllLBs();
        }

        #endregion

        #region Properties

        public DescribeVpcsResponse Vpcs
        {
            get
            {
                return _vpcs;
            }
            set
            {
                if (_vpcs == null)
                {
                    _vpcs = GetAllVpcs();
                }
            }
        }
        public DescribeInstancesResponse Instances
        {
            get
            {
                return _instances;
            }
            set
            {
                if (_instances == null)
                {
                    _instances = GetAllInstances();
                }
            }
        }
        public DescribeVolumesResponse Volumes
        {
            get
            {
                return _volumes;
            }
            set
            {
                if (_volumes == null)
                {
                    _volumes = GetAllVolumes();
                }
            }
        }


        public static IAmazonElasticLoadBalancing createLBClient(string accessKeyID, string secretKeyID, Amazon.RegionEndpoint region)
        {
            try
            {
                _LBservice = new AmazonElasticLoadBalancingClient(accessKeyID, secretKeyID, new AmazonElasticLoadBalancingConfig()
                {
                    RegionEndpoint = region
                });
                return _LBservice;
            }
            catch (AmazonElasticLoadBalancingException ex)
            {
                throw ex;
            }
        }

        public static IAmazonEC2 createEC2Client(string accessKeyID, string secretKeyID, Amazon.RegionEndpoint region)
        {
            try
            {
                _service = new AmazonEC2Client(accessKeyID, secretKeyID, new AmazonEC2Config()
                {
                    RegionEndpoint = region
                });
                return _service;
            }
            catch (AmazonEC2Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Methods

        internal List<Amazon.EC2.Model.Subnet> GetSubnets(string vpcId)
        {
            DescribeSubnetsRequest req = new DescribeSubnetsRequest();
            req.Filters.Add(new Filter()
            {
                Name = "vpc-id",
                Values = new List<string>()
                {
                    vpcId
                }

            });
             return _service.DescribeSubnets(req).Subnets;
        }

        internal List<DhcpOptions> getDhcpOptions(string dhcpOptionsId)
        {
            DescribeDhcpOptionsRequest req = new DescribeDhcpOptionsRequest();
            req.DhcpOptionsIds = new List<string>() { dhcpOptionsId };
            return _service.DescribeDhcpOptions(req).DhcpOptions;
        }

        public DescribeVpcsResponse GetAllVpcs()
        {
            _statusProvider.UpdateStatus("BUSY: Getting all the VPCs...");
            
            return _service.DescribeVpcs();
        }

        public DescribeVolumesResponse GetAllVolumes()
        {
            var x = _service.DescribeVolumes();
            return _service.DescribeVolumes();
        }

        public DescribeLoadBalancersResponse GetAllLBs()
        {
            _statusProvider.UpdateStatus("BUSY: Getting Loadbalancer details...");
            return _LBservice.DescribeLoadBalancers();
        }

        public List<Amazon.EC2.Model.Volume> GetVolume(string volumeId)
        {
            DescribeVolumesRequest req = new DescribeVolumesRequest();
            req.VolumeIds = new List<string>() { volumeId };
            return _service.DescribeVolumes(req).Volumes;
        }
        
        public DescribeInstancesResponse GetAllInstances()
        {
            _statusProvider.UpdateStatus("BUSY: Getting all the Instance details...");
            return _service.DescribeInstances();
        }

        internal List<NetworkAcl> getNetworkAcls(string subnetId)
        {
            _statusProvider.UpdateStatus("BUSY: Getting Network ACL for the Subnet " + subnetId + "...");
            DescribeNetworkAclsRequest req = new DescribeNetworkAclsRequest();
            req.Filters.Add(new Filter()
            {
                Name = "association.subnet-id",
                Values = new List<string>()
                {
                    subnetId
                }

            }); return _service.DescribeNetworkAcls(req).NetworkAcls;

        }

        public SecurityGroup getSecurityGroup(string groupid)
        {
            _statusProvider.UpdateStatus("BUSY: Getting Security Group " + groupid + "...");
            DescribeSecurityGroupsRequest req = new DescribeSecurityGroupsRequest();
            req.Filters.Add(new Filter()
            {
                Name = "group-id",
                Values = new List<string>()
                {
                    groupid
                }

            }); return _service.DescribeSecurityGroups(req).SecurityGroups[0];
        }

        internal List<RouteTable> getRouteTables(string subnetId)
        {
            _statusProvider.UpdateStatus("BUSY: Getting Route Table for the Subnet " + subnetId + "...");
            DescribeRouteTablesRequest req = new DescribeRouteTablesRequest();
            req.Filters.Add(new Filter()
            {
                Name = "association.subnet-id",
                Values = new List<string>()
                {
                    subnetId
                }

            }); return _service.DescribeRouteTables(req).RouteTables;

        }

        internal List<Vpc> getVPCbyId(string VpcId)
        {
      
            DescribeVpcsRequest req = new DescribeVpcsRequest();
            req.Filters.Add(new Filter()
            {
                Name = "vpc-id",
                Values = new List<string>()
                {
                    VpcId
                }

            }); return _service.DescribeVpcs(req).Vpcs;

        }

        internal Reservation getInstancebyId(string InstanceId)
        {

            DescribeInstancesRequest req = new DescribeInstancesRequest();
            req.Filters.Add(new Filter()
            {
                Name = "instance-id",
                Values = new List<string>()
                {
                    InstanceId
                }

            }); return _service.DescribeInstances(req).Reservations[0];

        }

        internal List<Subnet> getSubnetbyId(string SubnetId)
        {
            DescribeSubnetsRequest req = new DescribeSubnetsRequest();
            req.Filters.Add(new Filter()
            {
                Name = "subnet-id",
                Values = new List<string>()
                {
                    SubnetId
                }

            }); return _service.DescribeSubnets(req).Subnets;

        }


        internal List<NetworkInterface> getNICbyId(string NICId)
        {
            DescribeNetworkInterfacesRequest req = new DescribeNetworkInterfacesRequest();
            req.Filters.Add(new Filter()
            {
                Name = "network-interface-id",
                Values = new List<string>()
                {
                    NICId
                }

            }); return _service.DescribeNetworkInterfaces(req).NetworkInterfaces;

        }

        internal List<InternetGateway> getInternetGW(string GWId)
        {
            DescribeInternetGatewaysRequest req = new DescribeInternetGatewaysRequest();
            req.Filters.Add(new Filter()
            {
                Name = "internet-gateway-id",
                Values = new List<string>()
                {
                    GWId
                }

            }); return _service.DescribeInternetGateways(req).InternetGateways;

        }


        #endregion

    }
}
