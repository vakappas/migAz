using MigAz.Core.Interface;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace MigAz.Core
{
    public class VersionCheck
    {
        private ILogProvider _LogProvider;

        private VersionCheck() { }

        public VersionCheck(ILogProvider logProvider)
        {
            _LogProvider = logProvider;
        }

        public async Task<string> GetAvailableVersion(string url, string currentVersion)
        {
            _LogProvider.WriteLog("GetAvailableVersion", "Start");

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            string availableversion = String.Empty;

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                availableversion = result.ToString();
            }
            catch (Exception exc)
            {
                _LogProvider.WriteLog("GetAvailableVersion", exc.Message + exc.StackTrace);
            }

            _LogProvider.WriteLog("GetAvailableVersion", "End");
            return availableversion;
        }

        public bool IsVersionNewer(string currentVersion, string availableversion)
        {
            _LogProvider.WriteLog("IsVersionNewer", "Start");

            // previous use of " in strings, removing if existing
            currentVersion = currentVersion.Replace("\"", String.Empty);
            availableversion = availableversion.Replace("\"", String.Empty);

            string[] currentVersionArray = currentVersion.Split('.');
            string[] availableversionArray = availableversion.Split('.');

            if (currentVersionArray.Length != 4 || availableversionArray.Length != 4)
            {
                _LogProvider.WriteLog("IsVersionNewer", "Unable to split version for comparison, returning true.  Current: " + currentVersion + " Availale: " + availableversion);
                return true;
            }

            for (int i = 0; i < currentVersionArray.Length; i++)
            {
                try
                {
                    int current = Convert.ToInt32(currentVersionArray[i]);
                    int available = Convert.ToInt32(availableversionArray[i]);

                    if (current > available)
                    {
                        _LogProvider.WriteLog("IsVersionNewer", "No newer version available (current " + currentVersion + " exceeds available " + availableversion + ").");
                        return false;
                    }

                    if (current < available)
                    {
                        _LogProvider.WriteLog("IsVersionNewer", "Newer version available (current " + currentVersion + " available " + availableversion + ").");
                        return true;
                    }
                }
                catch
                {
                    _LogProvider.WriteLog("IsVersionNewer", "Unable to convert to int for comparison, returning true.  Current: " + currentVersion + " Availale: " + availableversion);
                    return true;
                }
            }

            _LogProvider.WriteLog("IsVersionNewer", "No new version found (matched version " + currentVersion + ").");
            return false;
        }
    }
}
