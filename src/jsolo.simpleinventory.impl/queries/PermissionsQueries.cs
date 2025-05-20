using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.models;
using jsolo.simpleinventory.impl.helpers;

using MediatR;
using System.Threading.Tasks;


namespace jsolo.simpleinventory.impl.queries
{
    public class GetPermissionsQuery : IRequest<List<PermissionViewModel>>
    {
        
    }



    public class GetPermissionsQueryHandler(IIdentityDatabaseContext context) : IRequestHandler<GetPermissionsQuery, List<PermissionViewModel>>
    {
        private readonly IIdentityDatabaseContext db = context;

        public async Task<List<PermissionViewModel>> Handle(GetPermissionsQuery request, CancellationToken token = default)
        {
            List<PermissionViewModel> getPermissions()
            {
                var permissions = this.db.Permissions.ToList();

                return [.. permissions.Select(p => PermissionsRequestsHelpers.MapToViewModel(p))];
            }

            return await Task.FromResult(getPermissions());
        }
    }


    public class GetPermissionDetailsQuery : IRequest<PermissionViewModel>
    {
        public string PermissionName { get; set; } = string.Empty;
    }



    public class GetPermissionDetailsQueryHandler(IIdentityDatabaseContext context) : IRequestHandler<GetPermissionDetailsQuery, PermissionViewModel?>
    {
        private readonly IIdentityDatabaseContext db = context;

        public async Task<PermissionViewModel?> Handle(GetPermissionDetailsQuery request, CancellationToken token = default)
        {
            PermissionViewModel? getPermission()
            {
                var permissions = this.db.Permissions.ToList();

                var permission = permissions.SingleOrDefault(
                    p => p.Name.ToLower().Equals(request.PermissionName.ToLower())
                );

                return permission is null ? null : PermissionsRequestsHelpers.MapToViewModel(
                    permission
                );
            }

            return await Task.FromResult(getPermission());
        }
    }
}
