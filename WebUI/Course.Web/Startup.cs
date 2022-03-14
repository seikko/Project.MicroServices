using Course.Shared.Services;
using Course.Web.Controllers;
using Course.Web.Handler;
using Course.Web.Models;
using Course.Web.Services;
using Course.Web.Services.Abstract;
using Course.Web.Services.Concretes;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var servicesApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();             
            services.AddHttpClient<IUserServices, UserServices>(opt=> {

                opt.BaseAddress = new Uri(servicesApiSettings.IdentityBaseUrl);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddScoped<ResourceOwnerPasswordTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();
            services.AddHttpClient<ICatalogServices, CatalogServices>(opt =>
             {
                 //url localhost:5000/services/catalog
                 opt.BaseAddress = new Uri($"{servicesApiSettings.GatewayBaseUrl}/{servicesApiSettings.Catalog.Path}");
             }).AddHttpMessageHandler<ClientCredentialTokenHandler>();
            services.AddScoped<IClientCridentialTokenServices, ClientCridentialTokenServices>();
            services.AddScoped<ISharedIdentityServices, SharedIdentityServices>();
          
            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();
            services.AddHttpClient<IIdentityService, IdentityService>();
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
             {
                 options.LoginPath = "/Auth/SignIn";
                 options.ExpireTimeSpan = TimeSpan.FromDays(60);
                 options.SlidingExpiration = true;
                 options.Cookie.Name = "Coursewebcookie";


             });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
