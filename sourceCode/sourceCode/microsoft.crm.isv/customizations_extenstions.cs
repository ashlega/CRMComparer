using System;

namespace Microsoft.Crm.Isv.Customizations
{
    partial class EntitiesTypeEntity : IIdentifiable
    {
        public string Identity
        {
            get { return this.Name.Value; }
        }
    }

    partial class EntityInfoTypeEntityAttribute : IIdentifiable
    {
        public string Identity
        {
            get { return this.PhysicalName; }
        }
    }

    partial class EntityRelationShipsTypeEntityRelationship : IIdentifiable
    {
        public string Identity
        {
            get { return this.Name; }
        }
    }

    partial class SavedQueriesTypeSavedqueries : IIdentifiable
    {
        public string Identity
        {
            get { return this.entity; }
        }
    }

    partial class SavedQueriesTypeSavedqueriesSavedquery : IIdentifiable
    {
        public string Identity
        {
            get { return this.savedqueryid; }
        }
    }

    partial class RolesTypeRole : IIdentifiable
    {
        public string Identity
        {
            get { return this.name; }
        }
    }

    partial class WorkflowsTypeWorkflow : IIdentifiable
    {
        public string Identity
        {
            get { return this.nameField; }
        }
    }

    partial class EntityMapsTypeEntityMap : IIdentifiable
    {
        public string Identity
        {
            get { return this.EntitySource + this.EntityTarget; }
        }
    }

    partial class EntityMapsTypeEntityMapAttributeMap : IIdentifiable
    {
        public string Identity
        {
            get { return this.AttributeSource + this.AttributeTarget; }
        }
    }

    partial class FormXmlEventsTypeEvent : IIdentifiable
    {
        public string Identity
        {
            get { return this.name; }
        }
    }
}
