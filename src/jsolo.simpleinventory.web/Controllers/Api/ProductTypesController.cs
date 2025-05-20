using jsolo.simpleinventory.impl.controllers;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.commands.ProductTypes;
using jsolo.simpleinventory.sys.models;
using jsolo.simpleinventory.sys.queries.ProductTypes;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace jsolo.simpleinventory.web.Controllers.Api;



public class ProductTypesController(UserManager<User> userManager, RoleManager<UserRole> roleManager) : AuthorizePermissionsBaseController(userManager, roleManager)
{

    // GET: ProductTypes
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <remarks>
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> All() => Ok(await Mediator.Send(new GetProductTypesQuery()));


    // GET: ProductTypes/5
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    /// <remarks>
    /// </remarks>
    [HttpGet("{name}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Details(string name)
    {
        var productType = await Mediator.Send(new GetProductTypeDetailsQuery
        {
            ProductTypeName = name
        });

        if (productType != null) { return Ok(productType); }

        return NotFound(new { message = "The product type with the specified id does not exist!" });
    }


    //POST: ProductTypes
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
    public async Task<IActionResult> Create(ProductTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new CreateProductTypeCommand
            {
                NewProductType = model
            });

            if (result.Succeeded) { return Created($"producttypes/{result.Data.Name}", result.Data); }

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


    // PUT: ProductTypes/Cheese
    // PATCH: ProductTypes/Cheese
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="202"></response>
    /// <response code="404"></response>
    /// <response code="400"></response>
    /// <remarks>
    /// </remarks>
    [HttpPut("{name}")]
    [HttpPatch("{name}")]
    [ProducesResponseType(202)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(string name, ProductTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new UpdateProductTypeCommand
            {
                ProductTypeName = name,
                UpdatedProductType = model
            });

            if (result.Succeeded) { return Accepted($"producttypes/{result.Data.Name}", result.Data); }

            if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = "The product type with the specified id does not exist!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }


    // DELETE: ProductTypes/Cheese
    /// <summary>
    /// </summary>
    /// <returns>
    /// </returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    /// <response code="401"></response>
    /// <remarks>
    /// </remarks>
    [HttpDelete("{name}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Delete(string name)
    {
        if (ModelState.IsValid)
        {
            var result = await Mediator.Send(new DeleteProductTypeCommand
            {
                ProductTypeName = name,
            });

            if (result.Succeeded)
            {
                return Ok(new { message = "Successfully deleted Product Type!" });
            }

            if (result.AlreadyExists == false)
            {
                return NotFound(new
                {
                    message = "The product type with the specified id does not exist!"
                });
            }
        }
        return BadRequest(new { message = "The information you submitted is not valid!" });
    }
}
