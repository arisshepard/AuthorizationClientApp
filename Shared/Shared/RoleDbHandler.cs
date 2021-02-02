using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationClientApp.Shared.Shared
{
    public class RoleDbHandler : AuthorizationHandler<RoleDbRequirement>
    {
        private readonly HttpClient _http;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleDbHandler(HttpClient http)
        {
            _http = http;
            //_httpContextAccessor = httpContextAccessor;
        }

        // TODO: pendiente terminar
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleDbRequirement requirement)
        {
            var roles = await _http.GetJsonAsync<List<string>>("api/Login/GetRoles");

            //var roles = new List<string>() { "Admin" };
            var userRole = context.User.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).FirstOrDefault();

            if (string.IsNullOrEmpty(userRole))
            {
                context.Fail();
            }

            if (roles.Contains(userRole))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            await Task.FromResult(0);
            //await Task.CompletedTask;
        }
    }
}