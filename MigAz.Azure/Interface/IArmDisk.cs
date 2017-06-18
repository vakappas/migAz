using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Interface
{
    public interface IArmDisk : IDisk
    {
        Task InitializeChildrenAsync(AzureContext azureContext);
    }
}
