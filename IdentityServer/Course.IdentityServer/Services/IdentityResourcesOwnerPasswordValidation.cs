using Course.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.IdentityServer.Services
{
    public class IdentityResourcesOwnerPasswordValidation : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourcesOwnerPasswordValidation(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var exitsUser = await _userManager.FindByEmailAsync(context.UserName);
            if(exitsUser == null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifreniz yanlıştır" });
                context.Result.CustomResponse = errors;
                return;
            }
            var passwordCheck = await _userManager.CheckPasswordAsync(exitsUser, context.Password);
            if (!passwordCheck)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifreniz yanlıştır" });
                context.Result.CustomResponse = errors;
                return;
            }
            context.Result = new GrantValidationResult(exitsUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
