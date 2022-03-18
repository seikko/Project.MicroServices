﻿using Course.Shared.Services;
using Course.Web.Controllers;
using Course.Web.Handler;
using Course.Web.Helpers;
using Course.Web.Models;
using Course.Web.Services;
using Course.Web.Services.Abstract;
using Course.Web.Services.Concretes;
using Course.Web.Services.Interfaces;
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
        public static IServiceCollection ServiceConfiguration(this IServiceCollection services)
        {

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

            return services;
        }
    }
}