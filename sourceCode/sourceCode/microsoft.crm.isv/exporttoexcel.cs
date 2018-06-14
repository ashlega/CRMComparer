using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Crm.Isv.Customizations;

namespace Microsoft.Crm.Isv
{
    public class ExportToExcel
    {
        private string _fileName;
        private CustomizationComparison _comparison;

        public ExportToExcel(string fileName, CustomizationComparison comparison)
        {
            _fileName = fileName;
            _comparison = comparison;
        }

        public void Export()
        {
            StringBuilder htmlOutput = new StringBuilder();
            htmlOutput.Append("<html><table width='100%' style='font-family:Trebuchet MS; font-size:10pt;'><tr style='text-align:left; background-color:#466094; color:white;'><td>Element Name</td><td>Element Type</td><td>Parent Entity</td><td>Result</td></tr>");
            WriteToTable(_comparison, "NA", "NA", 0, htmlOutput);
            htmlOutput.Append("</table></html>");

            using (StreamWriter writer = new StreamWriter(_fileName))
            {
                writer.Write(htmlOutput.ToString());
            }
        }

        private void WriteToTable(CustomizationComparison self, string parent, string type, int depth, StringBuilder htmlOutput)
        {
            object value = self.SourceValue ?? self.TargetValue;
            if (value is IIdentifiable[] || value is ImportExportXml)
            {
            }
            else
            {
                htmlOutput.Append("<tr style='border-bottom:solid 1px #466094;'>");
                htmlOutput.AppendFormat("<td>{0}</td>", GetSpacing(depth) + self.Name);
                htmlOutput.AppendFormat("<td>{0}</td>", type);
                htmlOutput.AppendFormat("<td>{0}</td>", parent);
                htmlOutput.AppendFormat("<td>{0}</td>", GetChangeStatus(self));
                htmlOutput.Append("</tr>");
            }

            type = GetTypeName(self, type);
            parent = GetParent(self, parent);    
            depth++;
            foreach (CustomizationComparison child in self.Children)
            {                
                WriteToTable(child, parent, type, depth, htmlOutput);
            }       
            depth--;
        }

        private string GetChangeStatus(CustomizationComparison self)
        {
            if (!self.IsDifferent)
            {
                return "Unchanged";
            }
            else
            {
                if (self.SourceValue != null && self.TargetValue != null)
                {
                    return "Changed";
                }
                else if (self.SourceValue == null)
                {
                    return "Not in Source";
                }
                else
                {
                    return "Not in Target";
                }
            }
        }

        private string GetSpacing(int depth)
        {
            string ret = "";
            for (int x = 0; x < depth; x++)
            {
                ret += "&nbsp;";
            }
            return ret;
        }

        private string GetTypeName(CustomizationComparison self, string typeString)
        {
            object selfValue = self.SourceValue ?? self.TargetValue;
            if (selfValue is IIdentifiable[])
            {
                typeString = ComparisonTypeMap.GetComparisonTypeName(selfValue);
            }

            return typeString;
        }

        private string GetParent(CustomizationComparison self, string parentString)
        {
            object selfValue = self.SourceValue ?? self.TargetValue;
            if (selfValue is ImportExportXml)
            {
                parentString = "NA";
            }
            else if (selfValue is EntitiesTypeEntity)
            {
                parentString = self.Name;
            }
            if (selfValue is EntityMapsTypeEntityMap)
            {
                EntityMapsTypeEntityMap entitymapValue = (EntityMapsTypeEntityMap)selfValue;
                parentString = String.Format("{0} / {1}", entitymapValue.EntitySource, entitymapValue.EntityTarget);
            }
            else if (selfValue is EntityRelationShipsTypeEntityRelationship)
            {
                EntityRelationShipsTypeEntityRelationship relationshipValue = (EntityRelationShipsTypeEntityRelationship)selfValue;
                parentString = String.Format("{0} / {1}", relationshipValue.FirstEntityName, relationshipValue.SecondEntityName);
                
            }
            else if (selfValue is RolesTypeRole)
            {
                parentString = "NA";
            }
            else if (selfValue is WorkflowsTypeWorkflow)
            {
                parentString = ((WorkflowsTypeWorkflow)selfValue).PrimaryEntity;
            }

            return parentString;
        }
    }
}
