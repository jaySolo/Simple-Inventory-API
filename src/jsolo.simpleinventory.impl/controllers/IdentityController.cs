using jsolo.simpleinventory.impl.identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace jsolo.simpleinventory.impl.controllers
{
    [ApiController]
    public class IdentityController(
        UserManager<User> userManager,
        RoleManager<UserRole> roleManager,
        SignInManager<User> signInManager
    ) : AuthorizePermissionsBaseController(userManager, roleManager)
    {
        private readonly SignInManager<User> _signInManager = signInManager;

        protected SignInManager<User> SignInManager => _signInManager;
    }
}
