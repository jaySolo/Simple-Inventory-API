using System.Linq;

using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.impl.objects;


namespace jsolo.simpleinventory.sys.common.interfaces
{
    public interface IIdentityDatabaseContext: IDatabaseContext
    {
        IQueryable<User> Users { get; }

        IQueryable<UserRole> Roles { get; }

        IQueryable<UserPermission> Permissions { get; }

        IQueryable<RefreshToken> RefreshTokens { get; }
    }
}
