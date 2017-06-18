﻿using MigAz.AWS.Models;
using MigAz.Core.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MigAz.AWS.Providers
{
    public class AwsToArmTelemetryProvider : ITelemetryProvider
    {
        public void PostTelemetryRecord(string AccessKey,Dictionary<string, string> processedItems, string AWSRegion)
        {
            AwsToArmTelemetryRecord telemetryrecord = new AwsToArmTelemetryRecord();
            telemetryrecord.ExecutionId = Guid.Empty; // TODO, move to TemplateResult ID
            telemetryrecord.AccessKeyId = AccessKey;
            telemetryrecord.AWSRegion = AWSRegion;
            telemetryrecord.SourceVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            telemetryrecord.ProcessedResources = processedItems;

            string jsontext = JsonConvert.SerializeObject(telemetryrecord, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(jsontext);

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://api.migaz.tools/v1/telemetry/AWStoARM");
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
