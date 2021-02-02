using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthorizationClientApp.Shared.Security
{
    public class CompanyDomainRequirement: IAuthorizationRequirement
    {
        public string CompanyDomain { get; }

        public CompanyDomainRequirement(string companyDomain)
        {
            CompanyDomain = companyDomain;
        }
    }
}
