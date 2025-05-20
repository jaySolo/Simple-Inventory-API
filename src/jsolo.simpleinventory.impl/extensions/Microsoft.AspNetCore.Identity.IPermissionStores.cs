using System;
using System.Linq;
using System.Threading.Tasks;


namespace Microsoft.AspNetCore.Identity
{
    /// <summary>
    /// Interface that exposes basic permission management apis
    /// </summary>
    /// <typeparam name="TPermission"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IPermissionStore<TPermission, in TKey> : IDisposable where TPermission : class, IPermission<TKey>
    {
        bool CloseContextOnDispose { get; set; }


        /// <summary>Inserts a new permission</summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task CreateAsync(TPermission permission);


        /// <summary>Deletes a permission</summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task DeleteAsync(TPermission permission);


        /// <summary>Finds a permission</summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        Task<TPermission> FindByIdAsync(TKey permissionId);


        /// <summary>Find a permission by name</summary>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        Task<TPermission> FindByNameAsync(string permissionName);


        /// <summary>Updates a permission</summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task UpdateAsync(TPermission permission);
    }



    /// <summary>
    /// Interface that exposes an IQueryable permissions
    /// </summary>
    /// <typeparam name="TPermission"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IQueryablePermissionStore<TPermission, in TKey> : IPermissionStore<TPermission, TKey>, IDisposable
        where TPermission : class, IPermission<TKey>
    {
        IQueryable<TPermission> Permissions { get; }
    }
}
