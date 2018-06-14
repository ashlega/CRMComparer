using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Crm.Isv
{
    public class MissingMetadata
    {
        public MissingMetadata()
        {
            this.MissingEntities = new List<String>();
            _missingAttributes = new List<MissingAttribute>();
        }
        
        public IList<String> MissingEntities { get; private set; }
        private List<MissingAttribute> _missingAttributes;
        public IList<MissingAttribute> MissingAttributes { get { return _missingAttributes; } }
        public void AddMissingAttributes(string entityName, IEnumerable<String> attributes)
        {
            _missingAttributes.AddRange(
                from a in attributes 
                select new MissingAttribute { EntityName = entityName, AttributeName = a });
        }
    }

    public class MissingAttribute
    {
        public string EntityName { get; set; }
        public string AttributeName { get; set; }
    }
}
