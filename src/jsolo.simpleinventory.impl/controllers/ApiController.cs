using jsolo.simpleinventory.impl.identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace jsolo.simpleinventory.impl.controllers;


// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApiController : BaseController
{
}



[ApiController]
[Route("api/data/[controller]")]
public class ApiDataController(UserManager<User> userManager, RoleManager<UserRole> roleManager) : AuthorizePermissionsBaseController(userManager, roleManager)
{
}



[ApiController]
[Route("api/stats/[controller]")]
public class ApiStatsController(UserManager<User> userManager, RoleManager<UserRole> roleManager) : AuthorizePermissionsBaseController(userManager, roleManager)
{
}
