using FoodstoreApi.Usecase.Interfaces;
using FoodstoreApi.Web.Authorization;
using FoodstoreApi.Usecase.DTOs.Discount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using FoodstoreApi.Web.Hubs;
using FoodstoreApi.Web.ApiResponse;

namespace FoodstoreApi.Web.Controllers;

[ApiController]
[Route("api/admin/discounts")]
[Authorize]
public class DiscountsController : ControllerBase
{
    private readonly IDiscountService _service;
    private readonly IHubContext<AppHub> _hubContext;

    public DiscountsController(IDiscountService service, IHubContext<AppHub> hubContext)
    {
        _service = service;
        _hubContext = hubContext;
    }

    [HttpGet]
    [RequirePermission("discount.view")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return ApiResult.Success(items);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("discount.view")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null) return ApiResult.NotFound();
        return ApiResult.Success(item);
    }

    [HttpPost]
    [RequirePermission("discount.create")]
    public async Task<IActionResult> Create(CreateDiscountDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);
        await _hubContext.Clients.All.SendAsync("ReceiveDiscountUpdate", cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("discount.update")]
    public async Task<IActionResult> Update(Guid id, CreateDiscountDto dto, CancellationToken cancellationToken)
    {
        var ok = await _service.UpdateAsync(id, dto, cancellationToken);
        if (!ok) return ApiResult.NotFound();
        await _hubContext.Clients.All.SendAsync("ReceiveDiscountUpdate", cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("discount.delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var ok = await _service.DeleteAsync(id, cancellationToken);
            if (!ok) return ApiResult.NotFound();
            await _hubContext.Clients.All.SendAsync("ReceiveDiscountUpdate", cancellationToken);
            return NoContent();
        }
        catch (Exception)
        {
            return ApiResult.BadRequest("This discount code cannot be deleted because it has already been used in an order.");
        }
    }
}




