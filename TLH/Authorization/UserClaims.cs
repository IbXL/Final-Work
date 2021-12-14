using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;

using TLH.Entity;

namespace TLH.Authorization
{
    internal static class UserClaimNames
    {
        public const string UserId = "UserId";
        public const string UserName = "UserName";
    }

    /// <summary>
    /// Provides static helper methods to work with user claims.
    /// </summary>
    internal static class UserClaims
    {
        public static List<Claim> Get(Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(UserClaimNames.UserId, user.Id.ToString()),
                new Claim(UserClaimNames.UserName, user.UserName),
            };

            return claims;
        }


        public static int GetUserId(this ClaimsPrincipal user)
        {
            var value = user.GetClaimValue(UserClaimNames.UserId);
            return Int32.Parse(value);
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.GetClaimValue(UserClaimNames.UserName);
        }

        private static string GetClaimValue(this ClaimsPrincipal user, string claimType)
        {
            var claims = user?.Claims;
            var claim = claims?.FirstOrDefault(c => c.Type == claimType);
            return claim?.Value;
        }
    }
}
