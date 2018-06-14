using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Crm.Isv
{
    class XmlFilter
    {
        public XmlFilter()
        {
            this.Elements = new List<string>();
        }

        public IList<String> Elements { get; private set; }
    }
}
