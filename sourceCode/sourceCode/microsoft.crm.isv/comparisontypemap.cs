// ======================================================================================
//  File:		ComparisonTypeMap.cs
//  Summary:	ComparisonTypeMap is used to determine which classes should be
//              extracted into CustomizationComparison instances, which ultimately
//              show up in the tree in the comparison utility.  Additional logic
//              helps determine how the CustomizationComparison instances are
//              rendered both in the tree and in the diff windows.
// ======================================================================================
//
//  This file is part of the Microsoft CRM 4.0 SDK Code Samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//
// =======================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.Crm.Isv.Customizations;
using System.Globalization;

namespace Microsoft.Crm.Isv
{
    /// <summary>
    /// ComparisonTypeMap is used to determine which classes should be 
    /// extracted into CustomizationComparison instances, which ultimately
    /// show up in the tree in the comparison utility.  Additional logic
    /// helps determine how the CustomizationComparison instances are
    /// rendered both in the tree and in the diff windows.
    /// </summary>
    public static class ComparisonTypeMap
    {
        private class ComparisonType
        {
            public ComparisonType()
            {
                this.XmlAttributeOverrides = new XmlAttributeOverrides();
            }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>Used when NameExtractor is undefined. Useful for 
            /// CustomizationComparison instances that have fixed names 
            /// (ie "Entities").</value>
            public string Name { get; set; }


            /// <summary>
            /// Gets or sets the name extractor.
            /// </summary>
            /// <value>A delegate that is called with the customization object 
            /// value as the lone argument to determine the Name that should 
            /// be displayed.</value>
            public Delegate NameExtractor { get; set; }


            /// <summary>
            /// Gets or sets the XmlAttributeOverrides.
            /// </summary>
            /// <value>These overrides are used to clean up the diff window 
            /// xml rendering. These are needed because those windows only 
            /// render parts of the customization file.</value>
            public XmlAttributeOverrides XmlAttributeOverrides { get; set; }
        }

        private static Dictionary<Type, ComparisonType> _comparisonTypeMap = new Dictionary<Type, ComparisonType>();

        private static void RegisterComparisonType<T>(string name)
        {
            RegisterComparisonType<T>(new ComparisonType { Name = name, }, null, null);
        }

        private static void RegisterComparisonType<T>(string name, string itemElement)
        {
            RegisterComparisonType<T>(new ComparisonType { Name = name }, itemElement, null);
        }

        private static void RegisterComparisonTypeWithContainer<T>(string containerName, string containerElement, string itemElement, Func<T,String> nameExtractor)
        {
            RegisterComparisonTypeContainer<T>(containerName, containerElement, itemElement);
            RegisterComparisonType<T>(nameExtractor, itemElement);
        }

        private static void RegisterComparisonTypeContainer<T>(string containerName, string containerElement, string itemElement)
        {
            RegisterComparisonType<T[]>(new ComparisonType { Name = containerName }, containerElement, itemElement);
        }

        //private static void RegisterComparisonTypeContainer<T>(Func<T[], String> nameExtractor, string containerElement, string itemElement)
        //{
        //    RegisterComparisonType<T[]>(new ComparisonType { NameExtractor = nameExtractor }, containerElement, itemElement);
        //}

        private static void RegisterComparisonType<T>(Func<T, String> nameExtractor, string elementName)
        {
            RegisterComparisonType<T>(new ComparisonType { NameExtractor = nameExtractor }, elementName, null);
        }

        private static void RegisterComparisonType<T>(ComparisonType comparisonType, string containerElement, string itemElement)
        {
            if (containerElement != null)
            {
                comparisonType.XmlAttributeOverrides.Add(typeof(T),
                    new XmlAttributes
                    {
                        XmlRoot = new XmlRootAttribute(containerElement)
                    });
            }

            if (itemElement != null && typeof(T).IsArray)
            {
                comparisonType.XmlAttributeOverrides.Add(typeof(T).GetElementType(),
                    new XmlAttributes
                    {
                        XmlType = new XmlTypeAttribute(itemElement)
                    });
            }

            _comparisonTypeMap.Add(typeof(T), comparisonType);
        }

        /// <summary>
        /// Gets the name of the comparison type when the source and the 
        /// target can be different.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static string GetComparisonTypeName(object source, object target)
        {
            String sourceName = GetComparisonTypeName(source);
            String targetName = GetComparisonTypeName(target);

            if (sourceName == null || targetName == null)
            {
                return sourceName ?? targetName;
            }

            if (String.Compare(sourceName, targetName, StringComparison.CurrentCulture) == 0)
            {
                return sourceName;
            }

            return String.Concat(sourceName, " -> ", targetName);
        }

        /// <summary>
        /// Gets the name of the comparison type for a single object.
        /// </summary>
        /// <param name="elem">The element to retrieve the name of.</param>
        /// <returns></returns>
        public static string GetComparisonTypeName(object element)
        {
            if (element == null) return null;

            ComparisonType comparisonType;
            if (_comparisonTypeMap.TryGetValue(element.GetType(), out comparisonType))
            {
                Delegate nameExtractor = comparisonType.NameExtractor;
                if (nameExtractor != null)
                {
                    try
                    {
                        return (string)nameExtractor.Method.Invoke(nameExtractor.Target, new object[] { element });
                    }
                    catch (TargetInvocationException)
                    {
                    }
                }
                else if (comparisonType.Name != null)
                {
                    return comparisonType.Name;
                }
            }

            return element.GetType().Name;
        }

        /// <summary>
        /// Gets the comparison type XML overrides.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static XmlAttributeOverrides GetComparisonTypeXmlOverrides(Type type)
        {
            ComparisonType comparisonType;
            if (_comparisonTypeMap.TryGetValue(type, out comparisonType))
            {
                return comparisonType.XmlAttributeOverrides;
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified type is comparison type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is a comparison type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTypeComparisonType(Type type)
        {
            return _comparisonTypeMap.ContainsKey(type);
        }

        /// <summary>
        /// Gets a list of comparison types.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> ComparisonTypes
        {
            get { return _comparisonTypeMap.Keys; }
        }

        /// <summary>
        /// Initializes the <see cref="ComparisonTypeMap"/> class.
        /// </summary>
        static ComparisonTypeMap()
        {
            RegisterComparisonTypeContainer<ImportExportXml>("Customizations", "ImportExportXml", "ImportExportXml");

            
            
            RegisterComparisonTypeWithContainer<EntitiesTypeEntity>("Entities", "Entities", "Entity",
                x => x.EntityInfo == null ? x.Name.Value.ToLower(CultureInfo.CurrentCulture) : x.EntityInfo.Item.Name.ToLower(CultureInfo.CurrentCulture));

            RegisterComparisonTypeWithContainer<EntityInfoTypeEntityAttribute>("Attributes", "attributes", "attribute",
                x => x.PhysicalName.ToLower(CultureInfo.CurrentCulture));

            RegisterComparisonType<optionstypeOption>(x => x.labels
                      .Where(l => l.languagecode == "1033")
                      .Select(l => l.description).First(), "option");


            //RegisterComparisonType<FormXmlType>("Forms");
            //RegisterComparisonTypeContainer<SystemFormsType>(x => x.Select(l => l.type.ToString()).FirstOrDefault() ?? "Forms"
            //    ,"forms", "systemform");

              RegisterComparisonTypeWithContainer<SystemFormsType>("Forms", "FormXml", "forms", x => x.type.ToString());

           // RegisterComparisonTypeWithContainer<FormsType>("Forms","FormXml","forms", x => x.type.ToString());

            RegisterComparisonType<SystemFormsTypeSystemform>(
                x => x.LocalizedNames 
                    .Where(l => l.languagecode == "1033")
                    .Select(l => l.description).FirstOrDefault() + " Form" ?? "System Form","systemform");

           // RegisterComparisonTypeWithContainer<FormXmlEventsTypeEvent>("Events", "events", "event",
            //    x => x.name);

            RegisterComparisonTypeWithContainer<FormTypeData>("Hidden Controls", "hiddencontrols", "data", x => x.id);

            RegisterComparisonTypeWithContainer<FormTypeTabsTab>("Tabs", "tabs", "tab",
                x => x.labels
                    .Where(l => l.languagecode == "1033")
                    .Select(l => l.description)
                    .FirstOrDefault() ?? "Tab");

            //RegisterComparisonTypeContainer<FormTypeTabsTabColumn>("Columns", "columns", "column");
            RegisterComparisonType<FormTypeTabsTabColumn>("Column","column");

            RegisterComparisonTypeWithContainer<FormTypeTabsTabColumnSectionsSection>("Sections", "sections", "section",
               x => x.labels
                    .Where(l => l.languagecode == "1033")
                    .Select(l => l.description)
                    .FirstOrDefault() ?? "Section");

              RegisterComparisonTypeContainer<FormTypeTabsTabColumnSectionsSectionRows>("Rows", "rows", "row");
              RegisterComparisonType<FormTypeTabsTabColumnSectionsSectionRowsRow>("Row", "row");

              RegisterComparisonTypeWithContainer<ConnectionRolesTypeConnectionRolesConnectionRole>("Connection Roles", "ConnectionRoles", "ConnectionRole",
                  x => x.name);

              RegisterComparisonTypeWithContainer<ConnectionRolesTypeConnectionRolesConnectionRoleConnectionRoleObjectTypeCode>("Connection Role Object Type Codes", "ConnectionRoleObjectTypeCodes", "ConnectionRoleObjectTypeCode",
                  x => x.connectionroleobjecttypecodeid);

              RegisterComparisonTypeWithContainer<SiteMapTypeArea>("Site Map", "SiteMap", "Area",
                  x => x.Id);

              RegisterComparisonType<SiteMapTypeAreaGroup>(x => x.Id, "Group");
              RegisterComparisonType<SiteMapTypeAreaGroupSubArea>(x => x.Id, "SubArea");

            //  RegisterComparisonType<FormTypeTabsTabColumnSectionsSectionRowsRowCell>(x => x.labels
            //          .Where(l => l.languagecode == "1033")
            //         .Select(l => l.description)
            //         .FirstOrDefault() ?? "Cell",
            //     "cell");

           // RegisterComparisonTypeWithContainer<SavedQueriesTypeSavedqueriesSavedquery>("Saved Queries", "savedqueries", "savedquery",
           //     x => x.LocalizedNames
           //         .Where(l => l.languagecode == "1033")
           //         .Select(l => l.description).FirstOrDefault() ?? "Saved Query");

            RegisterComparisonTypeWithContainer<EntityMapsTypeEntityMap>("Entity Maps", "EntityMaps", "EntityMap",
                x => String.Format(CultureInfo.CurrentCulture, "{0} => {1}", x.EntitySource, x.EntityTarget));

           // RegisterComparisonTypeWithContainer<EntityMapsTypeEntityMapAttributeMap>("Attribute Maps", "AttributeMaps", "AttributeMap",
           //     x => String.Format(CultureInfo.CurrentCulture, "{0} => {1}", x.AttributeSource, x.AttributeTarget));

           RegisterComparisonTypeWithContainer<EntityRelationShipsTypeEntityRelationship>("Entity Relationships", "EntityRelationships", "EntityRelationship",
                x => x.Name);

           RegisterComparisonTypeWithContainer<RolesTypeRole>("Roles", "Roles", "Role",
                x => x.name);

           RegisterComparisonTypeWithContainer<WorkflowsTypeWorkflow>("Workflows", "workflow", "Workflow",
               x => x.Name);

           RegisterComparisonTypeWithContainer<FieldSecurityProfilesTypeFieldSecurityProfile>("Field Security Profiles", "FieldSecurityProfiles", "FieldSecurityProfile",
            x => x.name);

           RegisterComparisonType<CrmTemplatesType>("Templates");

           RegisterComparisonTypeWithContainer<CrmTemplatesTypeKbarticletemplate>("KB Article Templates", "KBArticleTemplates", "kbarticletemplate",
               x => x.name);

           RegisterComparisonTypeWithContainer<CrmTemplatesTypeEmailtemplate>("Email Templates", "EmailTemplates", "emailtemplate",
               x => x.name);

           RegisterComparisonTypeWithContainer<CrmTemplatesTypeContracttemplate>("Contract Templates", "ContractTemplates", "contracttemplate",
               x => x.name);

           RegisterComparisonTypeWithContainer<CrmTemplatesTypeMailmergetemplate>("Mail Merge Templates", "MailMergeTemplates", "mailmergetemplate",
               x => x.name);


           RegisterComparisonType<RibbonGlobalDiffXmlType>("Ribbon Global Diff");
           RegisterComparisonType<IsvConfigurationType>("ISV Config");

           RegisterComparisonTypeWithContainer<optionsettype>("Option Sets", "optionsets", "optionset",
               x => x.Name);
           //RegisterComparisonTypeWithContainer<FieldSecurityProfilesTypeFieldSecurityProfile>("Field Security Profiles", "FieldSecurityProfiles", "FieldSecurityProfile",
          // x => x.name);

              RegisterComparisonType<OrganizationSettingsType>("Organization Settings", "OrganizationSettings");

              RegisterComparisonTypeWithContainer<ReportsTypeReport>("Reports", "Reports", "Report",
                  x => x.name);

              RegisterComparisonTypeWithContainer<DashboardsTypeDashboard>("Dashboards", "Dashboards", "Dashboards",
                  x => x.LocalizedNames
                    .Where(l => l.languagecode == "1033")
                    .Select(l => l.description).FirstOrDefault() ?? "Dashboard");

              RegisterComparisonTypeWithContainer<webresourcetype>("Web Resources", "WebResources", "WebResource",
                  x => x.DisplayName);

              RegisterComparisonTypeWithContainer<pluginassemblytype>("Plugin Assemblies", "SolutionPluginAssemblies", "PluginAssembly",
                  x => x.FullName);

              RegisterComparisonTypeWithContainer<sdkmessageprocessingsteptype>("SDK Message Processing Steps", "SdkMessageProcessingSteps", "SdkMessageProcessingStep",
                  x => x.Name);

            //  RegisterComparisonTypeWithContainer<

          //  RegisterComparisonTypeWithContainer<WorkflowsTypeWorkflow>("Workflows", "Workflows", "Workflow",
          //      x => x.Name);

          //  RegisterComparisonTypeContainer<FormTypeTabsTabColumnSectionsSectionRowsRow>("Cells", "rows", "row");
              RegisterComparisonType<FormTypeTabsTabColumnSectionsSectionRowsRowCell>(x => x.labels
                      .Where(l => l.languagecode == "1033")
                      .Select(l => l.description)
                      .FirstOrDefault() ?? "Cell",
                  "cell");

           // RegisterComparisonType<DashboardsTypeDashboard>("Dashboards");
        }
    }
}