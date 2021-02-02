using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationClientApp.Shared.Security
{
    public class CompanyDomainHandler : AuthorizationHandler<CompanyDomainRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CompanyDomainRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                return Task.CompletedTask;
            }

            var emailAddress = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;

            if (emailAddress.EndsWith(requirement.CompanyDomain))
            {
                //return context.Succeed(requirement);
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
