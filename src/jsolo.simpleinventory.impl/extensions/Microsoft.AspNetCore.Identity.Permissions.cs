using System.Security.Claims;



namespace Microsoft.AspNetCore.Identity;


/// <summary>
/// Minimal interface for a permission with id
/// and name.
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IPermission<out TKey>
{
    /// <summary>
    /// Unique key for the permission.
    /// </summary>
    TKey Id { get; }

    /// <summary>
    /// Unique permission name.
    /// </summary>
    string Name { get; set; }
}



public class IdentityPermission<TKey> :  IPermission<TKey> 
    where TKey : IEquatable<TKey>
{
    /// <summary>Gets or sets the primary key for this permission.</summary>
    public virtual TKey Id { get; set; }

    /// <summary>The name of this <see cref="IdentityPermission"/> entails.</summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// A explanation of what this <see cref="IdentityPermission"/> entails.
    /// </summary>
    public virtual string Description { get; set; }

    
    /// <summary>
    /// A list of <see cref="Claim">claims</see> associated with the permission. Used primarily to store the list of
    /// applications that this permission is valid for.
    /// </summary>
    public virtual List<Claim> Claims { get; set; }


    /// <summary>
    /// A list of <see cref="IdentityPermissionRole{TKey}"/>s that are assigned with this <see cref="IdentityPermission"/>.
    /// </summary>
    public virtual ICollection<IIdentityPermissionRole<IdentityPermission<TKey>, TKey>> Roles { get; set; } =
        new List<IIdentityPermissionRole<IdentityPermission<TKey>, TKey>>();


    #region constructor methods
    public IdentityPermission() { }
    #endregion


    #region instance methods
    public virtual void AddAssignedRole(IIdentityPermissionRole<IdentityPermission<TKey>, TKey> role)
    {
        role.Permissions.Add(this);
        this.Roles.Add(role);
    }
    #endregion
}
