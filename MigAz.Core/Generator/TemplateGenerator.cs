using System;
using System.Collections.Generic;
using System.IO;
using MigAz.Core.Interface;
using MigAz.Core.ArmTemplate;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;

namespace MigAz.Core.Generator
{
    public abstract class TemplateGenerator
    {
        private Guid _ExecutionGuid = Guid.NewGuid();
        private List<ArmResource> _Resources = new List<ArmResource>();
        private Dictionary<string, ArmTemplate.Parameter> _Parameters = new Dictionary<string, ArmTemplate.Parameter>();
        private List<MigAzGeneratorAlert> _Alerts = new List<MigAzGeneratorAlert>();
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;
        private Dictionary<string, MemoryStream> _TemplateStreams = new Dictionary<string, MemoryStream>();
        private string _OutputDirectory = String.Empty;
        private ISubscription _SourceSubscription;
        private ISubscription _TargetSubscription;

        public delegate Task AfterTemplateChangedHandler(TemplateGenerator sender);
        public event EventHandler AfterTemplateChanged;

        private TemplateGenerator() { }

        public TemplateGenerator(ILogProvider logProvider, IStatusProvider statusProvider, ISubscription sourceSubscription, ISubscription targetSubscription)
        {
            _logProvider = logProvider;
            _statusProvider = statusProvider;
            _SourceSubscription = sourceSubscription;
            _TargetSubscription = targetSubscription;
        }

        public ILogProvider LogProvider
        {
            get { return _logProvider; }
        }

        public IStatusProvider StatusProvider
        {
            get { return _statusProvider; }
        }

        public ISubscription SourceSubscription { get { return _SourceSubscription; } set { _SourceSubscription = value; } }
        public ISubscription TargetSubscription { get { return _TargetSubscription; } set { _TargetSubscription = value; } }

        public Guid ExecutionGuid
        {
            get { return _ExecutionGuid; }
        }

        public List<MigAzGeneratorAlert> Alerts
        {
            get { return _Alerts; }
            set { _Alerts = value; }
        }
        public String OutputDirectory
        {
            get { return _OutputDirectory; }
            set
            {
                if (value == null)
                    throw new ArgumentException("OutputDirectory cannot be null.");

                if (value.EndsWith(@"\"))
                    _OutputDirectory = value;
                else
                    _OutputDirectory = value + @"\";
            }
        }

        public List<ArmResource> Resources { get { return _Resources; } }
        public Dictionary<string, ArmTemplate.Parameter> Parameters { get { return _Parameters; } }
        public Dictionary<string, MemoryStream> TemplateStreams { get { return _TemplateStreams; } }

        public bool HasErrors
        {
            get
            {
                foreach (MigAzGeneratorAlert alert in this.Alerts)
                {
                    if (alert.AlertType == AlertType.Error)
                        return true;
                }

                return false;
            }
        }

        public bool IsProcessed(ArmResource resource)
        {
            return this.Resources.Contains(resource);
        }

        public void AddResource(ArmResource resource)
        {
            if (!IsProcessed(resource))
            {
                this.Resources.Add(resource);
                _logProvider.WriteLog("TemplateResult.AddResource", resource.type + resource.name + " added.");
            }
            else
                _logProvider.WriteLog("TemplateResult.AddResource", resource.type + resource.name + " already exists.");

        }

        public void AddAlert(AlertType alertType, string message, object sourceObject)
        {
            this.Alerts.Add(new MigAzGeneratorAlert(alertType, message, sourceObject));
        }

        public bool ResourceExists(Type type, string objectName)
        {
            object resource = GetResource(type, objectName);
            return resource != null;
        }

        public object GetResource(Type type, string objectName)
        {
            foreach (ArmResource armResource in this.Resources)
            {
                if (armResource.GetType() == type && armResource.name == objectName)
                    return armResource;
            }

            return null;
        }

        public virtual async Task UpdateArtifacts(IExportArtifacts artifacts)
        {
            throw new Exception("Must override UpdateArtifacts.");
        }

        public void Write()
        {
            if (!Directory.Exists(_OutputDirectory))
            {
                Directory.CreateDirectory(_OutputDirectory);
            }

            foreach (string key in TemplateStreams.Keys)
            {
                MemoryStream ms = TemplateStreams[key];
                using (FileStream file = new FileStream(_OutputDirectory + key, FileMode.Create, System.IO.FileAccess.Write))
                {
                    byte[] bytes = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(bytes, 0, (int)ms.Length);
                    file.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public string BuildMigAzMessages()
        {
            if (this.Alerts.Count == 0)
                return String.Empty;

            StringBuilder sbMigAzMessageResult = new StringBuilder();

            sbMigAzMessageResult.Append("<p>MigAz has identified the following advisements during template generation for review:</p>");

            sbMigAzMessageResult.Append("<p>");
            sbMigAzMessageResult.Append("<ul>");
            foreach (MigAzGeneratorAlert migAzMessage in this.Alerts)
            {
                sbMigAzMessageResult.Append("<li>");
                sbMigAzMessageResult.Append(migAzMessage);
                sbMigAzMessageResult.Append("</li>");
            }
            sbMigAzMessageResult.Append("</ul>");
            sbMigAzMessageResult.Append("</p>");

            return sbMigAzMessageResult.ToString();
        }

        //The event-invoking method that derived classes can override.
        protected virtual void OnTemplateChanged()
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler handler = AfterTemplateChanged;
            if (handler != null)
            {
                handler(this, null);
            }
        }

        public virtual async Task SerializeStreams() { }

        public virtual async Task GenerateStreams() { }
    }
}