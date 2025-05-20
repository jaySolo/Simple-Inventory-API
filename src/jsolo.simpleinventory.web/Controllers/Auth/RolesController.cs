using jsolo.simpleinventory.impl.commands;
using jsolo.simpleinventory.impl.controllers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.impl.queries;
using jsolo.simpleinventory.sys.models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace jsolo.simpleinventory.web.Controllers.Auth;



[Route("security/[controller]")]
public class RolesController(
    UserManager<User> usrMgr,
    RoleManager<UserRole> roleMgr,
    SignInManager<User> signInMgr
) : IdentityController(usrMgr, roleMgr, signInMgr)
{
    [HttpGet]
    public async Task<IActionResult> All() => Ok(await Mediator.Send(new GetRolesQuery()));


    [HttpGet("{value}")]
    public async Task<IActionResult> Details(string value)
    {
        var role = await Mediator.Send(new GetRoleDetailsQuery { RoleName = value });


        if (role == null)
        {
            return NotFound(new
            {
                message = $"Failed to find role '{value}'"
            });
        }

        return Ok(role);
    }


    [HttpPost]
    public async Task<IActionResult> Create(AddEditRoleViewModel model)
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

        var result = await Mediator.Send(new CreateRoleCommand
        {
            NewRole = model
        });

        if (result.Succeeded)
        {
            return Created($"system/roles/{result.Data.Name}", new
            {
                message = "Created role successfully!",
                data = result.Data
            });
        }
        else if (result.AlreadyExists == true)
        {
            return Conflict(new { message = "A role with this name already exists!" });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    [HttpPatch("{value}")]
    [HttpPut("{value}")]
    public async Task<IActionResult> Update(string value, AddEditRoleViewModel model)
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

        var result = await Mediator.Send(new UpdateRoleCommand
        {
            RoleName = value,
            UpdatedRole = model
        });

        if (result.Succeeded)
        {
            return Accepted($"system/roles/{result.Data.Name}", new
            {
                message = "Updated role successfully!",
                data = result.Data
            });
        }
        else if (result.AlreadyExists == true)
        {
            return Conflict(new { message = "A role with this name already exists!" });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    [HttpDelete("{value}")]
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

        var result = await Mediator.Send(new DeleteRoleCommand
        {
            RoleName = value
        });

        if (result.Succeeded)
        {
            return Accepted("", new { message = "Deleted role successfully!" });
        }
        else if (result.AlreadyExists == false)
        {
            return NotFound(new
            {
                message = $"Failed to find role '{value}'!"
            });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }
}
