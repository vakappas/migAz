using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Models
{
    public class CopyBlobDetail
    {
        public string SourceEnvironment;
        public string SourceSA;
        public string SourceContainer;
        public string SourceBlob;
        public string SourceKey;
        public string DestinationSA;
        public string DestinationContainer;
        public string DestinationBlob;
        public string DestinationKey = String.Empty;
        public string Status = String.Empty;
        public long TotalBytes = 0;
        public long BytesCopied = 0;
        public string StartTime = String.Empty;
        public string EndTime = String.Empty;
        public string SnapshotTime = String.Empty;
    }

}
