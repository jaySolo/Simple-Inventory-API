using System;
using System.Collections.Generic;


namespace Microsoft.AspNetCore.Identity
{
    public interface IIdentityPermissionRole<TPermission, TKey> 
        where TPermission : IdentityPermission<TKey>
        where TKey : IEquatable<TKey>
    {
        ICollection<TPermission> Permissions { get; }
    }
}
