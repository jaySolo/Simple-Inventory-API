using System;
using System.Linq;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.models;

namespace jsolo.simpleinventory.impl.helpers
{
    static class PermissionsRequestsHelpers
    {
        public static PermissionViewModel MapToViewModel(UserPermission permission) => new PermissionViewModel
        {
            Name = permission.Name,
            Description = permission.Description,
            Route = permission.Route,
            AcceptedMethods = permission.AllowedRequests,
            CreatedOn = permission.CreatedOn,
            LastUpdatedOn = permission.LastModifiedOn,
            Roles = permission.Roles.Select(r => r.Name).ToList()
        };
    }
}
