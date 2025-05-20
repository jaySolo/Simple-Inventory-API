using System.Linq;

using Microsoft.AspNetCore.Identity;

using jsolo.simpleinventory.sys.common;


namespace jsolo.simpleinventory.impl.identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result) => result.Succeeded ? 
            Result.Success :
            Result.Failure(result.Errors.Select(e => e.Description).ToArray());
    }
}
