using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using MigAz.Azure.Arm;
using MigAz.Azure.Asm;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Core.ArmTemplate;
using System.Windows.Forms;
using Newtonsoft.Json;
using MigAz.Azure.Interface;

namespace MigAz.Azure
{
    public class AzureRetriever
    {
        private AzureContext _AzureContext;
        private object _lockObject = new object();
        private AzureSubscription _AzureSubscription = null;
        private List<AzureSubscription> _AzureSubscriptions;

        public delegate void OnRestResultHandler(AzureRestResponse response);
        public event OnRestResultHandler OnRestResult;

        // ASM Object Cache (Subscription Context Specific)
        private List<Asm.VirtualNetwork> _VirtualNetworks;
        private List<Asm.RoleSize> _AsmRoleSizes;
        private List<Asm.StorageAccount> _StorageAccounts;
        private List<CloudService> _CloudServices;
        private List<ReservedIP> _AsmReservedIPs;

        // ARM Object Cache (Subscription Context Specific)
        private List<Arm.Location> _ArmLocations;
        private List<AzureTenant> _ArmTenants;
        private List<AzureSubscription> _ArmSubscriptions;
        private List<MigrationTarget.AvailabilitySet> _MigrationAvailabilitySets;

        private Dictionary<string, AzureRestResponse> _RestApiCache = new Dictionary<string, AzureRestResponse>();
        private Dictionary<AzureSubscription, AzureSubscriptionResourceCache> _AzureSubscriptionResourceCaches = new Dictionary<AzureSubscription, AzureSubscriptionResourceCache>();

        private AzureRetriever() { }

        public AzureRetriever(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        public AzureSubscription SubscriptionContext
        {
            get { return _AzureSubscription; }
        }

        public void ClearCache()
        {
            _RestApiCache = new Dictionary<string, AzureRestResponse>();
            _ArmLocations = null;
            _ArmTenants = null;
            _ArmSubscriptions = null;
            _MigrationAvailabilitySets = null;
            _VirtualNetworks = null;
            _StorageAccounts = null;
            _CloudServices = null;
    }

        public void LoadRestCache(string filepath)
        {
            StreamReader reader = new StreamReader(filepath);
            _RestApiCache = JsonConvert.DeserializeObject<Dictionary<string, AzureRestResponse>>(reader.ReadToEnd());
        }

        public void SaveRestCache()
        {
            string jsontext = JsonConvert.SerializeObject(_RestApiCache, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz";
            if (!Directory.Exists(filedir)) { Directory.CreateDirectory(filedir); }

            string filePath = filedir + "\\AzureRestResponse-" + this._AzureContext.AzureEnvironment.ToString() + "-" + this._AzureContext.TokenProvider.AuthenticationResult.UserInfo.DisplayableId + ".json";

            StreamWriter saveSelectionWriter = new StreamWriter(filePath);
            saveSelectionWriter.Write(jsontext);
            saveSelectionWriter.Close();
        }

        protected XmlDocument RemoveXmlns(String xml)
        {
            if (!xml.StartsWith("<"))
            {
                xml = $"<data>{xml}</data>";
            }
            XDocument d = XDocument.Parse(xml);
            d.Root.DescendantsAndSelf().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in d.Descendants())
                elem.Name = elem.Name.LocalName;

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(d.CreateReader());

            return xmlDocument;
        }

        private void writeRetreiverResultToLog(Guid requestGuid, string method, string url, string xml)
        {
            lock (_lockObject)
            {
                string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz\\MigAz-XML-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                string text = DateTime.Now.ToString() + "  " + requestGuid.ToString() + "  " + url + Environment.NewLine;
                File.AppendAllText(logfilepath, text);
                File.AppendAllText(logfilepath, xml + Environment.NewLine);
                File.AppendAllText(logfilepath, Environment.NewLine);
            }

            _AzureContext.LogProvider.WriteLog(method, requestGuid.ToString() + " Received REST Response");
        }


        public Dictionary<string, string> GetVMLBMapping(string cloudservicename, XmlNodeList roles)
        {
            Dictionary<string, string> aslbnamemapping = new Dictionary<string, string>();
            Dictionary<string, string> aslbnamemapping2 = new Dictionary<string, string>();

            foreach (XmlNode role in roles)
            {
                string availabilitysetname = String.Empty;
                if (role.SelectNodes("AvailabilitySetName").Count > 0)
                {
                    availabilitysetname = role.SelectSingleNode("AvailabilitySetName").InnerText;
                }

                if (!aslbnamemapping.ContainsKey(availabilitysetname))
                {
                    aslbnamemapping.Add(availabilitysetname, String.Empty);
                }
            }

            if (aslbnamemapping.Count == 1)
            {
                foreach (KeyValuePair<string, string> keyvaluepair in aslbnamemapping)
                {
                    aslbnamemapping2.Add(keyvaluepair.Key, cloudservicename);
                }
            }
            else
            {
                int increment = 1;
                foreach (KeyValuePair<string, string> keyvaluepair in aslbnamemapping)
                {
                    aslbnamemapping2.Add(keyvaluepair.Key, cloudservicename + "-LB" + increment.ToString());
                    increment += 1;
                }
            }

            Dictionary<string, string> vmlbmapping = new Dictionary<string, string>();

            foreach (XmlNode role in roles)
            {
                string virtualmachinename = role.SelectSingleNode("RoleName").InnerText;
                string availabilitysetname = String.Empty;
                if (role.SelectNodes("AvailabilitySetName").Count > 0)
                {
                    availabilitysetname = role.SelectSingleNode("AvailabilitySetName").InnerText;
                }
                string loadbalancername = aslbnamemapping2[availabilitysetname];

                vmlbmapping.Add(virtualmachinename, loadbalancername);
            }

            return vmlbmapping;
        }

        internal async Task<RoleSize> GetAzureASMRoleSize(string roleSize)
        {
            List<Asm.RoleSize> asmRoleSizes = await this.GetAzureASMRoleSizes();
            return asmRoleSizes.Where(a => a.Name == roleSize).FirstOrDefault();
        }

        public async Task SetSubscriptionContext(AzureSubscription azureSubscription)
        {
            _AzureSubscription = azureSubscription;
            await this._AzureContext.TokenProvider.GetToken(azureSubscription);
        }

        public async Task<List<AzureSubscription>> GetSubscriptions()
        {
            if (_AzureSubscriptions != null)
                return _AzureSubscriptions;

            _AzureSubscriptions = new List<AzureSubscription>();
            XmlDocument subscriptionsXml = await this.GetAzureAsmResources("Subscriptions", null);
            foreach (XmlNode subscriptionXml in subscriptionsXml.SelectNodes("//Subscription"))
            {
                AzureSubscription azureSubscription = new AzureSubscription(subscriptionXml, this._AzureContext.AzureEnvironment);
                _AzureSubscriptions.Add(azureSubscription);
            }

            return _AzureSubscriptions;
        }

        #region ASM Methods

        public async virtual Task<List<Asm.Location>> GetAzureASMLocations()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureASMLocations", "Start");

            XmlNode locationsXml = await this.GetAzureAsmResources("Locations", null);
            List<Asm.Location> azureLocations = new List<Asm.Location>();
            foreach (XmlNode locationXml in locationsXml.SelectNodes("/Locations/Location"))
            {
                azureLocations.Add(new Asm.Location(_AzureContext, locationXml));
            }

            return azureLocations.OrderBy(a => a.DisplayName).ToList();
        }

        public async virtual Task<List<ReservedIP>> GetAzureAsmReservedIPs()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmReservedIPs", "Start");

            if (_AsmReservedIPs != null)
                return _AsmReservedIPs;

            _AsmReservedIPs = new List<ReservedIP>();
            XmlDocument reservedIPsXml = await this.GetAzureAsmResources("ReservedIPs", null);
            foreach (XmlNode reservedIPXml in reservedIPsXml.SelectNodes("/ReservedIPs/ReservedIP"))
            {
                _AsmReservedIPs.Add(new ReservedIP(_AzureContext, reservedIPXml));
            }

            return _AsmReservedIPs;
        }

        public async virtual Task<List<Asm.StorageAccount>> GetAzureAsmStorageAccounts()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmStorageAccounts", "Start");

            if (_StorageAccounts != null)
                return _StorageAccounts;

            _StorageAccounts = new List<Asm.StorageAccount>();
            XmlDocument storageAccountsXml = await this.GetAzureAsmResources("StorageAccounts", null);
            foreach (XmlNode storageAccountXml in storageAccountsXml.SelectNodes("//StorageService"))
            {
                Asm.StorageAccount asmStorageAccount = new Asm.StorageAccount(_AzureContext, storageAccountXml);
                _StorageAccounts.Add(asmStorageAccount);
            }

            _AzureContext.StatusProvider.UpdateStatus("BUSY: Loading Storage Account Keys");
            List<Task> storageAccountKeyTasks = new List<Task>();
            foreach (Asm.StorageAccount asmStorageAccount in _StorageAccounts)
            {
                storageAccountKeyTasks.Add(asmStorageAccount.LoadStorageAccountKeysAsynch());
            }
            await Task.WhenAll(storageAccountKeyTasks);

            return _StorageAccounts;
        }

        public async virtual Task<Asm.StorageAccount> GetAzureAsmStorageAccount(string storageAccountName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmStorageAccount", "Start");

            if (storageAccountName == null)
                return null;

            foreach (Asm.StorageAccount asmStorageAccount in await this.GetAzureAsmStorageAccounts())
            {
                if (asmStorageAccount.Name == storageAccountName)
                    return asmStorageAccount;
            }

            return null;
        }

        public async virtual Task<List<Asm.VirtualNetwork>> GetAzureAsmVirtualNetworks()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetworks", "Start");

            if (_VirtualNetworks != null)
                return _VirtualNetworks;

            _VirtualNetworks = new List<Asm.VirtualNetwork>();
            foreach (XmlNode virtualnetworksite in (await this.GetAzureAsmResources("VirtualNetworks", null)).SelectNodes("//VirtualNetworkSite"))
            {
                Asm.VirtualNetwork asmVirtualNetwork = new Asm.VirtualNetwork(_AzureContext, virtualnetworksite);
                await asmVirtualNetwork.InitializeChildrenAsync();
                _VirtualNetworks.Add(asmVirtualNetwork);
            }
            return _VirtualNetworks;
        }

        public async virtual Task<List<Asm.RoleSize>> GetAzureASMRoleSizes()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureASMRoleSizes", "Start");

            if (_AsmRoleSizes != null)
                return _AsmRoleSizes;

            _AsmRoleSizes = new List<Asm.RoleSize>();
            foreach (XmlNode roleSizeNode in (await this.GetAzureAsmResources("RoleSize", null)).SelectNodes("//RoleSize"))
            {
                Asm.RoleSize asmRoleSize = new Asm.RoleSize(_AzureContext, roleSizeNode);
                _AsmRoleSizes.Add(asmRoleSize);
            }
            return _AsmRoleSizes;
        }

        public async Task<Asm.VirtualNetwork> GetAzureAsmVirtualNetwork(string virtualNetworkName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetwork", "Start");

            foreach (Asm.VirtualNetwork asmVirtualNetwork in await this.GetAzureAsmVirtualNetworks())
            {
                if (asmVirtualNetwork.Name == virtualNetworkName)
                {
                    return asmVirtualNetwork;
                }
            }

            return null;
        }

        internal async Task<AffinityGroup> GetAzureAsmAffinityGroup(string affinityGroupName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmAffinityGroup", "Start");

            Hashtable affinitygroupinfo = new Hashtable();
            affinitygroupinfo.Add("affinitygroupname", affinityGroupName);

            XmlNode affinityGroupXml = await this.GetAzureAsmResources("AffinityGroup", affinitygroupinfo);
            AffinityGroup asmAffinityGroup = new AffinityGroup(_AzureContext, affinityGroupXml.SelectSingleNode("AffinityGroup"));
            return asmAffinityGroup;
        }

        public async virtual Task<Asm.NetworkSecurityGroup> GetAzureAsmNetworkSecurityGroup(string networkSecurityGroupName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmNetworkSecurityGroup", "Start");

            Hashtable networkSecurityGroupInfo = new Hashtable();
            networkSecurityGroupInfo.Add("name", networkSecurityGroupName);

            XmlNode networkSecurityGroupXml = await this.GetAzureAsmResources("NetworkSecurityGroup", networkSecurityGroupInfo);
            Asm.NetworkSecurityGroup asmNetworkSecurityGroup = new Asm.NetworkSecurityGroup(_AzureContext, networkSecurityGroupXml.SelectSingleNode("NetworkSecurityGroup"));
            return asmNetworkSecurityGroup;
        }

        public async virtual Task<List<Asm.NetworkSecurityGroup>> GetAzureAsmNetworkSecurityGroups()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmNetworkSecurityGroups", "Start");

            List<Asm.NetworkSecurityGroup> networkSecurityGroups = new List<Asm.NetworkSecurityGroup>();

            XmlDocument x = await this.GetAzureAsmResources("NetworkSecurityGroups", null);
            foreach (XmlNode networkSecurityGroupNode in (await this.GetAzureAsmResources("NetworkSecurityGroups", null)).SelectNodes("//NetworkSecurityGroup"))
            {
                Asm.NetworkSecurityGroup asmNetworkSecurityGroup = new Asm.NetworkSecurityGroup(_AzureContext, networkSecurityGroupNode);
                networkSecurityGroups.Add(asmNetworkSecurityGroup);
            }

            return networkSecurityGroups;
        }

        public async virtual Task<Asm.RouteTable> GetAzureAsmRouteTable(string routeTableName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmRouteTable", "Start");

            Hashtable info = new Hashtable();
            info.Add("name", routeTableName);
            XmlDocument routeTableXml = await this.GetAzureAsmResources("RouteTable", info);
            return new Asm.RouteTable(_AzureContext, routeTableXml);
        }

        internal async Task<Asm.VirtualMachine> GetAzureAsmVirtualMachine(CloudService asmCloudService, string virtualMachineName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualMachine", "Start");

            Hashtable vmDetails = await this.GetVMDetails(asmCloudService.Name, virtualMachineName);
            XmlDocument virtualMachineXml = await this.GetAzureAsmResources("VirtualMachine", vmDetails);
            Asm.VirtualMachine asmVirtualMachine = new Asm.VirtualMachine(this._AzureContext, asmCloudService, this._AzureContext.SettingsProvider, virtualMachineXml, vmDetails);
            await asmVirtualMachine.InitializeChildren();

            return asmVirtualMachine;
        }

        private async Task<Hashtable> GetVMDetails(string cloudServiceName, string virtualMachineName)
        {
            _AzureContext.LogProvider.WriteLog("GetVMDetails", "Start");

            Hashtable cloudserviceinfo = new Hashtable();
            cloudserviceinfo.Add("name", cloudServiceName);

            Hashtable virtualmachineinfo = new Hashtable();
            virtualmachineinfo.Add("cloudservicename", cloudServiceName);
            virtualmachineinfo.Add("virtualmachinename", virtualMachineName);

            XmlDocument hostedservice = await GetAzureAsmResources("CloudService", cloudserviceinfo);
            if (hostedservice.SelectNodes("//Deployments/Deployment").Count > 0)
            {
                if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectNodes("RoleType").Count > 0)
                {
                    if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectSingleNode("RoleType").InnerText == "PersistentVMRole")
                    {
                        if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName") != null)
                        {
                            virtualmachineinfo.Add("virtualnetworkname", hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName").InnerText);
                        }
                        else
                        {
                            virtualmachineinfo.Add("virtualnetworkname", String.Empty);
                        }

                        virtualmachineinfo.Add("deploymentname", hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectSingleNode("Name").InnerText);

                        XmlNodeList roles = hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role");
                        // GetVMLBMapping is necessary because a Cloud Service can have multiple availability sets
                        // On ARM, a load balancer can only be attached to 1 availability set
                        // Because of this, if multiple availability sets exist, we are breaking the cloud service in multiple load balancers
                        //     to respect all availability sets
                        Dictionary<string, string> vmlbmapping = GetVMLBMapping(cloudServiceName, roles);
                        foreach (XmlNode role in roles)
                        {
                            string currentVM = role.SelectSingleNode("RoleName").InnerText;
                            if (currentVM == virtualMachineName)
                            {
                                virtualmachineinfo.Add("loadbalancername", vmlbmapping[virtualMachineName]);
                                return virtualmachineinfo;
                            }
                        }
                    }
                }
            }
            throw new InvalidOperationException("Requested VM could not be found");
        }

        public async virtual Task<List<CloudService>> GetAzureAsmCloudServices()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmCloudServices", "Start");

            if (_CloudServices != null)
                return _CloudServices;

            XmlDocument cloudServicesXml = await this.GetAzureAsmResources("CloudServices", null);
            _CloudServices = new List<CloudService>();
            foreach (XmlNode cloudServiceXml in cloudServicesXml.SelectNodes("//HostedService"))
            {
                CloudService tempCloudService = new CloudService(_AzureContext, cloudServiceXml);

                Hashtable cloudServiceInfo = new Hashtable();
                cloudServiceInfo.Add("name", tempCloudService.Name);
                XmlDocument cloudServiceDetailXml = await this.GetAzureAsmResources("CloudService", cloudServiceInfo);
                CloudService asmCloudService = new CloudService(_AzureContext, cloudServiceDetailXml);

                _CloudServices.Add(asmCloudService);
            }

            List<Task> cloudServiceVMTasks = new List<Task>();
            foreach (CloudService asmCloudService in _CloudServices)
            {
                cloudServiceVMTasks.Add(asmCloudService.LoadChildrenAsync());
            }

            await Task.WhenAll(cloudServiceVMTasks);

            return _CloudServices;
        }

        public async virtual Task<CloudService> GetAzureAsmCloudService(string cloudServiceName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmCloudService", "Start");

            foreach (CloudService asmCloudService in await this.GetAzureAsmCloudServices())
            {
                if (asmCloudService.Name == cloudServiceName)
                    return asmCloudService;
            }

            return null;
        }

        public async Task<StorageAccountKeys> GetAzureAsmStorageAccountKeys(string storageAccountName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmStorageAccountKeys", "Start");

            Hashtable storageAccountInfo = new Hashtable();
            storageAccountInfo.Add("name", storageAccountName);

            XmlDocument storageAccountKeysXml = await this.GetAzureAsmResources("StorageAccountKeys", storageAccountInfo);
            return new StorageAccountKeys(_AzureContext, storageAccountKeysXml);
        }

        public async virtual Task<List<ClientRootCertificate>> GetAzureAsmClientRootCertificates(Asm.VirtualNetwork asmVirtualNetwork)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmClientRootCertificates", "Start");

            Hashtable virtualNetworkInfo = new Hashtable();
            virtualNetworkInfo.Add("virtualnetworkname", asmVirtualNetwork.Name);
            XmlDocument clientRootCertificatesXml = await this.GetAzureAsmResources("ClientRootCertificates", virtualNetworkInfo);

            List<ClientRootCertificate> asmClientRootCertificates = new List<ClientRootCertificate>();
            foreach (XmlNode clientRootCertificateXml in clientRootCertificatesXml.SelectNodes("//ClientRootCertificate"))
            {
                ClientRootCertificate asmClientRootCertificate = new ClientRootCertificate(_AzureContext, asmVirtualNetwork, clientRootCertificateXml);
                await asmClientRootCertificate.InitializeChildrenAsync();
                asmClientRootCertificates.Add(asmClientRootCertificate);
            }

            return asmClientRootCertificates;
        }

        public async Task<XmlDocument> GetAzureAsmClientRootCertificateData(Asm.VirtualNetwork asmVirtualNetwork, string certificateThumbprint)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmClientRootCertificateData", "Start");

            Hashtable certificateInfo = new Hashtable();
            certificateInfo.Add("virtualnetworkname", asmVirtualNetwork.Name);
            certificateInfo.Add("thumbprint", certificateThumbprint);
            XmlDocument clientRootCertificateXml = await this.GetAzureAsmResources("ClientRootCertificate", certificateInfo);
            return clientRootCertificateXml;
        }

        public async virtual Task<Asm.VirtualNetworkGateway> GetAzureAsmVirtualNetworkGateway(Asm.VirtualNetwork asmVirtualNetwork)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetworkGateway", "Start");

            Hashtable virtualNetworkGatewayInfo = new Hashtable();
            virtualNetworkGatewayInfo.Add("virtualnetworkname", asmVirtualNetwork.Name);
            virtualNetworkGatewayInfo.Add("localnetworksitename", String.Empty);

            XmlDocument gatewayXml = await this.GetAzureAsmResources("VirtualNetworkGateway", virtualNetworkGatewayInfo);
            return new Asm.VirtualNetworkGateway(_AzureContext, asmVirtualNetwork, gatewayXml);
        }

        public async virtual Task<string> GetAzureAsmVirtualNetworkSharedKey(string virtualNetworkName, string localNetworkSiteName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetworkSharedKey", "Start");

            try
            {
                Hashtable virtualNetworkGatewayInfo = new Hashtable();
                virtualNetworkGatewayInfo.Add("virtualnetworkname", virtualNetworkName);
                virtualNetworkGatewayInfo.Add("localnetworksitename", localNetworkSiteName);

                XmlDocument connectionShareKeyXml = await this.GetAzureAsmResources("VirtualNetworkGatewaySharedKey", virtualNetworkGatewayInfo);
                if (connectionShareKeyXml.SelectSingleNode("//Value") == null)
                    return String.Empty;

                return connectionShareKeyXml.SelectSingleNode("//Value").InnerText;
            }
            catch (Exception exc)
            {
                _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetworkSharedKey", "Exception: " + exc.Message + exc.StackTrace);
                return String.Empty;
            }
        }

        #endregion

        #region ARM Methods

        public async Task<List<AzureTenant>> GetAzureARMTenants()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMTenants", "Start");

            if (_ArmTenants != null)
                return _ArmTenants;

            JObject tenantsJson = await this.GetAzureARMResources("Tenants", null, null);

            var tenants = from tenant in tenantsJson["value"]
                          select tenant;

            _ArmTenants = new List<AzureTenant>();

            foreach (JObject tenantJson in tenants)
            {
                AzureTenant azureTenant = new AzureTenant(tenantJson, _AzureContext);
                await azureTenant.InitializeChildren();
                _ArmTenants.Add(azureTenant);
            }

            return _ArmTenants;
        }

        public async Task<List<AzureDomain>> GetAzureARMDomains(AzureTenant azureTenant)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMDomains", "Start");

            Hashtable info = new Hashtable();
            info.Add("tenantId", azureTenant.TenantId);

            JObject domainsJson = await this.GetAzureARMResources("Domains", null, info);

            var domains = from domain in domainsJson["value"]
                          select domain;

            List<AzureDomain> armTenantDomains = new List<AzureDomain>();

            foreach (JObject domainJson in domains)
            {
                AzureDomain azureDomain = new AzureDomain(domainJson, _AzureContext);
                armTenantDomains.Add(azureDomain);
            }

            return armTenantDomains;
        }

        public async Task<List<AzureSubscription>> GetAzureARMSubscriptions()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMSubscriptions", "Start");

            if (_ArmSubscriptions != null)
                return _ArmSubscriptions;

            JObject subscriptionsJson = await this.GetAzureARMResources("Subscriptions", null, null);

            var subscriptions = from subscription in subscriptionsJson["value"]
                                select subscription;

            _ArmSubscriptions = new List<AzureSubscription>();

            foreach (JObject azureSubscriptionJson in subscriptions)
            {
                AzureSubscription azureSubscription = new AzureSubscription(azureSubscriptionJson, null, _AzureContext.AzureEnvironment);
                _ArmSubscriptions.Add(azureSubscription);
            }

            return _ArmSubscriptions;
        }

        public async Task<List<AzureSubscription>> GetAzureARMSubscriptions(AzureTenant azureTenant)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMSubscriptions", "Start");

            Hashtable info = new Hashtable();
            info.Add("tenantId", azureTenant.TenantId);

            JObject subscriptionsJson = await this.GetAzureARMResources("Subscriptions", null, info);

            var subscriptions = from subscription in subscriptionsJson["value"]
                                select subscription;

            List<AzureSubscription> tenantSubscriptions = new List<AzureSubscription>();

            foreach (JObject azureSubscriptionJson in subscriptions)
            {
                AzureSubscription azureSubscription = new AzureSubscription(azureSubscriptionJson, azureTenant, _AzureContext.AzureEnvironment);
                tenantSubscriptions.Add(azureSubscription);
            }

            return tenantSubscriptions;
        }

        public async Task<List<ResourceGroup>> GetAzureARMResourceGroups()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMResourceGroups", "Start");

            if (this._AzureSubscription.ArmResourceGroups.Count > 0)
                return this._AzureSubscription.ArmResourceGroups;

            JObject resourceGroupsJson = await this.GetAzureARMResources("ResourceGroups", null, null);

            var resourceGroups = from resourceGroup in resourceGroupsJson["value"]
                                 select resourceGroup;

            foreach (JObject resourceGroupJson in resourceGroups)
            {
                ResourceGroup resourceGroup = new ResourceGroup(resourceGroupJson, _AzureContext.AzureEnvironment, _AzureContext.AzureSubscription);
                await resourceGroup.InitializeChildrenAsync(_AzureContext);
                this._AzureSubscription.ArmResourceGroups.Add(resourceGroup);
                _AzureContext.LogProvider.WriteLog("GetAzureARMResourceGroups", "Loaded ARM Resource Group '" + resourceGroup.Name + "'.");

            }

            return this._AzureSubscription.ArmResourceGroups;
        }

        public virtual Arm.VirtualNetwork GetAzureARMVirtualNetwork(AzureSubscription azureSubscription, string virtualNetworkId)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetwork", "Start");

            if (azureSubscription == null || azureSubscription.ArmVirtualNetworks == null)
                return null;

            if (virtualNetworkId.ToLower().Contains("/subnets/"))
                virtualNetworkId = virtualNetworkId.Substring(0, virtualNetworkId.ToLower().IndexOf("/subnets/"));

            foreach (List<Arm.VirtualNetwork> listVirtualNetworks in azureSubscription.ArmVirtualNetworks.Values)
            {
                foreach (Arm.VirtualNetwork armVirtualNetwork in listVirtualNetworks)
                {
                    if (String.Compare(armVirtualNetwork.Id, virtualNetworkId, true) == 0)
                        return armVirtualNetwork;
                }
            }

            return null;
        }

        public async Task<List<Arm.VirtualNetwork>> GetAzureARMVirtualNetworks()
        {
            List<Arm.VirtualNetwork> virtualNetworks = new List<Arm.VirtualNetwork>();

            foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
            {
                foreach (Arm.VirtualNetwork virtualNetwork in  await this.GetAzureARMVirtualNetworks(resourceGroup))
                {
                    virtualNetworks.Add(virtualNetwork);
                }
            }

            return virtualNetworks;
        }

        public async Task<List<Arm.VirtualNetwork>> GetAzureARMVirtualNetworks(Arm.Location azureLocation)
        {
            List<Arm.VirtualNetwork> virtualNetworks = new List<Arm.VirtualNetwork>();

            foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
            {
                foreach (Arm.VirtualNetwork virtualNetwork in await this.GetAzureARMVirtualNetworks(resourceGroup))
                {
                    if (virtualNetwork.Location.Name == azureLocation.Name)
                        virtualNetworks.Add(virtualNetwork);
                }
            }

            return virtualNetworks;
        }

        public async virtual Task<List<Arm.VirtualNetwork>> GetAzureARMVirtualNetworks(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetworks", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmVirtualNetworks.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmVirtualNetworks[resourceGroup];

            JObject virtualNetworksJson = await this.GetAzureARMResources("VirtualNetworks", resourceGroup, null);

            var virtualNetworks = from vnet in virtualNetworksJson["value"]
                                  select vnet;

            List<Arm.VirtualNetwork> resourceGroupVirtualNetworks = new List<Arm.VirtualNetwork>();

            foreach (var virtualNetwork in virtualNetworks)
            {
                Arm.VirtualNetwork armVirtualNetwork = new Arm.VirtualNetwork(virtualNetwork);

                await armVirtualNetwork.InitializeChildrenAsync(_AzureContext);
                foreach (Arm.VirtualNetworkGateway v in await _AzureContext.AzureRetriever.GetAzureARMVirtualNetworkGateways(resourceGroup))
                {
                    // todo now asap, why is this here
                }

                resourceGroupVirtualNetworks.Add(armVirtualNetwork);
                _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetworks", "Loaded ARM Virtual Network '" + armVirtualNetwork.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Virtual Network '" + armVirtualNetwork.Name + "'.");

            }

            resourceGroup.AzureSubscription.ArmVirtualNetworks.Add(resourceGroup, resourceGroupVirtualNetworks);
            return resourceGroupVirtualNetworks;
        }

        public async virtual Task<List<Arm.ManagedDisk>> GetAzureARMManagedDisks(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMManagedDisks", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmManagedDisks.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmManagedDisks[resourceGroup];

            JObject managedDisksJson = await this.GetAzureARMResources("ManagedDisks", resourceGroup, null);

            var managedDisks = from managedDisk in managedDisksJson["value"]
                                  select managedDisk;

            List<Arm.ManagedDisk> resourceGroupManagedDisks = new List<Arm.ManagedDisk>();

            foreach (var managedDisk in managedDisks)
            {
                Arm.ManagedDisk armManagedDisk = new Arm.ManagedDisk(managedDisk);
                await armManagedDisk.InitializeChildrenAsync(_AzureContext);
                resourceGroupManagedDisks.Add(armManagedDisk);
            }

            resourceGroup.AzureSubscription.ArmManagedDisks.Add(resourceGroup, resourceGroupManagedDisks);
            return resourceGroupManagedDisks;
        }

        public async Task<List<Arm.StorageAccount>> GetAzureARMStorageAccounts()
        {
            List<Arm.StorageAccount> storageAccounts = new List<Arm.StorageAccount>();

            foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
            {
                foreach (Arm.StorageAccount storageAccount in await this.GetAzureARMStorageAccounts(resourceGroup))
                {
                    storageAccounts.Add(storageAccount);
                }
            }

            return storageAccounts;
        }

        public async virtual Task<List<Arm.StorageAccount>> GetAzureARMStorageAccounts(Arm.Location azureLocation)
        {
            List<Arm.StorageAccount> storageAccounts = new List<Arm.StorageAccount>();

            foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
            {
                foreach (Arm.StorageAccount storageAccount in await this.GetAzureARMStorageAccounts(resourceGroup))
                {
                    if (storageAccount.Location.Name == azureLocation.Name)
                        storageAccounts.Add(storageAccount);
                }
            }

            return storageAccounts;
        }

        public async virtual Task<List<Arm.StorageAccount>> GetAzureARMStorageAccounts(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMStorageAccounts", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmStorageAccounts.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmStorageAccounts[resourceGroup];

            JObject storageAccountsJson = await this.GetAzureARMResources("StorageAccounts", resourceGroup, null);

            var storageAccounts = from storage in storageAccountsJson["value"]
                                  select storage;

            List<Arm.StorageAccount> resouceGroupStorageAccounts = new List<Arm.StorageAccount>();

            foreach (var storageAccount in storageAccounts)
            {
                Arm.StorageAccount armStorageAccount = new Arm.StorageAccount(storageAccount, _AzureContext);
                await armStorageAccount.InitializeChildrenAsync(_AzureContext);
                armStorageAccount.ResourceGroup = await this.GetAzureARMResourceGroup(armStorageAccount.Id);
                _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetworks", "Loaded ARM Storage Account '" + armStorageAccount.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Storage Account '" + armStorageAccount.Name + "'.");


                await this.GetAzureARMStorageAccountKeys(armStorageAccount);

                resouceGroupStorageAccounts.Add(armStorageAccount);
            }

            resourceGroup.AzureSubscription.ArmStorageAccounts.Add(resourceGroup, resouceGroupStorageAccounts);
            return resouceGroupStorageAccounts;
        }

        public virtual Arm.StorageAccount GetAzureARMStorageAccount(AzureSubscription azureSubscription, string name)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMStorageAccount", "Start");

            if (azureSubscription == null || azureSubscription.ArmStorageAccounts == null)
                return null;

            foreach (List<Arm.StorageAccount> listStorageAccounts in azureSubscription.ArmStorageAccounts.Values)
            {
                foreach (Arm.StorageAccount armStorageAccount in listStorageAccounts)
                {
                    if (String.Compare(armStorageAccount.Name, name, true) == 0)
                        return armStorageAccount;
                }
            }

            return null;
        }

        public async Task<Arm.Location> GetAzureARMLocation(string location)
        {
            List<Arm.Location> armLocations = await this.GetAzureARMLocations();
            Arm.Location matchedLocation = armLocations.Where(a => a.DisplayName == location).FirstOrDefault();

            if (matchedLocation == null)
                matchedLocation = armLocations.Where(a => a.Name == location).FirstOrDefault();

            return matchedLocation;
        }

        public async virtual Task<List<Arm.Location>> GetAzureARMLocations()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMLocations", "Start");

            if (_ArmLocations != null)
                return _ArmLocations;

            JObject locationsJson = await this.GetAzureARMResources("Locations", null, null);

            var locations = from location in locationsJson["value"]
                                  select location;

            _ArmLocations = new List<Arm.Location>();

            foreach (var location in locations)
            {
                Arm.Location armLocation = new Arm.Location(_AzureContext, location);
                _ArmLocations.Add(armLocation);
            }

            List<Task> armLocationChildTasks = new List<Task>();
            foreach (Azure.Arm.Location armLocation in _ArmLocations)
            {
                Task armLocationChildTask = armLocation.InitializeChildrenAsync();
                armLocationChildTasks.Add(armLocationChildTask);
            }
            await Task.WhenAll(armLocationChildTasks.ToArray());

            return _ArmLocations;
        }

        internal async Task GetAzureARMLocationVMSizes(Arm.Location location)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMLocationVMSizes", "Start - Location : " + location.Name);

            string methodType = "GET";
            bool useCached = true;

            if (_AzureContext == null)
                throw new ArgumentNullException("AzureContext is null.  Unable to call Azure API without Azure Context.");
            if (_AzureContext.TokenProvider == null)
                throw new ArgumentNullException("TokenProvider Context is null.  Unable to call Azure API without TokenProvider.");
            if (_AzureContext.TokenProvider.AccessToken == null)
                throw new ArgumentNullException("AccessToken Context is null.  Unable to call Azure API without AccessToken.");

            String accessToken = _AzureContext.TokenProvider.AccessToken;

            // https://docs.microsoft.com/en-us/rest/api/compute/virtualmachines/virtualmachines-list-sizes-region
            string url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + String.Format(ArmConst.ProviderVMSizes, location.Name) + "?api-version=2016-04-30-preview";
            _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Azure VMSizes for Subscription ID : " + _AzureSubscription.SubscriptionId + " Location : " + location);

            AzureRestRequest azureRestRequest = new AzureRestRequest(url, accessToken, methodType, useCached);
            AzureRestResponse azureRestResponse = await GetAzureRestResponse(azureRestRequest);
            JObject locationsVMSizesJson = JObject.Parse(azureRestResponse.Response);

            _AzureContext.StatusProvider.UpdateStatus("BUSY: Loading VMSizes for Subscription ID : " + _AzureSubscription.SubscriptionId + " Location : " + location);

            var VMSizes = from VMSize in locationsVMSizesJson["value"]
                            select VMSize;

            List<VMSize> vmSizes = new List<VMSize>();
            foreach (var VMSize in VMSizes)
            {
                Arm.VMSize armVMSize = new Arm.VMSize(VMSize);
                vmSizes.Add(armVMSize);
            }

            location.VMSizes = vmSizes.OrderBy(a => a.Name).ToList();

            return;
        }

        public async Task<List<Arm.VirtualMachine>> GetAzureArmVirtualMachines(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureArmVirtualMachines", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmVirtualMachines.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmVirtualMachines[resourceGroup];

            JObject virtualMachineJson = await this.GetAzureARMResources("VirtualMachines", resourceGroup, null);

            var virtualMachines = from virtualMachine in virtualMachineJson["value"]
                            select virtualMachine;

            List<Arm.VirtualMachine> resourceGroupVirtualMachines = new List<Arm.VirtualMachine>();

            foreach (var virtualMachine in virtualMachines)
            {
                Arm.VirtualMachine armVirtualMachine = new Arm.VirtualMachine(virtualMachine);
                await armVirtualMachine.InitializeChildrenAsync(_AzureContext);
                resourceGroupVirtualMachines.Add(armVirtualMachine);
                _AzureContext.LogProvider.WriteLog("GetAzureArmVirtualMachines", "Loaded ARM Virtual Machine '" + armVirtualMachine.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Virtual Machine '" + armVirtualMachine.Name + "'.");
            }

            resourceGroup.AzureSubscription.ArmVirtualMachines.Add(resourceGroup, resourceGroupVirtualMachines);
            return resourceGroupVirtualMachines;
        }

        internal async Task GetAzureARMStorageAccountKeys(Arm.StorageAccount armStorageAccount)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMStorageAccountKeys", "Start - ARM Storage Account '" + armStorageAccount.Name + "'.");

            Hashtable storageAccountKeyInfo = new Hashtable();
            storageAccountKeyInfo.Add("ResourceGroupName", armStorageAccount.ResourceGroup.Name);
            storageAccountKeyInfo.Add("StorageAccountName", armStorageAccount.Name);

            JObject storageAccountKeysJson = await this.GetAzureARMResources("StorageAccountKeys", armStorageAccount.ResourceGroup, storageAccountKeyInfo);

            var storageAccountKeys = from keys in storageAccountKeysJson["keys"]
                                  select keys;

            armStorageAccount.Keys.Clear();
            foreach (var storageAccountKey in storageAccountKeys)
            {
                StorageAccountKey armStorageAccountKey = new StorageAccountKey(storageAccountKey);
                armStorageAccount.Keys.Add(armStorageAccountKey);
            }

            return;
        }

        public async Task<List<Arm.AvailabilitySet>> GetAzureARMAvailabilitySets(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMAvailabilitySets", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmAvailabilitySets.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmAvailabilitySets[resourceGroup];

            JObject availabilitySetJson = await this.GetAzureARMResources("AvailabilitySets", resourceGroup, null);

            var availabilitySets = from availabilitySet in availabilitySetJson["value"]
                                   select availabilitySet;

            List<Arm.AvailabilitySet> resourceGroupAvailabilitySets = new List<Arm.AvailabilitySet>();

            foreach (var availabilitySet in availabilitySets)
            {
                Arm.AvailabilitySet armAvailabilitySet = new Arm.AvailabilitySet(availabilitySet);
                await armAvailabilitySet.InitializeChildrenAsync(this._AzureContext);
                resourceGroupAvailabilitySets.Add(armAvailabilitySet);
            }

            resourceGroup.AzureSubscription.ArmAvailabilitySets.Add(resourceGroup, resourceGroupAvailabilitySets);
            return resourceGroupAvailabilitySets;
        }

        public Arm.AvailabilitySet GetAzureARMAvailabilitySet(AzureSubscription azureSubscription, string availabilitySetId)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMAvailabilitySet", "Start");

            if (azureSubscription == null || azureSubscription.ArmAvailabilitySets == null)
                return null;

            foreach (List<Arm.AvailabilitySet> listAvailabilitySet in azureSubscription.ArmAvailabilitySets.Values)
            {
                foreach (Arm.AvailabilitySet armAvailabilitySet in listAvailabilitySet)
                {
                    if (String.Compare(armAvailabilitySet.Id, availabilitySetId, true) == 0)
                        return armAvailabilitySet;
                }
            }

            return null;
        }

        public async Task<ResourceGroup> GetAzureARMResourceGroup(string id)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMResourceGroup", "Start");

            if (id != null && id != String.Empty)
            {
                string[] idSplit = id.Split('/');

                if (idSplit.Length >= 4)
                {
                    string seekResourceGroupId = "/" + idSplit[1] + "/" + idSplit[2] + "/" + idSplit[3] + "/" + idSplit[4];

                    foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
                    {
                        if (String.Equals(resourceGroup.Id, seekResourceGroupId, StringComparison.OrdinalIgnoreCase))
                            return resourceGroup;
                    }
                }
            }

            return null;
        }

        public async Task<List<Arm.NetworkInterface>> GetAzureARMNetworkInterfaces(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkInterfaces", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmNetworkInterfaces.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmNetworkInterfaces[resourceGroup];

            JObject networkInterfacesJson = await this.GetAzureARMResources("NetworkInterfaces", resourceGroup, null);

            var networkInterfaces = from networkInterface in networkInterfacesJson["value"]
                                   select networkInterface;

            List<Arm.NetworkInterface> resourceGroupNetworkInterfaces = new List<Arm.NetworkInterface>();

            foreach (var networkInterface in networkInterfaces)
            {
                Arm.NetworkInterface armNetworkInterface = new Arm.NetworkInterface(networkInterface);
                await armNetworkInterface.InitializeChildrenAsync(_AzureContext);
                resourceGroupNetworkInterfaces.Add(armNetworkInterface);
                _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkInterfaces", "Loaded ARM Network Interface '" + armNetworkInterface.Name + "'.");
            }

            resourceGroup.AzureSubscription.ArmNetworkInterfaces.Add(resourceGroup, resourceGroupNetworkInterfaces);
            return resourceGroupNetworkInterfaces;
        }

        public async Task<Arm.NetworkInterface> GetAzureARMNetworkInterface(string id)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkInterface", "Start");

            int providerIndexOf = id.ToLower().IndexOf(ArmConst.ProviderNetworkInterfaces.ToLower());
            int postNicSeperatorIndexOf = id.Substring(providerIndexOf + ArmConst.ProviderNetworkInterfaces.Length + 1).IndexOf("/");
            if (postNicSeperatorIndexOf > -1)
                id = id.Substring(0, providerIndexOf + ArmConst.ProviderNetworkInterfaces.Length + postNicSeperatorIndexOf + 1);

            foreach (Arm.NetworkInterface networkInterface in await this.GetAzureARMNetworkInterfaces(await GetAzureARMResourceGroup(id)))
            {
                if (String.Compare(networkInterface.Id, id, StringComparison.InvariantCultureIgnoreCase) == 0)
                    return networkInterface;
            }

            return null;
        }

        public async Task<List<Arm.VirtualNetworkGateway>> GetAzureARMVirtualNetworkGateways(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetworkGateways", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmVirtualNetworkGateways.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmVirtualNetworkGateways[resourceGroup];

            JObject virtualNetworkGatewaysJson = await this.GetAzureARMResources("VirtualNetworkGateways", resourceGroup, null);

            var virtualNetworkGateways = from virtualNetworkGateway in virtualNetworkGatewaysJson["value"]
                                    select virtualNetworkGateway;

            List<Arm.VirtualNetworkGateway> resourceGroupVirtualNetworkGateways = new List<Arm.VirtualNetworkGateway>();

            foreach (var virtualNetworkGateway in virtualNetworkGateways)
            {
                Arm.VirtualNetworkGateway armVirtualNetworkGateway = new Arm.VirtualNetworkGateway(virtualNetworkGateway);
                resourceGroupVirtualNetworkGateways.Add(armVirtualNetworkGateway);
            }

            resourceGroup.AzureSubscription.ArmVirtualNetworkGateways.Add(resourceGroup, resourceGroupVirtualNetworkGateways);
            return resourceGroupVirtualNetworkGateways;
        }

        
        public async Task<Arm.NetworkSecurityGroup> GetAzureARMNetworkSecurityGroup(string id)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkSecurityGroup", "Start");

            foreach (Arm.NetworkSecurityGroup networkSecurityGroup in await this.GetAzureARMNetworkSecurityGroups(await GetAzureARMResourceGroup(id)))
            {
                if (String.Compare(networkSecurityGroup.Id, id, StringComparison.InvariantCultureIgnoreCase) == 0)
                    return networkSecurityGroup;
            }

            return null;
        }

        public async Task<List<Arm.NetworkSecurityGroup>> GetAzureARMNetworkSecurityGroups(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkSecurityGroups", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmNetworkSecurityGroups.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmNetworkSecurityGroups[resourceGroup];

            JObject networkSecurityGroupsJson = await this.GetAzureARMResources("NetworkSecurityGroups", resourceGroup, null);

            var networkSecurityGroups = from networkSecurityGroup in networkSecurityGroupsJson["value"]
                                    select networkSecurityGroup;

            List<Arm.NetworkSecurityGroup> resourceGroupNetworkSecurityGroups = new List<Arm.NetworkSecurityGroup>();

            foreach (var networkSecurityGroup in networkSecurityGroups)
            {
                Arm.NetworkSecurityGroup armNetworkSecurityGroup = new Arm.NetworkSecurityGroup(networkSecurityGroup);
                await armNetworkSecurityGroup.InitializeChildrenAsync(this._AzureContext);
                resourceGroupNetworkSecurityGroups.Add(armNetworkSecurityGroup);
                _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkSecurityGroups", "Loaded ARM Network Security Group '" + armNetworkSecurityGroup.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Network Security Group '" + armNetworkSecurityGroup.Name + "'.");
            }

            resourceGroup.AzureSubscription.ArmNetworkSecurityGroups.Add(resourceGroup, resourceGroupNetworkSecurityGroups);
            return resourceGroupNetworkSecurityGroups;
        }

        public async Task<PublicIP> GetAzureARMPublicIP(string id)
        {
            ResourceGroup resourceGroup = await this.GetAzureARMResourceGroup(id);
            if (resourceGroup == null)
                return null;

            foreach (PublicIP publicIp in await this.GetAzureARMPublicIPs(resourceGroup))
            {
                if (String.Compare(publicIp.Id, id) == 0)
                    return publicIp;
            }

            return null;
        }

        public async Task<List<Arm.PublicIP>> GetAzureARMPublicIPs(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMPublicIPs", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmPublicIPs.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmPublicIPs[resourceGroup];

            JObject publicIPJson = await this.GetAzureARMResources("PublicIPs", resourceGroup, null);

            var publicIPs = from publicIP in publicIPJson["value"]
                                select publicIP;

            List<Arm.PublicIP> resourceGroupPublicIPs = new List<Arm.PublicIP>();

            foreach (var publicIP in publicIPs)
            {
                Arm.PublicIP armPublicIP = new Arm.PublicIP(publicIP);
                await armPublicIP.InitializeChildrenAsync(_AzureContext);
                resourceGroupPublicIPs.Add(armPublicIP);
            }

            resourceGroup.AzureSubscription.ArmPublicIPs.Add(resourceGroup, resourceGroupPublicIPs);
            return resourceGroupPublicIPs;
        }

        public async Task<List<Arm.LoadBalancer>> GetAzureARMLoadBalancers(Arm.ResourceGroup resourceGroup)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMLoadBalancers", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmLoadBalancers.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmLoadBalancers[resourceGroup];

            JObject loadBalancersJson = await this.GetAzureARMResources("LoadBalancers", resourceGroup, null);

            var loadBalancers = from loadBalancer in loadBalancersJson["value"]
                                        select loadBalancer;

            List<Arm.LoadBalancer> resourceGroupLoadBalancers = new List<Arm.LoadBalancer>();

            foreach (var loadBalancer in loadBalancers)
            {
                Arm.LoadBalancer armLoadBalancer = new Arm.LoadBalancer(loadBalancer);
                await armLoadBalancer.InitializeChildrenAsync(_AzureContext);
                resourceGroupLoadBalancers.Add(armLoadBalancer);
            }

            resourceGroup.AzureSubscription.ArmLoadBalancers.Add(resourceGroup, resourceGroupLoadBalancers);
            return resourceGroupLoadBalancers;
        }

        #endregion

        private async Task<XmlDocument> GetAzureAsmResources(string resourceType, Hashtable info)
        {
            _AzureContext.LogProvider.WriteLog("GetAzuereAsmResources", "Start");

            _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "Start REST Request");

            string url = null;
            switch (resourceType)
            {
                case "Subscriptions":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + "subscriptions";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Subscriptions...");
                    break;
                case "VirtualNetworks":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/virtualnetwork";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Networks for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                // https://msdn.microsoft.com/en-us/library/azure/dn469422.aspx
                case "RoleSize":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/rolesizes";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Role Sizes for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "ClientRootCertificates":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Client Root Certificates for Virtual Network : " + info["virtualnetworkname"] + "...");
                    break;
                case "ClientRootCertificate":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates/" + info["thumbprint"];
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting certificate data for certificate : " + info["thumbprint"] + "...");
                    break;
                case "NetworkSecurityGroup":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/networksecuritygroups/" + info["name"] + "?detaillevel=Full";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Network Security Group : " + info["name"] + "...");
                    break;
                case "NetworkSecurityGroups":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/networksecuritygroups";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Network Security Groups");
                    break;
                case "RouteTable":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/routetables/" + info["name"] + "?detailLevel=full";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Route Table : " + info["routetablename"] + "...");
                    break;
                case "NSGSubnet":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/virtualnetwork/" + info["virtualnetworkname"] + "/subnets/" + info["subnetname"] + "/networksecuritygroups";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting NSG for subnet " + info["subnetname"] + "...");
                    break;
                case "VirtualNetworkGateway":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway : " + info["virtualnetworkname"] + "...");
                    break;
                case "VirtualNetworkGatewaySharedKey":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/connection/" + info["localnetworksitename"] + "/sharedkey";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway Shared Key: " + info["localnetworksitename"] + "...");
                    break;
                case "StorageAccounts":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/storageservices";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "StorageAccount":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/storageservices/" + info["name"];
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Storage Account '" + info["name"] + " ' for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "StorageAccountKeys":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/storageservices/" + info["name"] + "/keys";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Storage Account '" + info["name"] + "' Keys.");
                    break;
                case "CloudServices":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/hostedservices";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Cloud Services for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "CloudService":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/hostedservices/" + info["name"] + "?embed-detail=true";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Machines for Cloud Service : " + info["name"] + "...");
                    break;
                case "VirtualMachine":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/hostedservices/" + info["cloudservicename"] + "/deployments/" + info["deploymentname"] + "/roles/" + info["virtualmachinename"];
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Machine '" + info["virtualmachinename"] + "' for Cloud Service '" + info["virtualmachinename"] + "'");
                    break;
                case "VMImages":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/images";
                    break;
                case "ReservedIPs":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/services/networking/reservedips";
                    break;
                case "AffinityGroup":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/affinitygroups/" + info["affinitygroupname"];
                    break;
                case "Locations":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + _AzureSubscription.SubscriptionId + "/locations";
                    break;
                default:
                    throw new ArgumentException("Unknown ResourceType: " + resourceType);
            }
            
            AzureRestRequest azureRestRequest = new AzureRestRequest(url, _AzureContext.TokenProvider.AccessToken);
            azureRestRequest.Headers.Add("x-ms-version", "2015-04-01");
            AzureRestResponse azureRestResponse = await GetAzureRestResponse(azureRestRequest);

            return RemoveXmlns(azureRestResponse.Response);
        }

        private async Task<JObject> GetAzureARMResources(string resourceType, Arm.ResourceGroup resourceGroup, Hashtable info)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "Start");

            string methodType = "GET";
            string url = null;
            bool useCached = true;

            if (_AzureContext == null)
                throw new ArgumentNullException("AzureContext is null.  Unable to call Azure API without Azure Context.");
            if (_AzureContext.TokenProvider == null)
                throw new ArgumentNullException("TokenProvider Context is null.  Unable to call Azure API without TokenProvider.");
            if (_AzureContext.TokenProvider.AccessToken == null)
                throw new ArgumentNullException("AccessToken Context is null.  Unable to call Azure API without AccessToken.");

            String accessToken = _AzureContext.TokenProvider.AccessToken;

            switch (resourceType)
            {
                case "Tenants":
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "tenants?api-version=2015-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Tenants...");
                    break;
                case "Domains": // todo, move to a graph class?
                    url = _AzureContext.AzureServiceUrls.GetGraphApiUrl() + "myorganization/domains?api-version=1.6";
                    useCached = false;

                    AuthenticationResult tenantAuthenticationResult = await _AzureContext.TokenProvider.GetGraphToken(info["tenantId"].ToString());
                    if (tenantAuthenticationResult != null)
                        accessToken = tenantAuthenticationResult.AccessToken;

                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Tenant Domain details from Graph...");
                    break;
                case "Subscriptions":
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions?api-version=2015-01-01";

                    if (info != null && info["tenantId"] != null)
                    {
                        AuthenticationResult subscriptionAuthenticationResult = await _AzureContext.TokenProvider.GetAzureToken(info["tenantId"].ToString());
                        if (subscriptionAuthenticationResult != null)
                            accessToken = subscriptionAuthenticationResult.AccessToken;

                        useCached = false;
                    }

                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Subscriptions...");
                    break;
                case "ResourceGroups":
                    // https://docs.microsoft.com/en-us/rest/api/resources/resourcegroups#ResourceGroups_List
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourcegroups?api-version=2016-09-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Resource Groups...");
                    break;
                case "Locations":
                    // https://docs.microsoft.com/en-us/rest/api/resources/subscriptions#Subscriptions_ListLocations
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + ArmConst.Locations + "?api-version=2016-06-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Azure Locations for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "AvailabilitySets":
                    // https://docs.microsoft.com/en-us/rest/api/compute/availabilitysets/availabilitysets-list-subscription
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderAvailabilitySets + "?api-version=2017-03-30";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Availability Sets for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "VirtualNetworks":
                    // https://msdn.microsoft.com/en-us/library/azure/mt163557.aspx
                    // https://docs.microsoft.com/en-us/rest/api/network/list-virtual-networks-within-a-subscription
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderVirtualNetwork + "?api-version=2016-12-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Virtual Networks for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "VirtualNetworkGateways":
                    // https://docs.microsoft.com/en-us/rest/api/network/virtualnetworkgateways#VirtualNetworkGateways_List
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderVirtualNetworkGateways + "?api-version=2016-12-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Virtual Network Gateways for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "NetworkSecurityGroups":
                    // https://docs.microsoft.com/en-us/rest/api/network/networksecuritygroups#NetworkSecurityGroups_ListAll
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderNetworkSecurityGroups + "?api-version=2017-03-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Network Security Groups for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "NetworkInterfaces":
                    // https://docs.microsoft.com/en-us/rest/api/network/networkinterfaces#NetworkInterfaces_ListAll
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderNetworkInterfaces + "?api-version=2017-03-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Network Interfaces for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "StorageAccounts":
                    // https://docs.microsoft.com/en-us/rest/api/storagerp/storageaccounts#StorageAccounts_List
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderStorageAccounts + "?api-version=2016-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Storage Accounts for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "StorageAccountKeys":
                    // https://docs.microsoft.com/en-us/rest/api/storagerp/storageaccounts#StorageAccounts_ListKeys
                    methodType = "POST";
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderStorageAccounts + info["StorageAccountName"] + "/listKeys?api-version=2016-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Storage Account Key for Subscription ID : " + _AzureSubscription.SubscriptionId + " / Storage Account: " + info["StorageAccountName"] + " ...");
                    break;
                case "VirtualMachines":
                    // https://docs.microsoft.com/en-us/rest/api/compute/virtualmachines/virtualmachines-list-subscription
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderVirtualMachines + "?api-version=2016-03-30";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Virtual Machines for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "ManagedDisks":
                    // https://docs.microsoft.com/en-us/rest/api/manageddisks/disks/disks-list-by-subscription
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderManagedDisks + "?api-version=2016-04-30-preview";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Managed Disks for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "LoadBalancers":
                    // https://docs.microsoft.com/en-us/rest/api/network/loadbalancer/list-load-balancers-within-a-subscription
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderLoadBalancers + "?api-version=2016-09-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Load Balancers for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "PublicIPs":
                    // https://docs.microsoft.com/en-us/rest/api/network/virtualnetwork/list-public-ip-addresses-within-a-resource-group
                    url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderPublicIpAddress + "?api-version=2016-09-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Public IPs for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                default:
                    throw new ArgumentException("Unknown ResourceType: " + resourceType);
            }

            AzureRestRequest azureRestRequest = new AzureRestRequest(url, accessToken, methodType, useCached);
            AzureRestResponse azureRestResponse = await GetAzureRestResponse(azureRestRequest);
            return JObject.Parse(azureRestResponse.Response);
        }

        private async Task<AzureRestResponse> GetAzureRestResponse(AzureRestRequest azureRestRequest)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Url: " + azureRestRequest.Url);

            if (azureRestRequest.UseCached && _RestApiCache.ContainsKey(azureRestRequest.Url))
            {
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Using Cached Response");
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " End REST Request");
                AzureRestResponse cachedRestResponse = (AzureRestResponse)_RestApiCache[azureRestRequest.Url];
                return cachedRestResponse;
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(azureRestRequest.Url);
            string authorizationHeader = "Bearer " + azureRestRequest.AccessToken;
            request.Headers.Add(HttpRequestHeader.Authorization, authorizationHeader);

            request.Method = azureRestRequest.Method;

            if (request.Method == "POST")
                request.ContentLength = 0;

            foreach (String headerKey in azureRestRequest.Headers.Keys)
            {
                request.Headers.Add(headerKey, azureRestRequest.Headers[headerKey]);
            }

            string webRequesetResult = String.Empty;
            try
            {
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " " + azureRestRequest.Method + " " + azureRestRequest.Url);

                // Retry Guidlines for 500 series with Backoff Timer - https://msdn.microsoft.com/en-us/library/azure/jj878112.aspx  https://msdn.microsoft.com/en-us/library/azure/gg185909.aspx
                HttpWebResponse response = null;
                Int32 retrySeconds = 1;
                bool boolRetryGetResponse = true;
                while (boolRetryGetResponse)
                {
                    try
                    {
                        response = (HttpWebResponse)await request.GetResponseAsync();
                        boolRetryGetResponse = false;
                    }
                    catch (WebException webException)
                    {
                        _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " EXCEPTION " + webException.Message);

                        HttpWebResponse exceptionResponse = (HttpWebResponse)webException.Response;

                        if ((exceptionResponse != null) &&
                            (
                                (int)exceptionResponse.StatusCode == 429 || // 429 Too Many Requests
                                ((int)exceptionResponse.StatusCode >= 500 && (int)exceptionResponse.StatusCode <= 599)
                            )
                            )
                        {
                            DateTime sleepUntil = DateTime.Now.AddSeconds(retrySeconds);
                            string sleepMessage = "Sleeping for " + retrySeconds.ToString() + " second(s) (until " + sleepUntil.ToString() + ") before web request retry.";

                            _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " " + sleepMessage);
                            _AzureContext.StatusProvider.UpdateStatus(sleepMessage);
                            while (DateTime.Now < sleepUntil)
                            {
                                Application.DoEvents();
                            }
                            retrySeconds = retrySeconds * 2;

                            _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Initiating retry of Web Request.");
                            _AzureContext.StatusProvider.UpdateStatus("Initiating retry of Web Request.");
                        }
                        else
                            throw webException;
                    }
                }

                webRequesetResult = new StreamReader(response.GetResponseStream()).ReadToEnd();

                writeRetreiverResultToLog(azureRestRequest.RequestGuid, "GetAzureRestResponse", azureRestRequest.Url, authorizationHeader);
                writeRetreiverResultToLog(azureRestRequest.RequestGuid, "GetAzureRestResponse", azureRestRequest.Url, webRequesetResult);
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + "  Status Code " + response.StatusCode);
            }
            catch (Exception exception)
            {
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + azureRestRequest.Url + "  EXCEPTION " + exception.Message);
                throw exception;
            }

            _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " End REST Request");

            AzureRestResponse azureRestResponse = new AzureRestResponse(azureRestRequest, webRequesetResult);

            if (!_RestApiCache.ContainsKey(azureRestRequest.Url))
                _RestApiCache.Add(azureRestRequest.Url, azureRestResponse);

            OnRestResult?.Invoke(azureRestResponse);

            return azureRestResponse;
        }







        public async Task BindAsmResources()
        {
            if (!_IsAsmLoaded)
            {
                _IsAsmLoaded = true;

                if (this._AzureSubscription != null)
                {
                    await this.GetAzureARMLocations();
                    await this.GetAzureASMRoleSizes();

                    foreach (Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup in await this.GetAzureAsmNetworkSecurityGroups())
                    {
                        // Ensure we load the Full Details to get NSG Rules
                        Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroupFullDetail = await this.GetAzureAsmNetworkSecurityGroup(asmNetworkSecurityGroup.Name);

                        Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = new Azure.MigrationTarget.NetworkSecurityGroup(this._AzureContext, asmNetworkSecurityGroupFullDetail);
                        _AsmTargetNetworkSecurityGroups.Add(targetNetworkSecurityGroup);
                    }

                    List<Azure.Asm.VirtualNetwork> asmVirtualNetworks = await this.GetAzureAsmVirtualNetworks();
                    foreach (Azure.Asm.VirtualNetwork asmVirtualNetwork in asmVirtualNetworks)
                    {
                        Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = new Azure.MigrationTarget.VirtualNetwork(this._AzureContext, asmVirtualNetwork, _AsmTargetNetworkSecurityGroups);
                        _AsmTargetVirtualNetworks.Add(targetVirtualNetwork);
                    }

                    foreach (Azure.Asm.StorageAccount asmStorageAccount in await this.GetAzureAsmStorageAccounts())
                    {
                        Azure.MigrationTarget.StorageAccount targetStorageAccount = new Azure.MigrationTarget.StorageAccount(_AzureContext, asmStorageAccount);
                        _AsmTargetStorageAccounts.Add(targetStorageAccount);
                    }

                    List<Azure.Asm.CloudService> asmCloudServices = await this.GetAzureAsmCloudServices();
                    foreach (Azure.Asm.CloudService asmCloudService in asmCloudServices)
                    {
                        List<Azure.MigrationTarget.VirtualMachine> cloudServiceTargetVirtualMachines = new List<Azure.MigrationTarget.VirtualMachine>();
                        Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = new Azure.MigrationTarget.AvailabilitySet(this._AzureContext, asmCloudService);

                        foreach (Azure.Asm.VirtualMachine asmVirtualMachine in asmCloudService.VirtualMachines)
                        {
                            Azure.MigrationTarget.VirtualMachine targetVirtualMachine = new Azure.MigrationTarget.VirtualMachine(this._AzureContext, asmVirtualMachine, _AsmTargetVirtualNetworks, _AsmTargetStorageAccounts, _AsmTargetNetworkSecurityGroups);
                            targetVirtualMachine.TargetAvailabilitySet = targetAvailabilitySet;
                            cloudServiceTargetVirtualMachines.Add(targetVirtualMachine);
                            _AsmTargetVirtualMachines.Add(targetVirtualMachine);
                        }

                        Azure.MigrationTarget.LoadBalancer targetLoadBalancer = new Azure.MigrationTarget.LoadBalancer();
                        targetLoadBalancer.Name = asmCloudService.Name;
                        targetLoadBalancer.SourceName = asmCloudService.Name + "-LB";

                        Azure.MigrationTarget.FrontEndIpConfiguration frontEndIpConfiguration = new Azure.MigrationTarget.FrontEndIpConfiguration(targetLoadBalancer);

                        Azure.MigrationTarget.BackEndAddressPool backEndAddressPool = new Azure.MigrationTarget.BackEndAddressPool(targetLoadBalancer);

                        // if internal load balancer
                        if (asmCloudService.ResourceXml.SelectNodes("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/Type").Count > 0)
                        {
                            string virtualnetworkname = asmCloudService.ResourceXml.SelectSingleNode("//Deployments/Deployment/VirtualNetworkName").InnerText;
                            string subnetname = asmCloudService.ResourceXml.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/SubnetName").InnerText.Replace(" ", "");

                            if (asmCloudService.ResourceXml.SelectNodes("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").Count > 0)
                            {
                                frontEndIpConfiguration.TargetPrivateIPAllocationMethod = "Static";
                                frontEndIpConfiguration.TargetPrivateIpAddress = asmCloudService.ResourceXml.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").InnerText;
                            }

                            if (cloudServiceTargetVirtualMachines.Count > 0)
                            {
                                if (cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface != null)
                                {
                                    if (cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
                                    {
                                        frontEndIpConfiguration.TargetVirtualNetwork = cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork;
                                        frontEndIpConfiguration.TargetSubnet = cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet;
                                    }
                                }
                            }
                        }
                        else // if external load balancer
                        {
                            Azure.MigrationTarget.PublicIp loadBalancerPublicIp = new Azure.MigrationTarget.PublicIp();
                            loadBalancerPublicIp.SourceName = asmCloudService.Name + "-PIP";
                            loadBalancerPublicIp.Name = asmCloudService.Name;
                            loadBalancerPublicIp.DomainNameLabel = asmCloudService.Name;

                            frontEndIpConfiguration.PublicIp = loadBalancerPublicIp;

                            targetLoadBalancer.LoadBalancerType = MigrationTarget.LoadBalancerType.Public;
                        }

                        foreach (Azure.MigrationTarget.VirtualMachine targetVirtualMachine in cloudServiceTargetVirtualMachines)
                        {
                            if (targetVirtualMachine.PrimaryNetworkInterface != null)
                                targetVirtualMachine.PrimaryNetworkInterface.BackEndAddressPool = backEndAddressPool;

                            Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)targetVirtualMachine.Source;
                            foreach (XmlNode inputendpoint in asmVirtualMachine.ResourceXml.SelectNodes("//ConfigurationSets/ConfigurationSet/InputEndpoints/InputEndpoint"))
                            {
                                if (inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName") == null) // if it's a inbound nat rule
                                {
                                    Azure.MigrationTarget.InboundNatRule targetInboundNatRule = new Azure.MigrationTarget.InboundNatRule(targetLoadBalancer);
                                    targetInboundNatRule.Name = asmVirtualMachine.RoleName + "-" + inputendpoint.SelectSingleNode("Name").InnerText;
                                    targetInboundNatRule.FrontEndPort = Int32.Parse(inputendpoint.SelectSingleNode("Port").InnerText);
                                    targetInboundNatRule.BackEndPort = Int32.Parse(inputendpoint.SelectSingleNode("LocalPort").InnerText);
                                    targetInboundNatRule.Protocol = inputendpoint.SelectSingleNode("Protocol").InnerText;
                                    targetInboundNatRule.FrontEndIpConfiguration = frontEndIpConfiguration;

                                    if (targetVirtualMachine.PrimaryNetworkInterface != null)
                                        targetVirtualMachine.PrimaryNetworkInterface.InboundNatRules.Add(targetInboundNatRule);
                                }
                                else // if it's a load balancing rule
                                {
                                    XmlNode probenode = inputendpoint.SelectSingleNode("LoadBalancerProbe");

                                    Azure.MigrationTarget.Probe targetProbe = null;
                                    foreach (Azure.MigrationTarget.Probe existingProbe in targetLoadBalancer.Probes)
                                    {
                                        if (existingProbe.Name == inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText)
                                        {
                                            targetProbe = existingProbe;
                                            break;
                                        }
                                    }

                                    if (targetProbe == null)
                                    {
                                        targetProbe = new Azure.MigrationTarget.Probe();
                                        targetProbe.Name = inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText;
                                        targetProbe.Port = Int32.Parse(probenode.SelectSingleNode("Port").InnerText);
                                        targetProbe.Protocol = probenode.SelectSingleNode("Protocol").InnerText;
                                        targetLoadBalancer.Probes.Add(targetProbe);
                                    }

                                    Azure.MigrationTarget.LoadBalancingRule targetLoadBalancingRule = null;
                                    foreach (Azure.MigrationTarget.LoadBalancingRule existingLoadBalancingRule in targetLoadBalancer.LoadBalancingRules)
                                    {
                                        if (existingLoadBalancingRule.Name == inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText)
                                        {
                                            targetLoadBalancingRule = existingLoadBalancingRule;
                                            break;
                                        }
                                    }

                                    if (targetLoadBalancingRule == null)
                                    {
                                        targetLoadBalancingRule = new Azure.MigrationTarget.LoadBalancingRule(targetLoadBalancer);
                                        targetLoadBalancingRule.Name = inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText;
                                        targetLoadBalancingRule.FrontEndIpConfiguration = frontEndIpConfiguration;
                                        targetLoadBalancingRule.BackEndAddressPool = targetLoadBalancer.BackEndAddressPools[0];
                                        targetLoadBalancingRule.Probe = targetProbe;
                                        targetLoadBalancingRule.FrontEndPort = Int32.Parse(inputendpoint.SelectSingleNode("Port").InnerText);
                                        targetLoadBalancingRule.BackEndPort = Int32.Parse(inputendpoint.SelectSingleNode("LocalPort").InnerText);
                                        targetLoadBalancingRule.Protocol = inputendpoint.SelectSingleNode("Protocol").InnerText;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        private List<Azure.MigrationTarget.NetworkSecurityGroup> _AsmTargetNetworkSecurityGroups = new List<MigrationTarget.NetworkSecurityGroup>();
        private List<Azure.MigrationTarget.StorageAccount> _AsmTargetStorageAccounts = new List<MigrationTarget.StorageAccount>();
        private List<Azure.MigrationTarget.VirtualNetwork> _AsmTargetVirtualNetworks = new List<MigrationTarget.VirtualNetwork>();
        private List<Azure.MigrationTarget.VirtualMachine> _AsmTargetVirtualMachines = new List<MigrationTarget.VirtualMachine>();
        private List<Azure.MigrationTarget.StorageAccount> _ArmTargetStorageAccounts = new List<MigrationTarget.StorageAccount>();
        private List<Azure.MigrationTarget.VirtualNetwork> _ArmTargetVirtualNetworks = new List<MigrationTarget.VirtualNetwork>();
        private List<Azure.MigrationTarget.VirtualMachine> _ArmTargetVirtualMachines = new List<MigrationTarget.VirtualMachine>();
        private List<Azure.MigrationTarget.AvailabilitySet> _ArmTargetAvailabilitySets = new List<MigrationTarget.AvailabilitySet>();
        private List<Azure.MigrationTarget.ManagedDisk> _ArmTargetManagedDisks = new List<MigrationTarget.ManagedDisk>();
        private List<Azure.MigrationTarget.LoadBalancer> _ArmTargetLoadBalancers = new List<MigrationTarget.LoadBalancer>();
        private List<Azure.MigrationTarget.NetworkSecurityGroup> _ArmTargetNetworkSecurityGroups = new List<MigrationTarget.NetworkSecurityGroup>();
        private List<Azure.MigrationTarget.PublicIp> _ArmTargetPublicIPs = new List<MigrationTarget.PublicIp>();
        private bool _IsAsmLoaded = false;
        private bool _IsArmLoaded = false;

        public List<Azure.MigrationTarget.NetworkSecurityGroup> AsmTargetNetworkSecurityGroups { get { return _AsmTargetNetworkSecurityGroups; } }
        public List<Azure.MigrationTarget.StorageAccount> AsmTargetStorageAccounts { get { return _AsmTargetStorageAccounts; } }
        public List<Azure.MigrationTarget.VirtualNetwork> AsmTargetVirtualNetworks { get { return _AsmTargetVirtualNetworks; } }
        public List<Azure.MigrationTarget.VirtualMachine> AsmTargetVirtualMachines { get { return _AsmTargetVirtualMachines; } }
        public List<Azure.MigrationTarget.StorageAccount> ArmTargetStorageAccounts { get { return _ArmTargetStorageAccounts; } }
        public List<Azure.MigrationTarget.VirtualNetwork> ArmTargetVirtualNetworks { get { return _ArmTargetVirtualNetworks; } }
        public List<Azure.MigrationTarget.VirtualMachine> ArmTargetVirtualMachines { get { return _ArmTargetVirtualMachines; } }
        public List<Azure.MigrationTarget.AvailabilitySet> ArmTargetAvailabilitySets { get { return _ArmTargetAvailabilitySets; } }
        public List<Azure.MigrationTarget.ManagedDisk> ArmTargetManagedDisks { get { return _ArmTargetManagedDisks; } }
        public List<Azure.MigrationTarget.LoadBalancer> ArmTargetLoadBalancers { get { return _ArmTargetLoadBalancers; } }
        public List<Azure.MigrationTarget.NetworkSecurityGroup> ArmTargetNetworkSecurityGroups { get { return _ArmTargetNetworkSecurityGroups; } }
        public List<Azure.MigrationTarget.PublicIp> ArmTargetPublicIPs { get { return _ArmTargetPublicIPs; } }

        private async Task LoadARMManagedDisks(ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.ManagedDisk armManagedDisk in await this.GetAzureARMManagedDisks(armResourceGroup))
            {
                Azure.MigrationTarget.ManagedDisk targetManagedDisk = new Azure.MigrationTarget.ManagedDisk(armManagedDisk);
                _ArmTargetManagedDisks.Add(targetManagedDisk);
            }
        }

        private async Task LoadARMPublicIPs(ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.PublicIP armPublicIp in await this.GetAzureARMPublicIPs(armResourceGroup))
            {
                Azure.MigrationTarget.PublicIp targetPublicIP = new Azure.MigrationTarget.PublicIp(armPublicIp);
                _ArmTargetPublicIPs.Add(targetPublicIP);
            }
        }

        private async Task LoadARMLoadBalancers(ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.LoadBalancer armLoadBalancer in await this.GetAzureARMLoadBalancers(armResourceGroup))
            {
                Azure.MigrationTarget.LoadBalancer targetLoadBalancer = new Azure.MigrationTarget.LoadBalancer(armLoadBalancer);
                foreach (Azure.MigrationTarget.FrontEndIpConfiguration targetFrontEndIpConfiguration in targetLoadBalancer.FrontEndIpConfigurations)
                {
                    if (targetFrontEndIpConfiguration.Source != null && targetFrontEndIpConfiguration.Source.PublicIP != null)
                    {
                        foreach (Azure.MigrationTarget.PublicIp targetPublicIp in _ArmTargetPublicIPs)
                        {
                            if (targetPublicIp.SourceName == targetFrontEndIpConfiguration.Source.PublicIP.Name)
                            {
                                targetFrontEndIpConfiguration.PublicIp = targetPublicIp;
                            }
                        }
                    }
                }
                _ArmTargetLoadBalancers.Add(targetLoadBalancer);
            }
        }
        private async Task LoadARMAvailabilitySets(ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.AvailabilitySet armAvailabilitySet in await this.GetAzureARMAvailabilitySets(armResourceGroup))
            {
                Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = new Azure.MigrationTarget.AvailabilitySet(this._AzureContext, armAvailabilitySet);
                _ArmTargetAvailabilitySets.Add(targetAvailabilitySet);
            }
        }

        private async Task LoadARMVirtualMachines(ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.VirtualMachine armVirtualMachine in await this.GetAzureArmVirtualMachines(armResourceGroup))
            {
                Azure.MigrationTarget.VirtualMachine targetVirtualMachine = new Azure.MigrationTarget.VirtualMachine(this._AzureContext, armVirtualMachine, _ArmTargetVirtualNetworks, _ArmTargetStorageAccounts, _ArmTargetNetworkSecurityGroups);
                _ArmTargetVirtualMachines.Add(targetVirtualMachine);

                if (armVirtualMachine.AvailabilitySet != null)
                {
                    foreach (Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet in _ArmTargetAvailabilitySets)
                    {
                        if (targetAvailabilitySet.SourceAvailabilitySet != null)
                        {
                            Azure.Arm.AvailabilitySet sourceAvailabilitySet = (Azure.Arm.AvailabilitySet)targetAvailabilitySet.SourceAvailabilitySet;
                            if (sourceAvailabilitySet.Id == armVirtualMachine.AvailabilitySet.Id)
                            {
                                targetVirtualMachine.TargetAvailabilitySet = targetAvailabilitySet;
                            }
                        }
                    }
                }

                foreach (Azure.MigrationTarget.NetworkInterface networkInterface in targetVirtualMachine.NetworkInterfaces)
                {
                    if (networkInterface.SourceNetworkInterface != null)
                    {
                        Azure.Arm.NetworkInterface sourceNetworkInterface = (Azure.Arm.NetworkInterface)networkInterface.SourceNetworkInterface;
                        if (sourceNetworkInterface.NetworkSecurityGroup != null)
                        {
                            foreach (Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup in _ArmTargetNetworkSecurityGroups)
                            {
                                if (targetNetworkSecurityGroup.TargetName == sourceNetworkInterface.NetworkSecurityGroup.Name)
                                    networkInterface.TargetNetworkSecurityGroup = targetNetworkSecurityGroup;
                            }
                        }
                    }

                    foreach (Azure.MigrationTarget.NetworkInterfaceIpConfiguration networkInterfaceIpConfiguration in networkInterface.TargetNetworkInterfaceIpConfigurations)
                    {
                        if (networkInterfaceIpConfiguration.SourceIpConfiguration != null)
                        {
                            Azure.Arm.NetworkInterfaceIpConfiguration sourceIpConfiguration = (Azure.Arm.NetworkInterfaceIpConfiguration)networkInterfaceIpConfiguration.SourceIpConfiguration;

                            if (sourceIpConfiguration.PublicIP != null)
                            {
                                foreach (Azure.MigrationTarget.PublicIp targetPublicIp in _ArmTargetPublicIPs)
                                {
                                    if (targetPublicIp.Name == sourceIpConfiguration.PublicIP.Name)
                                        networkInterfaceIpConfiguration.TargetPublicIp = targetPublicIp;
                                }
                            }


                        }
                    }
                }
            }
        }

        private async Task LoadARMStorageAccounts(ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.StorageAccount armStorageAccount in await this.GetAzureARMStorageAccounts(armResourceGroup))
            {
                Azure.MigrationTarget.StorageAccount targetStorageAccount = new Azure.MigrationTarget.StorageAccount(this._AzureContext, armStorageAccount);
                _ArmTargetStorageAccounts.Add(targetStorageAccount);
            }
        }

        private async Task LoadARMVirtualNetworks(ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.VirtualNetwork armVirtualNetwork in await this.GetAzureARMVirtualNetworks(armResourceGroup))
            {
                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = new Azure.MigrationTarget.VirtualNetwork(this._AzureContext, armVirtualNetwork, _ArmTargetNetworkSecurityGroups);
                _ArmTargetVirtualNetworks.Add(targetVirtualNetwork);
            }
        }

        private async Task LoadARMNetworkSecurityGroups(ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.NetworkSecurityGroup armNetworkSecurityGroup in await this.GetAzureARMNetworkSecurityGroups(armResourceGroup))
            {
                Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = new Azure.MigrationTarget.NetworkSecurityGroup(this._AzureContext, armNetworkSecurityGroup);
                _ArmTargetNetworkSecurityGroups.Add(targetNetworkSecurityGroup);
            }
        }

        public async Task BindArmResources()
        {

            if (!_IsArmLoaded)
            {
                _IsArmLoaded = true;

                if (this._AzureSubscription != null)
                {
                    await this.GetAzureARMLocations();

                    List<Task> armNetworkSecurityGroupTasks = new List<Task>();
                    foreach (Azure.Arm.ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                    {
                        Task armNetworkSecurityGroupTask = LoadARMNetworkSecurityGroups(armResourceGroup);
                        armNetworkSecurityGroupTasks.Add(armNetworkSecurityGroupTask);
                    }
                    await Task.WhenAll(armNetworkSecurityGroupTasks.ToArray());

                    List<Task> armPublicIPTasks = new List<Task>();
                    foreach (Azure.Arm.ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                    {
                        Task armPublicIPTask = LoadARMPublicIPs(armResourceGroup);
                        armPublicIPTasks.Add(armPublicIPTask);
                    }
                    await Task.WhenAll(armPublicIPTasks.ToArray());


                    List<Task> armVirtualNetworkTasks = new List<Task>();
                    foreach (Azure.Arm.ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                    {
                        Task armVirtualNetworkTask = LoadARMVirtualNetworks(armResourceGroup);
                        armVirtualNetworkTasks.Add(armVirtualNetworkTask);
                    }
                    await Task.WhenAll(armVirtualNetworkTasks.ToArray());

                    List<Task> armStorageAccountTasks = new List<Task>();
                    foreach (Azure.Arm.ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                    {
                        Task armStorageAccountTask = LoadARMStorageAccounts(armResourceGroup);
                        armStorageAccountTasks.Add(armStorageAccountTask);
                    }
                    await Task.WhenAll(armStorageAccountTasks.ToArray());

                    try
                    {
                        List<Task> armManagedDiskTasks = new List<Task>();
                        foreach (Azure.Arm.ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                        {
                            Task armManagedDiskTask = LoadARMManagedDisks(armResourceGroup);
                            armManagedDiskTasks.Add(armManagedDiskTask);
                        }
                        await Task.WhenAll(armManagedDiskTasks.ToArray());
                    }
                    catch (Exception exc) { }

                    List<Task> armAvailabilitySetTasks = new List<Task>();
                    foreach (Azure.Arm.ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                    {
                        Task armAvailabilitySetTask = LoadARMAvailabilitySets(armResourceGroup);
                        armAvailabilitySetTasks.Add(armAvailabilitySetTask);
                    }
                    await Task.WhenAll(armAvailabilitySetTasks.ToArray());

                    List<Task> armVirtualMachineTasks = new List<Task>();
                    foreach (Azure.Arm.ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                    {
                        Task armVirtualMachineTask = LoadARMVirtualMachines(armResourceGroup);
                        armVirtualMachineTasks.Add(armVirtualMachineTask);
                    }
                    await Task.WhenAll(armVirtualMachineTasks.ToArray());

                    List<Task> armLoadBalancerTasks = new List<Task>();
                    foreach (Azure.Arm.ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                    {
                        Task armLoadBalancerTask = LoadARMLoadBalancers(armResourceGroup);
                        armLoadBalancerTasks.Add(armLoadBalancerTask);
                    }
                    await Task.WhenAll(armLoadBalancerTasks.ToArray());
                }

            }
        }

    }
}
