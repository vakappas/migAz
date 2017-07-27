using MigAz.Core.Interface;
using MigAz.Core.Generator;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using MigAz.Core.ArmTemplate;
using System.Collections;
using System.Net;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using MigAz.Azure.Generator;

namespace MigAz.AWS.Generator
{
    public class AwsGenerator : TemplateGenerator
    {
        //private Dictionary<string, Parameter> this.Parameters;
        private Dictionary<string, string> _storageAccountNames;
        private ISettingsProvider _settingsProvider;

        private AwsGenerator() : base(null, null, null, null, null, null) { }

        public AwsGenerator(ILogProvider logProvider, IStatusProvider statusProvider) : base(logProvider, statusProvider, null, null, null, null)
        {
        }

        //public  async Task UpdateArtifacts2(IExportArtifacts artifacts)
        //{
        //    LogProvider.WriteLog("UpdateArtifacts", "Start - Execution " + this.ExecutionGuid.ToString());

        //    //Alerts.Clear();
        //    //TemplateStreams.Clear();
        //    //Resources.Clear();

        //    //_AWSExportArtifacts = (AWSExportArtifacts)artifacts;

        //    ////_storageAccountNames = new Dictionary<string, string>();
        //    ////var token = _tokenProvider.GetToken(tenantId);

        //    //LogProvider.WriteLog("UpdateArtifacts", "Start processing selected virtual networks");

        //    //// process selected virtual networks
        //    //foreach (var virtualnetworkname in _AWSExportArtifacts.VirtualNetworks)
        //    //{
        //    //    StatusProvider.UpdateStatus("BUSY: Exporting Virtual Network...");
        //    //    Application.DoEvents();

        //    //    var vpcs = _awsObjectRetriever.Vpcs;

        //    //    Application.DoEvents();
        //    //}
        //    //LogProvider.WriteLog("UpdateArtifacts", "End processing selected Vpcs");

        //    //String storageAccountName = RandomString(22);

        //    //if (_AWSExportArtifacts.Instances.Count > 0)
        //    //{
        //    //    LogProvider.WriteLog("UpdateArtifacts", "Start processing storage accounts");

        //    //    BuildStorageAccountObject(storageAccountName);

        //    //    LogProvider.WriteLog("UpdateArtifacts", "End processing selected storage accounts");
        //    //}

        //    //LogProvider.WriteLog("UpdateArtifacts", "Start processing selected Instances");

        //    //// process selected instances
        //    //foreach (var instance in _AWSExportArtifacts.Instances)
        //    //{
        //    //    StatusProvider.UpdateStatus("BUSY: Exporting Instances...");

        //    //    Application.DoEvents();

        //    //    var availableInstances = _awsObjectRetriever.Instances;
        //    //    if (availableInstances != null)
        //    //    {
        //    //        // var selectedInstances = availableInstances.Reservations[0].Instances.Find(x => x.InstanceId == instance.InstanceId);
        //    //        var selectedInstances = _awsObjectRetriever.getInstancebyId(instance.InstanceId);

        //    //        List<NetworkProfile_NetworkInterface> networkinterfaces = new List<NetworkProfile_NetworkInterface>();
        //    //        String vpcId = selectedInstances.Instances[0].VpcId.ToString();

        //    //        //Process LBs
        //    //        var LBs = _awsObjectRetriever.getLBs(vpcId);
        //    //        string instanceLBName = "";

        //    //        foreach (var LB in LBs)
        //    //        {
        //    //            foreach (var LBInstance in LB.Instances)
        //    //            {
        //    //                if ((LB.VPCId == vpcId) && (LBInstance.InstanceId == instance.InstanceId))
        //    //                {
        //    //                    if (LB.Scheme == "internet-facing")
        //    //                    {
        //    //                        BuildPublicIPAddressObject(LB);
        //    //                    }

        //    //                    instanceLBName = LB.LoadBalancerName;
        //    //                    BuildLoadBalancerObject(LB, instance.InstanceId.ToString());
        //    //                }
        //    //            }
        //    //        }

        //    //        //Process Network Interface
        //    //        BuildNetworkInterfaceObject(selectedInstances.Instances[0], ref networkinterfaces, LBs);

        //    //        //Process EC2 Instance
        //    //        BuildVirtualMachineObject(selectedInstances.Instances[0], networkinterfaces, storageAccountName, instanceLBName);
        //    //    }
        //    //    Application.DoEvents();

        //    //}

        //    //LogProvider.WriteLog("UpdateArtifacts", "End processing selected cloud services and virtual machines");

        //    //Template template = new Template();
        //    //template.resources = _resources;
        //    //template.parameters = this.Parameters;

        //    //// save JSON template
        //    //_statusProvider.UpdateStatus("BUSY: saving JSON template");
        //    //Application.DoEvents();
        //    //string jsontext = JsonConvert.SerializeObject(template, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
        //    //jsontext = jsontext.Replace("schemalink", "$schema");
        //    //WriteStream(templateWriter, jsontext);
        //    //LogProvider.WriteLog("GenerateTemplate", "Write file export.json");

        //    // OnTemplateChanged();

        //    ////post Telemetry Record to AWStoARMToolAPI
        //    //if (app.Default.AllowTelemetry)
        //    //{

        //    //    _statusProvider.UpdateStatus("BUSY: saving telemetry information");
        //    //    _telemetryProvider.PostTelemetryRecord(teleinfo["AccessKey"].ToString(), _processedItems, teleinfo["Region"].ToString());
        //    //}

        //    StatusProvider.UpdateStatus("Ready");

        //    LogProvider.WriteLog("UpdateArtifacts", "End - Execution " + this.ExecutionGuid.ToString());
        //}

        //private void BuildPublicIPAddressObject(ref NetworkInterface networkinterface)
        //{
        //    LogProvider.WriteLog("BuildPublicIPAddressObject", "Start");

        //    string publicipaddress_name = networkinterface.name;
        //    string publicipallocationmethod = "Dynamic";

        //    Hashtable dnssettings = new Hashtable();
        //    dnssettings.Add("domainNameLabel", (publicipaddress_name + _settingsProvider.StorageAccountSuffix).ToLower());

        //    PublicIPAddress_Properties publicipaddress_properties = new PublicIPAddress_Properties();
        //    publicipaddress_properties.dnsSettings = dnssettings;
        //    publicipaddress_properties.publicIPAllocationMethod = publicipallocationmethod;

        //    PublicIPAddress publicipaddress = new PublicIPAddress(this.ExecutionGuid);
        //    publicipaddress.name = networkinterface.name + "-PIP";
        //    // publicipaddress.location = "";
        //    publicipaddress.properties = publicipaddress_properties;

        //    this.AddResource(publicipaddress);
        //    LogProvider.WriteLog("BuildPublicIPAddressObject", "Microsoft.Network/publicIPAddresses/" + publicipaddress.name);

        //    NetworkInterface_Properties networkinterface_properties = (NetworkInterface_Properties)networkinterface.properties;
        //    networkinterface_properties.ipConfigurations[0].properties.publicIPAddress = new Reference();
        //    networkinterface_properties.ipConfigurations[0].properties.publicIPAddress.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/publicIPAddresses/" + publicipaddress.name + "')]";
        //    networkinterface.properties = networkinterface_properties;


        //    networkinterface.dependsOn.Add(networkinterface_properties.ipConfigurations[0].properties.publicIPAddress.id);
        //    LogProvider.WriteLog("BuildPublicIPAddressObject", "End");
        //}

        //private void BuildPublicIPAddressObject(dynamic resource)
        //{
        //    LogProvider.WriteLog("BuildPublicIPAddressObject", "Start");

        //    string publicipaddress_name = resource.LoadBalancerName + "-PIP";
        //    string publicipallocationmethod = "Dynamic";
        //    char[] delimiterChars = { '.' };
        //    string LBDNS = resource.DNSName.Split(delimiterChars)[0].ToLower();

        //    Hashtable dnssettings = new Hashtable();
        //    dnssettings.Add("domainNameLabel", LBDNS);

        //    PublicIPAddress_Properties publicipaddress_properties = new PublicIPAddress_Properties();
        //    publicipaddress_properties.dnsSettings = dnssettings;
        //    publicipaddress_properties.publicIPAllocationMethod = publicipallocationmethod;

        //    PublicIPAddress publicipaddress = new PublicIPAddress(this.ExecutionGuid);
        //    publicipaddress.name = publicipaddress_name;
        //    // publicipaddress.location = "";
        //    publicipaddress.properties = publicipaddress_properties;

        //    this.AddResource(publicipaddress);
        //    LogProvider.WriteLog("BuildPublicIPAddressObject", "Microsoft.Network/publicIPAddresses/" + publicipaddress.name);

        //    LogProvider.WriteLog("BuildPublicIPAddressObject", "End");
        //}


        // convert an hex string into byte array
        public static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }

        public static string Base64Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private string GetNewStorageAccountName(string oldStorageAccountName)
        {
            string newStorageAccountName = "";

            if (_storageAccountNames.ContainsKey(oldStorageAccountName))
            {
                _storageAccountNames.TryGetValue(oldStorageAccountName, out newStorageAccountName);
            }
            else
            {
                newStorageAccountName = oldStorageAccountName + _settingsProvider.StorageAccountSuffix;

                if (newStorageAccountName.Length > 24)
                {
                    string randomString = Guid.NewGuid().ToString("N").Substring(0, 4);
                    newStorageAccountName = newStorageAccountName.Substring(0, (24 - randomString.Length - _settingsProvider.StorageAccountSuffix.Length)) + randomString + _settingsProvider.StorageAccountSuffix;
                }

                _storageAccountNames.Add(oldStorageAccountName, newStorageAccountName);
            }

            return newStorageAccountName;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string GetRouteName(dynamic Route)
        {
            //RouteTableId
            string RouteName = Route.RouteTableId.Replace(' ', '-');

            foreach (var tag in Route.Tags)
            {
                if (tag.Key == "Name")
                {
                    RouteName = tag.Value;
                }
            }

            return RouteName;
        }

        private string GetSGName(dynamic SG)
        {
            string SGName = SG.NetworkAclId.Replace(' ', '-');

            foreach (var tag in SG.Tags)
            {
                if (tag.Key == "Name")
                {
                    SGName = tag.Value;
                }
            }

            return SGName;
        }

        private string GetInstanceName(dynamic Instance)
        {
            string InstanceName = Instance.InstanceId.Replace(' ', '-');

            foreach (var tag in Instance.Tags)
            {
                if (tag.Key == "Name")
                {
                    InstanceName = tag.Value;
                }
            }

            return InstanceName;
        }

        private string GetNICName(dynamic NIC)
        {
            string NICName = NIC.NetworkInterfaceId.Replace(' ', '-');

            foreach (var tag in NIC.TagSet)
            {
                if (tag.Key == "Name")
                {
                    NICName = tag.Value;
                }
            }

            return NICName;
        }

        private string GetVMSize(string instancetype)
        {
            Dictionary<string, string> VMSizeTable = new Dictionary<string, string>();
            VMSizeTable.Add("t2.nano", "Standard_A0");
            VMSizeTable.Add("t2.micro", "Standard_A1");
            VMSizeTable.Add("t2.small", "Standard_A1_v2");
            VMSizeTable.Add("t2.medium", "Standard_A2_v2");
            VMSizeTable.Add("t2.large", "Standard_A2m_v2");
            VMSizeTable.Add("t2.xlarge", "Standard_A4m_v2");
            VMSizeTable.Add("t2.2xlarge", "Standard_A8m_v2");
            VMSizeTable.Add("m4.large", "Standard_DS2_v2");
            VMSizeTable.Add("m4.xlarge", "Standard_DS3_v2");
            VMSizeTable.Add("m4.2xlarge", "Standard_DS4_v2");
            VMSizeTable.Add("m4.4xlarge", "Standard_DS5_v2");
            VMSizeTable.Add("m4.10xlarge", "Standard_DS15_v2");
            VMSizeTable.Add("m4.16xlarge", "Standard_DS15_v2");
            VMSizeTable.Add("m3.medium", "Standard_DS1_v2");
            VMSizeTable.Add("m3.large", "Standard_DS2_v2");
            VMSizeTable.Add("m3.xlarge", "Standard_DS3_v2");
            VMSizeTable.Add("m3.2xlarge", "Standard_DS4_v2");
            VMSizeTable.Add("c4.large", "Standard_F2s");
            VMSizeTable.Add("c4.xlarge", "Standard_F4s");
            VMSizeTable.Add("c4.2xlarge", "Standard_F8s");
            VMSizeTable.Add("c4.4xlarge", "Standard_F16s");
            VMSizeTable.Add("c4.8xlarge", "Standard_F16s");
            VMSizeTable.Add("c3.large", "Standard_F2s");
            VMSizeTable.Add("c3.xlarge", "Standard_F4s");
            VMSizeTable.Add("c3.2xlarge", "Standard_F8s");
            VMSizeTable.Add("c3.4xlarge", "Standard_F16s");
            VMSizeTable.Add("c3.8xlarge", "Standard_F16s");
            VMSizeTable.Add("g2.2xlarge", "Standard_NC6");
            VMSizeTable.Add("g2.8xlarge", "Standard_NC24");
            VMSizeTable.Add("r4.large", "Standard_DS11_v2");
            VMSizeTable.Add("r4.xlarge", "Standard_DS12_v2");
            VMSizeTable.Add("r4.2xlarge", "Standard_DS13_v2");
            VMSizeTable.Add("r4.4xlarge", "Standard_DS14_v2");
            VMSizeTable.Add("r4.8xlarge", "Standard_GS5");
            VMSizeTable.Add("r4.16xlarge", "Standard_GS5");
            VMSizeTable.Add("r3.large", "Standard_DS11_v2");
            VMSizeTable.Add("r3.xlarge", "Standard_DS12_v2");
            VMSizeTable.Add("r3.2xlarge", "Standard_DS13_v2");
            VMSizeTable.Add("r3.4xlarge", "Standard_DS14_v2");
            VMSizeTable.Add("r3.8xlarge", "Standard_GS5");
            VMSizeTable.Add("x1.16xlarge", "Standard_GS5");
            VMSizeTable.Add("x1.32xlarge", "Standard_GS5");
            VMSizeTable.Add("d2.xlarge", "Standard_DS12_v2");
            VMSizeTable.Add("d2.2xlarge", "Standard_DS13_v2");
            VMSizeTable.Add("d2.4xlarge", "Standard_DS14_v2");
            VMSizeTable.Add("d2.8xlarge", "Standard_GS5");
            VMSizeTable.Add("i2.xlarge", "Standard_DS12_v2");
            VMSizeTable.Add("i2.2xlarge", "Standard_DS13_v2");
            VMSizeTable.Add("i2.4xlarge", "Standard_DS14_v2");
            VMSizeTable.Add("i2.8xlarge", "Standard_GS5");

            if (VMSizeTable.ContainsKey(instancetype))
            {
                return VMSizeTable[instancetype];
            }
            else
            {
                return "Standard_DS2_v2";
            }
        }
    }
}
















//private void BuildAvailabilitySetObject(dynamic loadbalancer)
//{
//    LogProvider.WriteLog("BuildAvailabilitySetObject", "Start");

//    AvailabilitySet availabilityset = new AvailabilitySet(this.ExecutionGuid);
//    availabilityset.name = loadbalancer.name + "-AS";

//    this.AddResource(availabilityset);
//    LogProvider.WriteLog("BuildAvailabilitySetObject", "Microsoft.Compute/availabilitySets/" + availabilityset.name);

//    LogProvider.WriteLog("BuildAvailabilitySetObject", "End");
//}

//private void BuildLoadBalancerObject(dynamic LB, string instance)
//{
//    LogProvider.WriteLog("BuildLoadBalancerObject", "Start");

//    LoadBalancer loadbalancer = new LoadBalancer(this.ExecutionGuid);
//    loadbalancer.name = LB.LoadBalancerName;

//    FrontendIPConfiguration_Properties frontendipconfiguration_properties = new FrontendIPConfiguration_Properties();

//    //// if internal load balancer
//    if (LB.Scheme != "internet-facing")
//    {
//        //string virtualnetworkname = GetVPCName(LB.VPCId);

//        var subnet = _awsObjectRetriever.getSubnetbyId(LB.Subnets[0]);
//        string subnetname = "SubnetName";

//        frontendipconfiguration_properties.privateIPAllocationMethod = "Static";
//        try
//        {
//            IPHostEntry host = Dns.GetHostEntry(LB.DNSName);
//            frontendipconfiguration_properties.privateIPAddress = host.AddressList[0].ToString();
//        }
//        catch
//        {
//            frontendipconfiguration_properties.privateIPAllocationMethod = "Dynamic";
//        }

//        List<string> dependson = new List<string>();


//        Reference subnet_ref = new Reference();
//        subnet_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/virtualNetworks/" + "VNetName" + "/subnets/" + subnetname + "')]";
//        frontendipconfiguration_properties.subnet = subnet_ref;
//    }
//    //// if external load balancer
//    else
//    {
//        List<string> dependson = new List<string>();
//        dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/publicIPAddresses/" + loadbalancer.name + "-PIP')]");
//        loadbalancer.dependsOn = dependson;

//        Reference publicipaddress_ref = new Reference();
//        publicipaddress_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/publicIPAddresses/" + loadbalancer.name + "-PIP')]";
//        frontendipconfiguration_properties.publicIPAddress = publicipaddress_ref;
//    }


//    LoadBalancer_Properties loadbalancer_properties = new LoadBalancer_Properties();

//    FrontendIPConfiguration frontendipconfiguration = new FrontendIPConfiguration();
//    frontendipconfiguration.properties = frontendipconfiguration_properties;

//    List<FrontendIPConfiguration> frontendipconfigurations = new List<FrontendIPConfiguration>();
//    frontendipconfigurations.Add(frontendipconfiguration);
//    loadbalancer_properties.frontendIPConfigurations = frontendipconfigurations;

//    Hashtable backendaddresspool = new Hashtable();
//    backendaddresspool.Add("name", "default");
//    List<Hashtable> backendaddresspools = new List<Hashtable>();
//    backendaddresspools.Add(backendaddresspool);
//    loadbalancer_properties.backendAddressPools = backendaddresspools;

//    List<InboundNatRule> inboundnatrules = new List<InboundNatRule>();
//    List<LoadBalancingRule> loadbalancingrules = new List<LoadBalancingRule>();
//    List<Probe> probes = new List<Probe>();

//    foreach (var VM in LB.Instances)
//    {
//        if (VM.InstanceId == instance)
//        {
//            Hashtable virtualmachineinfo = new Hashtable();
//            virtualmachineinfo.Add("virtualmachinename", VM.InstanceId);
//            BuildLoadBalancerRules(VM.InstanceId, LB, ref inboundnatrules, ref loadbalancingrules, ref probes);
//        }
//    }

//    loadbalancer_properties.inboundNatRules = inboundnatrules;
//    loadbalancer_properties.loadBalancingRules = loadbalancingrules;
//    loadbalancer_properties.probes = probes;
//    loadbalancer.properties = loadbalancer_properties;

//    // Add the load balancer only if there is any Load Balancing rule or Inbound NAT rule
//    if (loadbalancingrules.Count > 0)
//    {
//        this.AddResource(loadbalancer);

//        // Add an Availability Set per load balancer
//        BuildAvailabilitySetObject(loadbalancer);

//        LogProvider.WriteLog("BuildLoadBalancerObject", "Microsoft.Network/loadBalancers/" + loadbalancer.name);
//    }
//    else
//    {
//        LogProvider.WriteLog("BuildLoadBalancerObject", "EMPTY Microsoft.Network/loadBalancers/" + loadbalancer.name);
//    }

//    LogProvider.WriteLog("BuildLoadBalancerObject", "End");
//}

//private void BuildLoadBalancerRules(String resource, dynamic LB, ref List<InboundNatRule> inboundnatrules, ref List<LoadBalancingRule> loadbalancingrules, ref List<Probe> probes)
//{
//    LogProvider.WriteLog("BuildLoadBalancerRules", "Start");

//    string virtualmachinename = resource;
//    string loadbalancername = LB.LoadBalancerName;

//    // process Probe
//    Probe_Properties probe_properties = new Probe_Properties();

//    char[] delimiterChars = { ':' };
//    probe_properties.protocol = LB.HealthCheck.Target.Split(delimiterChars)[0];
//    if (probe_properties.protocol == "HTTP")
//    {
//        probe_properties.protocol = "Http";
//    }
//    else
//    {
//        probe_properties.protocol = "Tcp";
//    }

//    string portPath = LB.HealthCheck.Target.Split(delimiterChars)[1];
//    if (portPath.IndexOf("/") == -1)
//    {
//        probe_properties.port = Convert.ToInt64(portPath);
//    }
//    else
//    {
//        delimiterChars[0] = '/';
//        probe_properties.port = Convert.ToInt64(portPath.Split(delimiterChars)[0]);
//        probe_properties.requestPath = portPath.Split(delimiterChars)[1];
//    }

//    Probe probe = new Probe();
//    probe.name = probe_properties.protocol + "-" + probe_properties.port;
//    probe.properties = probe_properties;

//    if (!probes.Contains(probe))
//        probes.Add(probe);

//    // end process Probe

//    foreach (var rule in LB.ListenerDescriptions)
//    {
//        string protocol = "Tcp";
//        //if (rule.Listener.InstanceProtocol == "HTTP")
//        //{
//        //    protocol = "Http";
//        //}

//        string name = protocol + "-" + rule.Listener.LoadBalancerPort;

//        // build load balancing rule
//        Reference frontendipconfiguration_ref = new Reference();
//        frontendipconfiguration_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/frontendIPConfigurations/default')]";

//        Reference backendaddresspool_ref = new Reference();
//        backendaddresspool_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/backendAddressPools/default')]";

//        Reference probe_ref = new Reference();
//        probe_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/probes/" + probe.name + "')]";

//        LoadBalancingRule_Properties loadbalancingrule_properties = new LoadBalancingRule_Properties();
//        loadbalancingrule_properties.frontendIPConfiguration = frontendipconfiguration_ref;
//        loadbalancingrule_properties.backendAddressPool = backendaddresspool_ref;
//        loadbalancingrule_properties.probe = probe_ref;
//        loadbalancingrule_properties.frontendPort = rule.Listener.LoadBalancerPort;
//        loadbalancingrule_properties.backendPort = rule.Listener.InstancePort;
//        loadbalancingrule_properties.protocol = protocol;

//        LoadBalancingRule loadbalancingrule = new LoadBalancingRule();
//        loadbalancingrule.name = name;
//        loadbalancingrule.properties = loadbalancingrule_properties;

//        if (!loadbalancingrules.Contains(loadbalancingrule))
//            loadbalancingrules.Add(loadbalancingrule);

//        LogProvider.WriteLog("BuildLoadBalancerRules", "Microsoft.Network/loadBalancers/" + loadbalancername + "/loadBalancingRules/" + loadbalancingrule.name);
//    }

//    LogProvider.WriteLog("BuildLoadBalancerRules", "End");
//}

//private NetworkSecurityGroup BuildNetworkSecurityGroup(Amazon.EC2.Model.NetworkAcl securitygroup)
//{
//    LogProvider.WriteLog("BuildNetworkSecurityGroup", "Start");

//    Hashtable nsginfo = new Hashtable();
//    nsginfo.Add("name", GetSGName(securitygroup));
//    //XmlDocument resource = _awsRetriever.GetAwsResources(ConfigurationManager.AppSettings["endpoint"], "DescribeNetworkAcls", "");

//    NetworkSecurityGroup networksecuritygroup = new NetworkSecurityGroup(this.ExecutionGuid);
//    networksecuritygroup.name = GetSGName(securitygroup);
//    //networksecuritygroup.location = securitygroup.SelectSingleNode("//Location").InnerText;

//    NetworkSecurityGroup_Properties networksecuritygroup_properties = new NetworkSecurityGroup_Properties();
//    networksecuritygroup_properties.securityRules = new List<SecurityRule>();

//    // for each rule
//    foreach (var rule in securitygroup.Entries)
//    {
//        string ruleDirection = "";
//        if (rule.Egress == true)
//            ruleDirection = "Outbound";
//        else if (rule.Egress == false)
//            ruleDirection = "Inbound";

//        SecurityRule_Properties securityrule_properties = new SecurityRule_Properties(); //Name -Inbound_32, Outbound_100
//                                                                                         //securityrule_properties.description = rule.SelectSingleNode("Name").InnerText; 
//        securityrule_properties.direction = ruleDirection;
//        if (rule.RuleNumber > 4096)
//            securityrule_properties.priority = 4096;
//        else
//        {
//            securityrule_properties.priority = rule.RuleNumber;//RuleNum

//            securityrule_properties.access = rule.RuleAction; //ruleAction
//            securityrule_properties.sourceAddressPrefix = rule.CidrBlock; //cidrBlock
//            securityrule_properties.destinationAddressPrefix = "0.0.0.0/0";

//            if (rule.Protocol == "6")
//                securityrule_properties.protocol = "tcp";
//            else if (rule.Protocol == "17")
//                securityrule_properties.protocol = "udp";
//            else
//                securityrule_properties.protocol = "*";

//            if (rule.PortRange != null)
//            {
//                int from, to;
//                from = rule.PortRange.From;
//                to = rule.PortRange.To;
//                if (from == to)
//                    securityrule_properties.sourcePortRange = from.ToString();
//                else
//                    securityrule_properties.sourcePortRange = from + "-" + to;
//                // number, number-num, * - if portrange not available - from and to tags of port range tag
//            }
//            else
//            {
//                securityrule_properties.sourcePortRange = "*";
//            }
//            securityrule_properties.destinationPortRange = "*";

//            SecurityRule securityrule = new SecurityRule();

//            securityrule.name = (ruleDirection + "-" + rule.RuleNumber);
//            securityrule.properties = securityrule_properties;

//            networksecuritygroup_properties.securityRules.Add(securityrule);
//            networksecuritygroup.properties = networksecuritygroup_properties;
//        }
//    }

//    this.AddResource(networksecuritygroup);

//    LogProvider.WriteLog("BuildNetworkSecurityGroup", "End");

//    return networksecuritygroup;

//}

//private NetworkSecurityGroup BuildNetworkSecurityGroup(Amazon.EC2.Model.GroupIdentifier securitygroupidentifier, ref NetworkInterface networkinterface)
//{
//    LogProvider.WriteLog("BuildNetworkSecurityGroup", "Start");

//    NetworkSecurityGroup networksecuritygroup = new NetworkSecurityGroup(this.ExecutionGuid);
//    networksecuritygroup.name = securitygroupidentifier.GroupName;

//    NetworkSecurityGroup_Properties networksecuritygroup_properties = new NetworkSecurityGroup_Properties();
//    networksecuritygroup_properties.securityRules = new List<SecurityRule>();

//    long inboundPriority = 100;
//    long outboundPriority = 101;

//    Amazon.EC2.Model.SecurityGroup securitygroup = _awsObjectRetriever.getSecurityGroup(securitygroupidentifier.GroupId);

//    // process inbound rules
//    foreach (Amazon.EC2.Model.IpPermission ippermission in securitygroup.IpPermissions)
//    {
//        SecurityRule securityrule = new SecurityRule();
//        securityrule.name = ("Inbound-" + inboundPriority);
//        securityrule.properties = new SecurityRule_Properties();

//        SecurityRule_Properties securityrule_properties = new SecurityRule_Properties();
//        securityrule_properties.direction = "Inbound";
//        securityrule_properties.access = "Allow";
//        securityrule_properties.sourcePortRange = "*";
//        securityrule_properties.destinationAddressPrefix = "0.0.0.0/0";
//        securityrule_properties.priority = inboundPriority;
//        inboundPriority = inboundPriority + 100;

//        if (ippermission.IpProtocol == "tcp")
//        {
//            securityrule_properties.protocol = "Tcp";
//        }
//        else if (ippermission.IpProtocol == "udp")
//        {
//            securityrule_properties.protocol = "Udp";
//        }
//        else
//        {
//            securityrule_properties.protocol = "*";
//        }

//        securityrule_properties.sourceAddressPrefix = ippermission.IpRanges[0];
//        securityrule_properties.destinationPortRange = "*";
//        if (ippermission.ToPort > 0)
//        {
//            securityrule_properties.destinationPortRange = ippermission.ToPort.ToString();
//        }

//        securityrule.properties = securityrule_properties;
//        networksecuritygroup_properties.securityRules.Add(securityrule);
//    }

//    // process outbound rules
//    foreach (Amazon.EC2.Model.IpPermission ippermissionegress in securitygroup.IpPermissionsEgress)
//    {
//        SecurityRule securityrule = new SecurityRule();
//        securityrule.name = ("Outbound-" + outboundPriority);
//        securityrule.properties = new SecurityRule_Properties();

//        SecurityRule_Properties securityrule_properties = new SecurityRule_Properties();
//        securityrule_properties.direction = "Outbound";
//        securityrule_properties.access = "Allow";
//        securityrule_properties.sourceAddressPrefix = "0.0.0.0/0";
//        securityrule_properties.sourcePortRange = "*";
//        securityrule_properties.priority = outboundPriority;
//        outboundPriority = outboundPriority + 100;

//        if (ippermissionegress.IpProtocol == "tcp")
//        {
//            securityrule_properties.protocol = "Tcp";
//        }
//        else if (ippermissionegress.IpProtocol == "udp")
//        {
//            securityrule_properties.protocol = "Udp";
//        }
//        else
//        {
//            securityrule_properties.protocol = "*";
//        }

//        securityrule_properties.destinationAddressPrefix = ippermissionegress.IpRanges[0];
//        securityrule_properties.destinationPortRange = "*";
//        if (ippermissionegress.ToPort > 0)
//        {
//            securityrule_properties.destinationPortRange = ippermissionegress.ToPort.ToString();
//        }

//        securityrule.properties = securityrule_properties;
//        networksecuritygroup_properties.securityRules.Add(securityrule);
//    }

//    networksecuritygroup.properties = networksecuritygroup_properties;

//    this.AddResource(networksecuritygroup);
//    LogProvider.WriteLog("BuildNetworkSecurityGroup", "Microsoft.Network/networkSecurityGroups/" + networksecuritygroup.name);

//    LogProvider.WriteLog("BuildNetworkSecurityGroup", "End");

//    return networksecuritygroup;
//}

//private RouteTable BuildRouteTable(Amazon.EC2.Model.RouteTable routeTable)
//{
//    LogProvider.WriteLog("BuildRouteTable", "Start");

//    Hashtable info = new Hashtable();
//    info.Add("name", GetRouteName(routeTable));
//    // XmlDocument resource = _awsRetriever.GetAwsResources(ConfigurationManager.AppSettings["endpoint"], "RouteTable", subscriptionId, info, token);

//    RouteTable routetable = new RouteTable(this.ExecutionGuid);
//    routetable.name = GetRouteName(routeTable);
//    // routetable.location = resource.SelectSingleNode("//Location").InnerText;

//    RouteTable_Properties routetable_properties = new RouteTable_Properties();
//    routetable_properties.routes = new List<Route>();

//    foreach (var rule in routeTable.Routes)
//    {

//        var GWResults = _awsObjectRetriever.getInternetGW(rule.GatewayId);

//        if ((((rule.DestinationCidrBlock == "0.0.0.0/0") && (GWResults.Count == 0)) || (rule.DestinationCidrBlock != "0.0.0.0/0")) && (rule.GatewayId != "local"))
//        {
//            Route_Properties route_properties = new Route_Properties();
//            route_properties.addressPrefix = rule.DestinationCidrBlock;

//            switch (rule.GatewayId)
//            {
//                case "local":
//                    route_properties.nextHopType = "VnetLocal";
//                    break;
//                case "Null":
//                    route_properties.nextHopType = "None";
//                    break;
//            }

//            //     route_properties.nextHopIpAddress = routenode.SelectSingleNode("NextHopType/IpAddress").InnerText;

//            Route route = new Route();
//            route.name = rule.GatewayId;
//            route.properties = route_properties;

//            routetable_properties.routes.Add(route);

//            routetable.properties = routetable_properties;


//        }

//    }

//    if (routetable_properties.routes.Count != 0)
//    {
//        this.AddResource(routetable);
//        LogProvider.WriteLog("BuildRouteTable", "Microsoft.Network/routeTables/" + routetable.name);
//    }

//    LogProvider.WriteLog("BuildRouteTable", "End");

//    return routetable;
//}

//private void BuildNetworkInterfaceObject(dynamic resource, ref List<NetworkProfile_NetworkInterface> networkinterfaces, dynamic LBs)
//{
//    LogProvider.WriteLog("BuildNetworkInterfaceObject", "Start");

//    string virtualmachinename = GetInstanceName(resource);

//    string virtualnetworkname = "VNetName"; //GetVPCName(resource.VpcId);

//    foreach (var additionalnetworkinterface in resource.NetworkInterfaces)
//    {

//        Hashtable NWInfo = new Hashtable();
//        NWInfo.Add("networkinterfaceId", additionalnetworkinterface.NetworkInterfaceId);

//        //Getting Subnet Details

//        string SubnetId = additionalnetworkinterface.SubnetId;

//        var Subdetails = _awsObjectRetriever.getSubnetbyId(SubnetId);
//        string subnet_name = "SubnetName";


//        Reference subnet_ref = new Reference();
//        subnet_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/virtualNetworks/" + virtualnetworkname + "/subnets/" + subnet_name + "')]";

//        string privateIPAllocationMethod = "Static";
//        string privateIPAddress = additionalnetworkinterface.PrivateIpAddress;

//        List<string> dependson = new List<string>();
//        if (ResourceExists(typeof(VirtualNetwork), virtualnetworkname))
//        {
//            dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/virtualNetworks/" + virtualnetworkname + "')]");
//        }


//        // Get the list of endpoints


//        List<Reference> loadBalancerBackendAddressPools = new List<Reference>();
//        List<Reference> loadBalancerInboundNatRules = new List<Reference>();

//        foreach (var LB in LBs)
//        {
//            foreach (var LBInstance in LB.Instances)
//            {
//                if ((LB.VPCId == resource.VpcId) && (LBInstance.InstanceId == resource.InstanceId))
//                {

//                    string loadbalancername = LB.LoadBalancerName;
//                    string BAPoolName = "default";


//                    Reference loadBalancerBackendAddressPool = new Reference();
//                    loadBalancerBackendAddressPool.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/backendAddressPools/" + BAPoolName + "')]";
//                    loadBalancerBackendAddressPools.Add(loadBalancerBackendAddressPool);

//                    dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "')]");
//                }
//            }
//        }

//        IpConfiguration_Properties ipconfiguration_properties = new IpConfiguration_Properties();
//        ipconfiguration_properties.privateIPAllocationMethod = privateIPAllocationMethod;
//        ipconfiguration_properties.privateIPAddress = privateIPAddress;
//        ipconfiguration_properties.subnet = subnet_ref;
//        ipconfiguration_properties.loadBalancerInboundNatRules = loadBalancerInboundNatRules;
//        ipconfiguration_properties.loadBalancerBackendAddressPools = loadBalancerBackendAddressPools;

//        string ipconfiguration_name = "ipconfig1";
//        IpConfiguration ipconfiguration = new IpConfiguration();
//        ipconfiguration.name = ipconfiguration_name;
//        ipconfiguration.properties = ipconfiguration_properties;

//        List<IpConfiguration> ipConfigurations = new List<IpConfiguration>();
//        ipConfigurations.Add(ipconfiguration);

//        NetworkInterface_Properties networkinterface_properties = new NetworkInterface_Properties();
//        networkinterface_properties.ipConfigurations = ipConfigurations;


//        networkinterface_properties.enableIPForwarding = (resource.SourceDestCheck == true) ? false : (resource.SourceDestCheck == false) ? true : false;

//        // networkinterface_properties.enableIPForwarding = resource.SourceDestCheck;

//        var NICDetails = _awsObjectRetriever.getNICbyId(additionalnetworkinterface.NetworkInterfaceId);
//        string networkinterfacename = GetNICName(NICDetails[0]);

//        string networkinterface_name = networkinterfacename;
//        NetworkInterface networkinterface = new NetworkInterface(this.ExecutionGuid);
//        networkinterface.name = networkinterface_name;
//        //  networkinterface.location = "";
//        networkinterface.properties = networkinterface_properties;
//        networkinterface.dependsOn = dependson;

//        NetworkProfile_NetworkInterface_Properties networkinterface_ref_properties = new NetworkProfile_NetworkInterface_Properties();
//        networkinterface_ref_properties.primary = additionalnetworkinterface.PrivateIpAddresses[0].Primary;

//        NetworkProfile_NetworkInterface networkinterface_ref = new NetworkProfile_NetworkInterface();
//        networkinterface_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/networkInterfaces/" + networkinterface.name + "')]";
//        networkinterface_ref.properties = networkinterface_ref_properties;

//        if (resource.PublicIpAddress != null)
//        {
//            BuildPublicIPAddressObject(ref networkinterface);
//        }

//        // Process Network Interface Security Group
//        foreach (var group in additionalnetworkinterface.Groups)
//        {
//            NetworkSecurityGroup networksecuritygroup = new NetworkSecurityGroup(this.ExecutionGuid);
//            networksecuritygroup = BuildNetworkSecurityGroup(group, ref networkinterface);
//            networkinterface_properties.NetworkSecurityGroup = new Reference();
//            networkinterface_properties.NetworkSecurityGroup.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/networkSecurityGroups/" + networksecuritygroup.name + "')]";
//            networkinterface.dependsOn.Add(networkinterface_properties.NetworkSecurityGroup.id);
//            networkinterface.properties = networkinterface_properties;
//        }

//        networkinterfaces.Add(networkinterface_ref);

//        this.AddResource(networkinterface);

//        LogProvider.WriteLog("BuildNetworkInterfaceObject", "Microsoft.Network/networkInterfaces/" + networkinterface.name);
//    }
//    LogProvider.WriteLog("BuildNetworkInterfaceObject", "End");
//}

//private void BuildVirtualMachineObject(Amazon.EC2.Model.Instance selectedInstance, List<NetworkProfile_NetworkInterface> networkinterfaces, String newstorageaccountname, string instanceLBName)
//{
//    LogProvider.WriteLog("BuildVirtualMachineObject", "Start");

//    string virtualmachinename = GetInstanceName(selectedInstance);
//    newstorageaccountname = GetNewStorageAccountName(newstorageaccountname);

//    var NICDetails = _awsObjectRetriever.getNICbyId(selectedInstance.NetworkInterfaces[0].NetworkInterfaceId);
//    string networkinterfacename = GetNICName(NICDetails[0]);
//    string ostype;

//    if (selectedInstance.Platform == null)
//    {
//        ostype = "Linux";
//    }
//    else
//    {
//        ostype = selectedInstance.Platform;
//    }


//    Hashtable storageaccountdependencies = new Hashtable();
//    storageaccountdependencies.Add(newstorageaccountname, "");

//    HardwareProfile hardwareprofile = new HardwareProfile();
//    hardwareprofile.vmSize = GetVMSize(selectedInstance.InstanceType.Value);

//    NetworkProfile networkprofile = new NetworkProfile();
//    networkprofile.networkInterfaces = networkinterfaces;

//    Vhd vhd = new Vhd();
//    vhd.uri = "http://" + newstorageaccountname + ".blob.core.windows.net/vhds/" + virtualmachinename + "-" + "osdisk0.vhd";

//    //CHECK
//    OsDisk osdisk = new OsDisk();
//    osdisk.name = virtualmachinename + selectedInstance.RootDeviceName.Replace("/", "_");
//    osdisk.vhd = vhd;
//    osdisk.caching = "ReadOnly";

//    ImageReference imagereference = new ImageReference();
//    OsProfile osprofile = new OsProfile();

//    // if the tool is configured to create new VMs with empty data disks
//    if (_settingsProvider.BuildEmpty)
//    {
//        osdisk.createOption = "FromImage";

//        osprofile.computerName = virtualmachinename;
//        osprofile.adminUsername = "[parameters('adminUsername')]";
//        osprofile.adminPassword = "[parameters('adminPassword')]";

//        if (!this.Parameters.ContainsKey("adminUsername"))
//        {
//            Parameter parameter = new Parameter();
//            parameter.type = "string";
//            this.Parameters.Add("adminUsername", parameter);
//        }

//        if (!this.Parameters.ContainsKey("adminPassword"))
//        {
//            Parameter parameter = new Parameter();
//            parameter.type = "securestring";
//            this.Parameters.Add("adminPassword", parameter);
//        }

//        if (ostype == "Windows")
//        {
//            imagereference.publisher = "MicrosoftWindowsServer";
//            imagereference.offer = "WindowsServer";
//            imagereference.sku = "2012-R2-Datacenter";
//            imagereference.version = "latest";
//        }
//        else if (ostype == "Linux")
//        {
//            imagereference.publisher = "Canonical";
//            imagereference.offer = "UbuntuServer";
//            imagereference.sku = "16.04.0-LTS";
//            imagereference.version = "latest";
//        }
//        else
//        {
//            imagereference.publisher = "<publisher>";
//            imagereference.offer = "<offer>";
//            imagereference.sku = "<sku>";
//            imagereference.version = "<version>";
//        }
//    }
//    // if the tool is configured to attach copied disks
//    else
//    {
//        osdisk.createOption = "Attach";
//        osdisk.osType = ostype;

//        // Block of code to help copying the blobs to the new storage accounts
//        Hashtable storageaccountinfo = new Hashtable();
//        //storageaccountinfo.Add("name", oldstorageaccountname);

//    }

//    // process data disks
//    List<DataDisk> datadisks = new List<DataDisk>();

//    int currentLun = 0;
//    foreach (var disk in selectedInstance.BlockDeviceMappings)
//    {
//        if (disk.DeviceName != selectedInstance.RootDeviceName)
//        {
//            DataDisk datadisk = new DataDisk();
//            datadisk.name = virtualmachinename + disk.DeviceName.Substring(0).Replace("/", "_");
//            datadisk.caching = "ReadOnly";
//            datadisk.diskSizeGB = _awsObjectRetriever.GetVolume(disk.Ebs.VolumeId)[0].Size;

//            datadisk.lun = currentLun++;

//            if (_settingsProvider.BuildEmpty)
//            {
//                datadisk.createOption = "Empty";
//            }
//            else
//            {
//                datadisk.createOption = "Attach";
//            }

//            vhd = new Vhd();

//            vhd.uri = "http://" + newstorageaccountname + ".blob.core.windows.net/vhds/" + virtualmachinename + "-" + datadisk.name + ".vhd";
//            datadisk.vhd = vhd;

//            try { storageaccountdependencies.Add(newstorageaccountname, ""); }
//            catch { }

//            datadisks.Add(datadisk);
//        }
//    }


//    StorageProfile storageprofile = new StorageProfile();
//    if (_settingsProvider.BuildEmpty) { storageprofile.imageReference = imagereference; }
//    storageprofile.osDisk = osdisk;
//    storageprofile.dataDisks = datadisks;

//    VirtualMachine_Properties virtualmachine_properties = new VirtualMachine_Properties();
//    virtualmachine_properties.hardwareProfile = hardwareprofile;
//    if (_settingsProvider.BuildEmpty) { virtualmachine_properties.osProfile = osprofile; }
//    virtualmachine_properties.networkProfile = networkprofile;
//    virtualmachine_properties.storageProfile = storageprofile;

//    List<string> dependson = new List<string>();
//    dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/networkInterfaces/" + networkinterfacename + "')]");

//    // Availability Set
//    if (instanceLBName != "")
//    {
//        string availabilitysetname = instanceLBName + "-AS";

//        Reference availabilityset = new Reference();
//        availabilityset.id = "[concat(resourceGroup().id, '/providers/Microsoft.Compute/availabilitySets/" + availabilitysetname + "')]";
//        virtualmachine_properties.availabilitySet = availabilityset;

//        dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Compute/availabilitySets/" + availabilitysetname + "')]");
//    }

//    foreach (DictionaryEntry storageaccountdependency in storageaccountdependencies)
//    {
//        if (ResourceExists(typeof(StorageAccount), (string)storageaccountdependency.Key))
//        {
//            dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Storage/storageAccounts/" + storageaccountdependency.Key + "')]");
//        }
//    }

//    VirtualMachine virtualmachine = new VirtualMachine(this.ExecutionGuid);
//    virtualmachine.name = virtualmachinename;
//    //virtualmachine.location = virtualmachineinfo["location"].ToString();
//    virtualmachine.properties = virtualmachine_properties;
//    virtualmachine.dependsOn = dependson;
//    virtualmachine.resources = new List<ArmResource>();
//    //if (extension_iaasdiagnostics != null) { virtualmachine.resources.Add(extension_iaasdiagnostics); }

//    this.AddResource(virtualmachine);
//    LogProvider.WriteLog("BuildVirtualMachineObject", "Microsoft.Compute/virtualMachines/" + virtualmachine.name);

//    LogProvider.WriteLog("BuildVirtualMachineObject", "End");
//}

//private void BuildStorageAccountObject(String StorageAccountName)
//{
//    LogProvider.WriteLog("BuildStorageAccountObject", "Start");

//    StorageAccount_Properties storageaccount_properties = new StorageAccount_Properties();
//    storageaccount_properties.accountType = "Standard_LRS";

//    StorageAccount storageaccount = new StorageAccount(this.ExecutionGuid);
//    storageaccount.name = GetNewStorageAccountName(StorageAccountName);
//    // storageaccount.location = "";
//    storageaccount.properties = storageaccount_properties;

//    this.AddResource(storageaccount);
//    LogProvider.WriteLog("BuildStorageAccountObject", "Microsoft.Storage/storageAccounts/" + storageaccount.name);

//    LogProvider.WriteLog("BuildStorageAccountObject", "End");
//}