using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using AuthorizationClientApp;
using AuthorizationClientApp.Shared.Security;
using AuthorizationClientApp.Shared.Shared;

namespace AuthorizationClientApp.Shared
{
    public static class Policies
    {
        public const string IsAdmin = "IsAdmin";
        public const string IsUser = "IsUser";
        public const string IsCompanyUser = "IsCompanyUser";
        public const string IsRoleDb = "IsRoleDb";

        public static AuthorizationPolicy IsAdminPolicy()
        {

            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("Admin", "Moderator")
                .Build();
        }

        public static AuthorizationPolicy IsUserPolicy()
        {

            return new AuthorizationPolicyBuilder()
              .RequireAuthenticatedUser()
              .RequireRole("User")
              .Build();
        }

        public static AuthorizationPolicy IsCompanyDomainPolicy()
        {

            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new CompanyDomainRequirement("gmail.com"));

            return policy.Build();
        }

        public static AuthorizationPolicy IsRoleDbPolicy()
        {

            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
            policy.AddRequirements(new RoleDbRequirement());

            return policy.Build();
        }

    }
}
