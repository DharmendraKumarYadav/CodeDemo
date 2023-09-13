using DAL.Models.Idenity;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BDB.Authorization
{

    public class UserValidator : IResourceOwnerPasswordValidator
    {
        public const string AdminClientID = "bikedekhbike_admin";
        public const string WebClientID = "bikedekhbike_web";
        private SignInManager<User> _signManager;
        private UserManager<User> _userManager;
        public UserValidator(UserManager<User> userManager, SignInManager<User> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {

            bool isValidUser = false;
            User user = null;
            var clientId = context.Request.ClientId;
            user = _userManager.FindByNameAsync(context.UserName).Result;
            if (user == null)
            {
                user = _userManager.FindByEmailAsync(context.UserName).Result;
            }

            if (user != null)
            {
                if (!user.IsEnabled) {
                    context.Result = new GrantValidationResult(
                        TokenRequestErrors.InvalidGrant, "user_inactive");

                    return Task.FromResult(0);
                }
                if (!user.PhoneNumberConfirmed)
                {
                    context.Result = new GrantValidationResult(
                         TokenRequestErrors.InvalidGrant, "phone_unverified", customResponse: new Dictionary<string, object>
    {
        { "mobile", user.PhoneNumber }
    });

                    return Task.FromResult(0);
                }

                var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                if (clientId == AdminClientID)
                {
                    if (role == AppUserRoles.Dealer || role == AppUserRoles.Administrator || role == AppUserRoles.Broker)
                    {
                        isValidUser = _userManager.CheckPasswordAsync(user, context.Password).Result;
                    }
                    else {
                        context.Result = new GrantValidationResult(
                            TokenRequestErrors.InvalidGrant, "unauthorize_access");
                    }
                }
                else
                {
                    isValidUser = _userManager.CheckPasswordAsync(user, context.Password).Result;
                }
                if (isValidUser)
                {
                    context.Result = new GrantValidationResult(
                      subject: user.Id.ToString(),
                      authenticationMethod: "custom",
                      claims: new Claim[] { }
                  );
                }
                else
                {
                    context.Result = new GrantValidationResult(
                      TokenRequestErrors.InvalidGrant, "invalid_grant");
                }
                return Task.FromResult(0);
            }
            else
            {
                context.Result = new GrantValidationResult(
                       TokenRequestErrors.InvalidGrant, "user_notfound");

                return Task.FromResult(0);
            }
        }
    }
}
