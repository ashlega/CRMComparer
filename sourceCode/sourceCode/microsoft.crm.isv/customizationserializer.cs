// ======================================================================================
//  File:		CustomizationSerializer.cs
//  Summary:	A utility class to ease the serializing of snippets of the customization xml.
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
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace Microsoft.Crm.Isv
{
    /// <summary>
    /// A utility class to ease the serializing of snippets of the customization xml.
    /// </summary>
    public class CustomizationSerializer
    {
        public IEnumerable<String> SerializeObjectToLines(CustomizationComparison comparison, object item)
        {
            if (item != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.OmitXmlDeclaration = true;
                    settings.Indent = true;

                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");

                    XmlAttributeOverrides overrides = ComparisonTypeMap.GetComparisonTypeXmlOverrides(item.GetType());

                    XmlSerializer serializer = new XmlSerializer(item.GetType(), overrides);

                    XmlFilter filter = new XmlFilter();
                    BuildXmlFilter(filter, item, comparison.ParentProperty);

                    using (FilteringXmlWriter writer = new FilteringXmlWriter(XmlWriter.Create(stream, settings)))
                    {
                        writer.Filter = filter;
                        serializer.Serialize(writer, item, ns);
                    }
                    stream.Seek(0, SeekOrigin.Begin);

                    using (StreamReader sourceReader = new StreamReader(stream))
                    {
                        string line;

                        while ((line = sourceReader.ReadLine()) != null)
                        {
                            yield return line;
                        }
                    }
                }
            }
        }

        private void BuildXmlFilter(XmlFilter filter, object obj, PropertyInfo objProp)
        {
            if (obj != null)
            {
                Type type = obj.GetType();
                if (type.IsArray && objProp != null)
                {
                    XmlArrayItemAttribute arrayItemAttr = (XmlArrayItemAttribute)Attribute.GetCustomAttribute(objProp, typeof(XmlArrayItemAttribute));
                    if (arrayItemAttr != null)
                    {
                        filter.Elements.Add(arrayItemAttr.ElementName);
                    }
                }

                foreach (PropertyInfo prop in type.GetProperties())
                {
                    if (ComparisonTypeMap.ComparisonTypes.Contains(prop.PropertyType))
                    {
                        XmlElementAttribute elemAttr = (XmlElementAttribute)Attribute.GetCustomAttribute(prop, typeof(XmlElementAttribute));
                        filter.Elements.Add(elemAttr != null ? elemAttr.ElementName : prop.Name);
                    }
                    else if(prop.PropertyType.Assembly == Assembly.GetExecutingAssembly())
                    {
                        BuildXmlFilter(filter, prop.GetValue(obj, null), prop);
                    }
                }
            }
        }
    }
}
