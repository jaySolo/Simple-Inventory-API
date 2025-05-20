using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.common.handlers;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.models;

using MediatR;
using Microsoft.AspNetCore.Identity;

namespace jsolo.simpleinventory.impl.queries;



public class GetRolesQuery : IRequest<List<RoleViewModel>>
{

}



public class GetRolesQueryHandler(RoleManager<UserRole> roleManager) : IRequestHandler<GetRolesQuery, List<RoleViewModel>>
{
    private readonly RoleManager<UserRole> _roleManager = roleManager;

    public async Task<List<RoleViewModel>> Handle(GetRolesQuery request, CancellationToken cancellationToken = default)
    {
        List<RoleViewModel> getRoles()
        {
            var roles = _roleManager.Roles.ToList();

            return [.. roles.Select(r => new RoleViewModel
            {
                Name = r.Name ?? "",
                Description = r.Description,
                HasAdminRights = _roleManager.HasClaim(r, Claims.Administrator),
                Permissions = [.. r.Permissions.Select(p => p.Name)],
                UserCount = r.Users.Count,
                CreatedOn = r.CreatedOn,
                LastUpdatedOn = r.LastModifiedOn
            })];
        }
        return await Task.FromResult(getRoles());
    }
}



public class GetRoleDetailsQuery : IRequest<RoleViewModel>
{
    public string RoleName { get; set; } = string.Empty;
}



public class GetRoleDetailsQueryHandler(RoleManager<UserRole> roleManager) : IRequestHandler<GetRoleDetailsQuery, RoleViewModel?>
{
    private readonly RoleManager<UserRole> _roleManager = roleManager;

    public async Task<RoleViewModel?> Handle(GetRoleDetailsQuery request, CancellationToken cancellationToken = default)
    {
        var role = _roleManager.FindByNameAsync(request.RoleName).Result;

        return await Task.FromResult(role is null ? null : new RoleViewModel
        {
            Name = role.Name ?? "",
            Description = role.Description,
            HasAdminRights = _roleManager.HasClaim(role, Claims.Administrator),
            Permissions = [.. role.Permissions.Select(p => p.Name)],
            UserCount = role.Users.Count,
            CreatedOn = role.CreatedOn,
            LastUpdatedOn = role.LastModifiedOn
        });
    }
}
