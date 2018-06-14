using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Crm.Isv
{
    public class ImportResults
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public MissingMetadata MissingMetadata { get; set; }
        public IEnumerable<EntityResult> EntityResults { get; set; }
    }
}
