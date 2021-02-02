using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorizationClientApp.Shared.Shared
{
    public class RoleDbRequirement: IAuthorizationRequirement
    {

        public RoleDbRequirement()
        {

        }
    }
}
