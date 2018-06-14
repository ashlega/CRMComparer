using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Crm.SdkTypeProxy;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.SdkTypeProxy.Metadata;

namespace Microsoft.Crm.Isv
{
	public class ServiceFactory
	{
        public Uri ServerUrl { get; private set; }
        public string OrganizationName { get; private set; }

        public ServiceFactory(Uri serverUrl, string organizationName)
        {
            this.ServerUrl = serverUrl;
            this.OrganizationName = organizationName;
        }

        public CrmService GetCrmService()
		{
            UriBuilder serviceUri = new UriBuilder(this.ServerUrl);
            serviceUri.Path = "/MSCRMServices/2007/CrmService.asmx";

            return new CrmService()
            {
                Url = serviceUri.ToString(),
                UseDefaultCredentials = true,
                CrmAuthenticationTokenValue = new CrmAuthenticationToken()
                {
                    OrganizationName = this.OrganizationName,
                },
            };
		}

        public MetadataService GetMetadataService()
        {
            UriBuilder serviceUri = new UriBuilder(this.ServerUrl);
            serviceUri.Path = "/MSCRMServices/2007/MetadataService.asmx";

            return new MetadataService()
            {
                Url = serviceUri.ToString(),
                UseDefaultCredentials = true,
                CrmAuthenticationTokenValue = new CrmAuthenticationToken()
                {
                    OrganizationName = this.OrganizationName,
                },
            };
        }
    }
}
