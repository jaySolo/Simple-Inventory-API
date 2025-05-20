using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Microsoft.AspNetCore.Identity
{
    /// <summary>
    /// Interface that maps roles to their permissions
    /// </summary>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRolePermissionStore<TRole, in TKey> : IRoleStore<TRole>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        Task AssignPermissionAsync(TRole role, string permission);


        Task<IList<string>> GetPermissionsAsync(TRole role);


        Task<bool> HasPermissionAsync(TRole role, string permission);


        Task RemovePermissionAsync(TRole role, string permission);
    }
}
