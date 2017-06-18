using MigAz.Core.Interface;
using System.Collections.Generic;

namespace MigAz.AWS
{
    public class AWSExportArtifacts : IExportArtifacts
    {
        public AWSExportArtifacts()
        {
            StorageAccounts = new List<EbsVolume>();
            VirtualNetworks = new List<VPC>();
            Instances = new List<Ec2Instance>();
        }

        public List<EbsVolume> StorageAccounts { get; private set; }
        public List<VPC> VirtualNetworks { get; private set; }
        public List<Ec2Instance> Instances { get; private set; }
    }
}