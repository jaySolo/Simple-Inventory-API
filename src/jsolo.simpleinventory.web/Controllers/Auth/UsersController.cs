using jsolo.simpleinventory.impl.commands;
using jsolo.simpleinventory.impl.controllers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.impl.queries;
using jsolo.simpleinventory.sys.models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace jsolo.simpleinventory.web.Controllers.Auth;



[ApiController]
[Route("api/security/[controller]")]
public class UsersController() : BaseController
{


    // GET: Users
    [HttpGet]
    public async Task<IActionResult> All() => Ok(await Mediator.Send(new GetUsersQuery()));


    // GET: Users/5
    [HttpGet("{value}")]
    public async Task<IActionResult> Details(string value)
    {
        var user = await Mediator.Send(new GetUserDetailsQuery
        {
            UserNameOrId = value
        });

        if (user != null) { return Ok(user); }

        return NotFound(new { message = $"Failed to find user with id, username or e-mail address: '{value}'!" });
    }


    // POST: Users
    [HttpPost]
    public async Task<IActionResult> Create(AddEditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "The information you submitted is not valid!" });
        }

        var result = await Mediator.Send(new CreateUserCommand
        {
            NewUser = model
        });

        if (result.Succeeded)
        {
            return Created($"security/users/{result.Data.Id}", new
            {
                message = "Created user successfully!",
                data = result.Data
            });
        }
        else if (result.AlreadyExists == true)
        {
            return Conflict(new
            {
                message = "A user with this username and/or e-mail address already exists!"
            });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // PATCH: Users/5
    // PUT: Users/5
    [HttpPatch("{value}")]
    [HttpPut("{value}")]
    public async Task<IActionResult> Update(string value, AddEditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "The information you submitted is not valid!" });
        }

        try
        {
            var result = await Mediator.Send(new UpdateUserCommand
            {
                UserIdNameEmail = value,
                UpdatedUser = model
            });

            if (result.Succeeded)
            {
                return Accepted($"system/users/{result.Data.Id}", new
                {
                    message = "Updated user successfully!",
                    data = result.Data
                });
            }
            else if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = $"Failed to find user with id, username or e-mail address: '{value}'!"
                });
            }
        }
        catch
        {

            return Forbid();//new { message = "The information you submitted is not valid!" });
        }
        return BadRequest(new { message = "The information you submitted is definitely not valid!" });

    }


    // PATCH: Users/5/Change-Password
    // PUT: Users/5/Change-Password
    [HttpPatch("{value}/Change-Password")]
    [HttpPut("{value}/Change-Password")]
    public async Task<IActionResult> ChangePassword(string value, ChangeUserPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "The information you submitted is not valid!" });
        }
        try
        {

            var result = await Mediator.Send(new ChangeUserPasswordCommand
            {
                UserIdNameEmail = value,
                NewUserPassword = model.NewPassword,
                AdminPassword = model.AdminPassword,
            });

            if (result.Succeeded)
            {
                return Accepted("", new
                {
                    message = "Changed user password successfully!"
                });
            }
            else if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = $"Failed to find user with id, username or e-mail address: '{value}'!"
                });
            }
        }
        catch
        {

            return Forbid();//new { message = "The information you submitted is not valid!" });
        }

        return BadRequest(new { message = "The information you submitted is definitely not valid!" });
    }


    // PATCH: Users/5/Lock
    // PUT: Users/5/Lock
    [HttpPatch("{value}/Lock")]
    [HttpPut("{value}/Lock")]
    public async Task<IActionResult> Lock(string value, string password)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "The information you submitted is not valid!" });
        }

        var result = await Mediator.Send(new LockUserCommand
        {
            UserIdNameEmail = value,
            AdminPassword = password,
        });

        if (result.Succeeded)
        {
            return Accepted("", new
            {
                message = "Locked user account successfully!"
            });
        }
        else if (result.AlreadyExists == false)
        {
            return NotFound(new
            {
                message = $"Failed to find user with id, username or e-mail address: '{value}'!"
            });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // PATCH: Users/5/Unlock
    // PUT: Users/5/Unlock
    [HttpPatch("{value}/Unlock")]
    [HttpPut("{value}/Unlock")]
    public async Task<IActionResult> Unlock(string value, string password)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "The information you submitted is not valid!" });
        }

        var result = await Mediator.Send(new UnlockUserCommand
        {
            UserIdNameEmail = value,
            AdminPassword = password,
        });

        if (result.Succeeded)
        {
            return Accepted("", new
            {
                message = "Unlocked user account successfully!"
            });
        }
        else if (result.AlreadyExists == false)
        {
            return NotFound(new
            {
                message = $"Failed to find user with id, username or e-mail address: '{value}'!"
            });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // DELETE: Users/5
    [HttpDelete("{value}")]
    public async Task<IActionResult> Delete(string value, string password)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "The information you submitted is not valid!" });
        }

        var result = await Mediator.Send(new DeleteUserCommand
        {
            UserIdNameEmail = value,
            AdminPassword = password
        });

        if (result.Succeeded)
        {
            return Accepted("", new { message = "Deleted user successfully!" });
        }
        else if (result.AlreadyExists == false)
        {
            return NotFound(new
            {
                message = $"Failed to find user with id, username or e-mail address: '{value}'!"
            });
        }

        return BadRequest(new { message = "The information you submitted is not valid!" });
    }
}
