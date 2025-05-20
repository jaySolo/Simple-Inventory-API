using jsolo.simpleinventory.impl.controllers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.commands.Products;
using jsolo.simpleinventory.sys.models;
using jsolo.simpleinventory.sys.queries.Products;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace jsolo.simpleinventory.web.Controllers.Api;


public class ProductsController(UserManager<User> userManager, RoleManager<UserRole> roleManager) : AuthorizePermissionsBaseController(userManager, roleManager)
{

    // GET: Products
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <remarks>
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> All() => Ok(await Mediator.Send(new GetProductsQuery()));


    // GET: Products/5
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
        var product = await Mediator.Send(new GetProductDetailsQuery
        {
            ProductId = id
        });

        if (product is not null) { return Ok(product); }

        return NotFound(new { message = "The product type with the specified id does not exist!" });
    }


    //POST: Products
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
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new CreateProductCommand
            {
                NewProduct = model
            });

            if (result.Succeeded) { return Created($"producttypes/{result.Data.Id}", result.Data); }

            if (result.AlreadyExists == true)
            {
                return Conflict(new
                {
                    message = "A product type with the specified name already exists!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // PUT: Products/5
    // PATCH: Products/5
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
    public async Task<IActionResult> Update(int id, ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new UpdateProductCommand
            {
                ProductId = id,
                UpdatedProduct = model
            });

            if (result.Succeeded) { return Accepted($"products/{result.Data.Id}", result.Data); }

            if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = "The product with the specified id does not exist!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // DELETE: Products/5
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
            var result = await Mediator.Send(new DeleteProductCommand
            {
                ProductId = id,
            });

            if (result.Succeeded)
            {
                return Ok(new { message = "Successfully deleted Products!" });
            }

            if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = "The product with the specified id does not exist!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }
}
