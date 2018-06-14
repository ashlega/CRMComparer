using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Crm.Sdk.Metadata;
using Microsoft.Crm.SdkTypeProxy.Metadata;
using Microsoft.Crm.Sdk;

namespace Microsoft.Crm.Isv
{
    public class ImportData
    {
        public ImportData()
        {
            _entities  = new List<DynamicEntity>();
        }

        private List<DynamicEntity> _entities;
        public IList<DynamicEntity> Entities { get { return _entities; } }


        public void AddEntities(IEnumerable<DynamicEntity> entities)
        {
            _entities.AddRange(entities);
        }
    }
}
