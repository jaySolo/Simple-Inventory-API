using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using jsolo.simpleinventory.impl.commands;
using jsolo.simpleinventory.impl.controllers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.impl.queries;
using jsolo.simpleinventory.sys.models;


namespace jsolo.simpleinventory.web.Controllers.Auth;



[Route("security/[controller]")]
public class PermissionsController(
    UserManager<User> usrMgr, RoleManager<UserRole> roleMgr, SignInManager<User> signInMgr
) : IdentityController(usrMgr, roleMgr, signInMgr)
{
    [HttpGet]
    public async Task<IActionResult> All() => Ok(await Mediator.Send(new GetPermissionsQuery()));


    [HttpGet("{value}")]
    public async Task<IActionResult> Details(string value)
    {
        var permission = await Mediator.Send(new GetPermissionDetailsQuery
        {
            PermissionName = value
        });


        if (permission == null)
        {
            return NotFound(new
            {
                message = $"Failed to find permission '{value}'"
            });
        }

        return Ok(permission);
    }


    [HttpPost]
    // [Authorize(Policy = Policies.AllowDevelopers)]
    public async Task<IActionResult> Create(AddEditPermissionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "The information you submitted is not valid!" });
        }

        var user = await UserManager.FindByIdAsync(User?.Identity?.Name ?? "");

        if (user is null)
        {
            return Forbid();
        }

        if (!await UserManager.CheckPasswordAsync(user, model.AdminPassword))
        {
            return Unauthorized(new { message = "Invalid credentials provided for this operation!" });
        }

        var result = await Mediator.Send(new CreatePermissionCommand
        {
            NewPermission = model
        });

        if (result.Succeeded == true)
        {
            return Created($"system/roles/{result.Data.Name}", new
            {
                message = "Created permission successfully!",
                data = result.Data
            });
        }
        else if (result.AlreadyExists == true)
        {
            return Conflict(new { message = "A permission with this name already exists!" });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    [HttpPatch("{value}")]
    [HttpPut("{value}")]
    // [Authorize(Policy = nameof(Policies.AllowDevelopers))]
    public async Task<IActionResult> Update(string value, AddEditPermissionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "The information you submitted is not valid!" });
        }

        var user = await UserManager.FindByIdAsync(User?.Identity?.Name ?? "");

        if (user is null)
        {
            return Forbid();
        }

        if (!await UserManager.CheckPasswordAsync(user, model.AdminPassword))
        {
            return Unauthorized(new { message = "Invalid credentials provided for this operation!" });
        }

        var result = await Mediator.Send(new UpdatePermisssionCommand
        {
            PermissionName = value,
            UpdatedPermission = model
        });

        if (result.Succeeded)
        {
            return Accepted($"system/roles/{result.Data.Name}", new
            {
                message = "Updated permission successfully!",
                data = result.Data
            });
        }
        else if (result.AlreadyExists == false)
        {
            return NotFound(new { message = $"Failed to find permission '{value}'" });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    [HttpDelete("{value}")]
    // [Authorize(Policy = nameof(Policies.AllowDevelopers))]
    public async Task<IActionResult> Delete(string value, string authorization)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "The information you submitted is not valid!" });
        }

        var user = await UserManager.FindByIdAsync(User?.Identity?.Name ?? "");

        if (user is null)
        {
            return Forbid();
        }

        if (!await UserManager.CheckPasswordAsync(user, authorization))
        {
            return Unauthorized(new { message = "Invalid credentials provided for this operation!" });
        }

        var result = await Mediator.Send(new DeletePermisssionCommand
        {
            PermissionName = value
        });

        if (result.Succeeded)
        {
            return Accepted("", new
            {
                message = "Deleted permission successfully!"
            });
        }
        else if (result.AlreadyExists == false)
        {
            return NotFound(new
            {
                message = $"Failed to find permission '{value}'!"
            });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }
}
