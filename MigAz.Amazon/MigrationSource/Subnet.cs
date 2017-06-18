using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;

namespace MigAz.AWS.MigrationSource
{
    public class Subnet : ISubnet
    {
        private Amazon.EC2.Model.Subnet _AmazonSubnet;

        public Subnet() { }

        public Subnet(AwsObjectRetriever awsObjectRetriever, Amazon.EC2.Model.Subnet amazonSubnet)
        {
            this._AmazonSubnet = amazonSubnet;

            //QUES: Single Sec group?
            // add Network Security Group if exists - 2 subnets - each acl is associated with both
            List<Amazon.EC2.Model.NetworkAcl> networkAcls = awsObjectRetriever.getNetworkAcls(this.Id);
            List<Amazon.EC2.Model.RouteTable> routeTable = awsObjectRetriever.getRouteTables(this.Id);


        }

        public string Id => _AmazonSubnet.SubnetId;

        public string Name
        {
            get
            {
                foreach (var tag in _AmazonSubnet.Tags)
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
