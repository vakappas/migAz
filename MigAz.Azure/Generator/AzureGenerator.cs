using MigAz.Azure.Interface;
using MigAz.Azure.Models;
using MigAz.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MigAz.Core.Generator;
using System.IO;
using MigAz.Core.ArmTemplate;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace MigAz.Azure.Generator.AsmToArm
{
    public class AzureGenerator : TemplateGenerator
    {

        private AzureGenerator() : base(null, null, null, null, null, null) { } 

        public AzureGenerator(
            ISubscription sourceSubscription, 
            ISubscription targetSubscription,
            ILogProvider logProvider, 
            IStatusProvider statusProvider, 
            ITelemetryProvider telemetryProvider, 
            ISettingsProvider settingsProvider) : base(logProvider, statusProvider, sourceSubscription, targetSubscription, telemetryProvider, settingsProvider)
        {
        }

        public override string GetVMSize(string vmsize)
        {
            Dictionary<string, string> VMSizeTable = new Dictionary<string, string>();
            VMSizeTable.Add("ExtraSmall", "Standard_A0");
            VMSizeTable.Add("Small", "Standard_A1");
            VMSizeTable.Add("Medium", "Standard_A2");
            VMSizeTable.Add("Large", "Standard_A3");
            VMSizeTable.Add("ExtraLarge", "Standard_A4");
            VMSizeTable.Add("A5", "Standard_A5");
            VMSizeTable.Add("A6", "Standard_A6");
            VMSizeTable.Add("A7", "Standard_A7");
            VMSizeTable.Add("A8", "Standard_A8");
            VMSizeTable.Add("A9", "Standard_A9");
            VMSizeTable.Add("A10", "Standard_A10");
            VMSizeTable.Add("A11", "Standard_A11");

            if (VMSizeTable.ContainsKey(vmsize))
            {
                return VMSizeTable[vmsize];
            }
            else
            {
                return vmsize;
            }
        }

        //public JObject GetTemplate()
        //{
        //    if (!TemplateStreams.ContainsKey("export.json"))
        //        return null;

        //    MemoryStream templateStream = TemplateStreams["export.json"];
        //    templateStream.Position = 0;
        //    StreamReader sr = new StreamReader(templateStream);
        //    String myStr = sr.ReadToEnd();

        //    return JObject.Parse(myStr);
        //}
    }
}