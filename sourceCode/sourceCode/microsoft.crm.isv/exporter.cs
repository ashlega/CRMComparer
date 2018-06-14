using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Crm.SdkTypeProxy;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Query;
using System.Xml.Serialization;
using System.Xml;
using System.Web.Services.Protocols;
using Microsoft.Crm.SdkTypeProxy.Metadata;
using Microsoft.Crm.Sdk.Metadata;
using System.Linq;
using System.Threading;
using System.ComponentModel;

namespace Microsoft.Crm.Isv
{
	public class Exporter
	{
		public string OutputPath { get; private set; }
		public IEnumerable<String> EntitiesForExport { get; private set; }

		private CrmService _crmService;
        private MetadataService _metadataService;

        private ServiceFactory _factory;

        delegate ExportResults ExportCaller();
        ExportCaller _exportCaller;

        public event EventHandler<ExportCompletedEventArgs> ExportCompleted;
        public event EventHandler<ProgressChangedEventArgs> ExportProgressChanged;
        public event EventHandler<StatusUpdatedEventArgs> ExportStatusUpdated;

        public Exporter(ServiceFactory serviceFactory, string outputPath, string entitiesForExportPath)
            :this(serviceFactory, outputPath, ExportConfig.Load(entitiesForExportPath).Entities)
        {
        }

		public Exporter(ServiceFactory serviceFactory, string outputPath, IEnumerable<String> entitiesForExport)
		{
			OutputPath = outputPath;
			EntitiesForExport = entitiesForExport;
            _factory = serviceFactory;

            _exportCaller = new ExportCaller(Export);
		}

        public void ExportAsync()
        {
            _exportCaller.BeginInvoke(ExportComplete, null);
        }

        private void ExportComplete(IAsyncResult result)
        {
            try
            {
                ExportResults results = _exportCaller.EndInvoke(result);
                OnExportCompleted(new ExportCompletedEventArgs(results, null));
            }
            catch (Exception e)
            {
                OnExportCompleted(new ExportCompletedEventArgs(null, e));
            }
        }

        protected virtual void OnExportCompleted(ExportCompletedEventArgs args)
        {
            EventHandler<ExportCompletedEventArgs> handler = this.ExportCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected virtual void OnProgressChanged(ProgressChangedEventArgs args)
        {
            EventHandler<ProgressChangedEventArgs> handler = this.ExportProgressChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void ReportInformation(string status)
        {
            OnExportStatusUpdated(new StatusUpdatedEventArgs(status, StatusLevel.Information));
        }

        private void ReportError(string status)
        {
            OnExportStatusUpdated(new StatusUpdatedEventArgs(status, StatusLevel.Error));
        }

        private void ReportDetail(string status)
        {
            OnExportStatusUpdated(new StatusUpdatedEventArgs(status, StatusLevel.Detail));
        }

        protected virtual void OnExportStatusUpdated(StatusUpdatedEventArgs args)
        {
            EventHandler<StatusUpdatedEventArgs> handler = this.ExportStatusUpdated;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public ExportResults Export()
		{
            ReportInformation(String.Format("Starting export to file \"{0}\".", this.OutputPath));
            ReportInformation(String.Format("Connecting to CRM server at {0}", this._factory.ServerUrl));

            _crmService = _factory.GetCrmService();
            _metadataService = _factory.GetMetadataService();

            ExportResults ret = new ExportResults();
            List<String> entitiesNotFound;

            if (VerifyExportShouldWork(out entitiesNotFound))
            {
                try
                {
                    List<EntityResult> entityResults = new List<EntityResult>();
                    List<BusinessEntity> allRecords = new List<BusinessEntity>();
                    int count = 0;
                    int total = EntitiesForExport.Count();
                    foreach (String entityName in EntitiesForExport)
                    {
                        count++;
                        int percentage = (int)Math.Round(100.0 * count / total);

                        ReportInformation(String.Format("Exporting {0}...", entityName));
                        OnProgressChanged(new ProgressChangedEventArgs(percentage, null));

                        List<BusinessEntity> entityRecords = RetrieveAllRecords(entityName);
                        foreach (DynamicEntity entity in entityRecords)
                        {
                            ReportDetail(String.Format(
                                "Exported {0} with id {1}.",
                                entity.Name,
                                entity.Properties.OfType<KeyProperty>().First().Value.Value));
                        }
                        entityResults.Add(new EntityResult(entityName, entityRecords.Count));
                        allRecords.AddRange(entityRecords);
                    }                

                    XmlSerializer serializer = new XmlSerializer(typeof(List<BusinessEntity>));
                    XmlTextWriter writer = new XmlTextWriter(OutputPath, Encoding.ASCII);
                    serializer.Serialize(writer, allRecords);
                    writer.Close();

                    ret.EntityResults = entityResults;
                    ret.Success = true;
                }
                catch (SoapException ex)
                {
                    ret.Success = false;
                    ret.ErrorMessage = String.Format(ex.Detail.InnerText);
                    ReportError(ex.ToString());
                }
                catch (Exception ex)
                {
                    ret.Success = false;
                    ret.ErrorMessage = ex.Message;
                    ReportError(ex.ToString());
                }                
            }
            else
            {
                ret.Success = false;
                ret.ErrorMessage = String.Format("Verification of Export Failed because entity(ies) {0} do not exist in the environment", String.Join(",", entitiesNotFound.ToArray()));
            }

            return ret;
		}

        private bool VerifyExportShouldWork(out List<String> entitiesNotFound)
        {
            entitiesNotFound = new List<string>();
            RetrieveAllEntitiesRequest entityRequest = new RetrieveAllEntitiesRequest();
            entityRequest.MetadataItems = MetadataItems.EntitiesOnly;
            entityRequest.RetrieveAsIfPublished = false;

            RetrieveAllEntitiesResponse entityResponse = (RetrieveAllEntitiesResponse)_metadataService.Execute(entityRequest);

            foreach (String entity in EntitiesForExport)
            {
                bool foundEntity = false;

                foreach (EntityMetadata entityMetadata in entityResponse.CrmMetadata)
                {
                    if (entity.ToLower() == entityMetadata.SchemaName.ToLower())
                    {
                        foundEntity = true;
                        break;
                    }
                }

                if (!foundEntity)
                {
                    entitiesNotFound.Add(entity);
                }
            }

            return entitiesNotFound.Count == 0;
        }

		private List<BusinessEntity> RetrieveAllRecords(string entityName)
		{

            RetrieveMultipleResponse getAllRecordsResponse = null;
            List<BusinessEntity> results = new List<BusinessEntity>();
            int page = 1;
            string pagingCookie = "";

            QueryExpression getAllRecordsQuery = new QueryExpression(entityName)
            {
                ColumnSet = GetColumnsForExport(entityName)
            };

            RetrieveMultipleRequest getAllRecordsRequest = new RetrieveMultipleRequest()
            {
                ReturnDynamicEntities = true
            };

            do
            {                
                getAllRecordsQuery.PageInfo = new PagingInfo();
                getAllRecordsQuery.PageInfo.Count = 5000;
                getAllRecordsQuery.PageInfo.PageNumber = page;
                getAllRecordsQuery.PageInfo.PagingCookie = pagingCookie;    

                getAllRecordsRequest.Query = getAllRecordsQuery;                
                getAllRecordsResponse = (RetrieveMultipleResponse)_crmService.Execute(getAllRecordsRequest);

                results.AddRange(getAllRecordsResponse.BusinessEntityCollection.BusinessEntities);
                page++;
                pagingCookie = getAllRecordsResponse.BusinessEntityCollection.PagingCookie;
            }
            while(getAllRecordsResponse.BusinessEntityCollection.MoreRecords);

             return results;
		}

        private ColumnSet GetColumnsForExport(string entityName)
        {
            EntityMetadata entityMetadata = MetadataUtility.RetrieveEntityMetadata(_metadataService, entityName);

            var findValidColumnsQuery = from attribute in entityMetadata.Attributes
                                        where (attribute.ValidForCreate.Value && attribute.ValidForUpdate.Value && String.IsNullOrEmpty(attribute.AttributeOf)) || 
                                            attribute.LogicalName == "statecode"
                                        select attribute.LogicalName;

            return new ColumnSet(findValidColumnsQuery.ToArray());
        }
	}

    public class ExportCompletedEventArgs : AsyncCompletedEventArgs
    {
        public ExportCompletedEventArgs(ExportResults results, Exception error)
            :base(error, false, null)
        {
            this.ExportResults = results;
        }
        
        public ExportResults ExportResults { get; private set; }
    }

}
