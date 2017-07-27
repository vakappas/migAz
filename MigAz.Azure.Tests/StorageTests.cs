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
using MIGAZ.Tests.Fakes;
using System.Collections.Generic;

namespace MigAz.Tests
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public async Task asdf()
        {
            string restResponseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\NewTest1\\test.json");
            AzureContext azureContextUSCommercial = await TestHelper.SetupAzureContext(restResponseFile);
            await azureContextUSCommercial.AzureRetriever.BindAsmResources();

            AzureGenerator templateGenerator = await TestHelper.SetupTemplateGenerator(azureContextUSCommercial);

            var artifacts = new ExportArtifacts();
            artifacts.ResourceGroup = await TestHelper.GetTargetResourceGroup(azureContextUSCommercial);
            foreach (Azure.MigrationTarget.StorageAccount s in azureContextUSCommercial.AzureRetriever.AsmTargetStorageAccounts)
            {
                artifacts.StorageAccounts.Add(s);
            }
            //// todo artifacts.StorageAccounts.Add(await azureContextUSCommercialRetriever.GetAzureAsmStorageAccount("mystorage"));
            await templateGenerator.UpdateArtifacts(artifacts);
            Assert.IsFalse(templateGenerator.HasErrors, "Template Generation cannot occur as the are error(s).");

            JObject templateJson = JObject.Parse(await templateGenerator.GetTemplateString());


            Assert.AreEqual(1, templateJson["resources"].Children().Count());

            //var resource = templateJson["resources"].Single();
            //Assert.AreEqual("Microsoft.Storage/storageAccounts", resource["type"].Value<string>());
            //Assert.AreEqual("mystoragev2", resource["name"].Value<string>());
            //Assert.AreEqual((await azureContextUSCommercialRetriever.GetAzureASMLocations())[0].Name, resource["location"].Value<string>());
            //Assert.AreEqual("Standard_LRS", resource["properties"]["accountType"].Value<string>());

        }
    }
}
