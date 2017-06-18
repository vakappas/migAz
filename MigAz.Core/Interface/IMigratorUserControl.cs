using MigAz.Core.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Core.Interface
{
    public class IMigratorUserControl : UserControl
    {
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;
        private TemplateGenerator _TemplateGenerator;

        public IMigratorUserControl() { }

        public IMigratorUserControl(IStatusProvider statusProvider, ILogProvider logProvider)
        {
            _statusProvider = statusProvider;
            _logProvider = logProvider;
        }

        public ILogProvider LogProvider
        {
            get { return _logProvider; }
        }

        public IStatusProvider StatusProvider
        {
            get { return _statusProvider; }
        }

        public TemplateGenerator TemplateGenerator
        {
            get { return _TemplateGenerator; }
            set { _TemplateGenerator = value; }
        }

        public virtual void SeekAlertSource(object sourceObject)
        {

        }

        public virtual void PostTelemetryRecord()
        {

        }
    }
}
