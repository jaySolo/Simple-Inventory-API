using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Identity;

using jsolo.simpleinventory.impl.common;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.models;


namespace jsolo.simpleinventory.impl.commands
{
    public class CreateRoleCommand : IRequest<DataOperationResult<RoleViewModel>>
    {
        public RoleViewModel NewRole { get; set; }
    }



    public class CreateRoleCommandHandler : IdentityRequestHandlerBase<CreateRoleCommand, DataOperationResult<RoleViewModel>>
    {
        private readonly RoleManager<UserRole> RoleManager;


        public CreateRoleCommandHandler(
            IIdentityDatabaseContext context,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService,
            RoleManager<UserRole> roleManager
        ) : base(context, currentUserService, dateTimeService) => RoleManager = roleManager;


        public override async Task<DataOperationResult<RoleViewModel>> Handle(
            CreateRoleCommand request, CancellationToken cancellationTokens
        ) {
            var role = await RoleManager.FindByNameAsync(request.NewRole.Name);

            if(!(role is null)) { return DataOperationResult<RoleViewModel>.Exists; }

            try
            {
                var newRole = new UserRole(
                    RoleManager.Roles.Count() + 1,
                    request.NewRole.Name,
                    request.NewRole.Description,
                    createdOn: DateTime.Now,
                    creator: CurrentUser.UserId.ToString()
                );
                
                foreach (var permission in request.NewRole.Permissions)
                {
                    var perm = Context.Permissions
                        .SingleOrDefault(p => p.Name.ToLower().Equals(permission.ToLower()));

                    if (!(perm is null))
                    {
                        newRole.AddPermission(perm);
                    }
                } 

                var status = await RoleManager.CreateAsync(newRole);


                if (!request.NewRole.HasAdminRights)
                {
                    return status.Succeeded ? DataOperationResult<RoleViewModel>.Success(new RoleViewModel {
                        Name = newRole.Name,
                        Description = newRole.Description,
                        HasAdminRights = RoleManager.HasClaim(newRole, Claims.Administrator),
                        Permissions = newRole.Permissions.Select(p => p.Name).ToList(),
                        UserCount = newRole.Users.Count,
                        CreatedOn = newRole.CreatedOn,
                        LastUpdatedOn = newRole.LastModifiedOn
                    }) : DataOperationResult<RoleViewModel>.Failure();
                }

                var addClaimResult = await RoleManager.AddClaimAsync(newRole, Claims.Administrator);
                
                return status.Succeeded && addClaimResult.Succeeded ? 
                    DataOperationResult<RoleViewModel>.Success(new RoleViewModel {
                        Name = newRole.Name,
                        Description = newRole.Description,
                        HasAdminRights = RoleManager.HasClaim(newRole, Claims.Administrator),
                        Permissions = newRole.Permissions.Select(p => p.Name).ToList(),
                        UserCount = newRole.Users.Count,
                        CreatedOn = newRole.CreatedOn,
                        LastUpdatedOn = newRole.LastModifiedOn
                    }) : (
                        status.Succeeded ?
                            DataOperationResult<RoleViewModel>.SomeChangesNotSaved :
                            DataOperationResult<RoleViewModel>.Failure()
                    );
            }
            catch
            {
                return DataOperationResult<RoleViewModel>.Failure();
            }
        }
    }


    
    public class UpdateRoleCommand : IRequest<DataOperationResult<RoleViewModel>>
    {
        public string RoleName { get; set; }

        public RoleViewModel UpdatedRole { get; set; }
    }



    public class UpdateRoleCommandHandler : IdentityRequestHandlerBase<UpdateRoleCommand, DataOperationResult<RoleViewModel>>
    {
        private readonly RoleManager<UserRole> RoleManager;


        public UpdateRoleCommandHandler(
            IIdentityDatabaseContext context,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService,
            RoleManager<UserRole> roleManager
        ) : base(context, currentUserService, dateTimeService) => RoleManager = roleManager;


        public override async Task<DataOperationResult<RoleViewModel>> Handle(
            UpdateRoleCommand request, CancellationToken token
        ) {
            var role = await RoleManager.FindByNameAsync(request.RoleName);

            if(role is null) { return DataOperationResult<RoleViewModel>.NotFound; }

            try
            {
                var usrId = CurrentUser.UserId.ToString();

                role.UpdateName(request.UpdatedRole.Name, string.Empty)
                    .UpdateDescription(request.UpdatedRole.Description, string.Empty);

                foreach (var permission in role.Permissions.ToArray())
                {
                    role.RemovePermission(permission);
                }

                foreach (var permission in request.UpdatedRole.Permissions)
                {
                    var perm = Context.Permissions
                        .SingleOrDefault(p => p.Name.ToLower().Equals(permission.ToLower()));

                    if (!(perm is null))
                    {
                        role.AddPermission(perm);
                    }
                }

                var status = await RoleManager.UpdateAsync(role);

                var isClaimUpToDate = request.UpdatedRole.HasAdminRights ? (
                    !RoleManager.HasClaim(role, Claims.Administrator) ? (
                        await RoleManager.AddClaimAsync(role, Claims.Administrator)
                    ).Succeeded : true
                ) : (
                    RoleManager.HasClaim(role, Claims.Administrator) ? (
                        await RoleManager.RemoveClaimAsync(role, Claims.Administrator)
                    ).Succeeded : true
                );
                
                if (status.Succeeded) 
                {
                    if (isClaimUpToDate)
                    {
                        var updatedRole = await RoleManager.FindByNameAsync(role.Name);

                        return DataOperationResult<RoleViewModel>.Success(new RoleViewModel {
                            Name = updatedRole.Name,
                            Description = updatedRole.Description,
                            HasAdminRights = RoleManager.HasClaim(updatedRole, Claims.Administrator),
                            Permissions = updatedRole.Permissions.Select(p => p.Name).ToList(),
                            UserCount = updatedRole.Users.Count,
                            CreatedOn = updatedRole.CreatedOn,
                            LastUpdatedOn = updatedRole.LastModifiedOn
                        });
                    }
                    return DataOperationResult<RoleViewModel>.SomeChangesNotSaved;
                }
                return DataOperationResult<RoleViewModel>.Failure();
            }
            catch (Exception ex)
            { 
                return DataOperationResult<RoleViewModel>.Failure(new string[] {
                    ex.ToString()
                });
            }
        }
    }
    


    public class DeleteRoleCommand : IRequest<DataOperationResult>
    {
        public string RoleName { get; set; }
    }



    public class DeleteRoleCommandHandler : IdentityRequestHandlerBase<DeleteRoleCommand, DataOperationResult>
    {
        private readonly RoleManager<UserRole> RoleManager;


        public DeleteRoleCommandHandler(
            IIdentityDatabaseContext context,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService,
            RoleManager<UserRole> roleManager
        ) : base(context, currentUserService, dateTimeService) => RoleManager = roleManager;


        public override async Task<DataOperationResult> Handle(
            DeleteRoleCommand request, CancellationToken token
        ) {
            var role = RoleManager.FindByNameAsync(request.RoleName).Result;

            if(role is null) { return DataOperationResult.NotFound; }

            try
            {
                if (RoleManager.HasClaim(role, Claims.Administrator))
                {
                    await RoleManager.RemoveClaimAsync(role, Claims.Administrator);
                }
                
                return (await RoleManager.DeleteAsync(role)).Succeeded ?
                        DataOperationResult.Success : DataOperationResult.Failure();
            }
            catch (Exception ex)
            {
                return DataOperationResult.Failure(ex.ToString());
            }
        }
    }
}
