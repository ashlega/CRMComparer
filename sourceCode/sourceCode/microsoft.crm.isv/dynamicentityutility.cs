using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.SdkTypeProxy;
using Microsoft.Crm.Sdk.Query;
using Microsoft.Crm.SdkTypeProxy.Metadata;

namespace Microsoft.Crm.Isv
{
    public class DynamicEntityUtility
    {
        public static DynamicEntity MergeDynamicEntities(string entityName, params DynamicEntity[] dynamicEntities)
        {
            DynamicEntity ret = new DynamicEntity(entityName);
            foreach (DynamicEntity dynamicEntity in dynamicEntities)
            {
                foreach (Property prop in dynamicEntity.Properties)
                {
                    ret.Properties.Remove(prop.Name);
                    ret.Properties.Add(prop);
                }
            }

            return ret;
        }

        public static void SetStateDynamicEntity(CrmService crmService, string entityName, Guid id, string statecode, int statuscode)
        {
            DynamicEntity currentRecord = GetById(crmService, entityName, id, "statecode", "statuscode");
            bool shouldUpdateState = true;
            if (currentRecord != null && currentRecord.Properties.Contains("statecode") && currentRecord.Properties.Contains("statuscode"))
            {
                if (statecode != currentRecord.Properties["statecode"].ToString() ||
                    statuscode != ((Status)currentRecord.Properties["statuscode"]).Value)
                    shouldUpdateState = true;
                else
                    shouldUpdateState = false;                
            }

            if (shouldUpdateState)
            {
                SetStateDynamicEntityRequest request = new SetStateDynamicEntityRequest()
                {
                    Entity = new Moniker(entityName, id),
                    State = statecode,
                };

                if (statuscode != -1)
                    request.Status = statuscode;

                crmService.Execute(request);
            }
        }

        public static DynamicEntity GetById(CrmService crmService, string entityName, Guid id, params string[] fields)
        {
            QueryExpression query = new QueryExpression(entityName)
            {
                ColumnSet = new ColumnSet(fields),
            };

            ConditionExpression keyExpression = new ConditionExpression(entityName + "id", ConditionOperator.Equal, id);
            FilterExpression filterExpression = new FilterExpression();
            filterExpression.Conditions.Add(keyExpression);

            query.Criteria.AddFilter(filterExpression);

            RetrieveMultipleRequest request = new RetrieveMultipleRequest()
            {
                Query = query,
                ReturnDynamicEntities = true
            };

            RetrieveMultipleResponse response = (RetrieveMultipleResponse)crmService.Execute(request);

            if (response.BusinessEntityCollection.BusinessEntities.Count > 0)
                return response.BusinessEntityCollection.BusinessEntities[0] as DynamicEntity;

            return null;
        }

        public static IEnumerable<DynamicEntity> GetByIdOrPrimaryAttribute(CrmService _crmService, MetadataService metadataService, string entityName, Guid id, string primaryAttributeValue, params string[] fields)
        {
            List<DynamicEntity> resultList = new List<DynamicEntity>();
            DynamicEntity ret = GetById(_crmService, entityName, id);
            if (ret == null)
            {
                string primaryAttribute = MetadataUtility.RetrievePrimaryAttribute(metadataService, entityName);
                QueryExpression query = new QueryExpression(entityName)
                {
                    ColumnSet = new ColumnSet(fields)
                };

                ConditionExpression primaryAttributeExpression = new ConditionExpression(primaryAttribute.ToLower(), ConditionOperator.Equal, primaryAttributeValue);
                query.Criteria.Conditions.Add(primaryAttributeExpression);

                RetrieveMultipleRequest request = new RetrieveMultipleRequest()
                {
                    Query = query,
                    ReturnDynamicEntities = true
                };

                RetrieveMultipleResponse response = (RetrieveMultipleResponse)_crmService.Execute(request);

                if (response.BusinessEntityCollection.BusinessEntities.Count > 0)
                    resultList.AddRange(response.BusinessEntityCollection.BusinessEntities.ConvertAll<DynamicEntity>(x=> (DynamicEntity)x));
            }
            else
            {
                resultList.Add(ret);
            }
            
            return resultList;
        }
    }
}
