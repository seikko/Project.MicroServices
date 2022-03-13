using Course.Shared.Dtos;
using Course.Web.Models;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SigninRequest userRequest);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
