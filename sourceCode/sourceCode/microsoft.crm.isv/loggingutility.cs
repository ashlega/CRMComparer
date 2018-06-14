using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Microsoft.Crm.Isv
{
    public static class LoggingUtility
    {
        public static void LogStatus(string status, StatusLevel level)
        {
            String logMessage = String.Format("{0} [{1}] - {2}{3}",
                DateTime.Now, level, status, Environment.NewLine);

            File.AppendAllText(GetLogFilename(), logMessage);
        }

        public static string GetLogFilename()
        {
            string program = Environment.GetCommandLineArgs()[0];
            return Path.ChangeExtension(program, ".log");
        }



    }
}
