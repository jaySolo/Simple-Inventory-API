using System.Linq;
using System.Security.Claims;

using jsolo.simpleinventory.impl.identity;


namespace Microsoft.AspNetCore.Identity
{
    public static class RoleManagerExtenstions
    {
        public static bool HasClaim(this RoleManager<UserRole> manager, UserRole role, Claim claim)
        {
            var claims = manager.GetClaimsAsync(role).Result;
            
            return claims.Any(c => c.Type.Equals(claim.Type) && c.Value.Equals(claim.Value));
        }

        public static bool HasPermission(this RoleManager<UserRole> manager, UserRole role, string route, string method)
        {
            if (!manager.Roles.Any(r => r.Id == role.Id)) {
                return false;
            }
            
            return role.Permissions.Any(
                p => p.Route.ToLower().Equals(route.ToLower()) && p.AllowedRequests.Any(
                    aReq => aReq.ToLower().Equals(method.ToLower())
                ) && p.Claims.Any(c => c.Equals(Claims.AppScope))
            );
        }
    }
}
