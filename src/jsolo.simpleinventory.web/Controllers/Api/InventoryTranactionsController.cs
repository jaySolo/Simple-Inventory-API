using jsolo.simpleinventory.impl.controllers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.commands.InventoryTransactions;
using jsolo.simpleinventory.sys.models;
using jsolo.simpleinventory.sys.queries.Inventories;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace jsolo.simpleinventory.web.Controllers.Api;


public class InventoryTransactionsController(UserManager<User> userManager, RoleManager<UserRole> roleManager) : AuthorizePermissionsBaseController(userManager, roleManager)
{

    // GET: InventoryTransactions
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <remarks>
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> All() => Ok(await Mediator.Send(new GetInventoryTransactionsQuery()));


    // GET: InventoryTransactions/5
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
    public async Task<IActionResult> Details(Guid id)
    {
        var inventoryTransaction = await Mediator.Send(new GetInventoryTransactionDetailsQuery
        {
            InventoryTransactionId = id
        });

        if (inventoryTransaction is not null) { return Ok(inventoryTransaction); }

        return NotFound(new { message = "The inventory transaction with the specified id does not exist!" });
    }


    //POST: InventoryTransactions
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
    public async Task<IActionResult> Create(InventoryTransactionViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new CreateInventoryTransactionCommand
            {
                NewInventoryTransaction = model
            });

            if (result.Succeeded) { return Created($"inventorytransactions/{result.Data.Id}", result.Data); }

            if (result.AlreadyExists == true)
            {
                return Conflict(new
                {
                    message = "A inventory transaction with the specified name already exists!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // PUT: InventoryTransactions/CA761232-ED42-11CE-BACD-00AA0057B223
    // PATCH: InventoryTransactions/CA761232-ED42-11CE-BACD-00AA0057B223
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
    public async Task<IActionResult> Update(Guid id, InventoryTransactionViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new UpdateInventoryTransactionCommand
            {
                InventoryTransactionId = id,
                UpdatedInventoryTransaction = model
            });

            if (result.Succeeded) { return Accepted($"inventorytransactions/{result.Data.Id}", result.Data); }

            if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = "The inventory transaction with the specified id does not exist!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // DELETE: InventoryTransactions/CA761232-ED42-11CE-BACD-00AA0057B223
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
    public async Task<IActionResult> Delete(Guid id)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new DeleteInventoryTransactionCommand
            {
                InventoryTransactionId = id,
            });

            if (result.Succeeded)
            {
                return Ok(new { message = "Successfully deleted inventory transaction!" });
            }

            if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = "The inventory transaction with the specified id does not exist!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }
}
