using MigAz.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class CloudService : IAvailabilitySetSource
    {
        private AzureContext _AzureContext;
        private XmlNode _XmlNode;
        private AffinityGroup _AsmAffinityGroup;
        private List<VirtualMachine> _VirtualMachines = new List<VirtualMachine>();
        private List<LoadBalancer> _LoadBalancers = new List<LoadBalancer>();
        private ReservedIP _AsmReservedIP;
        private VirtualNetwork _AsmVirtualNetwork;

        private CloudService() { }

        public CloudService(AzureContext azureContext, XmlNode cloudServiceXml)
        {
            this._AzureContext = azureContext;
            this._XmlNode = cloudServiceXml;
        }

        public XmlNode ResourceXml
        {
            get { return _XmlNode; }
        }

        public string Name
        {
            get
            {
                if (_XmlNode.SelectSingleNode("ServiceName") != null)
                    return _XmlNode.SelectSingleNode("ServiceName").InnerText;

                if (_XmlNode.SelectSingleNode("HostedService/ServiceName") != null)
                    return _XmlNode.SelectSingleNode("HostedService/ServiceName").InnerText;

                return "Unknown";
            }
        }

        public AffinityGroup AffinityGroup
        {
            get { return _AsmAffinityGroup; }
        }

        private string AffinityGroupName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("/HostedService/HostedServiceProperties/AffinityGroup") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("/HostedService/HostedServiceProperties/AffinityGroup").InnerText;
            }
        }

        public string Location
        {
            get
            {
                if (this.AffinityGroup != null)
                    return this.AffinityGroup.Location;

                if (_XmlNode.SelectSingleNode("HostedService/HostedServiceProperties/Location") != null)
                    return _XmlNode.SelectSingleNode("HostedService/HostedServiceProperties/Location").InnerText;

                return "Unknown";
            }
        }

        public VirtualNetwork VirtualNetwork
        {
            get { return _AsmVirtualNetwork; }
        }

        private string VirtualNetworkName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//Deployments/Deployment/VirtualNetworkName") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//Deployments/Deployment/VirtualNetworkName").InnerText;
            }
        }


        public ReservedIP AsmReservedIP
        {
            get { return _AsmReservedIP; }
        }
        
        public List<LoadBalancer> LoadBalancers
        {
            get { return _LoadBalancers; }
        }

        public List<VirtualMachine> VirtualMachines
        {
            get { return _VirtualMachines; }
        }

        public string StaticVirtualNetworkIPAddress
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress") == null)
                    return String.Empty;
                else
                    return _XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").InnerText;
            }
        }

        public virtual VirtualMachine GetVirtualMachine(string virtualMachineName)
        {
            if (virtualMachineName == null)
                return null;

            foreach (VirtualMachine asmVirtualMachine in this.VirtualMachines)
            {
                if (asmVirtualMachine.RoleName == virtualMachineName)
                    return asmVirtualMachine;
            }

            return null;
        }

        public async Task LoadChildrenAsync()
        {
            if (this.VirtualNetworkName != String.Empty)
                _AsmVirtualNetwork = await this._AzureContext.AzureRetriever.GetAzureAsmVirtualNetwork(this.VirtualNetworkName);

            if (this.AffinityGroupName == String.Empty)
                _AsmAffinityGroup = null;
            else
                _AsmAffinityGroup = await this._AzureContext.AzureRetriever.GetAzureAsmAffinityGroup(this.AffinityGroupName);

            _VirtualMachines = new List<VirtualMachine>();
            if (_XmlNode.SelectNodes("//Deployments/Deployment").Count > 0)
            {
                if (_XmlNode.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectNodes("RoleType").Count > 0)
                {
                    if (_XmlNode.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectSingleNode("RoleType").InnerText == "PersistentVMRole")
                    {

                        XmlNodeList roles = _XmlNode.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role");
                        if (roles != null)
                        {
                            foreach (XmlNode role in roles)
                            {
                                string virtualmachinename = role.SelectSingleNode("RoleName").InnerText;

                                VirtualMachine asmVirtualMachine = await _AzureContext.AzureRetriever.GetAzureAsmVirtualMachine(this, virtualmachinename);
                                _VirtualMachines.Add(asmVirtualMachine);
                            }
                        }
                    }
                }
            }

            List<ReservedIP> asmReservedIPs = await _AzureContext.AzureRetriever.GetAzureAsmReservedIPs();
            if (asmReservedIPs != null)
            {
                foreach (ReservedIP asmReservedIP in asmReservedIPs)
                {
                    if (asmReservedIP.ServiceName == this.Name)
                    {
                        _AsmReservedIP = asmReservedIP;
                    }
                }
            }

            XmlNodeList loadBalancersXml = _XmlNode.SelectNodes("//Deployments/Deployment/LoadBalancers/LoadBalancer");

            if (loadBalancersXml != null)
            {
                foreach (XmlNode loadBalancerXml in loadBalancersXml)
                {
                    LoadBalancer asmLoadBalancer = new LoadBalancer(_AzureContext, _AsmVirtualNetwork, loadBalancerXml);
                    _LoadBalancers.Add(asmLoadBalancer);
                }
            }
        }

    }
}
