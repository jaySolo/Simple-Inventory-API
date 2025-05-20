using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Identity;

using jsolo.simpleinventory.impl.helpers;
using jsolo.simpleinventory.impl.common;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.models;
using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.impl.commands
{
    public class CreatePermissionCommand : IRequest<DataOperationResult<PermissionViewModel>>
    {
        public required PermissionViewModel NewPermission { get; set; }
    }



    public class CreatePermisssionCommandHandler : IdentityRequestHandlerBase<CreatePermissionCommand, DataOperationResult<PermissionViewModel>>
    {
        private readonly RoleManager<UserRole> RoleManager;

        public CreatePermisssionCommandHandler(
            IIdentityDatabaseContext context,
            ICurrentUserService currentUsers,
            IDateTimeService dateTime,
            RoleManager<UserRole> roleManager
        ) : base(context, currentUsers, dateTime)
        {
            RoleManager = roleManager;
        }


        public override async Task<DataOperationResult<PermissionViewModel>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = Context.Permissions.SingleOrDefault(p => p.Name.ToLower().Equals(request.NewPermission.Name.ToLower()));

            var permId = Context.Permissions.OrderByDescending(p => p.Id)?.FirstOrDefault()?.Id ?? 0;

            if (!(permission is null))
            {
                return DataOperationResult<PermissionViewModel>.Exists;
            }

            try
            {
                var perm = new UserPermission(
                    id: permId + 1,
                    name: request.NewPermission.Name,
                    route: request.NewPermission.Route,
                    acceptedMethods: request.NewPermission.AcceptedMethods,
                    description: request.NewPermission.Description,
                    createdOn: DateTime.Now,
                    creator: CurrentUser.UserName
                );

                if (request.NewPermission.Roles?.Count > 0)
                {
                    foreach (var role in request.NewPermission.Roles)
                    {
                        if (!perm.Roles.Any(r => r.Name.ToLower().Equals(role.ToLower())))
                        {
                            var permRole = await this.RoleManager.FindByIdAsync(role);

                            if (permRole != null)
                            {
                                perm.Roles.Add(permRole);
                            }
                        }
                    }
                }

                this.Context.BeginTransaction()
                            .Add(perm)
                            .SaveChanges()
                            .CloseTransaction();

                return DataOperationResult<PermissionViewModel>.Success(
                    PermissionsRequestsHelpers.MapToViewModel(perm)
                );
            }
            catch (Exception ex)
            {
                return DataOperationResult<PermissionViewModel>.Failure(new string[] {
                    ex.ToString()
                });
            }
        }


        public override string ToString()
        {
            return base.ToString();
        }
    }



    public class UpdatePermisssionCommand : IRequest<DataOperationResult<PermissionViewModel>>
    {
        public required string PermissionName { get; set; }

        public required PermissionViewModel UpdatedPermission { get; set; }
    }



    public class UpdatePermisssionCommandHandler : IdentityRequestHandlerBase<UpdatePermisssionCommand, DataOperationResult<PermissionViewModel>>
    {
        private readonly RoleManager<UserRole> RoleManager;

        public UpdatePermisssionCommandHandler(
            IIdentityDatabaseContext context,
            ICurrentUserService currentUsers,
            IDateTimeService dateTime,
            RoleManager<UserRole> roleManager
        ) : base(context, currentUsers, dateTime)
        {
            RoleManager = roleManager;
        }

        public override Task<DataOperationResult<PermissionViewModel>> Handle(UpdatePermisssionCommand request, CancellationToken cancellationToken)
        {
            var permission = Context.Permissions.SingleOrDefault(p => p.Name.ToLower().Equals(request.PermissionName.ToLower()));

            if (permission is null)
            {
                return Task.FromResult(DataOperationResult<PermissionViewModel>.NotFound);
            }

            try
            {
                permission.Name = request.UpdatedPermission.Name;
                permission.Description = request.UpdatedPermission.Description;
                permission.Route = request.UpdatedPermission.Route;
                permission.AllowedRequests = request.UpdatedPermission.AcceptedMethods;
                permission.LastModifiedOn = DateTime.Now;
                permission.LastModifiedBy = CurrentUser.UserName;

                // if (request.UpdatedPermission.Roles?.Count > 0)
                // {
                //     foreach (var role in request.NewPermission.Roles)
                //     {
                //         if (!permission.Roles.Any(r => r.Name.ToLower().Equals(role.ToLower())))
                //         {
                //             var permRole = await this.RoleManager.FindByIdAsync(role);

                //             if(permRole != null)
                //             {
                //                 permission.Roles.Add(permRole);
                //             }
                //         }
                //     }
                // }

                this.Context.BeginTransaction()
                            .Update(permission)
                            .SaveChanges()
                            .CloseTransaction();

                return Task.FromResult(DataOperationResult<PermissionViewModel>.Success(
                    PermissionsRequestsHelpers.MapToViewModel(permission)
                ));
            }
            catch (Exception ex)
            {
                return Task.FromResult(DataOperationResult<PermissionViewModel>.Failure(new string[] {
                    ex.ToString()
                }));
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }



    public class DeletePermisssionCommand : IRequest<DataOperationResult>
    {
        public required string PermissionName { get; set; }
    }



    public class DeletePermisssionCommandHandler : IdentityRequestHandlerBase<DeletePermisssionCommand, DataOperationResult>
    {
        public DeletePermisssionCommandHandler(
            IIdentityDatabaseContext context,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService
        ) : base(context, currentUserService, dateTimeService) { }


        public override Task<DataOperationResult> Handle(DeletePermisssionCommand request, CancellationToken cancellationToken)
        {
            var permission = Context.Permissions.SingleOrDefault(p => p.Name.ToLower().Equals(request.PermissionName.ToLower()));

            if (permission is null)
            {
                return Task.FromResult(DataOperationResult.NotFound);
            }

            try
            {

                this.Context.BeginTransaction()
                            .Delete(permission)
                            .SaveChanges()
                            .CloseTransaction();

                return Task.FromResult(DataOperationResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(DataOperationResult.Failure(new string[] {
                    ex.ToString()
                }));
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
