using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Metadata;
using Microsoft.Crm.SdkTypeProxy;
using Microsoft.Crm.SdkTypeProxy.Metadata;
using System.IO;

namespace Microsoft.Crm.Isv
{
    public class Importer
    {
        public string InputPath { get; private set; }

        private CrmService _crmService;
        private MetadataService _metadataService;
        private ServiceFactory _factory;
        private int _totalRecordCount;
        private int _currentRecordCount;
        private Dictionary<string, int> _entityRecordCounts;

        delegate ImportResults ImportCaller();
        ImportCaller _importCaller;

        public event EventHandler<ImportCompletedEventArgs> ImportCompleted;
        public event EventHandler<ProgressChangedEventArgs> ImportProgressChanged;
        public event EventHandler<StatusUpdatedEventArgs> ImportStatusUpdated;

        public Importer(ServiceFactory serviceFactory, string inputPath)
        {
            InputPath = inputPath;

            _importCaller = new ImportCaller(Import);

            _factory = serviceFactory;
        }

        private MetadataService GetMetadataService()
        {
            if (_metadataService == null)
            {
                _metadataService = _factory.GetMetadataService();
            }

            return _metadataService;
        }

        public void ImportAsync()
        {
            _importCaller.BeginInvoke(ImportComplete, null);
        }

        private void ImportComplete(IAsyncResult result)
        {
            try
            {
                ImportResults results = _importCaller.EndInvoke(result);
                OnImportCompleted(new ImportCompletedEventArgs(results, null));
            }
            catch (Exception e)
            {
                OnImportCompleted(new ImportCompletedEventArgs(null, e));
            }
        }

        protected virtual void OnImportCompleted(ImportCompletedEventArgs args)
        {
            EventHandler<ImportCompletedEventArgs> handler = this.ImportCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void ReportInformation(string status)
        {
            OnImportStatusUpdated(new StatusUpdatedEventArgs(status, StatusLevel.Information));
        }

        private void ReportError(string status)
        {
            OnImportStatusUpdated(new StatusUpdatedEventArgs(status, StatusLevel.Error));
        }

        private void ReportDetail(string status)
        {
            OnImportStatusUpdated(new StatusUpdatedEventArgs(status, StatusLevel.Detail));
        }

        protected virtual void OnImportStatusUpdated(StatusUpdatedEventArgs args)
        {
            EventHandler<StatusUpdatedEventArgs> handler = this.ImportStatusUpdated;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected virtual void OnImportProgressChanged(ProgressChangedEventArgs args)
        {
            EventHandler<ProgressChangedEventArgs> handler = this.ImportProgressChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public ImportResults Import()
        {
            ReportInformation(String.Format("Starting import from file \"{0}\".", this.InputPath));
            ReportInformation(String.Format("Connecting to CRM server at {0}", this._factory.ServerUrl));

            _crmService = _factory.GetCrmService();

            ImportResults results = new ImportResults();

            ImportData importData = DeserializeRecords();
            MissingMetadata missingMetadata;
            if (IsMetadataMissing(importData, out missingMetadata))
            {
                results.Success = false;
                results.MissingMetadata = missingMetadata;
                results.ErrorMessage = String.Format("CRM server is missing required metadata.");
            }
            else
            {
                try
                {
                    ImportData invalidReferenceData = RemoveInvalidReferences(importData);

                    _currentRecordCount = 0;
                    _entityRecordCounts = importData.Entities.GroupBy(de => de.Name).ToDictionary(g => g.Key, g => 0);

                    _totalRecordCount = importData.Entities.Count;
                    
                    if (invalidReferenceData != null)
                    {
                        _totalRecordCount += invalidReferenceData.Entities.Count;
                    }
                    
                    ImportRecords(importData, "Importing");

                    if (invalidReferenceData != null)
                    {
                        ImportData unresolvableData = RemoveInvalidReferences(invalidReferenceData);
                        if (unresolvableData != null)
                        {
                            IEnumerable<DynamicEntity> unresolvableEntities = 
                                unresolvableData.Entities
                                .OrderBy(e => e.Name);

                            foreach (DynamicEntity unresolvableEntity in unresolvableEntities)
                            {
                                ReportError(String.Format("Could not resolve references on {0} with id of {1}.",
                                    unresolvableEntity.Name,
                                    unresolvableEntity.Properties.OfType<KeyProperty>().First().Value.Value));

                                // still count these records towards our progress
                                _currentRecordCount++;
                            }
                        }                        

                        ImportRecords(invalidReferenceData, "Resolving references on");
                    }
                    results.EntityResults = _entityRecordCounts.Select(pair => new EntityResult(pair.Key, pair.Value));
                    results.Success = true;
                }
                catch (SoapException ex)
                {
                    results.Success = false;
                    results.ErrorMessage = String.Format(ex.Detail.InnerText);
                    ReportError(ex.ToString());
                }
                catch (Exception ex)
                {
                    results.Success = false;
                    results.ErrorMessage = ex.Message;
                    ReportError(ex.ToString());
                }    
            }
            
            return results;
        }

        public bool CanDeserializeRecords()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<BusinessEntity>));
            
            using (XmlTextReader reader = new XmlTextReader(InputPath))
            {
                try
                {
                    return serializer.CanDeserialize(reader);
                }
                catch
                {
                    return false;
                }
            }
        }

        public ImportData DeserializeRecords()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<BusinessEntity>));
            List<BusinessEntity> results;
            using (XmlTextReader reader = new XmlTextReader(InputPath))
            {
                try
                {
                    results = (List<BusinessEntity>)serializer.Deserialize(reader);
                }
                catch (InvalidOperationException e)
                {
                    throw new InvalidDataException(String.Format("'{0}' is not a valid input file.", InputPath), e);
                }
            }

            ImportData importData = new ImportData();
            importData.AddEntities(results
                .Cast<DynamicEntity>()
                .OrderBy(de => de.Name)
                .ToList()
            );

            return importData;

                
        }

        private bool IsMetadataMissing(ImportData importData, out MissingMetadata result)
        {
            MetadataService metadataService = GetMetadataService();

            result = new MissingMetadata();

            foreach (IGrouping<string,DynamicEntity> group in importData.Entities.GroupBy(e => e.Name))
            {
                string entityName = group.Key;

                EntityMetadata entityMetadata = MetadataUtility.RetrieveEntityMetadata(metadataService, entityName);
                if (entityMetadata == null)
                {
                    result.MissingEntities.Add(entityName);
                }
                else
                {
                    IEnumerable<String> distinctAttributes = group
                        .SelectMany(e => e.Properties.Select(prop => prop.Name))
                        .Distinct();

                    List<string> missingAttributes =
                        distinctAttributes
                        .Except(entityMetadata.Attributes.Select(am => am.LogicalName))
                        .ToList();

                    if (missingAttributes.Count > 0)
                    {
                        result.AddMissingAttributes(entityName, missingAttributes);
                    }
                }
            }

            return result.MissingEntities.Any() || result.MissingAttributes.Any();
        }

        private void ImportRecords(ImportData importData, string description)
        {
            List<EntityResult> entityResults = new List<EntityResult>();

            foreach (IGrouping<String,DynamicEntity> group in importData.Entities.GroupBy(e => e.Name).OrderBy(g => g.Key))
            {
                ReportInformation(String.Format("{0} {1}...", description, group.Key));

                foreach (DynamicEntity entity in group)
                {
                    ImportRecord(entity);
                }
            }
        }

        private void ImportRecord(DynamicEntity entity)
        {
            string statecode = "";
            int statuscode = -1;
            if (entity.Properties.Contains("statecode"))
            {
                statecode = entity["statecode"].ToString();
                entity.Properties.Remove("statecode");
            }

            if (entity.Properties.Contains("statuscode"))
            {
                statuscode = ((Status)entity["statuscode"]).Value;
                entity.Properties.Remove("statuscode");
            }

            string primaryAttribute = MetadataUtility.RetrievePrimaryAttribute(_metadataService, entity.Name);
            string primaryAttributeValue = entity.Properties.Contains(primaryAttribute) ? entity[primaryAttribute].ToString() : "";
            Guid id = entity.Properties.OfType<KeyProperty>().First().Value.Value;
            Guid potentialNewId = Guid.Empty;
            int matchingRecordCount = GetMatchingRecordCount(entity.Name, id, primaryAttributeValue, ref potentialNewId);
            switch (matchingRecordCount)
	        {
		        case 0:
                    _crmService.Create(entity);
                    ReportDetail(String.Format("Created {0} with id of {1}.", entity.Name, id));
                    break;
                case 1:
                    if (potentialNewId != Guid.Empty)
                    {
                        id = potentialNewId;
                        entity.Properties.OfType<KeyProperty>().First().Value.Value = potentialNewId;
                    }
                    _crmService.Update(entity);
                    ReportDetail(String.Format("Updated {0} with id of {1}.", entity.Name, id));
                    break;
                default:
                    ReportError(String.Format("Cannot import {0} with id of {1} because it matches more than one record in the target system based on attribute {2}.", entity.Name, id, primaryAttribute));
                    break;
	        }

            if (matchingRecordCount < 2 && !String.IsNullOrEmpty(statecode))
            {
                DynamicEntityUtility.SetStateDynamicEntity(_crmService, entity.Name, id, statecode, statuscode);
                ReportDetail(String.Format("Set state on {0} with id of {1}.", entity.Name, id));
            }

            _currentRecordCount++;
            _entityRecordCounts[entity.Name]++;

            int percent = (int)Math.Round(100.0 * _currentRecordCount / _totalRecordCount);
            percent = percent > 100 ? 100 : percent;
            OnImportProgressChanged(new ProgressChangedEventArgs(percent, null));
        }

        private DynamicEntity RemoveInvalidReferences(DynamicEntity input)
        {
            DynamicEntity invalidReferenceEntity = null;

            IEnumerable<Property> referenceProperties =
                (from p in input.Properties
                where p.GetType() == typeof(LookupProperty) ||
                    p.GetType() == typeof(CustomerProperty) ||
                    p.GetType() == typeof(OwnerProperty)
                select p).ToList();

            foreach (Property prop in referenceProperties)
            {
                CrmReference reference = (CrmReference)prop.GetType().GetProperty("Value").GetValue(prop, null);
                Guid newReferenceId = Guid.Empty;

                int matchingRecordCount = GetMatchingRecordCount(reference.type, reference.Value, reference.name, ref newReferenceId);
                switch (matchingRecordCount)
	            {
		            case 0:
                        if (invalidReferenceEntity == null)
                        {
                            invalidReferenceEntity = new DynamicEntity(input.Name);
                            invalidReferenceEntity.Properties.Add(input.Properties.OfType<KeyProperty>().First());
                        }

                        invalidReferenceEntity.Properties.Add(prop);
                        input.Properties.Remove(prop.Name);
                        break;
                    case 1:
                        if (newReferenceId != Guid.Empty)
                        {
                            reference.Value = newReferenceId;
                        }
                        break;
                    default:
                        ReportError(String.Format("Cannot import attribute {0} on {1} with id {2} because the value matches more than one record in the target system.", prop.Name, input.Name, input.Properties.OfType<KeyProperty>().First().Value.Value ));
                        input.Properties.Remove(prop.Name);
                        break;
	            }
            }

            return invalidReferenceEntity;
        }

        private ImportData RemoveInvalidReferences(ImportData importData)
        {
            ImportData outputData = null;

            for (int i = importData.Entities.Count -1; i > -1; i--)
            {
                DynamicEntity entity = importData.Entities[i];

                DynamicEntity outputEntity = RemoveInvalidReferences(entity);
                if (outputEntity != null)
                {
                    if (outputData == null)
                    {
                        outputData = new ImportData();
                    }

                    outputData.Entities.Add(outputEntity);
                }

                if (entity.Properties.Count == 1)
                {
                    //if only valid property in the record is the id then we should remove it from the list
                    importData.Entities.RemoveAt(i);
                }
            }

            return outputData;
        }

        private int GetMatchingRecordCount(string entityName, Guid id, string primaryAttributeValue, ref Guid newReferenceId)
        {
            List<DynamicEntity> ret = new List<DynamicEntity>(DynamicEntityUtility.GetByIdOrPrimaryAttribute(_crmService, _metadataService, entityName, id, primaryAttributeValue));
            if (ret.Count == 0)
            {
            }
            else if (ret.Count == 1)
            {
                Guid returnValueKey = ret[0].Properties.OfType<KeyProperty>().First().Value.Value;
                if (returnValueKey != id)
                {
                    newReferenceId = returnValueKey;
                }
            }
            return ret.Count;
        }
    }
}
