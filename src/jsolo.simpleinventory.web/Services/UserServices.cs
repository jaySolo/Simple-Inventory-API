using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.models;
using jsolo.simpleinventory.web.Configurations;
using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.web.Services;



public interface IUserService
{
    Task<UserIdentityStatusViewModel> GetUser(int userId);

    Task<UserIdentityStatusViewModel?> AuthenticateLogin(string email, string password);

    Task InvalidateLogin(int userId);
}



public class UserService : IUserService
{
    #region properties
    /*
    
    private readonly UserManager<User> UserManager;
    */

    private readonly AppSettings Settings;

    private readonly IDateTimeService DateTime;

    private readonly UserManager<User> UserManager;

    private readonly RoleManager<UserRole> RoleManager;

    private readonly SignInManager<User> SignInManager;


    private readonly ILogger<UserService> Logger;

    #endregion


    #region constructor methods
    public UserService(
        IOptions<AppSettings> appSettings,
        IDateTimeService dateTime,
        UserManager<User> userManager,
        RoleManager<UserRole> roleManager,
        SignInManager<User> signInManager,
        ILogger<UserService> _logger
    )
    {
        this.Settings = appSettings.Value;

        this.DateTime = dateTime;

        this.UserManager = userManager;
        this.RoleManager = roleManager;
        this.SignInManager = signInManager;

        this.Logger = _logger;
    }
    #endregion


    public async Task<UserIdentityStatusViewModel> GetUser(int userId)
    {

        var user = await UserManager.FindByIdAsync(userId.ToString());

        if (user is not null)
        {
            var usrRoles = user.Roles?.Select(r => r.Name)?.ToArray() ?? Array.Empty<string>();
            var usrAdminStatus = user.Roles?.Any(r => RoleManager.HasClaim(r, Claims.Administrator)) == true;
            var usrPerms = user.Roles?.SelectMany(r => r.Permissions).Select(p => new PermissionViewModel
            {
                Name = p.Name,
                Description = p.Description,
                Route = p.Route,
                AcceptedMethods = p.AllowedRequests,
                CreatedOn = p.CreatedOn,
                LastUpdatedOn = p.LastModifiedOn
            }).ToArray() ?? new List<PermissionViewModel>().ToArray();


            Logger.LogInformation(message: "Retrieved user information for user with User ID '{userId}'.", userId);

            return new UserIdentityStatusViewModel(
                id: user.Id,
                user.FirstName,
                // user.OtherNames,
                user.Surname,
                user.UserName,
                user.Email,
                user.EmailConfirmed,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                islocked: user.LockoutEnabled && user.LockoutEnd >= DateTime.Now,
                position: user.Position,
                birthday: user.Birthdate,
                createdOn: user.CreatedOn,
                lastModifiedOn: user.LastModifiedOn,
                // userToken: null,
                // userSessionId: null,
                isUserAuthenticated: true,
                userRoles: usrRoles
            ).SetAdminStatus(usrAdminStatus)
             .DefinePermissions(usrPerms);
        }
        else
        {
            Logger.LogWarning(message: "Failed to retrieve user information for user with User ID '{userId}'.", userId);

            return new UserIdentityStatusViewModel(isUserAuthenticated: true);
        }
    }


    public async Task<UserIdentityStatusViewModel?> AuthenticateLogin(string email, string password)
    {
        var user = await UserManager.FindByEmailAsync(email);

        if (user is null)
        {
            return null;    // return null if user not found
        }

        if (!await UserManager.CheckPasswordAsync(user, password))
        {
            return null;    // return null if password is incorrect
        }

        // authentication successful so generate jwt token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.NormalizedUserName),
                new Claim(ClaimTypes.Role, user.Roles.FirstOrDefault()?.Name ?? string.Empty)
            }),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        Logger.LogInformation(
            message: "User {Username} (ID: {Id}, E-mail:{EmailAddress}) has logged in.",
            user.UserName, user.Id, user.Email
        );

        return new UserIdentityStatusViewModel(
            id: user.Id,
            firstNames: user.FirstName,
            surname: user.Surname,
            userName: user.UserName,
            isUserAuthenticated: true,
            userToken: tokenHandler.WriteToken(token),
            userRoles: user.Roles.Select(r => r.Name).ToArray()
        );
    }



    public async Task InvalidateLogin(int userId)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());

        await SignInManager.SignOutAsync();

        Logger.LogInformation(
            message: "User '{UserName} (ID: {Id}, E-mail:{Email})' has logged out.",
            user.UserName, user.Id, user.Email
        );
    }
}
