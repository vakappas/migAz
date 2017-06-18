using MigAz.Azure.Generator.AsmToArm;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Tests.Fakes
{
    class FakeLogProvider : ILogProvider
    {
        public void WriteLog(string function, string message)
        {
        }
    }
}
