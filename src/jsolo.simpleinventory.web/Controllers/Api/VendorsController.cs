using jsolo.simpleinventory.impl.controllers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.commands.Vendors;
using jsolo.simpleinventory.sys.models;
using jsolo.simpleinventory.sys.queries.Vendors;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace jsolo.simpleinventory.web.Controllers.Api;


public class VendorsController(UserManager<User> userManager, RoleManager<UserRole> roleManager) : AuthorizePermissionsBaseController(userManager, roleManager)
{

    // GET: Vendors
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <remarks>
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> All() => Ok(await Mediator.Send(new GetVendorsQuery()));


    // GET: Vendors/5
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    /// <remarks>
    /// </remarks>
    [HttpGet("{id}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Details(int id)
    {
        var vendor = await Mediator.Send(new GetVendorDetailsQuery
        {
            VendorId = id
        });

        if (vendor is not null) { return Ok(vendor); }

        return NotFound(new { message = "The vendor type with the specified id does not exist!" });
    }


    //POST: Vendors
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="201"></response>
    /// <response code="409"></response>
    /// <response code="400"></response>
    /// <remarks>
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(409)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(VendorViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new CreateVendorCommand
            {
                NewVendor = model
            });

            if (result.Succeeded) { return Created($"vendortypes/{result.Data.Id}", result.Data); }

            if (result.AlreadyExists == true)
            {
                return Conflict(new
                {
                    message = "A vendor type with the specified name already exists!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // PUT: Vendors/5
    // PATCH: Vendors/5
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="202"></response>
    /// <response code="404"></response>
    /// <response code="400"></response>
    /// <remarks>
    /// </remarks>
    [HttpPut("{id}")]
    [HttpPatch("{id}")]
    [ProducesResponseType(202)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(int id, VendorViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new UpdateVendorCommand
            {
                VendorId = id,
                UpdatedVendor = model
            });

            if (result.Succeeded) { return Accepted($"vendors/{result.Data.Id}", result.Data); }

            if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = "The vendor with the specified id does not exist!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // DELETE: Vendors/5
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    /// <response code="401"></response>
    /// <remarks>
    /// </remarks>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Delete(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new DeleteVendorCommand
            {
                VendorId = id,
            });

            if (result.Succeeded)
            {
                return Ok(new { message = "Successfully deleted Vendors!" });
            }

            if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = "The vendor with the specified id does not exist!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }
}
