using System.Configuration;
using System.IO;

namespace Tricentis.Tosca.Integration.JiraXray.Helpers
{
    public class ToscaTaskExecutionLogger
    {
        private static readonly string _logFilePath = ConfigurationManager.AppSettings["ClientsFilePath"];
        public static void WriteToLogFile(string line)
        {
            File.AppendAllText(_logFilePath, line + "\n\n");
        }
    }
}
