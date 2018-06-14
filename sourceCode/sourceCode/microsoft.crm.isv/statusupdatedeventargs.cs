using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Crm.Isv
{
    public enum StatusLevel
    {
        Information,
        Detail,
        Error,
    }

    public class StatusUpdatedEventArgs : EventArgs
    {
        public StatusUpdatedEventArgs(string status)
            : this(status, StatusLevel.Information)
        {
        }

        public StatusUpdatedEventArgs(string status, StatusLevel level)
        {
            this.Status = status;
            this.Level = level;
        }

        public string Status { get; private set; }
        public StatusLevel Level { get; private set; }
    }
}
