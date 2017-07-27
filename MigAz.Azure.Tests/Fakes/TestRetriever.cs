using MigAz.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Tests.Fakes
{
    public class TestRetriever : AzureRetriever
    {
        private AzureContext _AzureContext;

        public TestRetriever(AzureContext azureContext) : base(azureContext)
        {
            _AzureContext = azureContext;
        }

        

    }
}
