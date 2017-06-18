using Newtonsoft.Json;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System;
using MigAz.Core.Interface;
using MigAz.AWS.Models;

namespace MigAz.AWS.Providers
{
    public class AwsToArmSaveSelectionProvider : ISaveSelectionProvider
    {
        string filePath;
        List<SaveSelection> saveSelections;

        public AwsToArmSaveSelectionProvider()
        {
            string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MIGAZ";
            if (!Directory.Exists(filedir)) { Directory.CreateDirectory(filedir); }

            filePath = filedir + "\\MIGAZ-SaveSelection.json";
        }

        public void Save(string AWSRegion, ListView lvwVirtualNetworks, ListView lvwVirtualMachines)
        {
            string jsontext = "";

            try
            {
                StreamReader saveSelectionReader = new StreamReader(filePath);
                jsontext = saveSelectionReader.ReadToEnd();
                saveSelectionReader.Close();

                saveSelections = JsonConvert.DeserializeObject<List<SaveSelection>>(jsontext);

                // If save selection for this subscription alredy exists, remove it
                if (saveSelections.Exists(x => x.AWSRegion == AWSRegion))
                {
                    saveSelections.Remove(saveSelections.Find(x => x.AWSRegion == AWSRegion));
                }
            }
            catch
            {
                // If file does not exist, or invalid, starts a new object
                saveSelections = new List<SaveSelection>();
            }

            SaveSelection saveSelection = new SaveSelection();
            saveSelection.AWSRegion = AWSRegion;

            saveSelection.VirtualNetworks = new List<string>();
            foreach (ListViewItem virtualNetwork in lvwVirtualNetworks.CheckedItems)
            {
                saveSelection.VirtualNetworks.Add(virtualNetwork.Text);
            }

            saveSelection.VirtualMachines = new List<SaveSelectioVirtualMachine>();
            foreach (ListViewItem virtualMachine in lvwVirtualMachines.CheckedItems)
            {
                SaveSelectioVirtualMachine saveSelectionVirtualMachine = new SaveSelectioVirtualMachine();
                saveSelectionVirtualMachine.InstanceId = virtualMachine.SubItems[0].Text;
                saveSelectionVirtualMachine.InstanceName = virtualMachine.SubItems[1].Text;

                saveSelection.VirtualMachines.Add(saveSelectionVirtualMachine);
            }

            saveSelections.Add(saveSelection);

            // save blob copy details file
            jsontext = JsonConvert.SerializeObject(saveSelections, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            StreamWriter saveSelectionWriter = new StreamWriter(filePath);
            saveSelectionWriter.Write(jsontext);
            saveSelectionWriter.Close();
        }

        public void Read(string AWSRegion, ref ListView lvwVirtualNetworks, ref ListView lvwVirtualMachines)
        {
            try
            {
                StreamReader saveSelectionReader = new StreamReader(filePath);
                string jsontext = saveSelectionReader.ReadToEnd();
                saveSelectionReader.Close();

                saveSelections = JsonConvert.DeserializeObject<List<SaveSelection>>(jsontext);
            }
            catch
            {
                // If file does not exist, or invalid, starts a new object
                saveSelections = new List<SaveSelection>();
            }

            if (saveSelections.Exists(x => x.AWSRegion == AWSRegion))
            {
                SaveSelection saveSelection = saveSelections.Find(x => x.AWSRegion == AWSRegion);

                foreach (string virtualNetwork in saveSelection.VirtualNetworks)
                {
                    lvwVirtualNetworks.FindItemWithText(virtualNetwork).Checked = true;
                }

              
                foreach (SaveSelectioVirtualMachine saveSelectionVirtualMachine in saveSelection.VirtualMachines)
                {
                    int startAt = lvwVirtualMachines.FindItemWithText(saveSelectionVirtualMachine.InstanceId).Index;
                    lvwVirtualMachines.FindItemWithText(saveSelectionVirtualMachine.InstanceName, true, startAt).Checked = true;
                }
            }
        }
    }
}
