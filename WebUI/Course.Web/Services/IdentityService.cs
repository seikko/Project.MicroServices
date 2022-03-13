using Course.Shared.Dtos;
using Course.Web.Models;
using Course.Web.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Course.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpclient;
        private readonly IHttpContextAccessor _httpAccesor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpclient, IHttpContextAccessor httpAccesor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpclient = httpclient;
            _httpAccesor = httpAccesor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        /// <summary>
        /// Cookie updated token created
        /// </summary>
        /// <returns></returns>
        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var disco = await _httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {

                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (disco.IsError) throw disco.Exception;
            var refreshToken = await _httpAccesor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = disco.TokenEndpoint
            };
            var token = await _httpclient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError) return null;

            var authencationToken = new List<AuthenticationToken>(){
                new AuthenticationToken { Name=OpenIdConnectParameterNames.AccessToken,Value = token.AccessToken},
                new AuthenticationToken { Name=OpenIdConnectParameterNames.RefreshToken,Value = token.RefreshToken},
                new AuthenticationToken { Name=OpenIdConnectParameterNames.ExpiresIn,Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };

            var authencationResult = await _httpAccesor.HttpContext.AuthenticateAsync();
            var properties = authencationResult.Properties;
            properties.StoreTokens(authencationToken);

            await _httpAccesor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authencationResult.Principal, properties);
            return token;


        }

        public async Task RevokeRefreshToken()
        {
            #region Idendity Server  EndPointler 
            var disco = await _httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {

                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError) throw disco.Exception;
            #endregion

            #region Refresh Token

            var refreshToken = await _httpAccesor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = disco.RevocationEndpoint,//end point addresi 
                Token = refreshToken,//token 
                TokenTypeHint = "refresh_token"//tipi refresh token 
            };
            #endregion
            await _httpclient.RevokeTokenAsync(tokenRevocationRequest);
        }

        /// <summary>
        /// Token Created
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        public async Task<Response<bool>> SignIn(SigninRequest userRequest)
        {
            var disco = await _httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {

                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError) throw disco.Exception;
            var passwordTokenRequest = new PasswordTokenRequest
            {

                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = userRequest.Email,
                Password = userRequest.Password,
                Address = disco.TokenEndpoint
            };

            var token = await _httpclient.RequestPasswordTokenAsync(passwordTokenRequest);
            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true/*property ısımlerını buyuk kucuk onemseme */ });
                return Response<bool>.Fail(errorDto.Errors, 400);
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };

            var userInfo = await _httpclient.GetUserInfoAsync(userInfoRequest);
            if (userInfo.IsError) throw userInfo.Exception;

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>(){
                new AuthenticationToken { Name=OpenIdConnectParameterNames.AccessToken,Value = token.AccessToken},
                new AuthenticationToken { Name=OpenIdConnectParameterNames.RefreshToken,Value = token.RefreshToken},
                new AuthenticationToken { Name=OpenIdConnectParameterNames.ExpiresIn,Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            authenticationProperties.IsPersistent = userRequest.IsRemember;
            await _httpAccesor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);
        }
    }
}
