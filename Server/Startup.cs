using AuthorizationClientApp.Server.Data;
using AuthorizationClientApp.Shared;
using AuthorizationClientApp.Shared.Security;
using AuthorizationClientApp.Shared.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;
using System.Text;

namespace AuthorizationClientApp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]))
                    };
                });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost") });
            //services.AddHttpContextAccessor();
            //services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            AuthorizationServices(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

        private void AuthorizationServices(IServiceCollection services)
        {
            //services.AddAuthorization(config =>
            //{
            //    config.AddPolicy("IsDeveloper", policy => policy.RequireClaim("IsDeveloper", "true"));
            //});

            //services.AddAuthorization(config =>
            //{
            //    config.AddPolicy("IsAdmin", policy => policy.RequireRole("admin", "moderator"));
            //});
            services.AddHttpContextAccessor();
            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
                config.AddPolicy(Policies.IsCompanyUser, Policies.IsCompanyDomainPolicy());
                config.AddPolicy(Policies.IsRoleDb, Policies.IsRoleDbPolicy());
            });

            services.AddScoped<IAuthorizationHandler, RoleDbHandler>();
            //services.AddScoped<IAuthorizationHandler, CompanyDomainHandler>();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ThePolicy", policy => policy.Requirements.Add(new ThePolicyRequirement()));
            //});

            //services.AddScoped<IAuthorizationHandler, ThePolicyAuthorizationHandler>();

            //services.AddAuthorization(config =>
            //{
            //    config.AddPolicy("IsCompanyUser", policy =>
            //        policy.Requirements.Add(new CompanyDomainRequirement("gmail.com")));
            //});

        }
    }
}