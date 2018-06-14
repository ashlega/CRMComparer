using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Microsoft.Crm.Isv
{
    public class ImportCompletedEventArgs : AsyncCompletedEventArgs
    {
        public ImportCompletedEventArgs(ImportResults results, Exception error)
            : base(error, false, null)
        {
            this.ImportResults = results;
        }

        public ImportResults ImportResults { get; private set; }
    }
}
