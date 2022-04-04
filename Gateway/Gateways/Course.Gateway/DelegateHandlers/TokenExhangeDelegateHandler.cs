using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Gateway.DelegateHandlers
{
    public class TokenExhangeDelegateHandler:DelegatingHandler
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private string _accessToken; 

        public TokenExhangeDelegateHandler(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }
        private async Task<string> GetToken(string requestToken)
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }
            var disco = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _configuration["IdentityServerUrl"],
                Policy = new DiscoveryPolicy { RequireHttps = false}
            });
            if(disco.IsError) throw disco.Exception;    
            TokenExchangeTokenRequest tokenExchangeTokenRequest = new()
            {
                Address = disco.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                GrantType = _configuration["TokenGrantType"],
                SubjectToken = requestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access-token",
                Scope = "openid payment_fullpermisson discount_fullpermisson"
            };
            var tokenResponse = await _client.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);
            if (tokenResponse.IsError) throw tokenResponse.Exception;
            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }

        protected override async  Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter; // basket gateway order'a bu token ile istek gidebilirken.
            var newToken = await GetToken(requestToken);//discount ve payment'a istek yapabilr sadece.
            request.SetBearerToken(newToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
