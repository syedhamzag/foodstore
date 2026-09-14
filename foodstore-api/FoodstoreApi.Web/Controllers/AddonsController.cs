using FoodstoreApi.Usecase.Interfaces;
using FoodstoreApi.Web.Authorization;
using FoodstoreApi.Usecase.DTOs.Addon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodstoreApi.Web.ApiResponse;

namespace FoodstoreApi.Web.Controllers;

[ApiController]
[Route("api/admin/addons")]
[Authorize]
public class AddonsController(IAddonService service) : ControllerBase
{
    private readonly IAddonService _service = service;

    [HttpGet]
    [RequirePermission("addon.view")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var addons = await _service.GetAllAsync(cancellationToken);
        return ApiResult.Success(addons);
    }

    [HttpGet("/api/pos/addons")]
    [AllowAnonymous]
    public async Task<IActionResult> PosGetAll(CancellationToken cancellationToken)
    {
        var addons = await _service.GetAllAsync(cancellationToken);
        return ApiResult.Success(addons);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("addon.view")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var addon = await _service.GetByIdAsync(id, cancellationToken);
        if (addon == null) return ApiResult.NotFound();
        return ApiResult.Success(addon);
    }

    [HttpPost]
    [RequirePermission("addon.create")]
    public async Task<IActionResult> Create([FromBody] CreateAddonDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("addon.update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateAddonDto dto, CancellationToken cancellationToken)
    {
        var ok = await _service.UpdateAsync(id, dto, cancellationToken);
        if (!ok) return ApiResult.NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("addon.delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var ok = await _service.DeleteAsync(id, cancellationToken);
            if (!ok) return ApiResult.NotFound();
            return NoContent();
        }
        catch (Exception)
        {
            return ApiResult.BadRequest("This topping cannot be deleted because it is currently being used in an order.");
        }
    }
}




