using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using System.Windows.Forms;
using MigAz.Core.Interface;

namespace MigAz.AWS.Generator
{
    public class AWSRetriever
    {
        private AwsObjectRetriever _awsObjectRetriever;

        private DescribeVolumesResponse getEbsVolumes()
        {
            return _awsObjectRetriever.Volumes;
        }

        private DescribeVpcsResponse getVPCs()
        {
            return _awsObjectRetriever.Vpcs;
        }

        private DescribeInstancesResponse getEC2Instances()
        {
            return _awsObjectRetriever.Instances;
        }


        public void toname(string accessKey, string secretKey, TreeNode subscriptionNode, ILogProvider logProvider, IStatusProvider statusProvider)
        {
            List<Amazon.RegionEndpoint> regionsList = new List<Amazon.RegionEndpoint>();
            foreach (var region in Amazon.RegionEndpoint.EnumerableAllRegions)
            {
                try
                {
                    _awsObjectRetriever = new AwsObjectRetriever(accessKey, secretKey, region, logProvider, statusProvider);
                    //// todo, not needed in this method_templateGenerator = new TemplateGenerator(_logProvider, _statusProvider, _awsObjectRetriever, telemetryProvider);

                    TreeNode amazonRegionNode = new TreeNode(region.DisplayName);
                    amazonRegionNode.Text = region.DisplayName;
                    amazonRegionNode.Tag = region;

                    //DescribeVolumesResponse ebsVolumesResponse;

                    statusProvider.UpdateStatus("BUSY: Getting the VPC details");
                    DescribeVpcsResponse vpcResponse = getVPCs();
                    Application.DoEvents();

                    statusProvider.UpdateStatus("BUSY: Processing VPC");
                    foreach (var vpc in vpcResponse.Vpcs)
                    {
                        MigAz.AWS.MigrationSource.VirtualNetwork sourceVirtualNetwork = new MigrationSource.VirtualNetwork(_awsObjectRetriever, vpc);
                        Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = new Azure.MigrationTarget.VirtualNetwork(sourceVirtualNetwork);

                        TreeNode vpcTreeNode = new TreeNode(sourceVirtualNetwork.Id + " - " + sourceVirtualNetwork.Name);
                        vpcTreeNode.Tag = targetVirtualNetwork;
                        amazonRegionNode.Nodes.Add(vpcTreeNode);

                        Application.DoEvents();
                    }

                    DescribeInstancesResponse instResponse = getEC2Instances();
                    Application.DoEvents();

                    if (instResponse != null)
                    {
                        statusProvider.UpdateStatus("BUSY: Processing Instances");
                        if (instResponse.Reservations.Count > 0)
                        {
                            foreach (var instanceResp in instResponse.Reservations)
                            {
                                foreach (var instance in instanceResp.Instances)
                                {
                                    string name = "";
                                    foreach (var tag in instance.Tags)
                                    {
                                        if (tag.Key == "Name")
                                        {
                                            name = tag.Value;
                                        }
                                    }

                                    TreeNode instanceTreeNode = new TreeNode(instance.InstanceId + " - " + name);
                                    instanceTreeNode.Tag = instance;
                                    amazonRegionNode.Nodes.Add(instanceTreeNode);

                                    Application.DoEvents();
                                }
                            }
                        }
                    }




                    if (amazonRegionNode.Nodes.Count > 0)
                        subscriptionNode.Nodes.Add(amazonRegionNode);
                    else
                        logProvider.WriteLog("Load_Items", "Not adding Amazon Region '" + region.DisplayName + "' to Source Node list, as it contains no resources to export.");
                }
                catch (Exception exc)
                {
                    logProvider.WriteLog("Load_Items", "AWS Exception - " + region.DisplayName + ": " + exc.Message);
                }
            }
        }
    }
}
