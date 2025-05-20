using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using jsolo.simpleinventory.impl.identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace jsolo.simpleinventory.impl.controllers;



[ApiController]
[Route("api/[controller]")]
public class BaseController : Controller
{
    private IMediator _mediator;

    protected IMediator Mediator =>
        _mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));
}



[Authorize]
public class AuthorizePermissionsBaseController : BaseController
{
    private UserManager<User> _usrMgr;

    private RoleManager<UserRole> _roleMgr;


    public AuthorizePermissionsBaseController(
        UserManager<User> userManager,
        RoleManager<UserRole> roleManager
    )
    {
        _usrMgr = userManager;
        _roleMgr = roleManager;
    }


    // public override async Task OnActionExecutionAsync(
    //     ActionExecutingContext context,
    //     ActionExecutionDelegate next
    // )
    // {
    //     bool isAuthorized = false;

    //     string request_path = context.ActionDescriptor.AttributeRouteInfo.Template.ToLower(),
    //            request_method = context.HttpContext.Request.Method;

    //     System.Console.WriteLine($"Route: {request_path}");
    //     System.Console.WriteLine($"Method: {request_method}");

    //     var user = await UserManager.FindByIdAsync(context.HttpContext.User.Identity.Name);

    //     var isUsrDev = UserManager.HasClaim(user, identity.Claims.Developer);

    //     foreach (var role in user.Roles)
    //     {
    //         if (isUsrDev || isAuthorized) break;

    //         isUsrDev |= RoleManager.HasClaim(role, identity.Claims.Developer);

    //         isAuthorized |= RoleManager.HasPermission(role, request_path, request_method);

    //         // foreach (var permission in role.Permissions)
    //         // {
    //         //     isAuthorized |= permission.Route.ToLower().Equals(request_path) &&
    //         //         permission.AllowedRequests.Any(req => req.ToLower().Equals(request_method));

    //         //     if (isAuthorized) break;
    //         // }
    //     }

    //     var isUsrAdm = UserManager.HasClaim(user, identity.Claims.Administrator);

    //     foreach (var role in user.Roles)
    //     {
    //         if (isUsrAdm) break;

    //         isUsrAdm |= RoleManager.HasClaim(role, identity.Claims.Administrator);
    //     }

    //     System.Console.WriteLine($"Is User Developer: {isUsrDev}, Is User Administrator: {isUsrAdm}, Is User Authorized: {isAuthorized}");

    //     if (isUsrDev || isAuthorized)
    //     {
    //         await next();
    //     }
    //     else
    //     {
    //         context.Result = new ForbidResult();
    //     }
    // }


    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
    }


    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);
    }


    protected UserManager<User> UserManager => _usrMgr ??=
        (UserManager<User>)HttpContext.RequestServices.GetService(typeof(UserManager<User>));

    protected RoleManager<UserRole> RoleManager => _roleMgr ??=
        (RoleManager<UserRole>)HttpContext.RequestServices.GetService(typeof(RoleManager<UserRole>));
}
