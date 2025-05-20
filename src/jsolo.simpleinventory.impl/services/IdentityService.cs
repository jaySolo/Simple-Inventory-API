using Microsoft.AspNetCore.Identity;

using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.common;
using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.impl.services;



public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;

    public IdentityService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }


    public async Task<string> GetUserNameAsync(int userId)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

        return await Task.FromResult(user?.UserName ?? "");
    }


    public async Task<(Result Result, int UserId)> CreateUserAsync(
       int id, string surname, string firstNames,
       string userName, string email, string password
    )
    {
        var user = new User(id, surname, firstNames, userName, email);

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }


    public async Task<Result> DeleteUserAsync(int userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user != null)
        {
            return await DeleteUserAsync(user);
        }

        return Result.Success;
    }


    public async Task<Result> DeleteUserAsync(User user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
}
