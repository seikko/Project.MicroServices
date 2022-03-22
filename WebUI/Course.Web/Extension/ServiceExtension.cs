using Course.Shared.Services;
using Course.Web.Controllers;
using Course.Web.Handler;
using Course.Web.Helpers;
using Course.Web.Models;
using Course.Web.Services;
using Course.Web.Services.Abstract;
using Course.Web.Services.Concretes;
using Course.Web.Services.Interfaces;
using Course.Web.Validator;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection ServiceConfiguration(this IServiceCollection services,IConfiguration Configuration)
        {
            #region  ServiceApiSettings
            var servicesApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();
            services.AddHttpClient<IUserServices, UserServices>(opt =>
            {

                opt.BaseAddress = new Uri(servicesApiSettings.IdentityBaseUrl);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
            services.AddHttpClient<ICatalogServices, CatalogServices>(opt =>
            {
                //url localhost:5000/services/catalog
                opt.BaseAddress = new Uri($"{servicesApiSettings.GatewayBaseUrl}/{servicesApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();
            services.AddHttpClient<IPhotoStockServices, PhotoStockServices>(opt =>
            {
                opt.BaseAddress = new Uri($"{servicesApiSettings.GatewayBaseUrl}/{servicesApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IBasketServices, BasketServices>(opt =>
            {
                opt.BaseAddress = new Uri($"{servicesApiSettings.GatewayBaseUrl}/{servicesApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IDiscountServices, DiscountServices>(opt =>
            {
                opt.BaseAddress = new Uri($"{servicesApiSettings.GatewayBaseUrl}/{servicesApiSettings.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            #endregion


            #region Container
            services.AddSingleton<PhotoHelper>();
            services.AddScoped<ResourceOwnerPasswordTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();
            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();
            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddScoped<IClientCridentialTokenServices, ClientCridentialTokenServices>();
            services.AddScoped<ISharedIdentityServices, SharedIdentityServices>();
            services.AddHttpContextAccessor();
            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Auth/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                options.SlidingExpiration = true;
                options.Cookie.Name = "Coursewebcookie";
            });
            services.AddControllersWithViews();
            #endregion

            #region Configuration
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));
            #endregion

            services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CourseUpdateInputValidator>());//bana bir tane Sınıf abstractValidator'den tureyen bir sınıf ver ben onun assemblylerini bulurum
            return services;
        }
    }
}
