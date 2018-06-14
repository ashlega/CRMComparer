using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Microsoft.Crm.Isv.CustomizationComparisonUtility
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] != "/all")
                {
                    var connector = new CRMConnector();
                    connector.CreateChangleLogForSolution(args[0]);
                }
            }
            else 
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
