using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Identity;

using jsolo.simpleinventory.impl.common;
using jsolo.simpleinventory.impl.helpers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.handlers;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.models;


namespace jsolo.simpleinventory.impl.commands
{
    public class CreateUserCommand : IRequest<DataOperationResult<UserProfileViewModel>>
    {
        public AddEditUserViewModel NewUser { get; set; }
    }



    public class CreateUserCommandHandler : IdentityRequestHandlerBase<CreateUserCommand, DataOperationResult<UserProfileViewModel>>
    {
        private readonly UserManager<User> UserManager;

        private readonly RoleManager<UserRole> RoleManager;


        public CreateUserCommandHandler(
            IIdentityDatabaseContext context, ICurrentUserService userService, IDateTimeService dateTimeService,
            UserManager<User> userManger, RoleManager<UserRole> roleManager
        ) : base(context, userService, dateTimeService)
        {
            UserManager = userManger;
            RoleManager = roleManager;
        }


        public override async Task<DataOperationResult<UserProfileViewModel>> Handle(CreateUserCommand request, CancellationToken token)
        {
            var user = await UserManager.FindByNameAsync(request.NewUser.Username);
            var newUsrId = UserManager.Users.OrderByDescending(usr => usr.Id)?.FirstOrDefault()?.Id ?? 0;


            if (!(user is null)) { return DataOperationResult<UserProfileViewModel>.Exists; }

            try
            {
                var usr = new User(
                    id: newUsrId > 0 ? newUsrId + 1 : 1,
                    surname: request.NewUser.LastName,
                    firstNames: request.NewUser.FirstName,
                    username: request.NewUser.Username,
                    emailaddr: request.NewUser.EmailAddress,
                    phoneNum: request.NewUser.PhoneNumber,
                    createdOn: DateTime.Now,
                    creatorId: CurrentUser.UserId.ToString());

                if (request.NewUser.UserRoles?.Count > 0)
                {
                    foreach (var userRole in request.NewUser.UserRoles)
                    {
                        if (!usr.Roles.Any(r => r.Name.ToLower().Equals(userRole.ToLower())))
                        {
                            var role = await this.RoleManager.FindByNameAsync(userRole);
                            if (role != null) { usr.Roles.Add(role); }
                        }
                    }
                }
                // usr.UpdateName(surname: request.NewUser.LastName, firstNames: request.NewUser.FirstName);

                return (await this.UserManager.CreateAsync(usr, request.NewUser.Password)).Succeeded ?
                    DataOperationResult<UserProfileViewModel>.Success(
                        UsersRequestsHelpers.MapToProfileViewModel(usr)
                    ) : DataOperationResult<UserProfileViewModel>.Failure();
            }
            catch (Exception ex)
            {
                return DataOperationResult<UserProfileViewModel>.Failure(new string[] {
                    ex.ToString()
                });
            }
        }
    }



    public class UpdateUserCommand : IRequest<DataOperationResult<UserProfileViewModel>>
    {
        public string UserIdNameEmail { get; set; }

        public AddEditUserViewModel UpdatedUser { get; set; }
    }



    public class UpdateUserCommandHandler : RequestHandlerBase<UpdateUserCommand, DataOperationResult<UserProfileViewModel>>
    {
        private readonly UserManager<User> UserManager;

        private readonly RoleManager<UserRole> RoleManager;


        public UpdateUserCommandHandler(
            IIdentityDatabaseContext context,
            ICurrentUserService userService, IDateTimeService dateTimeService,
            UserManager<User> userManger, RoleManager<UserRole> roleManager
        ) : base(context, userService, dateTimeService)
        {
            UserManager = userManger;
            RoleManager = roleManager;
        }



        public override async Task<DataOperationResult<UserProfileViewModel>> Handle(
            UpdateUserCommand request, CancellationToken cancellationToken
        )
        {
            if (await UserManager.CheckPasswordAsync(
                await UserManager.FindByIdAsync(CurrentUser.UserId.ToString()),
                request.UpdatedUser.AdminPassword
            ))
            {

                // var user = await UserManager.FindByNameAsync(request.UpdatedUser.Username);
                var user = await UserManager.FindByIdAsync(request.UserIdNameEmail);

                if (user is null)
                {
                    return DataOperationResult<UserProfileViewModel>.NotFound;
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(request.UpdatedUser.Username))
                    {
                        user.UserName = request.UpdatedUser.Username;
                    }

                    if (!string.IsNullOrWhiteSpace(request.UpdatedUser.EmailAddress))
                    {
                        user.Email = request.UpdatedUser.EmailAddress;
                    }


                    user.PhoneNumber = request.UpdatedUser.PhoneNumber;
                    user.UpdateBirthdate(request.UpdatedUser.Birthday)
                        .UpdatePosition(request.UpdatedUser.Position);

                    if (request.UpdatedUser.UserRoles != null)
                    {
                        foreach (var role in user.Roles.ToArray())
                        {
                            user.Roles.Remove(role);
                        }
                        // foreach(var role in user.Roles) {
                        //     if(!request.UpdatedUser.UserRoles.Any(ur => ur.ToLower().Equals(role.Name.ToLower()))) {
                        //         user.Roles.Remove(role);
                        //     }
                        // }

                        foreach (var userRole in request.UpdatedUser.UserRoles)
                        {
                            if (!user.Roles.Any(r => r.Name.ToLower().Equals(userRole.ToLower())))
                            {
                                var role = await RoleManager.FindByNameAsync(userRole);
                                if (role != null) { user.Roles.Add(role); }
                            }
                        }
                    }

                    user.UpdateName(surname: request.UpdatedUser.LastName, firstNames: request.UpdatedUser.FirstName);

                    var result = await UserManager.UpdateAsync(user);

                    return result.Succeeded ? DataOperationResult<UserProfileViewModel>.Success(
                           UsersRequestsHelpers.MapToProfileViewModel(await UserManager.FindByNameAsync(user.UserName))
                        ) : DataOperationResult<UserProfileViewModel>.Failure();
                }
                catch (Exception ex)
                {
                    return DataOperationResult<UserProfileViewModel>.Failure(ex.ToString());
                }
            }
            throw new UnauthorizedAccessException();
        }
    }


    #region unused code
    /***********************************************************************************************************************************
    public class UpdateUserProfileCommand : IRequest<DataOperationResult<UserProfileViewModel>>
    {
        public string UserName { get; set; }

        public UserProfileViewModel UserProfile { get; set; }
    }



    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, DataOperationResult<UserProfileViewModel>>
    {
        private readonly UserManager<User> _userManager;

        public UpdateUserProfileCommandHandler(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<DataOperationResult<UserProfileViewModel>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user != null)
            {
                //TODO: apply update to user details
                // user.SetName(new Name());

                var result = await _userManager.UpdateAsync(user);


            }

            return DataOperationResult<UserProfileViewModel>.NotFound;

            // return IdentityResult.Failed(new IdentityError{
            //     Code = "404",
            //     Description = $"User with username '{request.UserName}' was not found."
            // });
        }
    }
    ***********************************************************************************************************************************/
    #endregion


    public class ChangeUserPasswordCommand : IRequest<DataOperationResult>
    {
        public string UserIdNameEmail { get; set; }

        public string NewUserPassword { get; set; }

        public string AdminPassword { get; set; }
    }



    public class ChangeUserPasswordCommandHandler : RequestHandlerBase<ChangeUserPasswordCommand, DataOperationResult>
    {
        private readonly UserManager<User> UserManager;

        private readonly RoleManager<UserRole> RoleManager;


        public ChangeUserPasswordCommandHandler(
            IIdentityDatabaseContext context,
            ICurrentUserService userService, IDateTimeService dateTimeService,
            UserManager<User> userManger
        ) : base(context, userService, dateTimeService) => UserManager = userManger;


        public override async Task<DataOperationResult> Handle(
            ChangeUserPasswordCommand request, CancellationToken cancellationToken
        )
        {
            if (await UserManager.CheckPasswordAsync(
                await UserManager.FindByIdAsync(CurrentUser.UserId.ToString()),
                request.AdminPassword
            ))
            {

                var user = await UserManager.FindByIdAsync(request.UserIdNameEmail);

                if (user == null)
                {
                    if ((user = await UserManager.FindByNameAsync(request.UserIdNameEmail)) == null)
                    {
                        return DataOperationResult.NotFound;
                    }
                }

                var token = await UserManager.GeneratePasswordResetTokenAsync(user);

                var result = await UserManager.ResetPasswordAsync(user, token, request.NewUserPassword);

                return result.Succeeded ? DataOperationResult.Success : DataOperationResult.Failure();
            }
            throw new UnauthorizedAccessException();

        }
    }



    public class LockUserCommand : IRequest<DataOperationResult>
    {
        public string UserIdNameEmail { get; set; }

        public string AdminPassword { get; set; }
    }



    public class LockUserCommandHandler : RequestHandlerBase<LockUserCommand, DataOperationResult>
    {
        public LockUserCommandHandler(
            IDatabaseContext context,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService
        ) : base(context, currentUserService, dateTimeService) { }


        public override Task<DataOperationResult> Handle(
            LockUserCommand request,
            CancellationToken cancellationToken
        )
        {
            return base.Handle(request, cancellationToken);
        }


        public override string ToString() => base.ToString();
    }



    public class UnlockUserCommand : IRequest<DataOperationResult>
    {
        public string UserIdNameEmail { get; set; }

        public string AdminPassword { get; set; }
    }



    public class UnlockUserCommandHandler : RequestHandlerBase<UnlockUserCommand, DataOperationResult>
    {
        public UnlockUserCommandHandler(
            IDatabaseContext context,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService
        ) : base(context, currentUserService, dateTimeService) { }


        public override Task<DataOperationResult> Handle(
            UnlockUserCommand request,
            CancellationToken cancellationToken
        )
        {
            return base.Handle(request, cancellationToken);
        }


        public override string ToString() => base.ToString();
    }



    public class DeleteUserCommand : IRequest<DataOperationResult>
    {
        public string UserIdNameEmail { get; set; }

        public string AdminPassword { get; set; }
    }



    public class DeleteUserCommandHandler : RequestHandlerBase<DeleteUserCommand, DataOperationResult>
    {
        private readonly UserManager<User> _userManager;


        public DeleteUserCommandHandler(
            IIdentityDatabaseContext context,
            ICurrentUserService userService, IDateTimeService dateTimeService,
            UserManager<User> userManger
        ) : base(context, userService, dateTimeService) => _userManager = userManger;


        public override async Task<DataOperationResult> Handle(
            DeleteUserCommand request, CancellationToken cancellationToken
        )
        {
            if (await _userManager.CheckPasswordAsync(
                await _userManager.FindByIdAsync(CurrentUser.UserId.ToString()),
                request.AdminPassword
            ))
            {

                var user = await _userManager.FindByEmailAsync(request.UserIdNameEmail);

                if (user == null)
                {
                    if ((user = await _userManager.FindByNameAsync(request.UserIdNameEmail)) == null)
                    {
                        if ((user = await _userManager.FindByIdAsync(request.UserIdNameEmail)) == null)
                        {
                            return DataOperationResult.NotFound;
                        }
                    }
                }

                var result = await _userManager.DeleteAsync(user);

                return result.Succeeded ? DataOperationResult.Success : DataOperationResult.Failure();

            }

            return DataOperationResult.Failure();
        }
    }
}
