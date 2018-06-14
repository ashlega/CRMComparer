using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Crm.Sdk.Messages;

namespace Microsoft.Crm.Isv
{

    public enum COMPONENT_ACTION
    {
        CREATED = 505000000,
        UPDATED = 505000001,
        REMOVED = 505000002
    }

    public enum COMPONENT_TYPE
    {
        APPLICATION = 505000003,
        ATTRIBUTE = 505000002,
        BUSINESS_RULE = 505000011,
        ENTITY = 505000000,
        FORM = 505000005,
        PLUGIN = 505000008,
        SDK_STEP = 505000009,
        SECURITY_ROLE = 505000001,
        SITE_MAP = 505000010,
        VIEW = 505000004,
        WEB_RESOURCE = 505000007,
        WORKFLOW = 505000006
    }


    public class CRMConnector
    {
        Microsoft.Xrm.Tooling.Connector.CrmServiceClient crmSvc = null;
        private bool extractXml = false;
        public CRMConnector()
        {
            crmSvc = new Microsoft.Xrm.Tooling.Connector.CrmServiceClient(ConfigurationManager.ConnectionStrings["Dynamics"].ConnectionString);
            extractXml = System.Configuration.ConfigurationManager.AppSettings["extractXml"] == "true";
        }

        public int GetLastVersionNumber(string solutionUniqueName, string folder)
        {
            var files = System.IO.Directory.GetFiles(folder, solutionUniqueName + "*");
            int maxVersionNumber = 0;
            foreach(var f in files)
            {
                string fileName = System.IO.Path.GetFileName(f);
                fileName = fileName.Replace(solutionUniqueName, "").Replace(".zip", "");
                if(fileName != "")
                {
                    int fileVersionNumber = 0;
                    if(int.TryParse(fileName, out fileVersionNumber))
                    {
                        if (fileVersionNumber > maxVersionNumber) maxVersionNumber = fileVersionNumber;
                    }
                }
            }
            return maxVersionNumber;
        }

        public string[] ExportSolution(string solutionUniqueName)
        {
            string folder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "solutions");
            if (!System.IO.Directory.Exists(folder)) System.IO.Directory.CreateDirectory(folder);
            int lastFileVersionNumber = GetLastVersionNumber(solutionUniqueName, folder);

            
            string[] result = new string[2];
            ExportSolutionRequest exportSolutionRequest = new ExportSolutionRequest();
            exportSolutionRequest.Managed = false;
            exportSolutionRequest.SolutionName = solutionUniqueName;

            ExportSolutionResponse exportSolutionResponse = (ExportSolutionResponse)crmSvc.Execute(exportSolutionRequest);

            byte[] exportXml = exportSolutionResponse.ExportSolutionFile;
            
            if(lastFileVersionNumber != 0)
            {
                result[0] = folder + "/" + solutionUniqueName + lastFileVersionNumber.ToString() + ".zip";
            }
            lastFileVersionNumber++;
            result[1] = folder + "/" + solutionUniqueName + lastFileVersionNumber.ToString() + ".zip";
            System.IO.File.WriteAllBytes(result[1], exportXml);
            return result;
        }

        public void CleanupOldFiles(string solutionUniqueName)
        {
            string versionsToKeep = System.Configuration.ConfigurationManager.AppSettings["oldVersionsToKeep"];
            if (versionsToKeep != "-1")
            {
                int number = int.Parse(versionsToKeep);
                string folder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "solutions");
                int lastFileVersionNumber = GetLastVersionNumber(solutionUniqueName, folder);
                int deleteVersion = lastFileVersionNumber - number;
                bool found = true;
                while (found)
                {
                    string fileName = folder + "/" + solutionUniqueName + deleteVersion.ToString() + ".zip";
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                        deleteVersion--;
                    }
                    else
                    {
                        found = false;
                    }

                }
            }

        }
        public void AddChangeLogComponent(Entity changeLog, CustomizationComparison comparison, CustomizationComparison parentComparison)
        {
            if (comparison.IsDifferent)
            {
                COMPONENT_ACTION action = COMPONENT_ACTION.UPDATED;
                if (comparison.SourceValue == null) action = COMPONENT_ACTION.CREATED;
                else if (comparison.TargetValue == null) action = COMPONENT_ACTION.REMOVED;

                if (comparison.IsDifferentBase || action != COMPONENT_ACTION.UPDATED)
                {
                    Entity changeLogComponent = new Entity("ita_changelogcomponent");
                    changeLogComponent["ita_name"] = comparison.GetFullPath();
                    changeLogComponent["ita_changelog"] = changeLog.ToEntityReference();
                    changeLogComponent["ita_action"] = new OptionSetValue((int)action);

                    if (extractXml)
                    {
                        CustomizationSerializer serializer = new CustomizationSerializer();
                        IEnumerable<String> sourceLines = serializer.SerializeObjectToLines(comparison, comparison.SourceValue);
                        IEnumerable<String> targetLines = serializer.SerializeObjectToLines(comparison, comparison.TargetValue);

                        string beforeXml = "";
                        if (sourceLines != null)
                        {

                            foreach (var s in sourceLines)
                            {
                                beforeXml = beforeXml + s + System.Environment.NewLine;
                            }
                        }

                        string afterXml = "";
                        if (targetLines != null)
                        {

                            foreach (var s in targetLines)
                            {
                                afterXml = afterXml + s + System.Environment.NewLine;
                            }
                        }

                        changeLogComponent["ita_xmlbefore"] = beforeXml;
                        changeLogComponent["ita_xmlafter"] = afterXml;
                    }
                    

                    changeLogComponent.Id = crmSvc.Create(changeLogComponent);
                }

                if (action == COMPONENT_ACTION.UPDATED)
                {
                    foreach (var c in comparison.Children)
                    {
                        AddChangeLogComponent(changeLog, c, comparison);
                    }
                }
            }
        }

        public void CreateChangleLogForSolution(string solutionUniqueName)
        {
            var result = ExportSolution(solutionUniqueName);
            if (result.Length > 1 && result[0] != null)
            {
                CustomizationComparer comparer = new CustomizationComparer();
                CustomizationComparison comparison = null;
                comparison = comparer.Compare(result[0], result[1]);
                if(comparison.IsDifferent)
                {
                    Entity changeLog = new Entity("ita_changelog");
                    changeLog["ita_name"] = solutionUniqueName;
                    changeLog.Id = crmSvc.Create(changeLog);
                    AddChangeLogComponent(changeLog, comparison, null);
                    CleanupOldFiles(solutionUniqueName);
                }
                else
                {
                    System.IO.File.Delete(result[1]);
                }
            }
        }
    }
}
