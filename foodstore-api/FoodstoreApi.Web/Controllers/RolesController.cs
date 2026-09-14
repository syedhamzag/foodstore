using System;
using FoodstoreApi.Usecase.Interfaces;
using FoodstoreApi.Web.Authorization;
using FoodstoreApi.Usecase.DTOs.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodstoreApi.Web.ApiResponse;

namespace FoodstoreApi.Web.Controllers;

[ApiController]
[Route("api/admin/roles")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleService _service;

    public RolesController(IRoleService service)
    {
        _service = service;
    }

    [HttpGet]
    [RequirePermission("role.view")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var roles = await _service.GetAllAsync(cancellationToken);
        return ApiResult.Success(roles);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("role.view")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var role = await _service.GetByIdAsync(id, cancellationToken);
        if (role == null) return ApiResult.NotFound();
        return ApiResult.Success(role);
    }

    [HttpPost]
    [RequirePermission("role.create")]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("role.update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateRoleDto dto, CancellationToken cancellationToken)
    {
        var ok = await _service.UpdateAsync(id, dto, cancellationToken);
        if (!ok) return ApiResult.NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("role.delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var ok = await _service.DeleteAsync(id, cancellationToken);
            if (!ok) return ApiResult.NotFound();
            return NoContent();
        }
        catch (InvalidOperationException) { throw; }
        catch (Exception)
        {
            return ApiResult.BadRequest("This role cannot be deleted because it is currently held by an employee.");
        }
    }

    [HttpPost("{id:guid}/permissions")]
    [RequirePermission("role.update")]
    public async Task<IActionResult> AssignPermissions(Guid id, [FromBody] AssignPermissionsDto dto, CancellationToken cancellationToken)
    {
        var ok = await _service.AssignPermissionsAsync(id, dto, cancellationToken);
        if (!ok) return ApiResult.NotFound();
        return NoContent();
    }
}




