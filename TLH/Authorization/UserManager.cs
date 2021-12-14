using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;

namespace TLH.Authorization
{
    public class UserManager
    {
        private readonly DataContext _context;

        public UserManager(DataContext context)
        {            
            _context = context;
        }

       /* public bool ValidateUser(string userName, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                if (CryptoHelper.Crypto.VerifyHashedPassword(user.PasswordHash, password))
                {
                    return true;
                }
            }

            return false;
        }

        public ClaimsIdentity GetUserIdentity(string userName, string authType)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return null;

            var userClaims = GetUserClaims(user);

            return new ClaimsIdentity(userClaims, authType);
        }

        private List<Claim> GetUserClaims(TLH.Entity.Users user)
        {
            var userClaims = UserClaims.Get(user);

            return userClaims;
        }*/
    }
}
