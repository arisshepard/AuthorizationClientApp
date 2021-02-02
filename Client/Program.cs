using AuthorizationClientApp.Client.Provider;
using AuthorizationClientApp.Client.Services;
using AuthorizationClientApp.Shared;
using AuthorizationClientApp.Shared.Security;
using AuthorizationClientApp.Shared.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationClientApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddScoped<IAuthorizationHandler, RoleDbHandler>();

            builder.Services.AddAuthorizationCore(config =>
            {
                config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
                config.AddPolicy(Policies.IsCompanyUser, Policies.IsCompanyDomainPolicy());
                config.AddPolicy(Policies.IsRoleDb, Policies.IsRoleDbPolicy());
                //config.AddPolicy(Policies.IsCustomPolicy, Policies.IsCustomAuthorizationPolicy());

            });

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazoredLocalStorage();

            //builder.Services.AddAuthorizationCore();


            //builder.Services.AddScoped<IAuthorizationHandler, CompanyDomainHandler>();

            //builder.Services.AddScoped<IAuthorizationHandler, ThePolicyAuthorizationHandler>();
            // builder.Services.AddScoped<IAuthorizationHandler, RolesInDBAuthorizationHandler>();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

            builder.Services.AddScoped<IAuthService, AuthService>();

            await builder.Build().RunAsync();
        }
    }
}
