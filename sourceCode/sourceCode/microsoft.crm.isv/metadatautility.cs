using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Crm.Sdk.Metadata;
using Microsoft.Crm.SdkTypeProxy.Metadata;
using System.Web.Services.Protocols;
using System.Xml;
using System.Globalization;

namespace Microsoft.Crm.Isv
{
    public static class MetadataUtility
    {
        private static Dictionary<string, EntityMetadata> _entityCache = new Dictionary<string, EntityMetadata>();
        private static object _entityCacheLock = new object();

        public static string RetrievePrimaryAttribute(MetadataService metadataService, string entityName)
        {
            EntityMetadata entitymetadata = RetrieveEntityMetadata(metadataService, entityName);
            return entitymetadata.PrimaryField;
        }

        public static EntityMetadata RetrieveEntityMetadata(MetadataService metadataService, string entityName)
        {
            string key = GetEntityKey(
                metadataService.Url,
                metadataService.CrmAuthenticationTokenValue.OrganizationName, 
                entityName);

            EntityMetadata entityMetadata = null;

            lock (_entityCacheLock)
            {
                if (!_entityCache.TryGetValue(key, out entityMetadata))
                {
                    RetrieveEntityRequest entityRequest = new RetrieveEntityRequest()
                    {
                        LogicalName = entityName,
                        EntityItems = EntityItems.IncludeAttributes,
                        RetrieveAsIfPublished = false
                    };

                    try
                    {
                        RetrieveEntityResponse entityResponse = (RetrieveEntityResponse)metadataService.Execute(entityRequest);
                        entityMetadata = entityResponse.EntityMetadata;
                    }
                    catch (SoapException ex)
                    {
                        // rethrow all exceptions except for a missing entity
                        XmlNode node;
                        if (ex.Detail == null ||
                            (node = ex.Detail.SelectSingleNode("//code")) == null ||
                            node.InnerText != "0x80040217")
                        {
                            throw;
                        }
                    }

                    _entityCache.Add(key, entityMetadata);
                }
            }

            return entityMetadata;
        }

        private static string GetEntityKey(string serviceUrl, string orgName, string entityName)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", entityName, orgName, serviceUrl);
        }

    }
}
