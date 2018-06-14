using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Crm.Isv
{
    public class EntityResult
    {
        public string EntityName { get; private set; }
        public int RecordCount { get; private set; }

        public EntityResult(string entityName, int recordCount)
        {
            EntityName = entityName;
            RecordCount = recordCount;
        }
    }
}
