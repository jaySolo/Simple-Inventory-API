using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace NHibernate.AspNetCore.Identity
{
    public class PermissionStore<TPermission, TKey> :
        IPermissionStore<TPermission, TKey>, IQueryablePermissionStore<TPermission, TKey>
        where TPermission : IdentityPermission<TKey>
        where TKey : IEquatable<TKey>
    {
        #region class variables
        public ISession Context { get; private set; }

        /// <summary>
        /// If true then disposing this object will also dispose (close) the session. 
        /// False means that external code is responsible for disposing the session.
        /// </summary>
        public bool CloseContextOnDispose { get; set; }
        #endregion


        #region constructor methods
        public PermissionStore(ISession session, bool disposeSession = true) 
        {
            Context = session;
            CloseContextOnDispose = disposeSession;
        }


        public PermissionStore(ISessionFactory sessionFactory, bool disposeSession = true)
        {
            Context = sessionFactory.OpenSession() ?? throw new ArgumentNullException(nameof(sessionFactory));
            CloseContextOnDispose = disposeSession;
        }
        #endregion


        #region IPermissionStore methods
        public Task CreateAsync(TPermission permission)
        {
            ThrowIfDisposed();

            if (permission is null) throw new ArgumentNullException(nameof(permission));

            Context.Save(permission);
            Context.Flush();

            return Task.FromResult(0);
        }


        public Task DeleteAsync(TPermission permission)
        {
            ThrowIfDisposed();

            if (permission is null) throw new ArgumentNullException(nameof(permission));

            Context.Delete(permission);
            Context.Flush();

            return Task.FromResult(0);
        }


        public Task<TPermission> FindByIdAsync(TKey permissionId)
        {
            ThrowIfDisposed();

            return Task.FromResult(Context.Query<TPermission>()
                .SingleOrDefault(p => p.Id.Equals(permissionId)));
        }


        public Task<TPermission> FindByNameAsync(string permissionName)
        {
            ThrowIfDisposed();

            return Task.FromResult(Queryable.SingleOrDefault(Context.Query<TPermission>().Where(
                r => r.Name.ToUpper() == permissionName.ToUpper())));
        }


        public Task UpdateAsync(TPermission permission)
        {
            ThrowIfDisposed();

            if (permission is null) throw new ArgumentNullException(nameof(permission));

            Context.Update(permission);
            Context.Flush();

            return Task.FromResult(0);
        }
        #endregion


        #region IQueryablePermissionStore
        public IQueryable<TPermission> Permissions
        {
            get
            {
                ThrowIfDisposed();
                return Context.Query<TPermission>();
            }
        }
        #endregion


        #region helpers
        public void ThrowIfDisposed()
        {
            if (_isAlreadyDisposed) throw new ObjectDisposedException(GetType().Name);
        }
        #endregion


        #region IDisposable Support
        /// <summary>
        /// <para>
        /// A value that indicates whether or the current instance of this class has been disposed of.
        /// </para>
        /// <para>
        /// Used to detect redundant calls and prevent useage of pre-disposed instances.
        /// </para>
        /// </summary>
        internal bool _isAlreadyDisposed = false;


        /// <summary>
        /// When disposing, actually dipose the session if (and only if) <see cref="CloseContextOnDispose"/>
        /// is set to true.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isAlreadyDisposed && CloseContextOnDispose)
            {
                Context.Close();
                Context.Dispose();
            }
            _isAlreadyDisposed = true;
        }


        /// <summary>Dispose this object.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
