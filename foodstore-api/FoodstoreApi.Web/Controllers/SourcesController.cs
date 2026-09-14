using FoodstoreApi.Usecase.Interfaces;
using FoodstoreApi.Web.Authorization;
using FoodstoreApi.Usecase.DTOs.Source;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR;
using FoodstoreApi.Web.Hubs;
using FoodstoreApi.Web.ApiResponse;

namespace FoodstoreApi.Web.Controllers;

[ApiController]
[Route("api/admin/sources")]
[Authorize]
public class SourcesController(ISourceService service, IHubContext<AppHub> hubContext) : ControllerBase
{
    private readonly ISourceService _service = service;
    private readonly IHubContext<AppHub> _hubContext = hubContext;

    [HttpGet]
    [RequirePermission("source.view")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var sources = await _service.GetAllAsync(cancellationToken);
        return ApiResult.Success(sources);
    }



    [HttpGet("{id:guid}")]
    [RequirePermission("source.view")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var source = await _service.GetByIdAsync(id, cancellationToken);
        if (source == null) return ApiResult.NotFound();
        return ApiResult.Success(source);
    }

    [HttpPost]
    [RequirePermission("source.create")]
    public async Task<IActionResult> Create([FromBody] CreateSourceDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);
        await _hubContext.Clients.All.SendAsync("ReceiveSourceUpdate", cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("source.update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateSourceDto dto, CancellationToken cancellationToken)
    {
        var ok = await _service.UpdateAsync(id, dto, cancellationToken);
        if (!ok) return ApiResult.NotFound();
        await _hubContext.Clients.All.SendAsync("ReceiveSourceUpdate", cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("source.delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var ok = await _service.DeleteAsync(id, cancellationToken);
            if (!ok) return ApiResult.NotFound();
            await _hubContext.Clients.All.SendAsync("ReceiveSourceUpdate", cancellationToken);
            return NoContent();
        }
        catch (Exception)
        {
            return ApiResult.BadRequest("This source cannot be deleted because there is an existing linked order.");
        }
    }
}




