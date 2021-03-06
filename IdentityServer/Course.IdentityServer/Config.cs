// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace Course.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]//usersiz giris full erişim
        {
            new ApiResource("resource_catalog"){Scopes= { "catalog_fullpermisson" }},
            new ApiResource("resource_photo_stock"){Scopes= { "photo_stock_fullpermisson" }},
            new ApiResource("resource_basket"){Scopes= { "basket_fullpermisson" }},
            new ApiResource("resource_discount"){Scopes= { "discount_fullpermisson" }},
            new ApiResource("resource_order"){Scopes= { "order_fullpermisson" }},
            new ApiResource("resource_payment"){Scopes= { "payment_fullpermisson" }},
            new ApiResource("resource_gateway"){Scopes= { "gateway_fullpermisson" }},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]//kısıtlı gırıs user giris 
         {
             //token içerisinde gondericeklerimiz
             new IdentityResources.Email(),
             new IdentityResources.OpenId(),
             new IdentityResources.Profile(),
             new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı rolleri",UserClaims = new[]{"role"}}
         };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]//usersiz giris full erişim
            {
              new ApiScope("catalog_fullpermisson","Catalog API için full erişim"),
              new ApiScope("photo_stock_fullpermisson","Photo Stock API için full erişim"),
              new ApiScope("basket_fullpermisson","Basket API için full erişim"),
              new ApiScope("discount_fullpermisson","Discount API için full erişim"),
              new ApiScope("order_fullpermisson","Order API için full erişim"),
              new ApiScope("payment_fullpermisson","Payment API için full erişim"),
              new ApiScope("gateway_fullpermisson","Gatway için full erişim"),
              new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                
              new Client//ClientCredentials
              {
                  ClientName = "Asp.Net Core MVC", // Client name 
                  ClientId = "WebMvcClient", // Client id 
                  ClientSecrets = {new Secret("secret".Sha256())}, // Client Sifresi ? 
                  AllowedGrantTypes = GrantTypes.ClientCredentials, // Grant Type ne  postmandan istek atarken grant_type client_credentials ? 
                  AllowedScopes = { "catalog_fullpermisson", "photo_stock_fullpermisson", "gateway_fullpermisson", IdentityServerConstants.LocalApi.ScopeName} // Usersiz Hangi api ye istek yapabilir ?
              },

               new Client//ClientCredentials
              {
                  ClientName = "Asp.Net Core MVC", // Client name 
                  ClientId = "WebMvcClientForUser", // Client id 
                  AllowOfflineAccess = true, // OfflineAccess icin izin
                  ClientSecrets = {new Secret("secret".Sha256())}, 
                  AllowedGrantTypes =GrantTypes.ResourceOwnerPassword, //postmandan istek atarken grant_type password ?,
                  AllowedScopes = { "basket_fullpermisson","order_fullpermisson",  "gateway_fullpermisson", IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.Address, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName, "roles" }, //bu izinlere sahip olan token'i ver 
                  
                  //Token icinde neler gondereyim.
                  //OfflineAccess = kullanıcı offline olsa dahi ben elimdeki refresh token ile kullanıcı ıcın yeni bir token alabilirim. OfllineAccess i kaldırırsak  elimde refresh token olmadıgı ıcın kullanıcıdan email ve password almak zorundayım. 
                   AccessTokenLifetime = 1*60*60, //AccessToken'in omru
                   RefreshTokenExpiration = TokenExpiration.Absolute,
                   AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,//Refresh token in suresi 60 gün - simdiki gün'ün saniyesi.
                   RefreshTokenUsage = TokenUsage.ReUse


                   },
               
                   new Client
              {

                       ///Gateway client ile birlikte identity server'a istek yapacak
                  ClientName = "Token Exchange Client",  
                  ClientId = "TokenExhangeClient",   
                  ClientSecrets = {new Secret("secret".Sha256())},  
                  AllowedGrantTypes =new []{ "urn:ietf:params:oauth:grant-type:token-exchange" },
                  AllowedScopes={ "discount_fullpermisson", "payment_fullpermisson", IdentityServerConstants.StandardScopes.OpenId } //Bu izinlere sahip olan token'i al

              },

              };
    };

}
