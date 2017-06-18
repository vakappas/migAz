using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MigAz.Providers
{
    class FileLogProvider : ILogProvider
    {
        private object lockObject = new object();

        public delegate void OnMessageHandler(string message);
        public event OnMessageHandler OnMessage;

        public FileLogProvider()
        {

        }

        public void WriteLog(string function, string message)
        {
            string logfiledir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz";
            string logfilepath = logfiledir + "\\MigAz-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
            string text = DateTime.Now.ToString() + "   " + function + "  " + message + Environment.NewLine;

            lock (lockObject)
            {
                if (!Directory.Exists(logfiledir)) { Directory.CreateDirectory(logfiledir); }

                File.AppendAllText(logfilepath, text);
            }

            OnMessage?.Invoke(text);
        }
    }
}
