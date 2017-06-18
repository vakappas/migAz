using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Tests.Fakes;
using System.IO;
using MigAz.Azure.Models;
using System.Xml;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using MigAz.Azure;
using MigAz.Azure.Generator;
using MigAz.Core.Generator;

namespace MigAz.Tests
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public async Task ValidateSingleStorageAccount()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\Storage1"));
            AzureGenerator templateGenerator = await TestHelper.SetupTemplateGenerator(azureContextUSCommercial);

            var artifacts = new ExportArtifacts();
            // todo artifacts.StorageAccounts.Add(await azureContextUSCommercialRetriever.GetAzureAsmStorageAccount("mystorage"));
            templateGenerator.UpdateArtifacts(artifacts);

            JObject templateJson = templateGenerator.GetTemplate();
            Assert.AreEqual(1, templateJson["resources"].Children().Count());
            var resource = templateJson["resources"].Single();
            Assert.AreEqual("Microsoft.Storage/storageAccounts", resource["type"].Value<string>());
            Assert.AreEqual("mystoragev2", resource["name"].Value<string>());
            Assert.AreEqual((await azureContextUSCommercialRetriever.GetAzureASMLocations())[0].Name, resource["location"].Value<string>());
            Assert.AreEqual("Standard_LRS", resource["properties"]["accountType"].Value<string>());

        }
    }
}
