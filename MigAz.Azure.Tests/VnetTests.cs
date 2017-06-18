﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MigAz.Tests.Fakes;
using MigAz.Azure.Generator.AsmToArm;
using System.IO;
using MigAz.Azure.Models;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using MigAz.Azure;
using MigAz.Azure.Generator;
using MigAz.Core.Generator;

namespace MigAz.Tests
{
    /// <summary>
    /// Summary description for VnetTests
    /// </summary>
    [TestClass]
    public class VnetTests
    {
        [TestMethod]
        public async Task ValidateComplexSingleVnet()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VNET1"));
            AzureGenerator templateGenerator = await TestHelper.SetupTemplateGenerator(azureContextUSCommercial);

            var artifacts = new ExportArtifacts();
            // todo artifacts.VirtualNetworks.Add(await azureContextUSCommercialRetriever.GetAzureAsmVirtualNetwork("10.2.0.0"));

            templateGenerator.UpdateArtifacts(artifacts);

            JObject templateJson = templateGenerator.GetTemplate();

            // Validate VNETs
            var vnets = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworks");
            Assert.AreEqual(1, vnets.Count());
            Assert.AreEqual("10.2.0.0-vnet", vnets.First()["name"].Value<string>());

            // Validate subnets
            var subnets = vnets.First()["properties"]["subnets"];
            Assert.AreEqual(8, subnets.Count());

            // Validate gateway
            var gw = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworkGateways");
            Assert.AreEqual(1, gw.Count());
            Assert.AreEqual("10.2.0.0-gw", gw.First()["name"].Value<string>());

            var localGw = templateJson["resources"].Children().Where(
               r => r["type"].Value<string>() == "Microsoft.Network/localNetworkGateways");
            Assert.AreEqual(2, localGw.Count());
            Assert.AreEqual("MOBILEDATACENTER-LocalGateway", localGw.First()["name"].Value<string>());
            Assert.AreEqual("EastUSNet-LocalGateway", localGw.Last()["name"].Value<string>());

            var pips = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/publicIPAddresses");
            Assert.AreEqual(1, pips.Count());
            Assert.AreEqual("10.2.0.0-gw-pip", pips.First()["name"].Value<string>());
            Assert.AreEqual("Dynamic", pips.First()["properties"]["publicIPAllocationMethod"].Value<string>());
        }

        [TestMethod]
        public async Task ValidateSingleVnetWithNsgAndRT()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VNET2"));
            AzureGenerator templateGenerator = await TestHelper.SetupTemplateGenerator(azureContextUSCommercial);

            var artifacts = new ExportArtifacts();
            // todo artifacts.VirtualNetworks.Add(await azureContextUSCommercialRetriever.GetAzureAsmVirtualNetwork("asmtest"));
            // todo artifacts.NetworkSecurityGroups.Add(await azureContextUSCommercialRetriever.GetAzureAsmNetworkSecurityGroup("asmnsg"));

            templateGenerator.UpdateArtifacts(artifacts);

            JObject templateJson = templateGenerator.GetTemplate();

            // Validate NSG
            var nsgs = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/networkSecurityGroups");
            Assert.AreEqual(1, nsgs.Count());
            Assert.AreEqual("asmnsg-nsg", nsgs.First()["name"].Value<string>());

            // Validate NSG rules
            JArray rules = (JArray) nsgs.First()["properties"]["securityRules"];
            Assert.AreEqual(2, rules.Count());
            Assert.AreEqual("Enable-Internal-DNS", rules[0]["name"].Value<string>());
            Assert.AreEqual("Port-7777-rule", rules[1]["name"].Value<string>());

            // Validate RouteTable
            var rt = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/routeTables");
            Assert.AreEqual(1, rt.Count());
            Assert.AreEqual("asmrt", rt.First()["name"].Value<string>());

            // Validate Routes
            JArray routes = (JArray)rt.First()["properties"]["routes"];
            Assert.AreEqual(1, routes.Count());
            Assert.AreEqual("all-traffic-to-fw", routes[0]["name"].Value<string>());

            // Validate VNETs
            var vnets = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworks");
            Assert.AreEqual(1, vnets.Count());
            Assert.AreEqual("asmtest-vnet", vnets.First()["name"].Value<string>());

            // Validate subnets
            var subnets = vnets.First()["properties"]["subnets"];
            Assert.AreEqual(1, subnets.Count());
            Assert.AreEqual("Subnet-1", subnets.First()["name"].Value<string>());
            StringAssert.Contains(subnets.First()["properties"]["networkSecurityGroup"]["id"].Value<string>(), "networkSecurityGroups/asmnsg");
            StringAssert.Contains(subnets.First()["properties"]["routeTable"]["id"].Value<string>(), "routeTables/asmrt");
        }

        [TestMethod]
        public async Task ValidateSingleVnetWithExpressRouteGateway()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VNET3"));
            AzureGenerator templateGenerator = await TestHelper.SetupTemplateGenerator(azureContextUSCommercial);

            var artifacts = new ExportArtifacts();
            // todo artifacts.VirtualNetworks.Add(await azureContextUSCommercialRetriever.GetAzureAsmVirtualNetwork("vnet3"));

            templateGenerator.UpdateArtifacts(artifacts);

            JObject templateJson = templateGenerator.GetTemplate();

            // Validate VNETs
            var vnets = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworks");
            Assert.AreEqual(1, vnets.Count());
            Assert.AreEqual("vnet3-vnet", vnets.First()["name"].Value<string>());

            // Validate subnets
            var subnets = vnets.First()["properties"]["subnets"];
            Assert.AreEqual(2, subnets.Count());

            // Validate gateway
            var gw = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworkGateways");
            Assert.AreEqual(1, gw.Count());
            Assert.AreEqual("vnet3-gw", gw.First()["name"].Value<string>());
            Assert.AreEqual("ExpressRoute", gw.First()["properties"]["gatewayType"].Value<string>());

            // Validate no local network
            var localGw = templateJson["resources"].Children().Where(
               r => r["type"].Value<string>() == "Microsoft.Network/localNetworkGateways");
            Assert.AreEqual(0, localGw.Count());

            // Validate connection
            var conn = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/connections");
            Assert.AreEqual(1, conn.Count());
            Assert.AreEqual("vnet3-gw-localsite-connection", conn.First()["name"].Value<string>());
            Assert.AreEqual("ExpressRoute", conn.First()["properties"]["connectionType"].Value<string>());
            Assert.IsNotNull(conn.First()["properties"]["peer"]["id"].Value<string>());

            // Validate message
            Assert.AreEqual(1, templateGenerator.Alerts.Count);
            StringAssert.Contains(templateGenerator.Alerts[0].Message, "ExpressRoute");
        }

        [TestMethod]
        public async Task ValidateSingleVnetWithNoSubnetsGetsNewDefaultSubet()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VNET4"));
            AzureGenerator templateGenerator = await TestHelper.SetupTemplateGenerator(azureContextUSCommercial);

            var artifacts = new ExportArtifacts();
            // todo artifacts.VirtualNetworks.Add(await azureContextUSCommercialRetriever.GetAzureAsmVirtualNetwork("asmnet"));

            await templateGenerator.UpdateArtifacts(artifacts);

            JObject templateJson = templateGenerator.GetTemplate();

            // Validate VNETs
            var vnets = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworks");
            Assert.AreEqual(1, vnets.Count());
            Assert.AreEqual("asmnet-vnet", vnets.First()["name"].Value<string>());
            Assert.AreEqual("10.0.0.0/20", vnets.First()["properties"]["addressSpace"]["addressPrefixes"][0].Value<string>());

            // Validate subnets
            var subnets = vnets.First()["properties"]["subnets"];
            Assert.AreEqual(1, subnets.Count());
            Assert.AreEqual("Subnet1", subnets[0]["name"].Value<string>());
            Assert.AreEqual("10.0.0.0/20", subnets[0]["properties"]["addressPrefix"].Value<string>());

        }
    }
}
