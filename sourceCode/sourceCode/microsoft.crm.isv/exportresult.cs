using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Crm.Isv
{
    public class ExportResults
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<EntityResult> EntityResults { get; set; }
    }
}
