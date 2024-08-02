using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using MovieShop.Security;
using System.Security.Claims;

namespace IdentityServer.Configuration
{
    public class ProfileService<TUser> : IProfileService where TUser : ApplicationUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;

        public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("No sub claim present");

            var user = await FindUserAsync(sub);
            if (user != null)
            {
                var principal = await GetUserClaimsAsync(user);
                context.AddRequestedClaims(principal.Claims);

                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles != null)
                {
                    var roleClaims = userRoles.Select(role => new Claim(JwtClaimTypes.Role, role)).ToList();
                    context.IssuedClaims.AddRange(roleClaims);
                }

                var usernameClaim = new Claim("username", user.UserName); 
                context.IssuedClaims.Add(usernameClaim);
            }
        }
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("No subject Id claim present");

            var user = await FindUserAsync(sub);
            if (user != null)
            {
                context.IsActive = await Task.FromResult(true);
            }
            else
            {
                context.IsActive = false;
            }
        }
        protected virtual async Task<ClaimsPrincipal> GetUserClaimsAsync(TUser user)
        {
            var principal = await _claimsFactory.CreateAsync(user);
            if (principal == null) throw new Exception("ClaimsFactory failed to create a principal");

            return principal;
        }
        protected virtual async Task<TUser> FindUserAsync(string subjectId)
        {
            var user = await _userManager.FindByIdAsync(subjectId);
            //if (user == null)
            //{
            //    Logger?.LogWarning("No user found matching subject Id: {subjectId}", subjectId);
            //}

            return (TUser)user;
        }
    }
}
