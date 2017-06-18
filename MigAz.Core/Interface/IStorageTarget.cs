using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core.Interface
{
    public enum StorageAccountType
    {
        Standard,
        Premium
    }

    public interface IStorageTarget
    {
        String BlobStorageNamespace { get; }
        StorageAccountType StorageAccountType { get; }
    }
}
