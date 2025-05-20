using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using jsolo.simpleinventory.impl.controllers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.commands.Currencies;
using jsolo.simpleinventory.sys.models;
using jsolo.simpleinventory.sys.queries.Currencies;


namespace jsolo.simpleinventory.web.Controllers.Api;



public class CurrenciesController(UserManager<User> userManager, RoleManager<UserRole> roleManager) : AuthorizePermissionsBaseController(userManager, roleManager)
{
    // GET: api/Currencies
    /// <summary>
    /// Gets a list of all Currencies in registry.
    /// </summary>
    /// <returns>The list of Currencies.</returns>
    /// <response code="200">A list of Currencies on record. NB: This list can be empty.</response>
    /// <remarks>
    /// Sample result:
    /// 
    /// GET api/Currencies
    /// 
    /// []
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> All() => Ok(await Mediator.Send(new GetCurrenciesQuery()));


    // GET: Currencies/XCD
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    /// <response code="401"></response>
    /// <remarks>
    /// </remarks>
    [HttpGet]
    [HttpGet("{code}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Details(string code)
    {
        var currency = await Mediator.Send(new GetCurrencyDetailsQuery
        {
            CurrencyCode = code
        });

        if (currency is not null)
        {
            return Ok(currency);
        }

        return NotFound();
    }


    // POST: Currencies
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
    public async Task<IActionResult> Create(CurrencyViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new CreateCurrencyCommand
            {
                NewCurrency = model
            });

            if (result.Succeeded) { return Created($"Currencies/{result.Data.Code}", result.Data); }

            if (result.AlreadyExists == true)
            {
                return Conflict(new
                {
                    message = "A currency with the specified name and parish already exists!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // DELETE: Currencies/XCD
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    /// <response code="400"></response>
    /// <remarks>
    /// </remarks>
    [HttpDelete("{code}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Delete(string code)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new DeleteCurrencyCommand
            {
                CurrencyCode = code,
            });

            if (result.Succeeded)
            {
                return Ok(new { message = "Successfully deleted Currency!" });
            }

            if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = "The currency with the specified id does not exist!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }
}
