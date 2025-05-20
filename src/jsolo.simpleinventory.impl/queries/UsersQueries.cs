using jsolo.simpleinventory.impl.helpers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.models;

using MediatR;
using Microsoft.AspNetCore.Identity;


namespace jsolo.simpleinventory.impl.queries;



public class GetUsersQuery : IRequest<List<UserProfileViewModel>>
{

}



public class GetUsersQueryHandler(UserManager<User> userManager) : IRequestHandler<GetUsersQuery, List<UserProfileViewModel>>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<List<UserProfileViewModel>> Handle(GetUsersQuery request, CancellationToken token = default)
    {
        List<UserProfileViewModel> getUsers()
        {
            var users = _userManager.Users.ToList();

            return [.. users.Select(
                u => UsersRequestsHelpers.MapToProfileViewModel(u)
            )];
        }


        return await Task.FromResult(getUsers());
    }
}



public class GetUserDetailsQuery : IRequest<UserProfileViewModel>
{
    public string UserNameOrId { get; set; } = string.Empty;
}



public class GetUserDetailsQueryHandler(UserManager<User> userManager) : IRequestHandler<GetUserDetailsQuery, UserProfileViewModel?>
{
    private readonly UserManager<User> _userManager = userManager;


    public async Task<UserProfileViewModel?> Handle(GetUserDetailsQuery request, CancellationToken token = default)
    {
        UserProfileViewModel? getProfile()
        {
            User? user = null;

            if (int.TryParse(request.UserNameOrId, out int userId))
            {
                user = _userManager.FindByIdAsync(request.UserNameOrId).Result;
            }

            if (user is null)
            {
                user = _userManager.FindByNameAsync(request.UserNameOrId).Result;
            }

            return user is null ? null : UsersRequestsHelpers.MapToProfileViewModel(user);
        }

        return await Task.FromResult(getProfile());
    }
}
