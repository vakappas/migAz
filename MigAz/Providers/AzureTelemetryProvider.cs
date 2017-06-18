using MigAz.Azure.Generator.AsmToArm;
using MigAz.Azure.Models;
using MigAz.Core.ArmTemplate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MigAz.Providers
{
    public class AzureTelemetryProvider : MigAz.Core.Interface.ITelemetryProvider
    {
        private Dictionary<string,string> GetProcessedItems(AzureGenerator templateResult)
        {
            Dictionary<string, string> processedItems = new Dictionary<string, string>();

            foreach (ArmResource resource in templateResult.Resources)
            {
                if (!processedItems.ContainsKey(resource.type + resource.name))
                    processedItems.Add(resource.type + resource.name, resource.location);
            }

            return processedItems;
        }

        public void PostTelemetryRecord(AzureGenerator templateGenerator)
        {
            TelemetryRecord telemetryrecord = new TelemetryRecord();
            telemetryrecord.ExecutionId = templateGenerator.ExecutionGuid;
            telemetryrecord.SubscriptionId = templateGenerator.SourceSubscription.SubscriptionId;
            telemetryrecord.TenantId = templateGenerator.SourceSubscription.AzureAdTenantId;
            telemetryrecord.OfferCategories = templateGenerator.SourceSubscription.offercategories;
            telemetryrecord.SourceVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            telemetryrecord.ProcessedResources = this.GetProcessedItems(templateGenerator);

            string jsontext = JsonConvert.SerializeObject(telemetryrecord, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(jsontext);

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://api.migaz.tools/v1/telemetry/ARMtoARM");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //TelemetryRecord mytelemetry = (TelemetryRecord)JsonConvert.DeserializeObject(jsontext, typeof(TelemetryRecord));
            }
            catch (Exception exception)
            {
                DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
