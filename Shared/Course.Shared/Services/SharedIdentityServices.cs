
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Course.Shared.Services
{
    public class SharedIdentityServices : ISharedIdentityServices
    {
        private IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;//kullanıcının sahip oldugu claimlerden sub olanı ver sub(userId)
    }
}
