using System.Linq;
using System.Security.Claims;

using jsolo.simpleinventory.impl.identity;


namespace Microsoft.AspNetCore.Identity
{
    public static class UserManagerExtenstions
    {
        public static bool HasClaim(this UserManager<User> manager, User user, Claim claim)
        {
            var claims = manager.GetClaimsAsync(user).Result;
            
            return claims.Any(c => c.Type.Equals(claim.Type) && c.Value.Equals(claim.Value));
        }
    }
}
