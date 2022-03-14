using Course.Web.Exceptions;
using Course.Web.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Web.Handler
{
    public class ClientCredentialTokenHandler:DelegatingHandler
    {
        private readonly IClientCridentialTokenServices _clientCridentialTokenServices;

        public ClientCredentialTokenHandler(IClientCridentialTokenServices clientCridentialTokenServices)
        {
            _clientCridentialTokenServices = clientCridentialTokenServices;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _clientCridentialTokenServices.GetToken());
            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new UnAuthroizeException();
            return response;
        }
    }
}
