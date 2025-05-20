using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using jsolo.simpleinventory.impl.controllers;
using jsolo.simpleinventory.sys.common.interfaces;
using jsolo.simpleinventory.sys.models;
using jsolo.simpleinventory.web.Services;


namespace jsolo.simpleinventory.web.Controllers;



[Authorize]
[Route("api")]
public class AuthController : BaseController
{
    #region private variables
    private readonly IUserService UserService;

    private readonly ICurrentUserService CurrentUser;
    #endregion


    #region constructor methods
    /// <summary>
    /// Constructor method
    /// </summary>
    public AuthController(IUserService userService, ICurrentUserService currentUserService)
    {
        UserService = userService;
        CurrentUser = currentUserService;
    }
    #endregion


    // GET: auth/identity
    // GET: users/identity
    /// <summary>Gets the status of the current user.</summary>
    /// <response code="200">
    /// The <see cref="UserIdentityStatusViewModel">information</see> of the currently logged
    /// in user. A user must have logged in prior to making this request.
    /// </response>
    /// <response code="403">The user is annonymous or has failed authentication.</response>
    [HttpGet("auth/identity")]
    [HttpGet("users/current")]
    [ProducesResponseType(200)]
    [ProducesResponseType(statusCode: 401)]
    public async Task<IActionResult> GetCurrentUser()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Ok(await UserService.GetUser(CurrentUser.UserId));
        }

        return Forbid();
    }


    // POST: auth/login
    // POST: auth/signin
    // POST: auth/sign-in
    /// <summary>Attempts to login using the supplied credentials.</summary>
    /// <response code="200">
    /// The <see cref="UserIdentityStatusViewModel">user</see> has been logged in successfully.
    /// </response>
    /// <response code="400">
    /// The <see cref="LoginViewModel">credentials</see> supplied is not in a valid format.
    /// </response>
    /// <response code="401">
    /// The <see cref="LoginViewModel">credentials</see> supplied are incorrect.
    /// </response>
    [AllowAnonymous]
    [HttpPost("auth/login")]
    [HttpPost("auth/signin")]
    [HttpPost("auth/sign-in")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(statusCode: 401)]
    public async Task<IActionResult> Authenticate([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {

            var user = await UserService.AuthenticateLogin(model.Email, model.Password);

            if (user is null)
            {
                return Unauthorized(new { message = "Username or password is incorrect" });
            }

            return Ok(user);
        }
        return BadRequest(ModelState);
    }


    // POST: auth/logout
    // POST: auth/signout
    // POST: auth/sign-out
    [HttpPost("auth/logout")]
    [HttpPost("auth/signout")]
    [HttpPost("auth/sign-Out")]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Invalidate()
    {
        await UserService.InvalidateLogin(CurrentUser.UserId);

        return Ok();
    }


    [HttpPost("auth/refresh-token")]  
    public async Task<IActionResult> Refresh()//[FromBody] TokenModel request)  
    {  
        var result = new {};    //await UserService.RefreshTokenAsync(request);

        return Ok(await Task.FromResult(result)); 
    }
}
