using Course.Web.Models;
using Course.Web.Services.Abstract;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Course.Web.Services.Concretes
{
    public class ClientCridentialTokenServices : IClientCridentialTokenServices
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache  _clientAccessTokenCache;
        private readonly HttpClient _client;
        public ClientCridentialTokenServices(IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient client)
        {
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
            _client = client;
        }

        public async Task<string> GetToken()
        {
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken");
            if (currentToken != null) return currentToken.AccessToken;

            var disco = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {

                Address = _serviceApiSettings.IdentityBaseUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (disco.IsError) throw disco.Exception;

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = _clientSettings.WebClient.ClientId,
                ClientSecret = _clientSettings.WebClient.ClientSecret,
                Address = disco.TokenEndpoint
            };
            var newToken = await _client.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);
            if (newToken.IsError) throw newToken.Exception;

            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken,newToken.ExpiresIn);

            return newToken.AccessToken;
        }
    }
}
